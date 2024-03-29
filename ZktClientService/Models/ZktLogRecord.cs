﻿namespace ZktClientService.Models;

public class ZktLogRecord
{
    public int IndRegID { get; set; }
    public string DateTimeRecord { get; set; }

    public DateTime DateOnlyRecord
    {
        get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("yyyy-MM-dd")); }
    }
    public DateTime TimeOnlyRecord
    {
        get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("hh:mm:ss tt")); }
    }  
    public DateTime DateTime 
    {
        get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("yyyy-MM-dd hh:mm:ss tt")); }
    }
}
