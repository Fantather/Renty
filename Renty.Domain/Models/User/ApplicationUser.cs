using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Renty.Domain.Models.Orders;
using Renty.Domain.Models.Locations;

namespace Renty.Domain.Models.User
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Модель пользователя, 
        /// для расширения возможностей IdentityUser
        /// 
        /// поля IdentityUser:
        /// Id 
        /// UserName 
        /// NormalizedUserName  - имя пользователя в верхнем регистре
        /// Email 
        /// NormalizedEmail 
        /// EmailConfirmed 
        /// 
        /// 
        /// PasswordHash 
        /// SecurityStamp 
        /// ConcurrencyStamp 
        /// 
        /// PhoneNumber
        /// PhoneNumberConfirmed 
        /// 
        /// TwoFactorEnabled 
        /// 
        /// LockoutEnabled 
        /// LockoutEnd
        /// AccessFailedCount 
        /// </summary>

        //полное имя пользователя
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        ///ссылка на аватарку пользователя
        public string? AvatarUrl { get; set; }

        //его адрес проживания (страна, город)
        public Guid? HomeCountryId { get; set; }
        public virtual Country? HomeCountry { get; set; }

        public Guid? HomeCityId { get; set; }
        public virtual City? HomeCity { get; set; }

        public string? TravelReason { get; set; }
        public bool IsTravellingWithPet { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Коллекции
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Favorite> Favorites { get; set; } 

    }
}


