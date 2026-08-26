using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.User;

namespace Renty.Domain.Models.Messages
{
    /// <summary>
    /// Сообщения между гостями и владельцами
    /// </summary>
    public class Message
    {
        public Guid Id { get; set; }

        // Отправитель
        public Guid SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser Sender { get; set; }

        // Получатель
        public Guid ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser Receiver { get; set; }

        // Связь с недвижимостью 
        public Guid? PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property? Property { get; set; }

        // Тема сообщения
        public string? Subject { get; set; }

        // Текст сообщения
        public string Content { get; set; } = string.Empty;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ID родительского сообщения, если отвечать на него
        public Guid? ParentMessageId { get; set; }
        [ForeignKey(nameof(ParentMessageId))]
        public virtual Message? ParentMessage { get; set; }

        // Навигационное свойство для ответов
        public virtual ICollection<Message> Replies { get; set; } = new List<Message>();
    }
}
