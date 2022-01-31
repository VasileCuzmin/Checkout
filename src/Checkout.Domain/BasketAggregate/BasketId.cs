using NBB.Domain;
using Newtonsoft.Json;
using System;

namespace Checkout.Domain.BasketAggregate
{
    public record BasketId : Identity<Guid>
    {
        [JsonConstructor]
        public BasketId(Guid value)
            : base(value)
        {
        }

        public BasketId()
            : this(Guid.NewGuid())
        {
        }

        public override string ToString() => Value.ToString();
    }
}