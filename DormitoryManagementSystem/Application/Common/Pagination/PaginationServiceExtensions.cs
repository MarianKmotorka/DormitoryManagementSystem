using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;

namespace Application.Common.Pagination
{
    public static class PaginationServiceExtensions
    {
        private static readonly int _maxPageSize = 100;
        private static readonly int _defaultPageSize = 20;

        public static async Task<PagedResponse<T>> GetPagedAsync<T>(this IPaginationService paginationService, IQueryable<T> query, SieveModel paginationModel = null) where T : class
        {
            var result = new PagedResponse<T>();

            var (pagedQuery, page, pageSize, rowCount, pageCount) = await GetPagedResultAsync(paginationService, query, paginationModel);

            result.PageNumber = page;
            result.PageSize = pageSize;
            result.TotalRecords = rowCount;
            result.Pages = pageCount;

            result.Data = await pagedQuery.ToListAsync();

            return result;
        }


        private static async Task<(IQueryable<T> pagedQuery, int page, int pageSize, int rowCount, int pageCount)> GetPagedResultAsync<T>(IPaginationService paginationService, IQueryable<T> query, SieveModel paginationModel = null) where T : class
        {
            var page = paginationModel?.Page ?? 1;
            var pageSize = paginationModel?.PageSize ?? _defaultPageSize;

            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            if (pageSize < 1)
                pageSize = 1;

            if (page < 1)
                page = 1;

            if (paginationModel != null)
                query = paginationService.Apply(paginationModel, query, applyPagination: false);

            var rowCount = await query.CountAsync();

            var pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

            var skip = (page - 1) * pageSize;

            var pagedQuery = query.Skip(skip).Take(pageSize);

            return (pagedQuery, page, pageSize, rowCount, pageCount);
        }
    }
}
