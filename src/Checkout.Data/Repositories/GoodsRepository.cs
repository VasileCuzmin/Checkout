using Checkout.Domain.Entities;
using Checkout.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBB.Core.Abstractions;
using NBB.Data.EntityFramework;
using NBB.Data.EntityFramework.Internal;
using System;
using System.Threading.Tasks;

namespace Checkout.Data.Repositories
{
    public class GoodsRepository : EfCrudRepository<Good, GoodsDbContext>, IGoodsRepository
    {
        private readonly GoodsDbContext _dbContext;

        public GoodsRepository(GoodsDbContext dbContext, IExpressionBuilder expressionBuilder, IUow<Good> uow,
            ILogger<EfCrudRepository<Good, GoodsDbContext>> logger) : base(dbContext, expressionBuilder, uow, logger)
        {
            _dbContext = dbContext;
        }

        public async Task<Good> GetGoodById(Guid goodId)
            => await _dbContext.Goods.SingleOrDefaultAsync(g => g.GoodId == goodId);
    }
}