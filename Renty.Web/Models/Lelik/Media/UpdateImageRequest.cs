using System.ComponentModel.DataAnnotations;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.Lelik.Media
{
    /// <summary>
    /// вот тут уже заполнить их данными
    /// </summary>  
    public class UpdateImageRequest
    {
        [Required]
        public Guid PropertyId { get; set; }

        public List<ImageItem> Images { get; set; } = new();
    }

    public class ImageItem
    {
        [Required]
        public Guid ImageId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
        //модели позволяют не привязывать фото к конкретной комнате, а просто закинуть в квартирку
        public Guid? RoomId { get; set; }
    }


}