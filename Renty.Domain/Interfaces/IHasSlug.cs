using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IHasSlug
    {
        string Name { get; set; }
        string Slug { get; set; }
    }
}
