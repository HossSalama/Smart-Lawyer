using System;
using System.Collections.Generic;

namespace SmartLawyerFinal.Models;

public partial class CaseOpponent
{
    public int Id { get; set; }

    public int CaseId { get; set; }

    public int OpponentId { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual Opponent Opponent { get; set; } = null!;
}
