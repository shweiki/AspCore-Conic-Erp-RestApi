﻿#nullable disable


namespace Domain.Entities;

public partial class FileDatum
{
    public long Id { get; set; }
    public string Type { get; set; }
    public string FileType { get; set; }
    public string FilePath { get; set; }
    public string File { get; set; }
    public int Status { get; set; }
    public string TableName { get; set; }
    public long Fktable { get; set; }
}
