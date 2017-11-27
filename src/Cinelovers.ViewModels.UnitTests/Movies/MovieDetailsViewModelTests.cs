using Cinelovers.Core.Services.Models;
using Cinelovers.ViewModels.Movies;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System;

namespace Cinelovers.ViewModels.UnitTests.Movies
{
    [TestFixture]
    public class MovieDetailsViewModelTests
    {
        private TestScheduler _testScheduler;
        private Mock<IScreen> _screenMock;

        [SetUp]
        public void Setup()
        {
            ModeDetector.InUnitTestRunner();

            _testScheduler = new TestScheduler();
            RxApp.MainThreadScheduler = _testScheduler;
            RxApp.TaskpoolScheduler = _testScheduler;

            var routingState = new RoutingState();
            _screenMock = new Mock<IScreen>();
            _screenMock
                .SetupGet(x => x.Router)
                .Returns(() => routingState);
        }

        [Test]
        public void MovieDetailsViewModel_MovieIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MovieDetailsViewModel(null));
        }

        [Test]
        public void UrlPathSegment_WhenVmIsCreated_IsSetToTheRightText()
        {
            var movie = new MovieCellViewModel(new Movie() { Id = 1, Title = "Title" });

            var target = new MovieDetailsViewModel(
                movie,
                _screenMock.Object,
                _testScheduler,
                _testScheduler);

            Assert.AreEqual("Movie Details", target.UrlPathSegment);
        }

        [Test]
        public void Movie_VmIsInstantiated_SetsMovie()
        {
            var movie = new MovieCellViewModel(new Movie() { Id = 1, Title = "Title" });

            var target = new MovieDetailsViewModel(
                movie,
                _screenMock.Object,
                _testScheduler,
                _testScheduler);

            Assert.AreEqual(movie, target.Movie);
        }
    }
}
