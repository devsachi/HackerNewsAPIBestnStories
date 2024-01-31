using Microsoft.VisualStudio.TestTools.UnitTesting;
using HackerNewsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using HackerNewsAPI.Models;
using FluentAssertions;

namespace HackerNewsAPI.Services.Tests
{
    [TestClass()]
    public class StoriesServiceTests
    {
        private readonly IFixture _fixture;
        public StoriesServiceTests()
        {
            _fixture = new Fixture()
               .Customize(new AutoMoqCustomization()
               {
                   ConfigureMembers = true
               });
        }

        [TestMethod()]
        public async Task GetBestStoriesAsyncTestAsync()
        {
            // Arrange
            List<Story> stories = _fixture.Build<Story>()
                .WithAutoProperties()
                .CreateMany(5)
                .ToList();
            Mock<IStoriesCache> mockStoriesCache = new();
            mockStoriesCache.Setup(x => x.GetCachedStoriesAsync(5).Result)
                .Returns(stories);

            var mockLogger = new Mock<ILogger<StoriesService>>();
            var storiesService = new StoriesService(mockLogger.Object, mockStoriesCache.Object);
            var bestStories = await storiesService.GetBestStoriesAsync(5);
            bestStories.Should().NotBeNull();
            IEnumerable<Story> bestStoriesFromFixture = stories.OrderByDescending(x => x.Score).Take(5);
            bestStoriesFromFixture.Should().NotBeNull();
            bestStoriesFromFixture.Should().BeEquivalentTo(bestStories);
        }
    }
}