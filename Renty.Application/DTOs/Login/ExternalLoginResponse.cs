using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Login
{
    public class ExternalLoginResponse
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
