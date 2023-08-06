namespace Application.Features.SystemConfiguration.Queries.GetSMSConfiguration;

public class SMSConfiguration
{
    public string BaseURL { get; set; }
    public string BaseURLBulkMessages { get; set; }
    public string SenderId { get; set; }
    public string AccName { get; set; }
    public string AccPassword { get; set; }
}
