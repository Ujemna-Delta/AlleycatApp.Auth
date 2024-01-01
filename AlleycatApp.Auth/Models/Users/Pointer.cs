namespace AlleycatApp.Auth.Models.Users
{
    public class Pointer : NamedUser
    {
        public int? PointId { get; set; }
        public Point? Point { get; set; }
    }
}
