﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Order
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int? CouponId { get; set; }

    public int TotalAmount { get; set; }

    public bool Status { get; set; }

    public int? CouponAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Coupon Coupon { get; set; }

    public virtual Member Member { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}