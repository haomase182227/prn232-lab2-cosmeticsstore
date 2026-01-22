namespace CosmeticsStore.API.Models.ResponseModels;

/// <summary>
/// Response Model for Cosmetic Information
/// </summary>
public class CosmeticResponse
{
    public string CosmeticId { get; set; } = null!;
    public string CosmeticName { get; set; } = null!;
    public string SkinType { get; set; } = null!;
    public string ExpirationDate { get; set; } = null!;
    public string CosmeticSize { get; set; } = null!;
    public decimal DollarPrice { get; set; }
    public string? CategoryId { get; set; }
    public CategoryResponse? Category { get; set; }
}

/// <summary>
/// Response Model for Category Information
/// </summary>
public class CategoryResponse
{
    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
}
