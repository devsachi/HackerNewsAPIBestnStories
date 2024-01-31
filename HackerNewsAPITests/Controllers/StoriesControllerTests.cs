using AutoFixture;
using AutoFixture.AutoMoq;
using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;
using HackerNewsAPI.Services;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HackerNewsAPITests.Controllers
{
    [TestClass]
    public class StoriesControllerTests
    {
        private readonly IFixture _fixture;

        public StoriesControllerTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization()
                {
                    ConfigureMembers = true
                });
        }

        [TestMethod]
        public async Task GetSameStoryCountsAsync()
        {
            // Arrange
            List<Story> stories = _fixture.Build<Story>()
                .WithAutoProperties()
                .CreateMany(10)
                .ToList();
            Mock<IStoriesCache> mockStoriesCache = new();
            mockStoriesCache.Setup(x => x.GetCachedStoriesAsync(10).Result)
                .Returns(stories);

            var mockLogger = new Mock<ILogger<StoriesService>>();
            var mockStories = new StoriesService(mockLogger.Object, mockStoriesCache.Object);
            mockStoriesCache.Setup(x => x.GetCachedStoriesAsync(10).Result)
                .Returns(stories);
            var mockLogger1 = new Mock<ILogger<StoriesController>>();
            var controller = new StoriesController(mockLogger1.Object, mockStories);

            // Act
            var actionResult = await controller.GetBestStories(10);
            var contentResult = actionResult as OkObjectResult;
            Assert.IsNotNull(contentResult);
            var response = contentResult.Value as List<Story>;
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(10, response.Count);
        }
    }
}
