using System.Threading.Tasks;

namespace LiteNotifications.WebApi._Common
{
    public interface IExternal <TKey, TValue>
    {
        Task<TValue> Get(TKey key);
    }
}
