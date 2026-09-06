using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Login
{
    public class ExternalLoginChallenge
    {
        public string Provider { get; set; }
        public AuthenticationProperties Properties { get; set; }
    }
}
