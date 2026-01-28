using System.ComponentModel;

namespace CosmeticsStore.API.Models.RequestModels;

/// <summary>
/// Sort field options
/// </summary>
public enum SortByOption
{
    [Description("created-at")]
    CreatedAt,
    
    [Description("updated-at")]
    UpdatedAt,
    
    [Description("name")]
    Name,
    
    [Description("code")]
    Code,
    
    [Description("price")]
    Price
}

/// <summary>
/// Sort order options
/// </summary>
public enum SortOrderOption
{
    [Description("asc")]
    Asc,
    
    [Description("desc")]
    Desc
}
