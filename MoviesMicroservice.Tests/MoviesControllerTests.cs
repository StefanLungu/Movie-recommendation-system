using Microsoft.AspNetCore.Mvc;
using MoviesMicroservice.Controllers;
using MoviesMicroservice.Entities;
using System.Threading.Tasks;
using Xunit;

namespace MoviesMicroservice.Tests
{
    public class MoviesControllerTests : IClassFixture<ControllersFixture>
    {
        private readonly ControllersFixture _controllersFixture;

        public MoviesControllerTests(ControllersFixture controllersFixture)
        {
            _controllersFixture = controllersFixture;
        }

        public MoviesController Create_SystemUnderTest()
        {
            return new MoviesController(_controllersFixture.DataContext);
        }


        [Fact]
        public void Test1_GetMovieWithId_1_ShouldReturnAMovie_WhenMovieWithId_1_IsInDatabase()
        {
            var _moviesController = Create_SystemUnderTest();

            // Arrange
            int movieId = 1;

            // Act
            ActionResult<Movie> actionResult = _moviesController.GetMovie(movieId);

            // Assert
            Assert.True(actionResult.Value.Id == movieId);
        }


        [Fact]
        public void Test2_GetMovieWithId_3_ShouldReturnMovieTitle_WhenMovieWithId_3_IsInDatabase()
        {
            var _moviesController = Create_SystemUnderTest();

            // Arrange
            int movieId = 3;
            string movieTitle = "Titanic";

            // Act
            ActionResult<Movie> actionResult = _moviesController.GetMovie(movieId);

            // Assert
            Assert.True(actionResult.Value.Title == movieTitle);
        }


        [Fact]
        public void Test3_PutMovieWithId_2_ShouldReturnBadRequestWhenAMovieWithADifferentIdIsSpecified()
        {
            var _moviesController = Create_SystemUnderTest();

            // Arrange
            int movieId = 2;
            Movie movie = new Movie
            {
                Id = 1,
                Title = "Title",
            };

            // Act
            Task<IActionResult> actionResult = _moviesController.UpdateMovie(movieId, movie);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }
    }
}
