using AutoFixture.Xunit2;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using ComputerManagement.API;
using ComputerManagement.API.Services;
using ComputerManagement.Shared.Computers;
using ComputerManagement.Shared.Results;

namespace ComputerManagement.Tests;

public class ComputerServiceTests
{
    private readonly Mock<FleetContext> _fleetContextMock;
    private readonly ComputerService _computerService;

    public ComputerServiceTests()
    {
        _fleetContextMock = new Mock<FleetContext>();
        _computerService = new ComputerService(_fleetContextMock.Object);
    }

    #region Add

    [Theory, AutoData]
    public async Task AddAsync_ValidComputer_CreateComputer(Laptop computer)
    {
        // Arrange
        _fleetContextMock
            .Setup(c => c.Computers.Add(It.IsAny<Computer>()));

        // Act
        var result = await _computerService.AddAsync(computer);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Add(computer), Times.Once);
        _fleetContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(computer);
    }

    #endregion

    #region Delete

    [Theory, AutoData]
    public async Task DeleteAsync_ComputerExist_DeleteComputer(Desktop computer)
    {
        // Arrange
        var computers = new List<Computer> { computer };

        _fleetContextMock
            .Setup(c => c.Computers)
            .ReturnsDbSet(computers);

        // Act
        var result = await _computerService.DeleteAsync(computer.Id);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Remove(computer), Times.Once);
        _fleetContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
    }

    [Theory, AutoData]
    public async Task DeleteAsync_ComputerNotExist_ReturnFailure(Guid id)
    {
        // Arrange
        var emptyComputersList = new List<Computer>();

        _fleetContextMock
            .Setup(c => c.Computers)
            .ReturnsDbSet(emptyComputersList);

        // Act
        var result = await _computerService.DeleteAsync(id);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Remove(It.IsAny<Computer>()), Times.Never);
        _fleetContextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(ComputerErrors.CouldNotFoundComputerById);
    }

    #endregion
}