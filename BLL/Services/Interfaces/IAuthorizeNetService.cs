namespace BLL.Services.Interfaces
{
    public interface IAuthorizeNetService
    {
        Task<string> ProcessPaymentAsync(decimal amount, string cardNumber, string expirationDate, string cardCode);
    }
}
