using System.Collections.Generic;
using System.Linq;
using UsersManagementMicroservice.Entities;
using UsersManagementMicroservice.Services;

namespace UsersManagementMicroservice.Data
{
    public class Seeder
    {
        private readonly DataContext _context;
        private readonly ICsvParser _parser;
        public Seeder(DataContext context, ICsvParser parser)
        {
            _context = context;
            _parser = parser;
        }

        public void SeedUsers()
        {
            List<User> users = _parser.GetUsers();
            foreach (User user in users)
            {
                if (!_context.Users.Any(u => u.Id == user.Id))
                {
                    _context.Users.Add(user);
                }
            }
            _context.SaveChanges();
        }
    }
}
