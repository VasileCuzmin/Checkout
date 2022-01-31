using Checkout.Domain.DomainEvents;
using NBB.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Domain.BasketAggregate
{
    public class Basket : EventSourcedAggregateRoot<BasketId>
    {
        public BasketId BasketId { get; private set; }
        public User User { get; private set; }
        private HashSet<Product> _products = new HashSet<Product>();
        public IReadonlyHashSet<Product> Products => _products.AsReadOnly();
        public decimal TotalNet { get; private set; }
        public decimal TotalGross
        {
            get
            {
                if (User.PaysVAT)
                    return TotalNet + TotalNet * 0.1m;

                return TotalNet;
            }
        }

        public Status Status { get; private set; } = Status.Opened;

        //needed for es repository
        public Basket()
        {
        }

        public static Basket New(BasketId basketId, User user)
        {
            if (basketId == null)
            {
                throw new ArgumentNullException(nameof(basketId));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var basket = new Basket();
            basket.Emit(new BasketCreated(basketId, user));
            return basket;
        }

        public void AddProduct(Guid productId, string name, string description, decimal? price, int quantity, int stock)
        {
            if (Status == Status.Closed)
            {
                throw new DomainException("This basket has been closed!");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new DomainException("The name of the product is required!");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("The quantity must be higher than 0 !");
            }

            if (price < 0)
            {
                throw new ArgumentException("The price must be higher than 0 !");
            }

            var existingProduct = _products.FirstOrDefault(x => x.ProductId == productId);
            if (existingProduct != null)
            {
                if (existingProduct.IsOutOfStock(quantity, stock))
                {
                    throw new DomainException($"The stock is out of order for {name} product");
                }

                Emit(new QuantityHasBeenIncremented(existingProduct.ProductId, quantity, existingProduct.Quantity + quantity, existingProduct.Price));
            }
            else
            {
                Emit(new NewProductHasBeenAdded(productId, name, price, quantity, description));
            }
        }

        public void Close()
        {
            if (Status == Status.Closed)
            {
                throw new DomainException("This basket has already been closed!");
            }

            Emit(new BasketHasBeenClosed(BasketId));
        }

        #region Apply
        private void Apply(BasketCreated e)
        {
            BasketId = new BasketId(e.BasketId.Value);
            User = e.user;
            Status = Status.Opened;
        }

        private void Apply(NewProductHasBeenAdded e)
        {
            var product = new Product(e.ProductId, e.Name, e.Description, e.Price, e.Quantity);
            _products.Add(product);
            TotalNet += e.Price.Value * e.Quantity.Value;
        }

        private void Apply(QuantityHasBeenIncremented e)
        {
            var existingProduct = _products.FirstOrDefault(x => x.ProductId == e.ProductId);
            existingProduct.Quantity = e.LastQuantity.Value;
            TotalNet += e.Price.Value * e.AddedQuantity;
        }

        private void Apply(BasketHasBeenClosed e)
        {
            Status = Status.Closed;
        }

        #endregion
        public override BasketId GetIdentityValue() => BasketId;
    }
}