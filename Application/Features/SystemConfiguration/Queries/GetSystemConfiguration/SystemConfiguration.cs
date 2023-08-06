namespace Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;

public class SystemConfiguration
{
    public string DefaultFilesPath { set; get; }
    public bool EmailNotificationEnabled { set; get; }
    public int LicenseExpiryNotificationPeriodInDays { set; get; }
    public int OTPExpiryPeriodInMinute { set; get; }
    public int OTPResendLimit { set; get; }
    public int OTPSessionTimeOutInMinute { set; get; }
    public bool UsingWinAuth { set; get; }
    public string HomePageUrl { set; get; }
    public string LocationServiceUrl { set; get; }
}
