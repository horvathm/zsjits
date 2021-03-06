using LockerManagement;
using LockerManagement.Interfaces;
using LockerServices;
using LockerServices.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LockerManagementTests
{
    public class LockerManagerTests
    {
        private ILoggerFactory loggerFactory;

        public LockerManagerTests()
        {
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
        }

        [Fact]
        [Trait("Method", "TurnEcoModeOn")]
        public void TurnEcoModeOn_WithAddedSubscriber_ShouldSendNotification()
        {
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();
            lockerSystemManagerMock
                .Setup(x => x.SwitchEcoOn())
                .Returns(Task.FromResult(new List<LockerState>()
                {
                    new LockerState() { LockerGuid = Guid.NewGuid(), RunsInEco = true},
                    new LockerState() { LockerGuid = Guid.NewGuid(), RunsInEco = true}
                }.AsEnumerable()));

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object, loggerFactory);

            var emailServiceAdapterMock = new Mock<IEmailServiceAdapter>();
            emailServiceAdapterMock
                .Setup(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()))
                .Returns(Task.CompletedTask);

            lockerManager.AttachSubscriber(emailServiceAdapterMock.Object);

            // Act
            lockerManager.TurnEcoModeOn();

            // Assert
            emailServiceAdapterMock.Verify(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()), Times.Once);
        }

        #region AttachSubscriberTests
        [Fact]
        [Trait("Method", "AttachSubscriber")]
        public void AttachSubscriber_WithNewSubscriber_ShouldReturnTrue()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            var emailServiceAdapterMock = new Mock<IEmailServiceAdapter>();

            // Act
            bool retval = lockerManager.AttachSubscriber(emailServiceAdapterMock.Object);

            // Assert
            Assert.True(retval);
        }

        [Fact]
        [Trait("Method", "AttachSubscriber")]
        public void AttachSubscriber_WhenSubscriberAlreadyAdded_ShouldReturnFalse()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            var emailServiceAdapterMock = new Mock<IEmailServiceAdapter>();
            
            lockerManager.AttachSubscriber(emailServiceAdapterMock.Object);

            // Act
            bool retval = lockerManager.AttachSubscriber(emailServiceAdapterMock.Object);

            // Assert
            Assert.False(retval);
        }

        [Fact]
        [Trait("Method", "AttachSubscriber")]
        public void AttachSubscriber_WithNotValidInputParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            // Act
            Action act = () => { lockerManager.AttachSubscriber(null); };

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }
        #endregion

        #region DetachSubscriberTests
        [Fact]
        [Trait("Method", "DetachSubscriber")]
        public void DetachSubscriber_WithExisting_ShouldReturnTrue()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            var emailServiceAdapterMock = new Mock<IEmailServiceAdapter>();

            lockerManager.AttachSubscriber(emailServiceAdapterMock.Object);

            // Act
            bool retval = lockerManager.DetachSubscriber(emailServiceAdapterMock.Object);

            // Assert
            Assert.True(retval);
        }

        [Fact]
        [Trait("Method", "DetachSubscriber")]
        public void DetachSubscriber_WhenSubscriberNotExists_ShouldReturnFalse()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            var emailServiceAdapterMock = new Mock<IEmailServiceAdapter>();

            // Act
            bool retval = lockerManager.DetachSubscriber(emailServiceAdapterMock.Object);

            // Assert
            Assert.False(retval);
        }

        [Fact]
        [Trait("Method", "DetachSubscriber")]
        public void DetachSubscriber_WithNullInputParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object,
                loggerFactory);

            // Act
            Action act = () => { lockerManager.DetachSubscriber(null); };

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }
        #endregion

        [Fact]
        [Trait("Method", "ActivateSubscriber")]
        public void DeactivateSubscriber_WithOneOfTwoSubscribersActive_ShouldSendNotification()
        {
            var lockerSystemManagerMock = new Mock<ILockerSystemManager>();
            lockerSystemManagerMock
                .Setup(x => x.SwitchEcoOn())
                .Returns(Task.FromResult(new List<LockerState>()
                {
                    new LockerState() { LockerGuid = Guid.NewGuid(), RunsInEco = true},
                    new LockerState() { LockerGuid = Guid.NewGuid(), RunsInEco = true}
                }.AsEnumerable()));

            ILockerManager lockerManager = new LockerManager(
                lockerSystemManagerMock.Object, loggerFactory);

            var emailServiceAdapterMock1 = new Mock<IEmailServiceAdapter>();
            emailServiceAdapterMock1
                .Setup(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()))
                .Returns(Task.CompletedTask);

            var emailServiceAdapterMock2 = new Mock<IEmailServiceAdapter>();
            emailServiceAdapterMock2
                .Setup(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()))
                .Returns(Task.CompletedTask);

            lockerManager.AttachSubscriber(emailServiceAdapterMock1.Object);

            // Act
            lockerManager.DeactivateSubscriber(emailServiceAdapterMock1.Object);

            // Assert
            lockerManager.AttachSubscriber(emailServiceAdapterMock2.Object);
            lockerManager.TurnEcoModeOn();
            emailServiceAdapterMock1.Verify(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()), Times.Never);
            emailServiceAdapterMock2.Verify(x => x.SendNotification(It.IsAny<IEnumerable<LockerState>>()), Times.Once);
        }
    }
}
