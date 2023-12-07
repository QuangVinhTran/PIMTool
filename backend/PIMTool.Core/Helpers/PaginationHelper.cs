using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PIMTool.Core.Models;

namespace PIMTool.Core.Helpers;

public static class PaginationHelper
{
    public static async Task<PaginatedResult> BuildPaginatedResult<T, TDto>(IMapper mapper, IQueryable<T> source, int pageSize, int pageIndex)
    {
        var total = await source.CountAsync().ConfigureAwait(false);
        if (total == 0)
        {
            return new PaginatedResult()
            {
                PageIndex = 1,
                PageSize = pageSize,
                Data = new List<TDto>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };
        }

        pageSize = Math.Max(1, pageSize);
        var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
        lastPage = lastPage < 1 ? 1 : lastPage;
        pageIndex = pageIndex > lastPage ? lastPage : pageIndex;
        var isLastPage = pageIndex == lastPage;

        if (pageIndex > lastPage / 2)
        {
            var mod = total % pageSize;
            var skip = Math.Max((lastPage - pageIndex - 1) * pageSize + mod, 0);
            var take = isLastPage ? mod : pageSize;
        
            var reverse = source.Reverse();
        
            var res = reverse.Skip(skip).Take(take);
            return new PaginatedResult()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Data = res.Reverse(),
                LastPage = lastPage,
                IsLastPage = isLastPage,
                Total = total
            };
        }
        
        var results = source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        return new PaginatedResult()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Data = results,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total
        };
    }
    
    public static async Task<PaginatedResult> BuildPaginatedResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
    {
        return await BuildPaginatedResult<T, T>(null!, source, pageSize, pageIndex);
    }
}