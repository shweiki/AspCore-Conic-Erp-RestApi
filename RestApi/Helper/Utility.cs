using System;

namespace RestApi.Helper;

public static class Utility
{
    public static double toFixed(this double number, uint decimals)
    {
        return Convert.ToDouble(number.ToString("N" + decimals));
    }
}
