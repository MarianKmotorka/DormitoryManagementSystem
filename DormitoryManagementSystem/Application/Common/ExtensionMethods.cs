using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common
{
    public static class ExtensionMethods
    {
        public static async Task<TEntity> SingleOrNotFoundAsync<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            var entity = await query.SingleOrDefaultAsync(predicate, cancellationToken);

            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} was not found.");

            return entity;
        }
    }
}
