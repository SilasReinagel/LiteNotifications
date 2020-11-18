using System.Threading.Tasks;
using Carvana;

namespace LiteNotifications.WebApi
{
    public interface AsyncIo
    {
        Task<Result<Unit>> Put<T>(string key, T data);
        Task<Result<T>> Get<T>(string key);
        Task<Result<bool>> Contains(string key);
        Task<Result<Unit>> Delete(string key);
    }
}
