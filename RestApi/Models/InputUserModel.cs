using System.ComponentModel.DataAnnotations;

namespace Api.Models;
#nullable disable
public class InputUserModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
