using System;

namespace Checkout.Domain.Entities
{
    public class Good
    {
        public Guid GoodId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}