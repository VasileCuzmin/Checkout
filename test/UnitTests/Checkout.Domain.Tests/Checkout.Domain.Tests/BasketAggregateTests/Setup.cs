using Checkout.Domain.BasketAggregate;

namespace Checkout.Domain.Tests.BasketAggregateTests
{
    internal static class Setup
    {

        public static BasketId BasketId = new BasketId();
        public static User User = new User("Josh Doe", true);

        public static Basket GenerateBasketAggregate()
            => Basket.New(BasketId, User);
    }
}