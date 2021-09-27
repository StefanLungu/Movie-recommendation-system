using MoviesMicroservice.Data;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MoviesMicroservice.Entities
{
    public class Actor : BaseEntity
    {
        public string Name { get; set; }
        public long Age { get; set; }

        [JsonIgnore]
        public ICollection<Movie> Movies { get; private set; } = new List<Movie>();
    }
}
