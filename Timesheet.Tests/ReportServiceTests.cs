using Moq;
using NUnit.Framework;
using System;
using Timesheet.BussinessLogic.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    class ReportServiceTests
    {
        [Test]
        public void GetEmployeeReport_Staff_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            var salary = 70000;
            decimal expectedTotal = 8750; // (8+8+4)/160 * 70000
            var expectedTotalHours = 20m; 

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Chief_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            decimal salary = 70000;
            decimal bonus = 20000;
            decimal expectedTotal = 8750;// (8+8+4)/160 * 70000
            var expectedTotalHours = 20m; 

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new ChiefEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test] 
        public void GetEmployeeReport_Freelancer_ShouldReturnReportPerDaysWithoutOvertime()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Иванов";
            decimal salary = 1000;
            decimal expectedTotal = 20000; // (8+8+4) * 1000 
            var expectedTotalHours = 20m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-2),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Date = DateTime.Now,
                        WorkingHours = 4,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }



        [Test]
        //[TestCase("Иванов", 60000, 106000)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 1000 (где 1000 бонус руководителей за переработку, внезависимости от переработанных часов)
        //[TestCase("Петров", 60000, 105750)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 375 * 1 * 2  (у сотрудников переработанный час в два раза больше оплачивается)
        //[TestCase("Сидоров", 1000, 281000)]// ставка за час = 1000; 1000 * 281
        public void GetEmployeeReport_Staff_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            
            var expectedLastName = "Петров";
            decimal salary = 60000;
            decimal expectedTotal = 105750;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
                        {
                            Name = expectedLastName,
                            Comment = Guid.NewGuid().ToString(),
                            Date = dateTime,
                            WorkingHours = 8
                        };
                    }
                    return timeLogs;
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            //act
            var result = service.GetEmployeeReport(expectedLastName);
            //assert

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        //[TestCase("Иванов", 60000, 106000)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 1000 (где 1000 бонус руководителей за переработку, внезависимости от переработанных часов)
        //[TestCase("Петров", 60000, 105750)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 375 * 1 * 2  (у сотрудников переработанный час в два раза больше оплачивается)
        //[TestCase("Сидоров", 1000, 281000)]// ставка за час = 1000; 1000 * 281
        public void GetEmployeeReport_Chief_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var expectedLastName = "Петров";
            decimal salary = 60000;
            decimal expectedTotal = 106000;
            decimal bonus = 20000;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new ChiefEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
                        {
                            Name = expectedLastName,
                            Comment = Guid.NewGuid().ToString(),
                            Date = dateTime,
                            WorkingHours = 8
                        };
                    }
                    return timeLogs;
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            //act
            var result = service.GetEmployeeReport(expectedLastName);
            //assert

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        //[TestCase("Иванов", 60000, 106000)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 1000 (где 1000 бонус руководителей за переработку, внезависимости от переработанных часов)
        //[TestCase("Петров", 60000, 105750)]// ставка за час = 60000 / 160 = 375; 35 * 8 * 375 + 375 * 1 * 2  (у сотрудников переработанный час в два раза больше оплачивается)
        //[TestCase("Сидоров", 1000, 281000)]// ставка за час = 1000; 1000 * 281
        public void GetEmployeeReport_Freelancer_ShouldReturnReportPerSeveralMonth()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var expectedLastName = "Петров";
            decimal salary = 1000;
            decimal expectedTotal = 281000;
            var expectedTotalHours = 281m;

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(x => x == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
                .Verifiable();

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(x => x == expectedLastName)))
                .Returns(() =>
                {
                    TimeLog[] timeLogs = new TimeLog[35];
                    DateTime dateTime = new DateTime(2020, 11, 1);
                    timeLogs[0] = new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = dateTime,
                        WorkingHours = 9
                    };
                    for (int i = 1; i < timeLogs.Length; i++)
                    {
                        dateTime = dateTime.AddDays(1);
                        timeLogs[i] = new TimeLog
                        {
                            Name = expectedLastName,
                            Comment = Guid.NewGuid().ToString(),
                            Date = dateTime,
                            WorkingHours = 8
                        };
                    }
                    return timeLogs;
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);
            //act
            var result = service.GetEmployeeReport(expectedLastName);
            //assert

            Assert.IsNotNull(result);

            Assert.AreEqual(expectedLastName, result.LastName);
            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        [TestCase("Иванов")]
        [TestCase("Петров")]
        public void GetEmployeeReport_Staff_WithoutTimeLogs(string expectedLastName)
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            decimal salary = 70000;
            var expectedTotal = 0m;
            var expectedTotalHours = 0;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        [TestCase("Иванов")]
        [TestCase("Петров")]
        public void GetEmployeeReport_Chief_WithoutTimeLogs(string expectedLastName)
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            decimal salary = 70000;
            decimal bonus = 20000;
            var expectedTotal = 0m;
            var expectedTotalHours = 0;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new ChiefEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        [TestCase("Иванов")]
        [TestCase("Петров")]
        public void GetEmployeeReport_Freelancer_WithoutTimeLogs(string expectedLastName)
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotal = 0m;
            var expectedTotalHours = 0;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, 70000))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        //[TestCase("Иванов", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Петров", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Сидоров", 1000, 8000)]// 8m * 1000 
        public void GetEmployeeReport_Staff_TimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            decimal salary = 70000;
            decimal expectedTotal = 3500;
            var expectedTotalHours = 8m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        //[TestCase("Иванов", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Петров", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Сидоров", 1000, 8000)]// 8m * 1000 
        public void GetEmployeeReport_Chief_TimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            decimal salary = 70000;
            decimal expectedTotal = 3500;
            decimal bonus = 20000;
            var expectedTotalHours = 8m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new ChiefEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        //[TestCase("Иванов", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Петров", 70000, 3500)]// 8m / 160m * 70000m
        //[TestCase("Сидоров", 1000, 8000)]// 8m * 1000 
        public void GetEmployeeReport_Freelancer_TimeLogsForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            decimal salary = 1000;
            decimal expectedTotal = 8000;
            var expectedTotalHours = 8m;

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Staff_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;

            var expectedLastName = "Петров";
            var salary = 70000; 
            var expectedTotal = 7000; // (8m / 160m * 70000m) + (4m / 160m * 70000m * 2)

            timesheetRepositoryMock
                    .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Freelancer_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;

            var expectedLastName = "Сидоров";
            var salary = 1000;
            var expectedTotal = 12000; // 12m * 1000 = 12000

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new FreelancerEmployee(expectedLastName, salary))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_Manager_ShouldReturnTimeLogWithOvertimeForOneDay()
        {
            //arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedTotalHours = 12;

            var expectedLastName = "Иванов";
            var salary = 70000;
            var bonus = 20000m;
            var expectedTotal = 4500; // 8m / 160m * 70000m + 8m / 100m * 20000m (у руководителей бонус 1000 за день вне зависимости от переаботанных часов)

            timesheetRepositoryMock
                .Setup(x => x.GetTimeLogs(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        Name = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkingHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.Get(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new ChiefEmployee(expectedLastName, salary, bonus))
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);

            //act
            var result = service.GetEmployeeReport(expectedLastName);

            //assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHours, result.TotalHours);
        }
    }
}
