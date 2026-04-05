using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? NationalId { get; set; }

    public string? CommercialReg { get; set; }

    public string Phone { get; set; } = null!;

    public string? SecondaryPhone { get; set; }

    public string Address { get; set; } = null!;

    public string? JobTitle { get; set; }

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string ClientType { get; set; } = null!;

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();
}
