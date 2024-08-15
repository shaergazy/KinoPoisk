using AutoMapper;
using BLL.DTO;
using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Models.Users;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

public class SubscriptionServiceTests
{
    private readonly Mock<IUnitOfWork<Subscription, Guid>> _uowMock;
    private readonly Mock<IAuthorizeNetService> _authorizeNetServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork<Subscription, Guid>>();
        _authorizeNetServiceMock = new Mock<IAuthorizeNetService>();
        _mapperMock = new Mock<IMapper>();
        _subscriptionService = new SubscriptionService(_uowMock.Object, _authorizeNetServiceMock.Object, _mapperMock.Object);

        var subscriptions = new List<Subscription>
        {
            new Subscription { UserId = "user1", SubscriptionId = "sub1", IsActive = true },
            new Subscription { UserId = "user2", SubscriptionId = "sub2", IsActive = false }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Subscription>>();
        mockSet.As<IQueryable<Subscription>>().Setup(m => m.Provider).Returns(subscriptions.Provider);
        mockSet.As<IQueryable<Subscription>>().Setup(m => m.Expression).Returns(subscriptions.Expression);
        mockSet.As<IQueryable<Subscription>>().Setup(m => m.ElementType).Returns(subscriptions.ElementType);
        mockSet.As<IQueryable<Subscription>>().Setup(m => m.GetEnumerator()).Returns(subscriptions.GetEnumerator());

    }

    [Fact]
    public async Task CreateSubscriptionAsync_Should_CreateSubscription()
    {
        var dto = new PaymentDetailsDto { UserId = "user1", PlanId = 1 };
        var user = new User { Id = "user1" };
        var plan = new SubscriptionPlan { Id = dto.PlanId };
        var subscriptionId = Guid.NewGuid().ToString();

        _uowMock.Setup(u => u.Users.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _uowMock.Setup(u => u.SubscriptionPlans.FirstOrDefaultAsync(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>()))
            .ReturnsAsync(plan);
        _authorizeNetServiceMock.Setup(a => a.CreateSubscriptionAsync(dto, plan, user))
            .ReturnsAsync(subscriptionId);
        _uowMock.Setup(u => u.Subscriptions.AddAsync(It.IsAny<Subscription>()))
            .Returns((Subscription subscription) => Task.FromResult(subscription));
        _uowMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _subscriptionService.CreateSubscriptionAsync(dto);

        Assert.Equal(subscriptionId, result);
        _uowMock.Verify(u => u.Subscriptions.AddAsync(It.Is<Subscription>(s => s.UserId == dto.UserId && s.SubscriptionId == subscriptionId)), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CancelSubscriptionAsync_Should_CancelSubscription()
    {
        var subscriptionId = "sub123";
        var subscription = new Subscription { SubscriptionId = subscriptionId, IsActive = true };

        _uowMock.Setup(u => u.Subscriptions.FirstOrDefaultAsync(It.IsAny<Expression<Func<Subscription, bool>>>()))
            .ReturnsAsync(subscription);
        _authorizeNetServiceMock.Setup(a => a.CancelSubscriptionAsync(subscriptionId))
            .Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        await _subscriptionService.CancelSubscriptionAsync(subscriptionId);

        Assert.False(subscription.IsActive);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
