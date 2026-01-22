using System.ComponentModel.DataAnnotations;

namespace CosmeticsStore.API.Models.RequestModels;

/// <summary>
/// Request Model for login endpoint
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}
