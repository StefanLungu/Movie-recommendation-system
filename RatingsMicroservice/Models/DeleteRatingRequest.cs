namespace RatingsMicroservice.Models
{

    public class DeleteRatingRequest
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
