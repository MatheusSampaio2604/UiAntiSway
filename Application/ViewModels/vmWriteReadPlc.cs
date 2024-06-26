using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class vmWritePlc
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
    }

    public class vmApiRequestWritePlc
    {
        public string? AddressPlc { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
    }
}
