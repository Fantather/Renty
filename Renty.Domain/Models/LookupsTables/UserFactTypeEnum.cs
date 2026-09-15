using System.ComponentModel;

namespace Renty.Domain.Models.LookupsTables
{
    public enum UserFactTypeEnum
    {
        [Description("Моя работа")]
        Work = 0,
        [Description("Годы рождения")]
        Generation = 1,
        [Description("Что я делаю часами")]
        HowISpendTime = 2,
        [Description("Питомцы")]
        Pets = 3,
        [Description("Школьные годы")]
        SchoolYears = 4,
        [Description("Любимая песня в школе")]
        FavoriteSchoolSong = 5,
        [Description("Интересный факт обо мне")]
        InterestingFact = 6,
        [Description("Мой самый бесполезный навык")]
        UselessSkill = 7,
        [Description("История моей жизни")]
        LifeStory = 8,
        [Description("Что я безумно люблю")]
        WhatILove = 9,
        [Description("Где мне всегда хотелось побывать")]
        WhereIWantToGo = 10
    }
}
