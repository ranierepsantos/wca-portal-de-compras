namespace wca.share.infrastructure.Integration.Email
{
    public sealed class EmailConfiguration
    {
        public string From { get; init; }
        public string SmtpServer { get; init; }
        public int Port { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }

    }
}
