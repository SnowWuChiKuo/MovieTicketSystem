﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Ticket
{
    public int Id { get; set; }

    public int ScreeningId { get; set; }

    public string SalesType { get; set; }

    public string TicketType { get; set; }

    public int Price { get; set; }

    public int ReservedSeats { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Screening Screening { get; set; }

    public virtual ICollection<TicketSeat> TicketSeats { get; set; } = new List<TicketSeat>();
}