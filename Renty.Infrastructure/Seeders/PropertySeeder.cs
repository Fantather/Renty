using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
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

            // Айди комнат, чтобы привязать их к фотографиям
            var odessaIds = new Dictionary<string, Guid>
            {
                { "bedroom", Guid.CreateVersion7() },
                { "bathroom", Guid.CreateVersion7() },
                { "livingRoom", Guid.CreateVersion7() },
                { "kitchen", Guid.CreateVersion7() },
                { "balcony", Guid.CreateVersion7() }
            };

            var kievIds = new Dictionary<string, Guid>
            {
                { "studio", Guid.CreateVersion7() },
                { "bathroom", Guid.CreateVersion7() },
                { "livingRoom", Guid.CreateVersion7() },
                { "kitchen", Guid.CreateVersion7() },
                { "balcony", Guid.CreateVersion7() }
            };

            if (hostOdesa == null || hostKyiv == null || odesa == null || kyiv == null || catSea == null)
            {
                Console.WriteLine("Ошибка: Не найдены необходимые зависимости для сидирования квартир.");
                return;
            }

            // Список квартир
            var propertiesToSeed = new List<Property>
            {
                new Property
                {
                    Id = Guid.CreateVersion7(),
                    Name = "Панорамная квартира в Аркадии",
                    Description = "Светлая квартира с прямым видом на море, в двух минутах от пляжа.",
                    HostId = hostOdesa.Id,
                    CategoryId = catSea.Id,
                    Address = "ул. Аркадийское плато, 3б",
                    CityId = odesa.Id,
                    CountryId = odesa.CountryId,
                    
                    // (Долгота, Широта)
                    Location = new Point(30.767277773685088, 46.429824395462816),

                    PricePerNight = 1500,
                    Currency = "UAH",
                    Status = PropertyStatusEnum.Active,
                    Details = new PropertyDetails
                    {
                        MaxGuests = 4, BedsCount = 2, BedroomsCount = 1, BathroomsCount = 1, FloorsCount = 24, Floor = 18
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
                        
                        new Room { Id = odessaIds["bedroom"], Name = "Главная спальня", RoomTypeId = roomTypeBedroom!.Id, IsSharedSpace = false, BedsCount = 1, Area = 20.5m },
                        new Room { Id = odessaIds["bathroom"], Name = "Ванная комната", RoomTypeId = roomTypeBathroom!.Id, IsSharedSpace = false, BedsCount = 0, Area = 6.0m },
                        new Room { Id = odessaIds["livingRoom"], Name = "Гостиная", RoomTypeId = roomTypeLivingRoom!.Id, IsSharedSpace = false, BedsCount = 1, Area = 25.0m },
                        new Room { Id = odessaIds["kitchen"], Name = "Кухня", RoomTypeId = roomTypeKitchen!.Id, IsSharedSpace = false, BedsCount = 0, Area = 10.0m },
                        new Room { Id = odessaIds["balcony"], Name = "Балкон", RoomTypeId = roomTypeBalcony!.Id, IsSharedSpace = false, BedsCount = 0, Area = 5.0m }
                    },

                    PropertyImages = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["balcony"],
                            Title = "Вид на море",
                            ImageUrl = "https://a0.muscache.com/im/pictures/ecee9eaa-9a4d-49bd-926e-3d72aa7854bc.jpg?im_w=1200",
                            IsPrimary = true,
                            DisplayOrder = 1
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["bedroom"],
                            Title = "Вид на море из спальни",
                            ImageUrl = "https://a0.muscache.com/im/pictures/ef2eddb4-5c5d-4fe8-b0b2-5bdbd87f40ac.jpg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 2
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["bedroom"],
                            Title = "Главная спальня",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/bca8042d-5fc4-48f1-8208-09d7749de36a.jpeg?im_w=1200", 
                            IsPrimary = false,
                            DisplayOrder = 3
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["bedroom"],
                            Title = "Спальня вид сбоку",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/a5c7e960-4018-4d28-80a8-ddb3ad132d89.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 4
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["bathroom"],
                            Title = "Ванная комната",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/be64056b-fc90-4854-b547-84a35bf6c51f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 4
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["livingRoom"],
                            Title = "Гостиная",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/7f88f0e8-789d-463e-a723-9bf6ee9cefd0.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 5
                        },
                         new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["livingRoom"],
                            Title = "Гостиная вид на окно",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/8c32798d-b370-4ebd-90d2-28d086080775.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 6
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = odessaIds["kitchen"],
                            Title = "Кухня",
                            ImageUrl = "https://a0.muscache.com/im/pictures/miso/Hosting-52212646/original/ad732ca9-9bab-4dbd-8bd0-95f0bf33e0b3.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 7
                        }
                    }
                },
                new Property
                {
                    Id = Guid.CreateVersion7(),
                    Name = "Лофт у Крещатика",
                    Description = "Стильный лофт в самом центре столицы. Идеально для работы и отдыха.",
                    HostId = hostKyiv.Id,
                    CategoryId = catCenter!.Id,
                    Address = "8 ул. Прорезная",
                    CityId = kyiv.Id,
                    CountryId = kyiv.CountryId,
                    
                    // (Долгота, Широта)
                    Location = new Point(30.52030844107298, 50.448625765764874),

                    PricePerNight = 2500,
                    Currency = "UAH",
                    Status = PropertyStatusEnum.Active,
                    Details = new PropertyDetails
                    {
                        MaxGuests = 2, BedsCount = 1, BedroomsCount = 1, BathroomsCount = 1, FloorsCount = 5, Floor = 3
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        ifNotNullCreateAmenity(wifi?.Id)
                    }.Where(a => a != null).ToList()!,

                    Rooms = new List<Room>
                    {
                        new Room { Id = kievIds["studio"], Name = "Студия", RoomTypeId = roomTypeStudio!.Id, IsSharedSpace = false, BedsCount = 1, Area = 45.0m },
                        new Room { Id = kievIds["bathroom"], Name = "Ванная комната", RoomTypeId = roomTypeBathroom!.Id, IsSharedSpace = false, BedsCount = 0, Area = 5.0m },
                    },

                    PropertyImages = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = kievIds["studio"], 
                            Title = "Основное пространство",
                            ImageUrl = "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/22e42db8-be40-497d-96aa-b3566d1f36d9.jpeg?im_w=1200",
                            IsPrimary = true,
                            DisplayOrder = 1
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = kievIds["studio"],
                            Title = "Студия вид с другой стороны",
                            ImageUrl = "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/a3a2751c-31cc-42c1-a28d-0c83af1eff0f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 2
                        },
                        new PropertyImage
                        {
                            Id = Guid.CreateVersion7(),
                            RoomId = kievIds["bathroom"],
                            Title = "Ванная комната",
                            ImageUrl = "https://a0.muscache.com/im/pictures/hosting/Hosting-1757238356447557231/original/d1cdaaaa-2ded-4065-9926-2a88aef9780f.jpeg?im_w=1200",
                            IsPrimary = false,
                            DisplayOrder = 3
                        }
                    }
                }
            };

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
    }
}