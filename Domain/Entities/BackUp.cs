﻿using System;

#nullable disable

namespace Domain.Entities;

public partial class BackUp
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime? DateTime { get; set; }
    public string BackUpPath { get; set; }
    public string DataBaseName { get; set; }
}
