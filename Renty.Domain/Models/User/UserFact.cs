using System;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.LookupsTables;

namespace Renty.Domain.Models.User
{
    /// <summary>
    /// Один произвольный факт о пользователе для страницы профиля (работа, поколение, любимая песня и т.п.)
    /// </summary>
    public class UserFact
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public UserFactTypeEnum Type { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
