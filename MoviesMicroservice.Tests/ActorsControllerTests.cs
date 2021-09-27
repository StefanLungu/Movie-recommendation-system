using Microsoft.AspNetCore.Mvc;
using MoviesMicroservice.Controllers;
using MoviesMicroservice.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoviesMicroservice.Tests
{
    public class ActorsControllerTests : IClassFixture<ControllersFixture>
    {
        private readonly ControllersFixture _controllersFixture;

        public ActorsControllerTests(ControllersFixture controllersFixture)
        {
            _controllersFixture = controllersFixture;
        }

        private ActorsController Create_SystemUnderTest()
        {
            return new ActorsController(_controllersFixture.DataContext);
        }


        [Fact]
        public async Task Test1_GetActors_ShouldReturnAListWith_4_Actors_When_4_ActorsAreInDatabase()
        {
            var _actorsController = Create_SystemUnderTest();

            // Act
            ActionResult<IEnumerable<Actor>> actionResult = await _actorsController.GetActors();

            // Assert
            Assert.True(actionResult.Value.Count() == 4);
        }


        [Fact]
        public async Task Test2_GetActorWithId_1_ShouldReturnAnActor_WhenActorWithId_1_IsInDatabase()
        {

            var _actorsController = Create_SystemUnderTest();

            // Arrange
            int actorId = 1;

            // Act
            ActionResult<Actor> actionResult = await _actorsController.GetActor(actorId);

            // Assert
            Assert.True(actionResult.Value.Id == actorId);
        }


        [Fact]
        public async Task Test3_DeleteActorWithId_2_ShouldReturnAListWith_3_Actors_After_Delete_AnActorFromDatabase()
        {
            var _actorsController = Create_SystemUnderTest();

            // Arrange
            int actorId = 2;

            // Act
            await _actorsController.DeleteActor(actorId);
            ActionResult<IEnumerable<Actor>> actionResult = await _actorsController.GetActors();

            // Assert
            Assert.True(actionResult.Value.Count() == 3);
        }


        [Fact]
        public void Test4_PutActorWithId_2_ShouldReturnBadRequestWhenAnActorWithADifferentIdIsSpecified()
        {
            var _actorsController = Create_SystemUnderTest();

            // Arrange
            int actorId = 2;
            Actor actor = new Actor
            {
                Id = 1,
                Name = "Name"
            };

            // Act
            Task<IActionResult> actionResult = _actorsController.PutActor(actorId, actor);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }


        [Fact]
        public void Test5_AddActorWithId_1_ToMovieWithId_10_ShouldReturnNotFoundObjectResultWhenMovieWithId_10_IsNotInDatabase()
        {
            var _actorsController = Create_SystemUnderTest();

            // Arrange
            int actorId = 1;
            int movieId = 10;

            // Act
            Task<IActionResult> actionResult = _actorsController.AddActorToMovie(actorId, movieId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }


        [Fact]
        public void Test6_AddActorWithId_10_ToMovieWithId_1_ShouldReturnNotFoundObjectResultWhenActorWithId_1_IsNotInDatabase()
        {
            var _actorsController = Create_SystemUnderTest();

            // Arrange
            int actorId = 10;
            int movieId = 1;

            // Act
            Task<IActionResult> actionResult = _actorsController.AddActorToMovie(actorId, movieId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
    }
}
