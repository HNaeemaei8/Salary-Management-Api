using Domain.Entities;
using EmployeeSalary.Infrastucture.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EmployeeSalary.Tests.Repositories;

public class SalaryRepositoryTests
{
    #region Props
    private readonly Mock<AppDbContext> _dbMock;
    private readonly Mock<DbSet<Salary>> _setMock;
    private readonly SalaryRepository _repo;
    #endregion

    #region Constructor
    public SalaryRepositoryTests()
    {
        _setMock = new Mock<DbSet<Salary>>();

        _dbMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        _dbMock.SetupGet(x => x.Salaries).Returns(_setMock.Object);

        var configMock = BuildConfigMock("FakeConnectionString");
        _repo = new SalaryRepository(_dbMock.Object, configMock.Object);
    }
    #endregion

    #region Tests

    [Fact]
    public async Task AddAsync_Should_Call_Ef_Add_And_SaveChanges()
    {
        // Arrange
        var salary = BuildFakeSalary(1);

        // Act
        await _repo.AddAsync(salary);

        // Assert
        _setMock.Verify(x => x.Add(salary), Times.Once);
        _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Ef_Update_And_SaveChanges()
    {
        // Arrange
        var salary = BuildFakeSalary(1);

        // Act
        await _repo.UpdateAsync(salary);

        // Assert
        _setMock.Verify(x => x.Update(salary), Times.Once);
        _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_When_IdExists_Should_Remove_And_Save()
    {
        // Arrange
        var targetId = 10;
        var existingSalary = BuildFakeSalary(targetId);

        _setMock.Setup(x => x.FindAsync(targetId))
                .ReturnsAsync(existingSalary);

        // Act
        await _repo.DeleteAsync(targetId);

        // Assert
        _setMock.Verify(x => x.Remove(existingSalary), Times.Once);
        _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_When_IdDoesNotExist_Should_Never_Call_Remove_Or_Save()
    {
        // Arrange
        var nonExistingId = 999;

        _setMock.Setup(x => x.FindAsync(nonExistingId))
                .ReturnsAsync((Salary?)null);

        // Act
        await _repo.DeleteAsync(nonExistingId);

        // Assert
        _setMock.Verify(x => x.Remove(It.IsAny<Salary>()), Times.Never);
        _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region Helpers

    private static Salary BuildFakeSalary(int id) => new()
    {
        Id = id,
        BaseSalary = 1000,
        EmployeeFirstName = "Test",
        EmployeeLastName = "User",
    };

    private static Mock<IConfiguration> BuildConfigMock(string connectionString)
    {
        var configMock = new Mock<IConfiguration>();

        var connStringsSectionMock = new Mock<IConfigurationSection>();
        connStringsSectionMock.Setup(s => s["DefaultConnection"])
                              .Returns(connectionString);

        configMock.Setup(c => c.GetSection("ConnectionStrings"))
                  .Returns(connStringsSectionMock.Object);

        return configMock;
    }

    #endregion
}