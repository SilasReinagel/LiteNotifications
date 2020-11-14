using System.Threading.Tasks;

namespace LiteNotifications.WebApi._Common
{
    public interface IPost<T>
    {
        Task Post(T value);
    }
}
