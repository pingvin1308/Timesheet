using Moq;
using NUnit.Framework;
using System;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;
using static Timesheet.BussinessLogic.Services.AuthService;

namespace Timesheet.Tests
{
    class TimesheetServiceTests
    {

        private readonly TimesheetService _service;
        private readonly Mock<ITimesheetRepository> _timesheetRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

        public TimesheetServiceTests()
        {
            _timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _service = new TimesheetService(_timesheetRepositoryMock.Object, _employeeRepositoryMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            UserSessions.Sessions.Clear();
        }

        [Test]
        public void TrackTime_StaffEmployee_ShouldReturnTrue()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = 1,
                Name = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, timeLog.Name);

            //assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_ChiefEmployee_ShouldReturnTrue()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = 1,
                Name = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new ChiefEmployee(expectedLastName, 0m, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, timeLog.Name);

            //assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeTriesAddWrongLastName_ShouldReturnFalse()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = 1,
                Name = Guid.NewGuid().ToString(),
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
        }

        [Test]
        public void TrackTime_StaffEmployeeTrackPreviousTime_ShouldReturnTrue()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-10),
                WorkingHours = 1,
                Name = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new StaffEmployee(expectedLastName, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, timeLog.Name);

            //assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        // WorkingHours = 25, - should return false

        [TestCase(25, "")]
        [TestCase(25, null)]
        [TestCase(25, "TestUser")]
        [TestCase(-1, "")]
        [TestCase(-1, null)]
        [TestCase(-1, "TestUser")]
        [TestCase(4, "")]
        [TestCase(4, null)]
        [TestCase(4, "TestUser")]
        public void TrackTime_ShouldReturnFalse(int hours, string lastName)
        {
            //arrange
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkingHours = hours,
                Name = lastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(lastName))
                .Returns(() => null)
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, timeLog.Name);

            //assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
        }

        [Test]
        public void TrackTime_Freelancer_ShouldReturnTrue()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkingHours = 2,
                Name = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert
            var lowerBoundDate = DateTime.Now.AddDays(-2);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);

            Assert.IsTrue(timeLog.Date > lowerBoundDate);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_Freelancer_ShouldReturnFalse()
        {
            //arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-3),
                WorkingHours = 2,
                Name = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
                .Setup(x => x.Get(expectedLastName))
                .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
                .Verifiable();

            //act
            var result = _service.TrackTime(timeLog, expectedLastName);

            //assert
            Assert.IsFalse(result);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
        }
    }
}
