using Microsoft.AspNetCore.Mvc;
using MoviesMicroservice.Controllers;
using MoviesMicroservice.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoviesMicroservice.Tests
{
    public class MovieGenresControllerTests : IClassFixture<ControllersFixture>
    {
        private readonly ControllersFixture _controllersFixture;

        public MovieGenresControllerTests(ControllersFixture controllersFixture)
        {
            _controllersFixture = controllersFixture;
        }

        public MovieGenresController Create_SystemUnderTest()
        {
            return new MovieGenresController(_controllersFixture.DataContext);
        }


        [Fact]
        public async System.Threading.Tasks.Task Test1_GetMovieGenres_ShouldReturnAListWith_5_MovieGenres_When_5_MovieGenresAreInDatabase()
        {
            var _movieGenresController = Create_SystemUnderTest();

            // Act
            ActionResult<IEnumerable<MovieGenre>> actionResult = await _movieGenresController.GetAll();

            // Assert
            Assert.True(actionResult.Value.Count() == 5);
        }


        [Fact]
        public async void Test2_GetMovieGenreWithId_1_ShouldReturnAGenreMovie_WhenMovieWithId_1_IsInDatabase()
        {
            var _movieGenresController = Create_SystemUnderTest();

            // Arrange
            int genreMovieId = 1;
            string genreName = "comedy";

            // Act
            ActionResult<MovieGenre> actionResult = await _movieGenresController.GetGenreById(genreMovieId);

            // Assert
            Assert.True(actionResult.Value.Name == genreName);
        }


        [Fact]
        public void Test3_PutMovieGenreWithId_2_ShouldReturnBadRequestWhenAMovieGenreWithADifferentIdIsSpecified()
        {
            var _movieGenresController = Create_SystemUnderTest();

            // Arrange
            int movieGenreId = 2;
            MovieGenre movieGenre = new MovieGenre
            {
                Id = 1,
                Name = "Name",
            };

            // Act
            Task<IActionResult> actionResult = _movieGenresController.Update(movieGenreId, movieGenre);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }
    }
}
