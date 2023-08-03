using System;

namespace Application.Common.Helper;

public static class Utility
{
    public static double toFixed(this double number, uint decimals)
    {
        return Convert.ToDouble(number.ToString("N" + decimals));
    }
}
