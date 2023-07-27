namespace Application.Common.Enums;

public static class RolesEnum
{
    public static class SystemAdmin
    {
        public const string Name = "SystemAdmin";
        public const string Display = "System Admin";
    }
    public static class Admin
    {
        public const string Name = "Admin";
        public const string Display = "Admin";
    }
    public static class User
    {
        public const string Name = "User";
        public const string Display = "User";
    }
    public static class Signer
    {
        public const string Name = "Signer";
        public const string Display = "Signer";
        public const string AlternativeDisplay = "Has authority to sign";
    }
    public static class CanSendToExternalUsers
    {
        public const string Name = "CanSendToExternalUsers";
        public const string Display = "CanSendToExternalUsers";
        public const string AlternativeDisplay = "Can send to external users";
    }
}
