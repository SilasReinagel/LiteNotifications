using Carvana;

namespace LiteNotifications.WebApi
{
    public interface IDelete<T>
    {
        Result Remove(T item);
    }
}
