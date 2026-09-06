using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Login
{
    public class LoginDtoRequest
    {
        public string Email { get; set; } 
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
