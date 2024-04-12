using System;
using System.Collections.Generic;

namespace INTEX3.Models;

public partial class ItemRecommendation
{
    public int ProductId { get; set; }

    public int? Rec1ProductId { get; set; }

    public int? Rec2ProductId { get; set; }

    public int? Rec3ProductId { get; set; }

    public int? Rec4ProductId { get; set; }

    public int? Rec5ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
