using NBB.Domain;
using System.Text.Json.Serialization;

namespace Checkout.Domain.BasketAggregate
{
    public class Status : Enumeration
    {
        public static readonly Status Opened = new Status(1, "Opened");
        public static readonly Status Closed = new Status(2, "Closed");

        [JsonConstructor]
        public Status(int id, string name)
            : base(id, name)
        {
        }
    }
}