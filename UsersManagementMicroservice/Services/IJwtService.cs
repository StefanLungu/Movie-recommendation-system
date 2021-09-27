using UsersManagementMicroservice.Entities;

namespace UsersManagementMicroservice.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
