//using AuthorizeNet.Api.Contracts.V1;
//using AuthorizeNet.Api.Controllers;
//using BLL.DTO;
//using BLL.Services.Implementation;
//using DAL.Enums;
//using DAL.Models;
//using DAL.Models.Users;
//using DTO;
//using Microsoft.Extensions.Options;
//using Moq;

//public class AuthorizeNetServiceTests
//{
//    private readonly Mock<IOptions<SettingsDto.AuthorizeNetOptions>> _optionsMock;
//    private readonly AuthorizeNetService _authorizeNetService;

//    public AuthorizeNetServiceTests()
//    {
//        _optionsMock = new Mock<IOptions<SettingsDto.AuthorizeNetOptions>>();
//        _optionsMock.Setup(o => o.Value).Returns(new SettingsDto.AuthorizeNetOptions
//        {
//            ApiLoginID = "7vW2Khp4X",
//            TransactionKey = "77nwsxK34Vt3zG5E"
//        });

//        _authorizeNetService = new AuthorizeNetService(_optionsMock.Object);
//    }

//    [Fact]
//    public async Task CreateSubscriptionAsync_Should_ReturnSubscriptionId()
//    {
//        // Arrange
//        var dto = new PaymentDetailsDto
//        {
//            CardNumber = "4111111111111111",
//            ExpirationDate = "0228",
//            CardCode = "123"
//        };

//        var plan = new SubscriptionPlan
//        {
//            Name = "Test Plan",
//            IntervalType = IntervalType.Monthly,
//            Cost = 9.99m
//        };

//        var user = new User
//        {
//            FirstName = "John",
//            LastName = "Doe"
//        };

//        //var response = new ARBCreateSubscriptionResponse
//        //{
//        //    messages = new messagesType
//        //    {
//        //        resultCode = messageTypeEnum.Ok,
//        //        message = new messagesTypeMessage[] { new messagesTypeMessage{text = "success" } }
//        //    }
//        //};

//        //var controllerMock = new Mock<ARBCreateSubscriptionController>();
//        //controllerMock.Setup(c => c.GetApiResponse()).Returns(response);
//        //controllerMock.Setup(c => c.Execute(AuthorizeNet.Environment.SANDBOX));


//        // Act
//        var subscriptionId = await _authorizeNetService.CreateSubscriptionAsync(dto, plan, user);

//        // Assert
//        Assert.NotNull(subscriptionId);
//    }

//    [Fact]
//    public async Task CancelSubscriptionAsync_Should_NotThrowException()
//    {
//        // Arrange
//        var subscriptionId = "test_subscription_id";

//        var response = new ARBCancelSubscriptionResponse
//        {
//            messages = new messagesType
//            {
//                resultCode = messageTypeEnum.Ok,
//                message = new messagesTypeMessage[] { new messagesTypeMessage { text = "Success" } }
//            }
//        };

//        var controllerMock = new Mock<ARBCancelSubscriptionController>();
//        controllerMock.Setup(c => c.Execute(AuthorizeNet.Environment.SANDBOX));
//        controllerMock.Setup(c => c.GetApiResponse()).Returns(response);

//        // Act & Assert
//        await _authorizeNetService.CancelSubscriptionAsync(subscriptionId);
//    }
//}
