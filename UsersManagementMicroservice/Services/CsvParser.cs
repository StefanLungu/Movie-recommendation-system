using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Web.Helpers;
using UsersManagementMicroservice.Entities;
using UsersManagementMicroservice.Helpers;

namespace UsersManagementMicroservice.Services
{
    public class CsvParser : ICsvParser
    {
        private List<User> Users { set; get; }

        private readonly AppSettings _appSettings;

        public CsvParser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            Users = new List<User>();
            GenerateUsers();
        }

        private void GenerateUsers()
        {
            var parser = new TextFieldParser(_appSettings.RatingsCsvPath)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(new string[] { "," });
            parser.ReadFields();

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();
                int userId = int.Parse(row[0]);

                User user = Users.Find(u => u.Id == userId);
                if (user == null)
                {
                    user = GenerateUser(userId);
                    Users.Add(user);
                }
            }
        }

        private User GenerateUser(int userId)
        {
            return new User { Id = userId, Username = GenerateUserName(userId), Email = GenerateUserEmail(userId), Role = GenerateUserRole(), Password = GenerateUserPassword(userId) };
        }

        private string GenerateUserName(int userId)
        {
            return "UserName" + userId;
        }

        private string GenerateUserEmail(int userId)
        {
            return GenerateUserName(userId) + "@gmail.com";
        }

        private string GenerateUserRole()
        {
            return "user";
        }

        private string GenerateUserPassword(int userId)
        {
            return Crypto.SHA256(GenerateUserName(userId));
        }

        public List<User> GetUsers()
        {
            return Users;
        }
    }
}
