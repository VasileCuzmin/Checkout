using Checkout.Domain.BasketAggregate;
using Checkout.Domain.DomainEvents;
using FluentAssertions;
using NBB.Domain;
using System;
using System.Linq;
using Xunit;
using static Checkout.Domain.Tests.BasketAggregateTests.Setup;

namespace Checkout.Domain.Tests.BasketAggregateTests
{
    public class Basket_AddProduct_Tests
    {
        [Fact]
        public void AddProduct_StatusClosed_ThrowsDomainException()
        {
            // Arrange
            var sut = GenerateBasketAggregate();

            //Act
            sut.Close();
            var addProduct = new Action(() => sut.AddProduct(Guid.NewGuid(), string.Empty, string.Empty, null, 1, 1));

            // Assert
            var ex = Assert.Throws<DomainException>(addProduct);
            Assert.Equal("This basket has been closed!", ex.Message);
        }

        [Fact]
        public void AddProduct_QuantityLowerThanZero_ThrowsArgumentException()
        {
            // Arrange
            var sut = GenerateBasketAggregate();

            //Act
            var addProduct = new Action(() => sut.AddProduct(Guid.NewGuid(), "TV", string.Empty, null, 0, 1));

            // Assert
            var ex = Assert.Throws<ArgumentException>(addProduct);
            Assert.Equal("The quantity must be higher than 0 !", ex.Message);
        }

        [Fact]
        public void AddProduct_PriceLowerThanZero_ThrowsArgumentException()
        {
            // Arrange
            var sut = GenerateBasketAggregate();

            //Act
            var addProduct = new Action(() => sut.AddProduct(Guid.NewGuid(), "TV", string.Empty, -1, 1, 1));

            // Assert
            var ex = Assert.Throws<ArgumentException>(addProduct);
            Assert.Equal("The price must be higher than 0 !", ex.Message);
        }
    }
}