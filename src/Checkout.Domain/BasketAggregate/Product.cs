using NBB.Domain;
using System;

namespace Checkout.Domain.BasketAggregate
{
    //for our example keep the properties as primitive types not value objects
    public class Product : Entity<Guid>
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal? Price { get; private set; }
        public int? Quantity { get; set; } = 0;

        public Product(Guid productId, string name, string description, decimal? price, int? quantity)
        {
            if (price == null)
            {
                throw new ArgumentNullException(nameof(price));
            }

            if (quantity == null)
            {
                throw new ArgumentNullException(nameof(quantity));
            }

            if (quantity < 0)
            {
                throw new ArgumentException("The quantity must be higher than 0 !");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new DomainException("You must specify the name of the product!");
            }

            ProductId = productId;
            Price = price;
            Name = name;
            Description = description;
            Quantity = quantity;
        }

        public void IncrementQuantity(int value)
        {
            if (value < 0)
            {
                throw new DomainException("The price must be higher than 0!");
            }

            Quantity += value;
        }

        public bool IsOutOfStock(int addedQuantity, int stock)
        {
            if (Quantity + addedQuantity > stock)
                return true;

            return false;
        }

        public override Guid GetIdentityValue() => ProductId;
    }
}