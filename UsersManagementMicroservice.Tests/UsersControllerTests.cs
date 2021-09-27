using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersManagementMicroservice.Controllers;
using UsersManagementMicroservice.Entities;
using UsersManagementMicroservice.Models;
using Xunit;

namespace UsersManagementMicroservice.Tests
{
    public class UsersControllerTests : IClassFixture<ControllersFixture>
    {
        private readonly ControllersFixture _controllersFixture;

        public UsersControllerTests(ControllersFixture controllersFixture)
        {
            _controllersFixture = controllersFixture;
        }

        private UsersController Create_SystemUnderTest()
        {
            return new UsersController(_controllersFixture.DataContext, null);
        }


        [Fact]
        public async Task Test1_GetUsers_ShouldReturnAListWith_2_Users_When_2_UsersAreInDatabase()
        {
            var _usersController = Create_SystemUnderTest();

            // Act
            ActionResult<IEnumerable<User>> actionResult = await _usersController.GetUsers();

            // Assert
            Assert.True(actionResult.Value.Count() == 2);
        }


        [Fact]
        public async Task Test2_GetUserName_ShouldReturnUsernameForId_1_WhenUserWithId_1_IsInDatabase()
        {
            var _usersController = Create_SystemUnderTest();

            // Arrange
            int userId = 1;
            string userName = "UserName1";

            // Act
            ActionResult<User> actionResult = await _usersController.GetUser(userId);

            // Assert
            Assert.True(actionResult.Value.Username == userName);
        }


        [Fact]
        public async Task Test3_DeleteUser_ShouldReturnNoContent_WhenUserWithId_1_IsInDatabase()
        {
            var _usersController = Create_SystemUnderTest();

            // Arrange
            int userId = 1;

            // Act
            ActionResult<User> actionResult = await _usersController.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(actionResult.Result);
        }


        [Fact]
        public async Task Test4_RegisterUser_ShouldReturnCreatedAtActionResult_WhenAnUserWithIsRegistered()
        {
            var _usersController = Create_SystemUnderTest();

            // Arrange
            RegisterRequest registerRequest = new RegisterRequest
                {
                Email = "newuser@gmail.com",
                Username = "NewUser",
                Password = "NewUser123"
            };

            // Act
            ActionResult<User> actionResult = await _usersController.Register(registerRequest);

            // Assert
            Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        }


        [Fact]
        public void Test5_AuthenticateUser_ShouldReturnBadRequestObjectResult_WhenAnUserWithInvalidCredentialsIsAuthenticated()
        {
            var _usersController = Create_SystemUnderTest();

            // Arrange
            AuthenticateRequest authenticateRequest = new AuthenticateRequest
            {
                Username = "UserName1",
                Password = "UserName1"
            };

            // Act
            Task<IActionResult> actionResult = _usersController.Authenticate(authenticateRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
    }
}