namespace AlleycatApp.Auth.Models.Dto.User
{
    public class AttendeeDto : NamedUserDto
    {
        public string Nickname { get; set; } = string.Empty;
        public string Marks { get; set; } = string.Empty;
    }
}
