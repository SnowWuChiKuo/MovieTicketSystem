﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Review
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int MovieId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Member Member { get; set; }

    public virtual Movie Movie { get; set; }
}