namespace Application.Features.SystemConfiguration.Queries.GetSMTPConfiguration;

public class SMTPConfiguration
{
    public string SmtpServer { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public int SmtpPort { get; set; }
    public bool SmtpTLS { get; set; }
    public string SmtpFromAddress { get; set; }
}
