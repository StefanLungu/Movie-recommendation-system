using MoviesMicroservice.Data;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MoviesMicroservice.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public long ReleaseYear { get; set; }
        public long TmdbId { get; set; }

        public ICollection<MovieGenre> Genres { get; private set; } = new List<MovieGenre>();

        [JsonIgnore]
        public ICollection<Actor> Actors { get; private set; } = new List<Actor>();
    }
}
