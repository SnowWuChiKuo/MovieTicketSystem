﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ServerSide.Models.EFModels;

public partial class User
{
    public int Id { get; set; }

    public string Account { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Name { get; set; }

    public bool IsAdmin { get; set; }
}