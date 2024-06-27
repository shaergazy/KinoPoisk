using System.Collections.Generic;

namespace DTO;

public static class SettingsDto
{
    public sealed record Cors
    {
        public IEnumerable<string> Origins { get; init; }
    }

    public class VirtualDir
    {
        public string BaseDir { get; set; }

        public string BaseSuffixUri { get; set; }
    }

    public class Mail
    {
        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }
    }

    public sealed record ServiceUri
    {
        public Self Self { get; init; }
    }

    public sealed record Self
    {
        public string BaseUri { get; init; }
    }
}
