using Cinelovers.Core.Caching;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Cinelovers.Core.UnitTests.Services
{
    [TestFixture]
    public class MovieServiceTests
    {
        [Test]
        public void GetUpcomingMovies_PagingInfoIsNull_ReturnsEmpty()
        {
            int expectedPage = 2;
            var expectedLanguage = "en-US";
            MoviePagingInfo pagingInfo = null;

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.FetchUpcomingMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));

            var apiServiceMock = new Mock<ITmdbApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MoviePagingInfo>>>()))
                .Returns<string, Func<IObservable<MoviePagingInfo>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            IEnumerable<Movie> resultData = null;
            Assert.DoesNotThrow(
                () => target
                    .GetUpcomingMovies(expectedPage)
                    .Subscribe(results => resultData = results));

            Assert.IsEmpty(resultData);
        }

        [Test]
        public void GetUpcomingMovies_SpecificPage_InvokeServiceWithCorrectParameters()
        {
            int expectedPage = 2;
            var expectedLanguage = "en-US";
            var pagingInfo = new MoviePagingInfo();

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.FetchUpcomingMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));

            var apiServiceMock = new Mock<ITmdbApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MoviePagingInfo>>>()))
                .Returns<string, Func<IObservable<MoviePagingInfo>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetUpcomingMovies(expectedPage).Subscribe();

            apiServiceMock.Verify(x => x.GetClient(), Times.Once);
            apiClientMock.Verify(
                x => x.FetchUpcomingMovies(
                    It.IsAny<string>(),
                    It.Is<int>(page => page == expectedPage),
                    It.Is<string>(language => language == expectedLanguage)),
                Times.Once);
        }

        [Test]
        public void GetUpcomingMovies_WhenInvoked_UsesSpecifiedCacheKey()
        {
            int expectedPage = 2;
            var pagingInfo = new MoviePagingInfo();

            var apiClientMock = new Mock<ITmdbApiClient>();
            apiClientMock
                .Setup(x => x.FetchUpcomingMovies(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(() => Observable.Return(pagingInfo));

            var apiServiceMock = new Mock<ITmdbApiService>();
            apiServiceMock
                .Setup(x => x.GetClient())
                .Returns(apiClientMock.Object);

            var cacheMock = new Mock<ICache>();
            cacheMock
                .Setup(x => x.GetAndFetchLatest(
                    It.IsAny<string>(),
                    It.IsAny<Func<IObservable<MoviePagingInfo>>>()))
                .Returns<string, Func<IObservable<MoviePagingInfo>>>((arg, targetMethod) => targetMethod());

            var target = new MovieService(
                apiServiceMock.Object,
                cacheMock.Object);

            target.GetUpcomingMovies(expectedPage).Subscribe();

            cacheMock.Verify(
                x => x.GetAndFetchLatest(
                    It.Is<string>(cacheKey => cacheKey == $"upcoming_movies_{expectedPage}"),
                    It.IsAny<Func<IObservable<MoviePagingInfo>>>()),
                Times.Once);
        }
    }
}
