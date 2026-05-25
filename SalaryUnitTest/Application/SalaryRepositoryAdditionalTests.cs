using EmployeeSalary.Domain.Entities;
using EmployeeSalary.Infrastructure.Context;
using EmployeeSalary.Infrastucture.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EmployeeSalary.Tests.Repositories;

public class SalaryRepositoryAdditionalTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Salary>> _mockSet;
    private readonly SalaryRepository _repo;

    public SalaryRepositoryAdditionalTests()
    {
        _mockSet = new Mock<DbSet<Salary>>();

        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        _mockContext = new Mock<AppDbContext>(options);

        _mockContext.Setup(c => c.Salaries).Returns(_mockSet.Object);

        var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        _repo = new SalaryRepository(_mockContext.Object, mockConfig.Object);
    }

    [Fact]
    public async Task DeleteAsync_WhenSalaryDoesNotExist_ShouldNotThrow()
    {
        // Arrange
        _mockSet.Setup(x => x.FindAsync(999))
            .ReturnsAsync((Salary)null);

        // Act
        var act = async () => await _repo.DeleteAsync(999);

        // Assert
        await act.Should().NotThrowAsync();
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task AddAsync_ShouldCallAddAsyncForEachSalary()
    {
        // Arrange
        var salaries = new[]
        {
            new Salary { Id = 1, BaseSalary = 1000, EmployeeFirstName = "Ali" },
            new Salary { Id = 2, BaseSalary = 2000, EmployeeFirstName = "Reza" },
            new Salary { Id = 3, BaseSalary = 3000, EmployeeFirstName = "Sara" }
        };

        // Act
        foreach (var salary in salaries)
        {
            await _repo.AddAsync(salary);
        }

        // Assert
        _mockSet.Verify(x => x.AddAsync(It.IsAny<Salary>(), default), Times.Exactly(3));
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Exactly(3));
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallUpdateAndSaveChanges()
    {
        // Arrange
        var salary = new Salary
        {
            Id = 1,
            BaseSalary = 5000
        };

        // Act
        salary.BaseSalary = 7777;
        await _repo.UpdateAsync(salary);

        // Assert
        _mockSet.Verify(x => x.Update(salary), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}