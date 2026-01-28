using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CosmeticsStore.Repositories.Models;

public partial class CosmeticCategory
{
    [Key]
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// User-friendly code for display purposes
    /// </summary>
    public string CategoryCode { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string UsagePurpose { get; set; } = null!;

    public string FormulationType { get; set; } = null!;

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

    public virtual ICollection<CosmeticInformation> CosmeticInformations { get; set; } = new List<CosmeticInformation>();
}
