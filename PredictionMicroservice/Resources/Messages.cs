namespace PredictionMicroservice.Resources
{
    public static class Messages
    {
        public const string Origins = "_myAllowSpecificOrigins";
        public const string BaseUrl = "http://localhost:5000";
        public const string GetUsersUrl = "users";
        public const string GetMoviesUrl = "movies";
        public const string GetRatingsUrl = "ratings";
        public const string EntitiesPerPage = "entitiesPerPage=1000";

        public static string NotFoundMessage(string entityName, int entityId)
        {
            return $"{entityName} with id {entityId} not found.";
        }
    }
}
