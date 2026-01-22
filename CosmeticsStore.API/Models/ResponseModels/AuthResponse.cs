namespace CosmeticsStore.API.Models.ResponseModels;

/// <summary>
/// Response Model for authentication endpoints
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = null!;
    public string AccountId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
