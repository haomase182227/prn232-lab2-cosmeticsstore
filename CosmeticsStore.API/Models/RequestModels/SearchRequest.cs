using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsStore.API.Models.RequestModels;

/// <summary>
/// Request Model for searching and filtering cosmetics
/// Query parameters use kebab-case (lowercase with hyphens)
/// </summary>
public class CosmeticSearchRequest
{
    // Search
    [FromQuery(Name = "search-term")]
    public string? SearchTerm { get; set; }
    
    // Filtering
    [FromQuery(Name = "cosmetic-name")]
    public string? CosmeticName { get; set; }
    
    [FromQuery(Name = "cosmetic-code")]
    public string? CosmeticCode { get; set; }
    
    [FromQuery(Name = "skin-type")]
    public string? SkinType { get; set; }
    
    [FromQuery(Name = "category-id")]
    public string? CategoryId { get; set; }

    [FromQuery(Name = "category-code")]
    public string? CategoryCode { get; set; }
    
    [FromQuery(Name = "min-price")]
    [Range(0, double.MaxValue, ErrorMessage = "Min price must be >= 0")]
    public decimal? MinPrice { get; set; }
    
    [FromQuery(Name = "max-price")]
    [Range(0, double.MaxValue, ErrorMessage = "Max price must be >= 0")]
    public decimal? MaxPrice { get; set; }

    // Sorting (không sort theo ID, sort theo timestamp, alphabetic, code)
    [FromQuery(Name = "sort-by")]
    public string SortBy { get; set; } = "created-at";
    
    [FromQuery(Name = "sort-order")]
    public string SortOrder { get; set; } = "desc"; // asc or desc

    // Paging (default 50-100, có max)
    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be >= 1")]
    public int Page { get; set; } = 1;
    
    [FromQuery(Name = "page-size")]
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 50;

    // Field selection (ch?n các field c?n tr? v?)
    [FromQuery(Name = "fields")]
    public string? Fields { get; set; }

    // Extension (include related entities)
    [FromQuery(Name = "include-category")]
    public bool IncludeCategory { get; set; } = false;
}
