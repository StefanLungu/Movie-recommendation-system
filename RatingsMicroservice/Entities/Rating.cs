using RatingsMicroservice.Data;
using System;

namespace RatingsMicroservice.Entities
{
    public class Rating : BaseEntity
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public float Value { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
