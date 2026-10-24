using AutoFixture.Xunit2;
using ComputerManagement.API;
using ComputerManagement.API.Services;
using ComputerManagement.Shared;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;

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
    public void Add_ValidComputer_CreateComputer(Laptop computer)
    {
        // Arrange
        _fleetContextMock
            .Setup(c => c.Computers.Add(It.IsAny<Computer>()));

        // Act
        _computerService.Add(computer);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Add(computer), Times.Once);
        _fleetContextMock.Verify(c => c.SaveChanges(), Times.Once);
    }

    #endregion

    #region Delete

    [Theory, AutoData]
    public void Delete_ComputerExist_DeleteComputer(Desktop computer)
    {
        // Arrange
        var computers = new List<Computer> { computer };

        _fleetContextMock
            .Setup(c => c.Computers)
            .ReturnsDbSet(computers);

        // Act
        var result = _computerService.Delete(computer.Id);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Remove(computer), Times.Once);
        _fleetContextMock.Verify(c => c.SaveChanges(), Times.Once);
        result.ShouldBeTrue();
    }

    [Theory, AutoData]
    public void Delete_ComputerNotExist_ReturnFalse(Guid id)
    {
        // Arrange
        var emptyComputersList = new List<Computer>();

        _fleetContextMock
            .Setup(c => c.Computers)
            .ReturnsDbSet(emptyComputersList);

        // Act
        var result = _computerService.Delete(id);

        // Assert
        _fleetContextMock.Verify(c => c.Computers.Remove(It.IsAny<Computer>()), Times.Never);
        _fleetContextMock.Verify(c => c.SaveChanges(), Times.Never);
        result.ShouldBeFalse();
    }

    #endregion
}