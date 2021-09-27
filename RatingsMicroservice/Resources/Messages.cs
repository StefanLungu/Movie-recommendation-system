namespace RatingsMicroservice.Resources
{
    public static class Messages
    {
        public const string InvalidRatingMessage = "Invalid rating value. Should be between 1-5.";
        public const string MovieHasNoRatings = "There are no ratings for this movie.";
        public const string Origins = "_myAllowSpecificOrigins";
        public const string Database = "ratings.db";
        public const string BaseUrl = "http://localhost:5000";
        public const string GetUsersUrl = "users";
        public const string GetMoviesUrl = "movies";

        public static string DuplicateRatingForMovie(int userId, int movieId)
        {
            return $"User with id {userId} already has a rating for movie with id {movieId}.";
        }

        public static string UserHasNoRatingForMovie(int userId, int movieId)
        {
            return $"User with id {userId} does not have a rating for the movie with id {movieId}.";
        }

        public static string NotFoundMessage(string entityName, int entityId)
        {
            return $"{entityName} with id {entityId} not found.";
        }
    }
}
