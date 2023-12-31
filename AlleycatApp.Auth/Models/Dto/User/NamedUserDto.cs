namespace AlleycatApp.Auth.Models.Dto.User
{
    public class NamedUserDto : UserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
