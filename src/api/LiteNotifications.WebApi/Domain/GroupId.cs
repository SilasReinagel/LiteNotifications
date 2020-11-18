namespace LiteNotifications.WebApi.Domain
{
    public sealed class GroupId
    {
        private int Value { get; }

        public GroupId(int id) => Value = id;

        public static implicit operator int(GroupId id) => id.Value;
        public static implicit operator GroupId(int id) => new GroupId(id);
    }
}
