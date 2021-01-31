using Moq;
using NUnit.Framework;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;
using static Timesheet.BussinessLogic.Services.AuthService;

namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Иванов")]
        [TestCase("Петров")]
        [TestCase("Сидоров")]
        public void Login_ShouldReturnTrue(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.
                Setup(x => x.Get(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);
            //act

            var result = service.Login(lastName);

            //assert
            employeeRepositoryMock.VerifyAll();

            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.IsNotNull(UserSessions.Sessions);
            Assert.IsNotEmpty(UserSessions.Sessions);
            Assert.IsTrue(UserSessions.Sessions.Contains(lastName));
        }

        public void Login_InvokeLoginTwiceForOneLastName_ShouldReturnTrue()
        {
            //arrange
            string lastName = "Иванов";
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.
                Setup(x => x.Get(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act

            var result = service.Login(lastName);
            result = service.Login(lastName);

            //assert
            employeeRepositoryMock.VerifyAll();

            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.IsNotNull(UserSessions.Sessions);
            Assert.IsNotEmpty(UserSessions.Sessions);
            Assert.IsTrue(UserSessions.Sessions.Contains(lastName));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Login_NotValidArgument_ShouldReturnFalse(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(lastName);

            //assert
            employeeRepositoryMock.Verify(x => x.Get(lastName), Times.Never);

            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.IsEmpty(UserSessions.Sessions);
            Assert.IsTrue(UserSessions.Sessions.Contains(lastName) == false);
        }

        [TestCase("TestUser")]
        public void Login_UserDoesntExist_ShouldReturnFalse(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.
                Setup(x => x.Get(It.Is<string>(y => y == lastName)))

                .Returns(() => null);

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.Verify(x => x.Get(lastName), Times.Once);

            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.IsTrue(UserSessions.Sessions.Contains(lastName) == false);
        }

        [Test]
        public void Test()
        {
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var service = new AuthService(employeeRepositoryMock.Object);

            var employee = new ChiefEmployee("Иванов", 0, 0);

            var token = AuthService.GenerateToken("secret secret secret secret secret", employee);
        }
    }
}