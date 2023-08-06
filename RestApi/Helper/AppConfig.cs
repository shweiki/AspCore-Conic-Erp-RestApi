namespace RestApi.Helper;

public static class AppConfig
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetConfigValue(string key)
    {
        return _configuration[key];
    }
    public static string GetDefaultConnection()
    {
        return _configuration.GetConnectionString("DefaultConnection");
    }

}
