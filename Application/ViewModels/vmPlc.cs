﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class vmPlc
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AddressPlc { get; set; }
        public required string Type { get; set; }
        public object? Value { get; set; }
    }
}