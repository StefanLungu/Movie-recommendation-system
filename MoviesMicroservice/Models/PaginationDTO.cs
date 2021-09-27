using MoviesMicroservice.Resources;

namespace MoviesMicroservice.Models
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int EntitiesPerPage { get; set; } = Messages.EntitiesPerPage;
        public string Title { get; set; }
        public int IdGenre { get; set; } = 0;
        public string SortByColumnName { get; set; }
        public string SortDirection { get; set; }
    }
}
