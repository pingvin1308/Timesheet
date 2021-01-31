using Moq;
using NUnit.Framework;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    
    class EmployeeServiceTest
    {

        [Test]
        [TestCase("Иванов", 20000)]
        [TestCase("Петров", 30000)]
        [TestCase("Сидоров", 40000)]
        public void Add_ShouldReturnTrue(string lastName, int salary)
        {
            //arrange
            var staffEmployee = new StaffEmployee(lastName, salary);

            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.Add(staffEmployee)).Verifiable();

            var service = new EmployeeService(employeeRepository.Object);

            //act
            var result = service.Add(staffEmployee);

            //assert
            employeeRepository.Verify(x => x.Add(staffEmployee), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Иванов", 0)]
        [TestCase("Иванов", -1000)]
        [TestCase("", 30000)]
        [TestCase(null, 40000)]
        public void Add_ShouldReturnFalse(string lastName, int salary)
        {
            //arrange
            var staffEmployee = new StaffEmployee(lastName, salary);

            var employeeRepository = new Mock<IEmployeeRepository>();

            var service = new EmployeeService(employeeRepository.Object);

            //act
            var result = service.Add(staffEmployee);

            //assert
            employeeRepository.Verify(x => x.Add(It.IsAny<StaffEmployee>()), Times.Never);
            Assert.IsFalse(result);
        }
    }
}
