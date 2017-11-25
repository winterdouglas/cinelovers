﻿using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class UpcomingMoviesViewModelTests
    {
        private TestScheduler _testScheduler;
        private Mock<IScreen> _screenMock;

        [SetUp]
        public void Setup()
        {
            ModeDetector.InUnitTestRunner();

            _testScheduler = new TestScheduler();

            _screenMock = new Mock<IScreen>();
            _screenMock
                .Setup(x => x.Router)
                .Returns(() => new RoutingState());
        }

        [Test]
        public void GetUpcomingMovies_HasMovies_SetsMovies()
        {
            int expectedPage = 2;
            var movies = new List<Movie>()
            {
                new Movie() { Id = 1, Title = "Movie 1" },
                new Movie() { Id = 2, Title = "Movie 2" }
            };

            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock
                .Setup(x => x.GetUpcomingMovies(
                    It.IsAny<int>()))
                .Returns(() => Observable.Return(movies));

            var target = new UpcomingMoviesViewModel(
                movieServiceMock.Object,
                _testScheduler,
                _testScheduler,
                _screenMock.Object);

            target.Activator.Activate();

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            Observable.Return(expectedPage).InvokeCommand(target.GetUpcomingMovies);

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(500).Ticks);

            movieServiceMock
                .Verify(x => x.GetUpcomingMovies(
                    It.Is<int>(page => page == expectedPage)),
                    Times.Once);

            Assert.AreEqual(movies.Count, target.Movies.Count);
        }
    }
}