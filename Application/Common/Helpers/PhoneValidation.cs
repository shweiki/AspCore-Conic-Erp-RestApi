namespace Application.Common.Helpers;

public static class PhoneValidation
{
    public static bool IsPhoneValid(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        try
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            var phoneNumber = phoneNumberUtil.Parse(phone, null);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
