using Cinelovers.ViewModels.Movies;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;

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
        public void UrlPathSegment_WhenVmIsCreated_IsSetToTheRightText()
        {
            var target = new MovieDetailsViewModel(
                _screenMock.Object,
                _testScheduler,
                _testScheduler);

            Assert.AreEqual("Movie Details", target.UrlPathSegment);
        }
    }
}
