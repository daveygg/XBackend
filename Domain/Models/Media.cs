﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public class Media
{
    public int Id { get; set; }
    public IFormFile MediaContent { get; set; }
    public string FilePath { get; set; }
}
