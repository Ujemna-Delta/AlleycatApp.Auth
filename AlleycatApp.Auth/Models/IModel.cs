namespace AlleycatApp.Auth.Models
{
    public interface IModel<TId>
    {
        TId Id { get; set; }
    }
}
