namespace Application.Common.Interfaces;

public interface ISMSService
{
    Task<bool> SendSMSAsync(string tonumber, string message);
}