using Application.Dto;
using Application.Interfaces;
using EmployeeSalary.Application.Implementations;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace EmployeeSalary.Tests;

public class SalaryServiceTests
{
    private readonly Mock<ISalaryRepository> _salaryRepositoryMock;
    private readonly Mock<IDataParserFactory> _parserFactoryMock;
    private readonly SalaryService _salaryService;
    private readonly decimal _fixedHourlyRate = 10000;

    public SalaryServiceTests()
    {
        _salaryRepositoryMock = new Mock<ISalaryRepository>();
        _parserFactoryMock = new Mock<IDataParserFactory>();

        _salaryService = new SalaryService(
            _salaryRepositoryMock.Object,
            _parserFactoryMock.Object,
            _fixedHourlyRate);
    }

    [Fact]
    public async Task AddSalaryAsync_ShouldCreateSalaryAndCallRepository()
    {
        // Arrange
        var fixedDate = new DateTime(2025, 01, 01);

        var request = new SalaryCalculationRequest
        {
            EmployeeFirstName = "Test",
            EmployeeLastName = "User",
            BaseSalary = 5000,
            AttractionAllowance = 500,
            TransportationAllowance = 200,
            OvertimeHours = 3,
            SalaryDate = fixedDate,
            OverTimeCalculatorMethod = "Standard"
        };

        _salaryRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Salary>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _salaryService.AddSalaryAsync(request);

        // Assert - خروجی
        Assert.NotNull(result);
        Assert.Equal("Test", result.EmployeeFirstName);
        Assert.Equal("User", result.EmployeeLastName);
        Assert.Equal(5000, result.BaseSalary);
        Assert.Equal(500, result.AttractionAllowance);
        Assert.Equal(200, result.TransportationAllowance);
        Assert.Equal(30000, result.CalculatedOvertimePay);
        Assert.Equal(5000 + 500 + 200 + 30000, result.TotalSalary);

        // Assert - رفتار
        _salaryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Salary>(s =>
            s.EmployeeFirstName == "Test" &&
            s.EmployeeLastName == "User" &&
            s.BaseSalary == 5000 &&
            s.AttractionAllowance == 500 &&
            s.TransportationAllowance == 200 &&
            s.CalculatedOvertimePay == 30000 &&
            s.TotalSalary == (5000 + 500 + 200 + 30000)
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateSalaryAsync_ShouldUpdateSalaryAndCallRepository()
    {
        // Arrange
        var existingSalary = new Salary
        {
            Id = 1,
            BaseSalary = 5000,
            AttractionAllowance = 500,
            TransportationAllowance = 200,
            CalculatedOvertimePay = 10000,
            TotalSalary = 15700,
            SalaryDate = new DateTime(2025, 01, 01)
        };

        var paymentDate = new DateTime(2025, 01, 02);

        var updateData = new SalaryUpdateData
        {
            BasicSalary = 6000,
            Allowance = 600,
            Transport = 250,
            OvertimePay = 12000,
            PaymentDate = paymentDate
        };

        _salaryRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                             .ReturnsAsync(existingSalary);

        _salaryRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Salary>()))
                             .Returns(Task.CompletedTask);

        // Act
        var updatedSalary = await _salaryService.UpdateSalaryAsync(1, updateData);

        // Assert - خروجی
        Assert.NotNull(updatedSalary);
        Assert.Equal(6000, updatedSalary.BaseSalary);
        Assert.Equal(600, updatedSalary.AttractionAllowance);
        Assert.Equal(250, updatedSalary.TransportationAllowance);
        Assert.Equal(12000, updatedSalary.CalculatedOvertimePay);
        Assert.Equal(paymentDate.Date, updatedSalary.SalaryDate.Date);
        Assert.Equal(6000 + 600 + 250 + 12000, updatedSalary.TotalSalary);

        // Assert - رفتار
        _salaryRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);

        _salaryRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Salary>(s =>
            s.Id == 1 &&
            s.BaseSalary == 6000 &&
            s.AttractionAllowance == 600 &&
            s.TransportationAllowance == 250 &&
            s.CalculatedOvertimePay == 12000 &&
            s.TotalSalary == (6000 + 600 + 250 + 12000) &&
            s.SalaryDate.Date == paymentDate.Date
        )), Times.Once);
    }

    [Fact]
    public async Task DeleteSalaryAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        _salaryRepositoryMock.Setup(repo => repo.DeleteAsync(1))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _salaryService.DeleteSalaryAsync(1);

        // Assert
        // اگر سرویس شما bool برمی‌گرداند:
        Assert.True(result);

        _salaryRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetSalaryByIdAsync_ShouldCallRepositoryGetById()
    {
        // Arrange
        var expectedSalary = new Salary { Id = 1, BaseSalary = 1000 };

        _salaryRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                             .ReturnsAsync(expectedSalary);

        // Act
        var actualSalary = await _salaryService.GetSalaryByIdAsync(1);

        // Assert
        Assert.NotNull(actualSalary);
        Assert.Equal(expectedSalary.Id, actualSalary.Id);
        Assert.Equal(expectedSalary.BaseSalary, actualSalary.BaseSalary);

        _salaryRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }
}