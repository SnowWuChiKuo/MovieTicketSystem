﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Coupon
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string DiscountType { get; set; }

    public int DiscountValue { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}