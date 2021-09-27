namespace PredictionMicroservice.Models
{
    public class GetRating
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public float Value { get; set; }
    }
}
