using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class Opponent
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? NationalId { get; set; }

    public string? Phone { get; set; }

    public string Address { get; set; } = null!;

    public string? OpponentLawyerName { get; set; }

    public string? OpponentLawyerPhone { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CaseOpponent> CaseOpponents { get; set; } = new List<CaseOpponent>();
}
