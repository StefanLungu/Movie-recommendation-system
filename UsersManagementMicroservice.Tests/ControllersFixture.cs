using Microsoft.EntityFrameworkCore;
using UsersManagementMicroservice.Data;
using UsersManagementMicroservice.Entities;

namespace UsersManagementMicroservice.Tests
{
    public class ControllersFixture
    {
        public DataContext DataContext { get; private set; }

        private void SeedUsers()
        {
            DataContext.Users.Add(new User { Id = 1, Username = "UserName1", Password = "UserName1", Email = "username1@gmail.com" });
            DataContext.Users.Add(new User { Id = 2, Username = "UserName2", Password = "UserName2", Email = "username2@gmail.com" });
        }

        public ControllersFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "MoviesDatabase")
                    .Options;

            DataContext = new DataContext(options);
            SeedUsers();
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
