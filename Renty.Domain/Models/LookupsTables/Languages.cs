using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Models.LookupsTables
{
    public class Languages:Lookup
    {
        /// <summary>
        /// Cписок языков, которые знает пользователь
        /// </summary> 
        /// 

        /// <summary>
        /// eu-eu, ru-ru, en-en, es-es, fr-fr, de-de, it-it, ja-ja, zh-zh
        /// </summary>
        public string Code { get; set; } = string.Empty;
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
