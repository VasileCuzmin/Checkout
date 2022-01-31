using Checkout.Domain.BasketAggregate;
using Checkout.Domain.DomainEvents;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using static Checkout.Domain.Tests.BasketAggregateTests.Setup;

namespace Checkout.Domain.Tests.BasketAggregateTests
{
    public class Basket_New_Tests
    {
        [Fact]
        public void CreateNewBasket_IdIsNull_ThrowsException()
        {
            // Arrange && Act
            var createNewBasketAction = new Action(() => Basket.New(null, new User("John Doe", true)));

            // Assert
            Assert.Throws<ArgumentNullException>(createNewBasketAction).ParamName.Should().Be("basketId");
        }

        [Fact]
        public void CreateNewBasket_UserIsNull_ThrowsException()
        {
            // Arrange && Act
            var createNewBasketAction = new Action(() => Basket.New(new BasketId(), null));

            // Assert
            Assert.Throws<ArgumentNullException>(createNewBasketAction).ParamName.Should().Be("user");
        }

        [Fact]
        public void Creating_Basket_Emits_BasketCreated_DomainEvent()
        {
            // Arrange && Act
            var sut = GenerateBasketAggregate();
            var changes = sut.GetUncommittedChanges().ToList();

            // Assert
            changes.Should().HaveCount(1);
            changes.Should().AllBeOfType<BasketCreated>();
        }
    }
}