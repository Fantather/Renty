using Renty.Domain.Models.LookupsTables;

namespace Renty.Web.Extensions
{
    public record UserFactMeta(string IconName, string Title, string Hint, int MaxLength);

    public static class UserFactTypeExtensions
    {
        private static readonly Dictionary<UserFactTypeEnum, UserFactMeta> Meta = new()
        {
            [UserFactTypeEnum.Work] = new("profile/briefcase", "Кем вы работаете?", "Какая у вас профессия? Если вы не работаете, расскажите о своем призвании. Пример: «медсестра», «родитель четверых детей» или «серфингист на пенсии».", 20),
            [UserFactTypeEnum.Generation] = new("profile/balloon", "В каких годах вы родились", "Не волнуйтесь, точную дату никто не увидит.", 40),
            [UserFactTypeEnum.HowISpendTime] = new("profile/clock", "Что вы готовы делать часами?", "Расскажите о любимых занятиях, на которые тратите свободное время. Пример: «смотрю видео с котиками» или «играю в шахматы».", 40),
            [UserFactTypeEnum.Pets] = new("profile/paw", "У вас есть домашние животные?", "Если да, расскажите, какие именно и как их зовут. Пример: «трехцветная кошка Уискерс» или «проворная черепаха Леонардо».", 40),
            [UserFactTypeEnum.SchoolYears] = new("profile/graduation-cap", "Где вы учились?", "Домашнее обучение или средняя школа, колледж или профессиональное образование? Какое заведение помогло вам стать собой?", 40),
            [UserFactTypeEnum.FavoriteSchoolSong] = new("profile/music-note", "Любимая песня в школе", "Не смущайтесь! Расскажите, что вы слушали снова и снова, когда были подростком.", 40),
            [UserFactTypeEnum.InterestingFact] = new("profile/lightbulb", "Поделитесь интересным фактом о себе", "Вспомните уникальный или примечательный факт о себе. Пример: «меня сняли в видеоклипе» или «я умею жонглировать».", 40),
            [UserFactTypeEnum.UselessSkill] = new("profile/magic-wand", "Какой навык вам не пригодился?", "Расскажите о своих удивительных, но совершенно бесполезных способностях. Пример: «я умею тасовать колоду карт одной рукой».", 40),
            [UserFactTypeEnum.LifeStory] = new("profile/book", "Девиз — или биография?", "Представьте, что кто-то пишет книгу о вас. Как ее можно было бы назвать? Пример: «Рожденный для странствий» или «Записки собачницы».", 40),
            [UserFactTypeEnum.WhatILove] = new("outline-heart", "Что вам особенно нравится?", "Расскажите о том, что готовы делать бесконечно. Пример: «печь фокаччу с розмарином».", 40),
            [UserFactTypeEnum.WhereIWantToGo] = new("profile/globe", "Где вам всегда хотелось побывать?", "Расскажите, куда бы вы обязательно отправились, будь то мечта всей жизни или сиюминутное желание.", 40),
        };

        public static UserFactMeta GetMeta(this UserFactTypeEnum type) => Meta[type];
    }
}
