namespace MoviesMicroservice.Resources
{
    public static class Messages
    {
        public const string Origins = "_myAllowSpecificOrigins";
        public const string Database = "movies.db";
        public const int NumberOfActors = 300;
        public const int NumberOfMoviesPerActor = 10;
        public const int MinActorAge = 10;
        public const int MaxActorAge = 70;
        public const int EntitiesPerPage = 10;
        public const string NumberOfPagesHeader = "numberOfPages";
        public const string BaseUrl = "http://localhost:5000";
        public const string GetUsersUrl = "users";

        public static string NotFoundMessage(string entityName, int entityId)
        {
            return $"{entityName} with id {entityId} not found.";
        }
    }
}
