using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CosmeticsStore.Repositories.Models;

public partial class CosmeticInformation
{
    [Key]
    public string CosmeticId { get; set; } = null!;

    /// <summary>
    /// User-friendly code for display purposes
    /// </summary>
    public string CosmeticCode { get; set; } = null!;

    public string CosmeticName { get; set; } = null!;

    public string SkinType { get; set; } = null!;

    public string ExpirationDate { get; set; } = null!;

    public string CosmeticSize { get; set; } = null!;

    public decimal DollarPrice { get; set; }

    public string? CategoryId { get; set; }

    /// <summary>
    /// Soft delete status: 1 = Active, 0 = Deleted
    /// </summary>
    public int Status { get; set; } = 1;

    /// <summary>
    /// Timestamp when record was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual CosmeticCategory? Category { get; set; }
}
