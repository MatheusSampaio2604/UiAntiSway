﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class vmLoginUser
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
