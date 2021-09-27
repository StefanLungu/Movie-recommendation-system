namespace UsersManagementMicroservice.Resources
{
    public static class Messages
    {
        public const string InvalidCredentials = "Invalid credentials.";
        public const string DuplicateUsernameOrEmail = "A user with the same username or email already exists.";
        public const string Origins = "_myAllowSpecificOrigins";
        public const string Database = "users.db";

        public static string NotFoundMessage(string entityName, int entityId)
        {
            return $"{entityName} with id {entityId} not found.";
        }
    }
}
