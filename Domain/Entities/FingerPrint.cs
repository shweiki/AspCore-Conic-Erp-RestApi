﻿#nullable disable


namespace Domain.Entities;

public partial class FingerPrint
{
    public long Id { get; set; }
    public int Length { get; set; }
    public string Str { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public string Fk { get; set; }

}
