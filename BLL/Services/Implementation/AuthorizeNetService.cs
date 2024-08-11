using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Users;
using Microsoft.Extensions.Options;
using static DTO.SettingsDto;

namespace BLL.Services.Implementation
{
    public class AuthorizeNetService : IAuthorizeNetService
    {
        private readonly AuthorizeNetOptions _options;

        public AuthorizeNetService(IOptions<AuthorizeNetOptions> options)
        {
            _options = options.Value;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
        }

        public async Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto, SubscriptionPlan plan, User user)
        {
            var merchantAuthentication = new merchantAuthenticationType
            {
                name = _options.ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _options.TransactionKey
            };

            var creditCard = new creditCardType
            {
                cardNumber = dto.CardNumber,
                expirationDate = dto.ExpirationDate,
                cardCode = dto.CardCode,
            };

            var paymentType = new paymentType { Item = creditCard };

            var interval = new paymentScheduleTypeInterval();
            switch (plan.IntervalType)
            {
                case IntervalType.Monthly:
                    interval.length = 1;
                    interval.unit = ARBSubscriptionUnitEnum.months;
                    break;
                case IntervalType.Yearly:
                    interval.length = 12;
                    interval.unit = ARBSubscriptionUnitEnum.months;
                    break;
                default:
                    throw new ArgumentException("Invalid interval type");
            }

            var subscription = new ARBSubscriptionType
            {
                name = plan.Name,
                paymentSchedule = new paymentScheduleType
                {
                    interval = interval,
                    startDate = DateTime.UtcNow,
                    totalOccurrences = 9999
                },
                amount = plan.Cost,
                payment = paymentType,
                billTo = new nameAndAddressType
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                }
            };

            var request = new ARBCreateSubscriptionRequest
            {
                merchantAuthentication = merchantAuthentication,
                subscription = subscription
            };

            var controller = new ARBCreateSubscriptionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                return response.subscriptionId;
            }

            throw new Exception("Subscription creation failed: " + response.messages.message[0].text);
        }


        public async Task CancelSubscriptionAsync(string subscriptionId)
        {
            var merchantAuthentication = new merchantAuthenticationType
            {
                name = _options.ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _options.TransactionKey
            };

            var request = new ARBCancelSubscriptionRequest
            {
                merchantAuthentication = merchantAuthentication,
                subscriptionId = subscriptionId
            };

            var controller = new ARBCancelSubscriptionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                return;
            }

            throw new Exception("Subscription cancellation failed: " + response.messages.message[0].text);
        }
    }
}
