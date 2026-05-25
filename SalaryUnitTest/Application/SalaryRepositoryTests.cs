using EmployeeSalary.Domain.Entities;
using EmployeeSalary.Infrastructure.Context;
using EmployeeSalary.Infrastucture.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EmployeeSalary.Tests.Repositories;

public class SalaryRepositoryTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Salary>> _mockSet;
    private readonly SalaryRepository _repo;

    public SalaryRepositoryTests()
    {
        _mockSet = new Mock<DbSet<Salary>>();

        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        _mockContext = new Mock<AppDbContext>(options);

        _mockContext.Setup(c => c.Salaries).Returns(_mockSet.Object);

        var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

        _repo = new SalaryRepository(_mockContext.Object, mockConfig.Object);
    }

    [Fact]
    public async Task AddAsync_Should_Call_Add_And_SaveChangesAsync()
    {
        // Arrange
        var salary = new Salary { Id = 1, BaseSalary = 1000 };

        // Act
        await _repo.AddAsync(salary);

        // Assert
        _mockSet.Verify(m => m.AddAsync(salary, default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_When_Exists()
    {
        // Arrange
        var salary = new Salary { Id = 1 };
        _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(salary);

        // Act
        await _repo.DeleteAsync(1);

        // Assert
        _mockSet.Verify(m => m.Remove(salary), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_When_Not_Exists_Should_Not_Delete()
    {
        // Arrange
        _mockSet.Setup(m => m.FindAsync(999)).ReturnsAsync((Salary)null);

        // Act
        await _repo.DeleteAsync(999);

        // Assert
        _mockSet.Verify(m => m.Remove(It.IsAny<Salary>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Update_And_SaveChangesAsync()
    {
        // Arrange
        var salary = new Salary { Id = 1, BaseSalary = 20000 };

        // Act
        await _repo.UpdateAsync(salary);

        // Assert
        _mockSet.Verify(m => m.Update(salary), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}