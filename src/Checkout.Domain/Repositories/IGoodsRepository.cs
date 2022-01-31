using Checkout.Domain.Entities;
using NBB.Data.Abstractions;
using System;
using System.Threading.Tasks;

namespace Checkout.Domain.Repositories
{
    public interface IGoodsRepository : ICrudRepository<Good>
    {
        Task<Good> GetGoodById(Guid goodId);        
    }
}