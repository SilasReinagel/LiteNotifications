using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi.Auth;
using LiteNotifications.WebApi.Tenancy;

namespace LiteNotifications.WebApi.UseCases
{
    public sealed class CreateNewUserAccount : IExternal<RegisterUserRequest, Unit>
    {
        private readonly IExternal<RegisterUserRequest, int> _createUser;
        private readonly IExternal<CreateGroupRequest, int> _createGroup;
        private readonly IExternal<AddUserToGroupRequest, Unit> _addUserToGroup;

        public CreateNewUserAccount(IExternal<RegisterUserRequest, int> createUser, IExternal<CreateGroupRequest, int> createGroup, IExternal<AddUserToGroupRequest, Unit> addUserToGroup)
        {
            _createUser = createUser;
            _createGroup = createGroup;
            _addUserToGroup = addUserToGroup;
        }

        public Task<Result<Unit>> Get(RegisterUserRequest key)
            => _createUser.Get(key)
                .ThenConcatWith(userId => _createGroup.Get(new CreateGroupRequest {GroupName = $"{key.Email} Personal"}))
                .Then(ctx => _addUserToGroup.Get(new AddUserToGroupRequest {UserId = ctx.Item1, GroupId = ctx.Item2}))
                .Then(_ => new Unit());
    }
}
