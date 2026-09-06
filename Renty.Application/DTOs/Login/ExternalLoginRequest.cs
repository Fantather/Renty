using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Login
{
    public class ExternalLoginRequest
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
    }
}
