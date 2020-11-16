using System.Threading.Tasks;

namespace LiteNotifications.WebApi
{
    public interface IPost<T>
    {
        Task Post(T value);
    }
}
