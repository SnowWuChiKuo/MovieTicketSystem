﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int TicketId { get; set; }

    public int Qty { get; set; }

    public int SubTotal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string SeatsName { get; set; }

    public virtual Cart Cart { get; set; }

    public virtual Ticket Ticket { get; set; }
}