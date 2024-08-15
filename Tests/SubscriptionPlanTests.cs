using AutoMapper;
using BLL.DTO.SubscriptionPlan;
using BLL.Services.Implementation;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

public class SubscriptionPlanServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork<SubscriptionPlan, int>> _uowMock;
    private readonly Mock<ILogger<SubscriptionPlanService>> _loggerMock;
    private readonly SubscriptionPlanService _service;

    public SubscriptionPlanServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _uowMock = new Mock<IUnitOfWork<SubscriptionPlan, int>>();
        _loggerMock = new Mock<ILogger<SubscriptionPlanService>>();
        _service = new SubscriptionPlanService(_mapperMock.Object, _uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task BuildEntityForCreate_Should_CreateSubscriptionPlan()
    {
        // Arrange
        var dto = new AddSubscriptionPlanDto { Name = "Basic Plan" };
        var subscriptionPlan = new SubscriptionPlan { Name = dto.Name };

        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>()))
            .Returns(false);
        _mapperMock.Setup(m => m.Map<SubscriptionPlan>(dto)).Returns(subscriptionPlan);

        // Act
        var result = await _service.BuildEntityForCreate(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task BuildEntityForCreate_Should_ThrowArgumentNullException_When_NameIsNull()
    {
        // Arrange
        var dto = new AddSubscriptionPlanDto { Name = null };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.BuildEntityForCreate(dto));
        Assert.Equal("You have to complete all properties", exception.Message);
    }

    [Fact]
    public async Task BuildEntityForCreate_Should_ThrowException_When_SubscriptionPlanAlreadyExists()
    {
        // Arrange
        var dto = new AddSubscriptionPlanDto { Name = "Existing Plan" };

        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>()))
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.BuildEntityForCreate(dto));
        Assert.Equal("SubscriptionPlan already exist", exception.Message);
    }

    [Fact]
    public async Task BuildEntityForUpdate_Should_UpdateSubscriptionPlan()
    {
        // Arrange
        var dto = new EditSubscriptionPlanDto { Id = 1, Name = "Updated Plan" };
        var subscriptionPlan = new SubscriptionPlan { Id = dto.Id, Name = dto.Name };

        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>()))
            .Returns(false);
        _mapperMock.Setup(m => m.Map<SubscriptionPlan>(dto)).Returns(subscriptionPlan);

        // Act
        var result = await _service.BuildEntityForUpdate(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task BuildEntityForUpdate_Should_ThrowArgumentNullException_When_NameIsNull()
    {
        // Arrange
        var dto = new EditSubscriptionPlanDto { Id = 1, Name = null };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.BuildEntityForUpdate(dto));
        Assert.Equal("Value can not be null", exception.Message);
    }

    [Fact]
    public async Task BuildEntityForUpdate_Should_ThrowException_When_SubscriptionPlanAlreadyExists()
    {
        // Arrange
        var dto = new EditSubscriptionPlanDto { Id = 1, Name = "Existing Plan" };

        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>()))
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.BuildEntityForUpdate(dto));
        Assert.Equal("SubscriptionPlan already exist", exception.Message);
    }
}
