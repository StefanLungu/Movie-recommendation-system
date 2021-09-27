using System.Text.Json.Serialization;
using UsersManagementMicroservice.Data;

namespace UsersManagementMicroservice.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
