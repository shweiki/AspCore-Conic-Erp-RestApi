﻿#nullable disable


namespace Domain.Entities;

public partial class UserRouter
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Router { get; set; }
    public string DefulateRedirect { get; set; }


}
