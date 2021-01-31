using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class IssuesServiceTests
    {
        [Test]
        public void Get_ShouldReturnIssues()
        {
            // arrange
            var lastName = Guid.NewGuid().ToString();
            var expectedLogin = Guid.NewGuid().ToString();

            var employeeRepository = new Mock<IEmployeeRepository>();

            var expectedEmployee = new StaffEmployee(lastName, 20000);

            employeeRepository
                .Setup(x => x.Get(lastName))
                .Returns(expectedEmployee)
                .Verifiable();

            var issuesClientMock = new Mock<IIssuesClient>();
            var expectedIssue = new Issue
            {
                Id = 124,
                Name = "sdgds",
                SourceId = 144
            };

            issuesClientMock
                .Setup(x => x.Get(expectedLogin))
                .ReturnsAsync(new[] { expectedIssue })
                .Verifiable();

            var service = new IssuesService(employeeRepository.Object, issuesClientMock.Object);

            // act
            var issues = service.Get();

            // assert
            issuesClientMock.VerifyAll();
            employeeRepository.VerifyAll();
            Assert.IsNotNull(issues);
            Assert.IsNotEmpty(issues);
        }
    }
}
