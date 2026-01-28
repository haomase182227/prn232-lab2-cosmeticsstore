using System.ComponentModel.DataAnnotations;

namespace CosmeticsStore.API.Models.RequestModels;

/// <summary>
/// Request Model for creating a new cosmetic
/// </summary>
public class CreateCosmeticRequest
{
    [Required(ErrorMessage = "Cosmetic ID is required")]
    [StringLength(50, ErrorMessage = "Cosmetic ID cannot exceed 50 characters")]
    public string CosmeticId { get; set; } = null!;

    [Required(ErrorMessage = "Cosmetic Code is required")]
    [StringLength(50, ErrorMessage = "Cosmetic Code cannot exceed 50 characters")]
    public string CosmeticCode { get; set; } = null!;

    [Required(ErrorMessage = "Cosmetic Name is required")]
    [StringLength(200, ErrorMessage = "Cosmetic Name cannot exceed 200 characters")]
    public string CosmeticName { get; set; } = null!;

    [Required(ErrorMessage = "Skin Type is required")]
    [StringLength(50, ErrorMessage = "Skin Type cannot exceed 50 characters")]
    public string SkinType { get; set; } = null!;

    [Required(ErrorMessage = "Expiration Date is required")]
    public string ExpirationDate { get; set; } = null!;

    [Required(ErrorMessage = "Cosmetic Size is required")]
    [StringLength(50, ErrorMessage = "Cosmetic Size cannot exceed 50 characters")]
    public string CosmeticSize { get; set; } = null!;

    [Required(ErrorMessage = "Dollar Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal DollarPrice { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    [StringLength(50, ErrorMessage = "Category ID cannot exceed 50 characters")]
    public string CategoryId { get; set; } = null!;
}

/// <summary>
/// Request Model for updating an existing cosmetic
/// </summary>
public class UpdateCosmeticRequest
{
    [Required(ErrorMessage = "Cosmetic Code is required")]
    [StringLength(50, ErrorMessage = "Cosmetic Code cannot exceed 50 characters")]
    public string CosmeticCode { get; set; } = null!;

    [Required(ErrorMessage = "Cosmetic Name is required")]
    [StringLength(200, ErrorMessage = "Cosmetic Name cannot exceed 200 characters")]
    public string CosmeticName { get; set; } = null!;

    [Required(ErrorMessage = "Skin Type is required")]
    [StringLength(50, ErrorMessage = "Skin Type cannot exceed 50 characters")]
    public string SkinType { get; set; } = null!;

    [Required(ErrorMessage = "Expiration Date is required")]
    public string ExpirationDate { get; set; } = null!;

    [Required(ErrorMessage = "Cosmetic Size is required")]
    [StringLength(50, ErrorMessage = "Cosmetic Size cannot exceed 50 characters")]
    public string CosmeticSize { get; set; } = null!;

    [Required(ErrorMessage = "Dollar Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal DollarPrice { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    [StringLength(50, ErrorMessage = "Category ID cannot exceed 50 characters")]
    public string CategoryId { get; set; } = null!;
}
