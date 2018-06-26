using Moq;
using NUnit.Framework;
using ReactiveUI;
using Splat;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels.UnitTests
{
    [TestFixture]
    public class ViewModelBaseUnitTests
    {
        private Mock<IScheduler> _taskPoolSchedulerMock;
        private Mock<IScheduler> _mainSchedulerMock;
        private Mock<IScreen> _screenMock;

        [SetUp]
        public void Setup()
        {
            ModeDetector.InUnitTestRunner();

            _taskPoolSchedulerMock = new Mock<IScheduler>();
            _mainSchedulerMock = new Mock<IScheduler>();
            _screenMock = new Mock<IScreen>();

            Locator.CurrentMutable.RegisterConstant(_taskPoolSchedulerMock.Object, "TaskPoolScheduler");
            Locator.CurrentMutable.RegisterConstant(_mainSchedulerMock.Object, "MainScheduler");
            Locator.CurrentMutable.RegisterConstant(_screenMock.Object);
        }

        [Test]
        public void HostScreen_HasValue_ReturnsSame()
        {
            var screenMock = new Mock<IScreen>();
            var mock = new Mock<ViewModelBase>(screenMock.Object, null, null);

            var vm = mock.Object;

            Assert.AreSame(screenMock.Object, vm.HostScreen);
        }

        [Test]
        public void HostScreen_ValueIsNull_RegisteredInstance()
        {
            var mock = new Mock<ViewModelBase>(null, null, null);

            var vm = mock.Object;

            Assert.AreSame(_screenMock.Object, vm.HostScreen);
        }

        [Test]
        public void TaskPoolScheduler_HasValue_ReturnsSame()
        {
            var schedulerMock = new Mock<IScheduler>();
            var mock = new Mock<ViewModelBase>(null, null, schedulerMock.Object);

            var vm = mock.Object;

            Assert.AreSame(schedulerMock.Object, vm.TaskPoolScheduler);
        }

        [Test]
        public void TaskPoolScheduler_ValueIsNull_RegisteredInstance()
        {
            var mock = new Mock<ViewModelBase>(null, null, null);

            var vm = mock.Object;

            Assert.AreSame(_taskPoolSchedulerMock.Object, vm.TaskPoolScheduler);
        }

        [Test]
        public void MainScheduler_HasValue_ReturnsSame()
        {
            var schedulerMock = new Mock<IScheduler>();
            var mock = new Mock<ViewModelBase>(null, schedulerMock.Object, null);

            var vm = mock.Object;

            Assert.AreSame(schedulerMock.Object, vm.MainScheduler);
        }

        [Test]
        public void MainScheduler_ValueIsNull_ReturnsRegisteredInstance()
        {
            var mock = new Mock<ViewModelBase>(null, null, null);

            var vm = mock.Object;

            Assert.AreSame(_mainSchedulerMock.Object, vm.MainScheduler);
        }
    }
}
