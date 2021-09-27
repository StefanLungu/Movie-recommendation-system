using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesMicroservice.Helpers
{
    public static class HttpContextExtensions
    {
        public static async Task InsertPaginationParameterInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
        {
            double count = await queryable.CountAsync();
            double numberOfPages = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add(Messages.NumberOfPagesHeader, numberOfPages.ToString());
        }
    }
}
