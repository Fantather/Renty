using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetUser
{
    public class UserFactInputDto
    {
        public UserFactTypeEnum Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
