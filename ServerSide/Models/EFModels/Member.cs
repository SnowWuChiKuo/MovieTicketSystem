﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Member
{
    public int Id { get; set; }

    public string Account { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsBlackList { get; set; }

    public bool IsConfirmed { get; set; }

    public string ConfirmCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}