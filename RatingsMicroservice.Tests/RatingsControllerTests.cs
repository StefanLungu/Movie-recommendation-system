using Microsoft.AspNetCore.Mvc;
using RatingsMicroservice.Controllers;
using RatingsMicroservice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RatingsMicroservice.Tests
{
    public class RatingsControllerTests : IClassFixture<ControllersFixture>
    {
        private readonly ControllersFixture _controllersFixture;

        public RatingsControllerTests(ControllersFixture controllersFixture)
        {
            _controllersFixture = controllersFixture;
        }

        public RatingsController Create_SystemUnderTest()
        {
            return new RatingsController(_controllersFixture.DataContext);
        }


        [Fact]
        public async System.Threading.Tasks.Task Test1_GetRatings_ShouldReturnAListWith_6_Ratings_When_6_RatingsAreInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Act
            ActionResult<IEnumerable<Rating>> actionResult = await _ratingsController.GetRatings();

            // Assert
            Assert.True(actionResult.Value.Count() == 6);
        }


        [Fact]
        public void Test2_GetRatingWithId_1_ShouldReturnARating_WhenRatingWithId_1_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 1;

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.Id == ratingId);
        }


        [Fact]
        public void Test3_GetRatingValueWithId_2_ShouldReturnValueRating_WhenRatingWithId_2_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 2;
            float ratingValue = 4.5F;

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.Value == ratingValue);
        }


        [Fact]
        public void Test4_GetMovieIdForRatingWithId_3_ShouldReturnAMovieId_WhenRatingWithId_3_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 3;
            int movieId = 1;

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.MovieId == movieId);
        }


        [Fact]
        public void Test5_GetUserIdForRatingWithId_4_ShouldReturnAnUserId_WhenRatingWithId_4_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 4;
            int userId = 2;

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.UserId == userId);
        }


        [Fact]
        public void Test6_GetDateCreatedForRatingWithId_1_ShouldReturnDateCreated_WhenRatingWithId_1_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 1;
            DateTime differentTimeFromCreatedTime = DateTime.Now;

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.False(actionResult.Value.DateCreated == differentTimeFromCreatedTime);
        }


        [Fact]
        public void Test7_GetDateCreatedForRatingWithId_2_ShouldReturnDateCreated_WhenRatingWithId_2_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 2;
            DateTime dateCreated = new DateTime(2020, 12, 30, 4, 15, 10);

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.DateCreated == dateCreated);
        }


        [Fact]
        public void Test8_GetDateUpdatedForRatingWithId_3_ShouldReturnDateUpdated_WhenRatingWithId_3_IsInDatabase()
        {
            var _ratingsController = Create_SystemUnderTest();

            // Arrange
            int ratingId = 3;
            DateTime dateUpdated = new DateTime(2020, 12, 31, 4, 15, 10);

            // Act
            ActionResult<Rating> actionResult = _ratingsController.GetRating(ratingId);

            // Assert
            Assert.True(actionResult.Value.DateUpdated.Day == dateUpdated.Day);
        }
    }
}