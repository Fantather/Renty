using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;
using Renty.Domain.Models;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Helpers;

namespace Renty.Infrastructure.Seeders
{
    public class PropertySeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Properties.AnyAsync())
            {
                return;
            }
           

            // Получаем зависимости
            var hostOdesa = await context.Users.FirstOrDefaultAsync(u => u.UserName == "izya-troff");
            var hostKyiv = await context.Users.FirstOrDefaultAsync(u => u.UserName == "psyduck-user");

            var odesa = await context.Cities.FirstOrDefaultAsync(c => c.Name == "Одесса");
            var kyiv = await context.Cities.FirstOrDefaultAsync(c => c.Name == "Киев");

            var catSea = await context.PropertiesCategory.FirstOrDefaultAsync(c => c.Name == "У моря");
            var catCenter = await context.PropertiesCategory.FirstOrDefaultAsync(c => c.Name == "В центре города");

            // Ищем типы комнат
            var roomTypeStudio = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Студия");
            var roomTypeBedroom = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Спальня");
            var roomTypeBathroom = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Ванная комната");
            var roomTypeLivingRoom = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Гостиная");
            var roomTypeKitchen = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Кухня");
            var roomTypeBalcony = await context.Set<RoomType>().FirstOrDefaultAsync(rt => rt.Name == "Балкон / Терраса");

            var wifi = await context.Anemities.FirstOrDefaultAsync(a => a.Name == "Wi-Fi");
            var ac = await context.Anemities.FirstOrDefaultAsync(a => a.Name == "Портативная система кондиционирования");
            var petFriendly = await context.Tags.FirstOrDefaultAsync(t => t.Name == "Можно с питомцами");

            if (hostOdesa == null || hostKyiv == null || odesa == null || kyiv == null || catSea == null)
            {
                Console.WriteLine("Ошибка: Не найдены необходимые зависимости для сидирования квартир.");
                return;
            }

            var counter = 4;
            var propertiesToSeed = new List<Property>();
            //главная картинка что бы разные были у квартир
            var odesaImages = new string[]
            {
                "https://a0.muscache.com/im/pictures/7814194f-6ba6-450c-87d8-63c77e8c894c.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/ddf4e825-14e3-47c0-9bb1-76a1eec15778.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/d46705e6-9fd0-4f87-9e0b-c6e3055672fd.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1420454997229991629/original/c3305f2a-6e04-43e2-8975-a522af929a41.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1420454997229991629/original/de47e355-36ac-4d16-8b1b-ad9db9dbe7d9.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-48317252/original/8e59ab06-cb04-4dcd-b6bc-51f625bc6a83.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-48317252/original/2f4b679a-f82f-4e21-833d-bda684b2357e.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-48317252/original/e6df563e-c0d5-40ed-9dbc-3e2cca17b317.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-48317252/original/dd59d2ba-8cc6-4af1-aafd-181bbaecc044.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1443395481586661306/original/498e96da-b734-48de-a4f1-aea3eb9a971e.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/10a2a64f-244e-4caa-99f8-3732a8fc09fb.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/0527462d-e39b-4722-b27e-add60a33dcc1.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/92ed8672-ccc7-4e21-b2a3-9244224ea92f.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/26b0f9aa-69c8-4185-9f0a-565dccc05df3.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/825e0be6-8df3-4269-8027-16bc94233090.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/0dfff02b-0d2f-4484-831f-98829ff81544.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/ba337d67-1750-493c-b285-db796a0091f6.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/5a3734c6-231f-47cc-9017-e94eb65435f3.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/e5f6a006-ed2c-46f7-81cc-b5f3d713d9c1.jpg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-1123112527141299752/original/fe47fb06-f846-44b0-b10e-afcad0fe7daa.jpeg?im_w=1200"


            };
            //тоже самое для квартир в киеве
            var kyivImages = new string[]
            {
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/22e42db8-be40-497d-96aa-b3566d1f36d9.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/a1574795-8a77-4fd1-965f-7399212c27a4.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/miso/Hosting-559861058728070984/original/06f4ec87-03ff-4441-b97b-604a55691d26.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1098545345469964728/original/8f55960e-14a0-4bea-8785-e4b1ce7f8328.jpeg?im_w=1200",
                "https://a0.muscache.com/im/pictures/hosting/Hosting-1716050899513982782/original/634235ce-a49c-4963-8d12-3ea1d3d7f5e8.jpeg?im_w=1200"
            };

  
            //квартиры одесса
            Property CreateOdessaClone(string mainImageUrl, int index)
            {

                var bedroomId = Guid.CreateVersion7();
                var bathroomId = Guid.CreateVersion7();
                var livingRoomId = Guid.CreateVersion7();
                var kitchenId = Guid.CreateVersion7();
                var balconyId = Guid.CreateVersion7();

                var reviews = new List<Review>();
                reviews.AddRange(Enumerable.Range(0, counter + 1)
                .Select(i => GetRandomReview(min: 1, max: 5, hostId: hostKyiv.Id, index: i)));

                return new Property
                {
                    Id = Guid.CreateVersion7(),
                    Name = $"Панорамная квартира в Аркадии #{index}",
                    Description = "Светлая квартира с прямым видом на море, в двух минутах от пляжа.",
                    HostId = hostOdesa.Id,
                    CategoryId = catSea.Id,
                    Address = $"ул. Аркадийское плато, 36 (Кв. {index})",
                    CityId = odesa.Id,
                    CountryId = odesa.CountryId,
                    Location = new Point(30.767277773685088, 46.429824395462816),
                    PricePerNight = Random.Shared.Next(1000, 5000),
                    Currency = "UAH",
                    Status = PropertyStatusEnum.Active,
                    Reviews = reviews,
                    AverageRating = reviews.Average(r => r.Rating),
                    ReviewsCount = reviews.Count,
                    Details = new PropertyDetails
                    {
                        MaxGuests = 4,
                        BedsCount = 2,
                        BedroomsCount = 1,
                        BathroomsCount = 1,
                        RoomsCount = 2,
                        FloorsCount = 24,
                        Floor = 18
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        ifNotNullCreateAmenity(wifi?.Id),
                        ifNotNullCreateAmenity(ac?.Id)
                    }.Where(a => a != null).ToList()!,
                    PropertyTags = new List<PropertyTag>
                    {
                        ifNotNullCreateTag(petFriendly?.Id)
                    }.Where(t => t != null).ToList()!,
                    Rooms = new List<Room>
                    {
                        new Room { Id = bedroomId, Name = "Главная спальня", RoomTypeId = roomTypeBedroom!.Id, IsSharedSpace = false, BedsCount = 1, Area = 20.5m },
                        new Room { Id = bathroomId, Name = "Ванная комната", RoomTypeId = roomTypeBathroom!.Id, IsSharedSpace = false, BedsCount = 0, Area = 6.0m },
                        new Room { Id = livingRoomId, Name = "Гостиная", RoomTypeId = roomTypeLivingRoom!.Id, IsSharedSpace = false, BedsCount = 1, Area = 25.0m },
                        new Room { Id = kitchenId, Name = "Кухня", RoomTypeId = roomTypeKitchen!.Id, IsSharedSpace = false, BedsCount = 0, Area = 10.0m },
                        new Room { Id = balconyId, Name = "Балкон", RoomTypeId = roomTypeBalcony!.Id, IsSharedSpace = false, BedsCount = 0, Area = 5.0m }
                    },
                    PropertyImages = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = livingRoomId,
                            Title = $"Вид на квартиру {index}",
                            ImageUrl = mainImageUrl,
                            IsPrimary = true,
                            DisplayOrder = 1
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = balconyId,
                            Title = "Вид на море",
                            ImageUrl = "https://a0.muscache.com/im/pictures/ecee9eaa-9a4d-49bd-926e-3d72aa7854bc.jpg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 2
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = bedroomId,
                            Title = "Вид на море из спальни",
                            ImageUrl = "https://a0.muscache.com/im/pictures/ef2eddb4-5c5d-4fe8-b0b2-5bdbd87f40ac.jpg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 3
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = bedroomId,
                            Title = "Главная спальня",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/bca8042d-5fc4-48f1-8208-09d7749de36a.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 4
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = bedroomId,
                            Title = "Спальня вид сбоку",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/a5c7e960-4018-4d28-80a8-ddb3ad132d89.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 5
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = bathroomId,
                            Title = "Ванная комната",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/be64056b-fc90-4854-b547-84a35bf6c51f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 6
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = kitchenId,
                            Title = "Кухня",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/ad732ca9-9bab-4dbd-8bd0-95f0bf33e0b3.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 7
                        }
                    }
                };
            }

            Property CreateKyivClone(string mainImageUrl, int index)
            {
                var studioId = Guid.CreateVersion7();
                var bathroomId = Guid.CreateVersion7();
                var reviews = new List<Review>();
                reviews.AddRange(Enumerable.Range(0, counter + 1)
                .Select(i => GetRandomReview(min: 1, max: 5, hostId: hostOdesa.Id, index: i)));
                return new Property
                {
                    Id = Guid.CreateVersion7(),
                    Name = $"Лофт у Крещатика #{index}",
                    Description = "Стильный лофт в самом центре столицы. Идеально для работы и отдыха.",
                    HostId = hostKyiv.Id,
                    CategoryId = catCenter!.Id,
                    Address = $"8 ул. Прорезная (Кв. {index})",
                    CityId = kyiv.Id,
                    CountryId = kyiv.CountryId,
                    Location = new Point(30.52030844107298, 50.448625765764874),
                    PricePerNight = Random.Shared.Next(1000, 2500),
                    Reviews = reviews,
                    Currency = "UAH",
                    AverageRating = reviews.Average(r => r.Rating),
                    ReviewsCount = reviews.Count,
                    Status = PropertyStatusEnum.Active,

                    Details = new PropertyDetails
                    {
                        MaxGuests = 2,
                        BedsCount = 1,
                        BedroomsCount = 1,
                        BathroomsCount = 1,
                        RoomsCount = 2,
                        FloorsCount = 5,
                        Floor = 3
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        ifNotNullCreateAmenity(wifi?.Id)
                    }.Where(a => a != null).ToList()!,
                    Rooms = new List<Room>
                    {
                        new Room { Id = studioId, Name = "Студия", RoomTypeId = roomTypeStudio!.Id, IsSharedSpace = false, BedsCount = 1, Area = 45.0m },
                        new Room { Id = bathroomId, Name = "Ванная комната", RoomTypeId = roomTypeBathroom!.Id, IsSharedSpace = false, BedsCount = 0, Area = 5.0m },
                    },
                    PropertyImages = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = studioId,
                            Title = $"Основное пространство {index}",
                            ImageUrl = mainImageUrl,
                            IsPrimary = true,
                            DisplayOrder = 1
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = studioId,
                            Title = "Студия вид с другой стороны",
                            ImageUrl = "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/a3a2751c-31cc-42c1-a28d-0c83af1eff0f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 2
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = bathroomId,
                            Title = "Ванная комната",
                            ImageUrl = "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/d1cdaaaa-2ded-4065-9926-2a88aef9780f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 3
                        }
                    }
                };
            }
            for (int i = 0; i < odesaImages.Length; i++)
            {
                propertiesToSeed.Add(CreateOdessaClone(odesaImages[i], i + 1));
            }

            for (int i = 0; i < kyivImages.Length; i++)
            {
                propertiesToSeed.Add(CreateKyivClone(kyivImages[i], i + 1));
            }

            foreach (var property in propertiesToSeed)
            {
                try
                {
                    await SeedPropery(context, property);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении квартиры '{property.Name}': {ex.Message}");
                }
            }
        }

        private static async Task<Property> SeedPropery(AppDbContext context, Property property)
        {
            if (string.IsNullOrWhiteSpace(property.Slug))
            {
                property.Slug = SlugGenerator.GenerateSlug(property.Name);
            }

            await context.Properties.AddAsync(property);
            await context.SaveChangesAsync();
            return property;
        }

        private static PropertyAmenity? ifNotNullCreateAmenity(Guid? amenityId) =>
            amenityId.HasValue ? new PropertyAmenity { AmenityId = amenityId.Value } : null;

        private static PropertyTag? ifNotNullCreateTag(Guid? tagId) =>
            tagId.HasValue ? new PropertyTag { TagId = tagId.Value } : null;

        private static decimal GetRandomRating(double min, double max) =>
        (decimal)Math.Round(min + Random.Shared.NextDouble() * (max - min), 1);

        private static Review GetRandomReview(double min, double max, Guid hostId, int index) 
        {
                    var positiveComments = new[]
          {
                        "Отличная квартира с потрясающим видом! Очень доволен. Рекомендую!",
                        "Очень уютно, все необходимое для проживания есть. Вид шикарный!",
                        "Прекрасное расположение, чисто и комфортно. Обязательно вернусь еще раз.",
                        "Во имя императора эта квартира заставила мою кровь бурлить!",
                        "Я ВЫЖИЛ УРА"
                    };
                    var neutralComments = new[]
                                {
                        "В целом неплохо, но были мелкие недочеты. Расположение удобное.",
                        "Нормальная квартира за свои деньги. Чисто, но мебель уставшая.",
                        "Настроения совсем не подняло, не зашло",
                        "В подъезде воняло",
                        "Император не одобрил"
                    };

            decimal rating2 = GetRandomRating(3.0, 4.5);
            decimal cleanliness2 = Random.Shared.Next(3, 5);
            decimal communication2 = Random.Shared.Next(3, 5);
            decimal accuracy2 = Random.Shared.Next(3, 5);
            decimal location2 = Random.Shared.Next(4, 6);
            decimal checkIn2 = Random.Shared.Next(3, 5);
            decimal value2 = Random.Shared.Next(3, 5);

            string comment2 = rating2 >= 4.0m
                ? positiveComments[Random.Shared.Next(positiveComments.Length)]
                : neutralComments[Random.Shared.Next(neutralComments.Length)];

            var review = new Review()
            {
                Id = Guid.CreateVersion7(),
                UserId = hostId,
                Rating = rating2,
                CleanlinessRating = cleanliness2,
                CommunicationRating = communication2,
                AccuracyRating = accuracy2,
                LocationRating = location2,
                CheckInRating = checkIn2,
                ValueRating = value2,
                Comment = $"Отличная квартира с потрясающим видом на море! Останавливался в квартире и остался очень доволен. Рекомендую!",
                CreatedAt = DateTime.UtcNow.AddDays(-index * 3)
            };

           return review;

        }
    }
}