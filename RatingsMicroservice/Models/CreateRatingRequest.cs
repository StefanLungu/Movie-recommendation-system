namespace RatingsMicroservice.Models
{
    public class CreateRatingRequest
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public float Value { get; set; }
    }

}
