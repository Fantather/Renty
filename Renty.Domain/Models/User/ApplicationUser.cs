using Microsoft.AspNetCore.Identity;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Orders;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

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
        public ApplicationUser()
        {
            // Инициализируем Id, унаследованный от IdentityUser, как UUID v7
            //он не должен сломать инициализацию остальных полей айдентити
            Id = Guid.CreateVersion7();

            // Инициализируем пустые списки для навигационных свойств
            Reviews = new List<Review>();
            Bookings = new List<Booking>();
            Favorites = new List<Favorite>();
            Languages = new List<Languages>();
            Properties = new List<Property>();
        }
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

        public bool IsVerified { get; set; } = false;

        public string? ResponseSpeed { get; set; }

        public string? Info { get; set; }

   
        //Коллекции
        public virtual ICollection<Languages> Languages { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Property> Properties { get; set; } 

    }
}


