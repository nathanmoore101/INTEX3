using System;
using System.Collections.Generic;

namespace INTEX3.Models;

public partial class UserRecommendation
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public string? Recommendation1 { get; set; }

    public string? Recommendation2 { get; set; }

    public string? Recommendation3 { get; set; }

    public string? Recommendation4 { get; set; }

    public string? Recommendation5 { get; set; }
}
