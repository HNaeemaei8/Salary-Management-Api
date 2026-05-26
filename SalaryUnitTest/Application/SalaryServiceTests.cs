using Application.Dto;
using Application.Interfaces;
using Application.Dto;
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
        var request = new SalaryCalculationRequest
        {
            EmployeeFirstName = "Test",
            EmployeeLastName = "User",
            BaseSalary = 5000,
            AttractionAllowance = 500,
            TransportationAllowance = 200,
            OvertimeHours = 3, 
            SalaryDate = DateTime.Now,
            OverTimeCalculatorMethod = "Standard"
        };

        _salaryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Salary>()))
                           .Returns(Task.FromResult(new Salary())); 

        // Act
        var result = await _salaryService.AddSalaryAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.EmployeeFirstName);
        Assert.Equal("User", result.EmployeeLastName);
        Assert.Equal(5000, result.BaseSalary);
        Assert.Equal(500, result.AttractionAllowance);
        Assert.Equal(200, result.TransportationAllowance);
        Assert.Equal(30000, result.CalculatedOvertimePay); 
        Assert.Equal(5000 + 500 + 200 + 30000, result.TotalSalary); 

        _salaryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Salary>()), Times.Once);
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
            TotalSalary = 5700, 
            SalaryDate = DateTime.Now
        };

        var updateData = new SalaryUpdateData
        {
            BasicSalary = 6000,
            Allowance = 600, 
            Transport = 250, 
            OvertimePay = 12000, 
            PaymentDate = DateTime.Now.AddDays(1) 
        };

        _salaryRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                           .ReturnsAsync(existingSalary);

        _salaryRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Salary>()))
                           .Returns(Task.FromResult(existingSalary));

        // Act
        var updatedSalary = await _salaryService.UpdateSalaryAsync(1, updateData);

        // Assert
        Assert.NotNull(updatedSalary);
        Assert.Equal(6000, updatedSalary.BaseSalary);
        Assert.Equal(600, updatedSalary.AttractionAllowance);
        Assert.Equal(250, updatedSalary.TransportationAllowance);
        Assert.Equal(12000, updatedSalary.CalculatedOvertimePay);
        Assert.Equal(updateData.PaymentDate.Value.Date, updatedSalary.SalaryDate.Date);  
        Assert.Equal(6000 + 600 + 250 + 12000, updatedSalary.TotalSalary); 

        _salaryRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Salary>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSalaryAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        _salaryRepositoryMock.Setup(repo => repo.DeleteAsync(1))
                           .Returns(Task.FromResult(true)); 
        // Act
        var result = await _salaryService.DeleteSalaryAsync(1);

        // Assert
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
        Assert.Equal(expectedSalary.Id, actualSalary.Id);
        Assert.Equal(expectedSalary.BaseSalary, actualSalary.BaseSalary);
        _salaryRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }
}