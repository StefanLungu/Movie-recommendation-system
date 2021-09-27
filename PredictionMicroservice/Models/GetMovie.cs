using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PredictionMicroservice.Models
{
    public class GetMovie : IEquatable<GetMovie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long ReleaseYear { get; set; }
        public ICollection<GetMovieGenre> Genres { get; set; } = new List<GetMovieGenre>();

        public bool Equals([AllowNull] GetMovie other)
        {
            return Id == other.Id;
        }
    }
}
