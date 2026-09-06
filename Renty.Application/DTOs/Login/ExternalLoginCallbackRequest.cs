using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Login
{
    public class ExternalLoginCallbackRequest
    {
        public string ReturnUrl { get; set; }
        public string RemoteError { get; set; }
    }
}
