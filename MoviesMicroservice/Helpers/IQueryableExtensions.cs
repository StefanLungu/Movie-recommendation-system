using MoviesMicroservice.Models;
using System.Linq;

namespace MoviesMicroservice.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable.Skip((pagination.Page - 1) * pagination.EntitiesPerPage).Take(pagination.EntitiesPerPage);
        }
    }
}
