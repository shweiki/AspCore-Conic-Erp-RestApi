﻿#nullable disable

namespace Domain;

public partial class Setting
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }

}
