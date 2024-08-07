﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class Post
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModified { get; set; }
    public string? AvatarUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
}

