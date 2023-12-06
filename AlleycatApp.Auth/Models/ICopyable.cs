namespace AlleycatApp.Auth.Models
{
    public interface ICopyable<in T> where T : class
    {
        void CopyTo(T item);
    }
}
