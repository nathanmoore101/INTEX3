﻿using System;
using System.Collections.Generic;

namespace INTEX3.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public short? Year { get; set; }

    public short? NumParts { get; set; }

    public short? Price { get; set; }

    public string? ImgLink { get; set; }

    public string? PrimaryColor { get; set; }

    public string? SecondaryColor { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public virtual ItemRecommendation? ItemRecommendation { get; set; }
}
