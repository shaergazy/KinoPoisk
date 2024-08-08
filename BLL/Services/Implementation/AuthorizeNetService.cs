using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
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
        }

        public async Task<string> ProcessPaymentAsync(decimal amount, string cardNumber, string expirationDate, string cardCode)
        {
            var merchantAuthentication = new merchantAuthenticationType
            {
                name = _options.ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _options.TransactionKey
            };

            var creditCard = new creditCardType
            {
                cardNumber = cardNumber,
                expirationDate = expirationDate,
                cardCode = cardCode
            };

            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = amount,
                payment = paymentType
            };

            var request = new createTransactionRequest
            {
                transactionRequest = transactionRequest,
                merchantAuthentication = merchantAuthentication
            };

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                return response.transactionResponse.transId;
            }

            throw new Exception("Payment failed: " + response.messages.message[0].text);
        }
    }
}
