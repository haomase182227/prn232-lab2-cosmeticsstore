using CosmeticsStore.API.Models;
using CosmeticsStore.API.Models.RequestModels;
using CosmeticsStore.API.Models.ResponseModels;
using CosmeticsStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CosmeticsStore.API.Controllers;

/// <summary>
/// Controller for Authentication - Simple login without role authorization
/// URL: lowercase
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISystemAccountService _systemAccountService;
    private readonly IConfiguration _configuration;

    public AuthController(
        ISystemAccountService systemAccountService,
        IConfiguration configuration)
    {
        _systemAccountService = systemAccountService;
        _configuration = configuration;
    }

    /// <summary>
    /// Login endpoint - Simple authentication without role checking
    /// Any valid account can login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        var account = await _systemAccountService.Login(request.Email, request.Password);
        
        if (account == null)
        {
            return Unauthorized(ApiResponse<AuthResponse>.ErrorResponse(
                "Authentication failed",
                "Invalid email or password"
            ));
        }

        // Simple claims - no role
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, account.EmailAddress ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            new Claim("AccountId", account.AccountId.ToString()),
        };

        var symmetricKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT:SecretKey not configured"))
        );
        var signCredential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.Now.AddMinutes(60); // 1 hour expiration

        var preparedToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: signCredential
        );

        var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);

        var response = new AuthResponse
        {
            Token = generatedToken,
            AccountId = account.AccountId.ToString(),
            Email = account.EmailAddress ?? string.Empty,
            ExpiresAt = expiresAt
        };

        return Ok(ApiResponse<AuthResponse>.SuccessResponse(
            response,
            "Login successful"
        ));
    }
}
