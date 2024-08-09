using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using BLL.DTO;
using BLL.Services.Interfaces;
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

        public async Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto)
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

            var subscription = new ARBSubscriptionType
            {
                name = "Monthly Subscription",
                paymentSchedule = new paymentScheduleType
                {
                    interval = new paymentScheduleTypeInterval
                    {
                        length = d,
                        unit = (ARBSubscriptionUnitEnum)Enum.Parse(typeof(ARBSubscriptionUnitEnum), billingUnit)
                    },
                    startDate = DateTime.UtcNow, 
                    totalOccurrences = 9999 
                },
                amount = amount,
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
