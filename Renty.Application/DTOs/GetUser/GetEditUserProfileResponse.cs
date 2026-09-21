namespace Renty.Application.DTOs.GetUser
{
    public class GetEditUserProfileResponse
    {
        public EditUserProfileInputDto Input { get; set; } = new();
        public List<LanguageOptionDto> AvailableLanguages { get; set; } = new();
    }


}
