namespace CosmeticsStore.Services.Models;

/// <summary>
/// Business Model for Cosmetic - used in Service layer
/// Contains business logic validation and processing
/// </summary>
public class CosmeticBusinessModel
{
    public string CosmeticId { get; set; } = null!;
    public string CosmeticName { get; set; } = null!;
    public string SkinType { get; set; } = null!;
    public string ExpirationDate { get; set; } = null!;
    public string CosmeticSize { get; set; } = null!;
    public decimal DollarPrice { get; set; }
    public string? CategoryId { get; set; }
    public CategoryBusinessModel? Category { get; set; }

    /// <summary>
    /// Business logic: Check if cosmetic is expired
    /// </summary>
    public bool IsExpired()
    {
        if (DateTime.TryParse(ExpirationDate, out DateTime expDate))
        {
            return expDate < DateTime.Now;
        }
        return false;
    }

    /// <summary>
    /// Business logic: Calculate discounted price
    /// </summary>
    public decimal CalculateDiscountedPrice(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100");
        
        return DollarPrice * (1 - discountPercentage / 100);
    }
}

/// <summary>
/// Business Model for Category
/// </summary>
public class CategoryBusinessModel
{
    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
}

/// <summary>
/// Business Model for paginated results
/// </summary>
public class PagedBusinessModel<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
