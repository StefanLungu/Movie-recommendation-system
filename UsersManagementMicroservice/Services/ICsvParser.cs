using System.Collections.Generic;
using UsersManagementMicroservice.Entities;

namespace UsersManagementMicroservice.Services
{
    public interface ICsvParser
    {
        List<User> GetUsers();
    }
}