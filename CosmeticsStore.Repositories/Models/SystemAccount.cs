using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CosmeticsStore.Repositories.Models;

public partial class SystemAccount
{
    [Key]
    public int AccountId { get; set; }

    public string AccountPassword { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string AccountNote { get; set; } = null!;

    public int? Role { get; set; }
}
