﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}