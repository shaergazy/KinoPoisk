//using System;
//using System.Threading.Tasks;
//using BLL.Services.Implementation;
//using BLL.Services.Interfaces;
//using Common.Helpers;
//using DAL.Models.Users;
//using DTO;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using Moq;
//using Xunit;
//using FluentAssertions;
//using System.Net.Sockets;

//namespace BLL.Tests.Services
//{
//    public class EmailServiceTests
//    {
//        private readonly Mock<IOptions<SettingsDto.Mail>> _emailSettingsMock;
//        private readonly Mock<ILogger<EmailService>> _loggerMock;
//        private readonly EmailService _emailService;
//        private readonly Mock<SmtpClient> _smtpClientMock;

//        public EmailServiceTests()
//        {
//            _emailSettingsMock = new Mock<IOptions<SettingsDto.Mail>>();
//            _emailSettingsMock.Setup(x => x.Value).Returns(new SettingsDto.Mail
//            {
//                DisplayName = "Test Display Name",
//                UserName = "cordelia.dare@ethereal.email",
//                Password = "FGKVPe3wWeqC8sa8me",
//                SmtpServer = "smtp.ethereal.email",
//                Port = 587
//            });

//            _loggerMock = new Mock<ILogger<EmailService>>();
//            _emailService = new EmailService(_emailSettingsMock.Object, _loggerMock.Object);
//            _smtpClientMock = new Mock<SmtpClient>();
//        }

//        [Fact]
//        public async Task SendWelcomeEmailAsync_Should_SendEmail_WithCorrectSubjectAndBody()
//        {
//            // Arrange
//            var user = new User { UserName = "TestUser", Email = "testuser@example.com" };
//            var expectedSubject = ResourceHelper.GetString("WelcomeEmailSubject");
//            var expectedBody = string.Format(ResourceHelper.GetString("WelcomeEmailBody"), user.UserName, "Kinopoiskweb");

//            // Act
//            await _emailService.SendWelcomeEmailAsync(user);

//            // Assert
//            _smtpClientMock.Verify(client => client.ConnectAsync(
//                It.Is<string>(s => s == _emailSettingsMock.Object.Value.SmtpServer),
//                It.Is<int>(port => port == _emailSettingsMock.Object.Value.Port),
//                It.Is<SecureSocketOptions>(options => options == SecureSocketOptions.StartTls)),
//                Times.Once);

//            _smtpClientMock.Verify(client => client.AuthenticateAsync(
//                It.Is<string>(s => s == _emailSettingsMock.Object.Value.UserName),
//                It.Is<string>(p => p == _emailSettingsMock.Object.Value.Password)),
//                Times.Once);

//            _smtpClientMock.Verify(client => client.SendAsync(It.Is<MimeMessage>(m =>
//                m.From.Mailboxes.Any(mb => mb.Address == _emailSettingsMock.Object.Value.UserName) &&
//                m.To.Mailboxes.Any(mb => mb.Address == user.Email) &&
//                m.Subject == expectedSubject &&
//                m.Body.ToString().Contains(user.UserName))),
//                Times.Once);

//            _smtpClientMock.Verify(client => client.DisconnectAsync(It.Is<bool>(quit => quit)),
//                Times.Once);
//        }


//        [Fact]
//        public async Task SendEmailAsync_Should_SendEmail()
//        {
//            var toEmail = "receiver@example.com";
//            var subject = "Test Subject";
//            var body = "<h1>Test Body</h1>";

//            Func<Task> act = async () => await _emailService.SendEmailAsync(toEmail, subject, body);

//            await act.Should().NotThrowAsync<Exception>();
//        }

//        [Fact]
//        public async Task SendEmailAsync_Should_LogError_OnException()
//        {
//            // Arrange
//            var toEmail = "receiver@example.com";
//            var subject = "Test Subject";
//            var body = "<h1>Test Body</h1>";

//            // Simulate an exception in the SMTP client
//            var clientMock = new Mock<SmtpClient>();
//            clientMock.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>())).ThrowsAsync(new Exception("SMTP Error"));

//            // Act
//            Func<Task> act = async () => await _emailService.SendEmailAsync(toEmail, subject, body);

//            // Assert
//            await act.Should().ThrowAsync<Exception>().WithMessage("SMTP Error");
//            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
//        }
//    }
//}
