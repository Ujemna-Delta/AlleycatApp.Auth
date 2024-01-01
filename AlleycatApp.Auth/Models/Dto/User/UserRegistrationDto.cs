namespace AlleycatApp.Auth.Models.Dto.User
{
    public class UserRegistrationDto<TDto> where TDto : UserDto
    {
        public TDto User { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
    }
}
