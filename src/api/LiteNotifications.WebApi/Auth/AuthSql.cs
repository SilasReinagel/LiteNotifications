using System.Linq;
using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi.Infrastructure.Sql;

namespace LiteNotifications.WebApi.Auth
{
    public sealed class AuthSql : IExternal<RegisterUserRequest, int>, IExternal<LoginUserRequest, LoginResponse>
    {
        private readonly string _secret;
        private readonly DapperSqlDb _db;
        
        public AuthSql(string secret, DapperSqlDb db)
        {
            _secret = secret;
            _db = db;
        }

        private Task<bool> EmailIsAvailable(string email) 
            => _db.Query<string>("SELECT UserId FROM NotifyApp.Users WHERE Email = @email", new { email })
                .Then(x => !x.Any())
                .OrDefault(() => false);

        public async Task<Result<int>> Get(RegisterUserRequest req)
        {
            if (!await EmailIsAvailable(req.Email))
                return Result.Errored<int>(ResultStatus.Conflict, "Email Address is already registered");

            return await _db.QuerySingle<int>(
                @"INSERT INTO NotifyApp.Users(Email, Password)
                        VALUES(@email, @password) 
                        SELECT SCOPE_IDENTITY()",
                new {email = req.Email, password = BCrypt.Net.BCrypt.HashPassword(req.Password)});
        }
        
        public Task<Result<LoginResponse>> Get(LoginUserRequest req) 
            =>  _db.QuerySingle<AccountRecord>(@"SELECT UserId, Password FROM NotifyApp.Users WHERE IsDisabled = 0 AND Email = @email", new { email = req.Email })
                .Then(x => BCrypt.Net.BCrypt.Verify(req.Password, x.Password)
                    ? Result.Success(x)
                    : Result.Errored<AccountRecord>(ResultStatus.InvalidRequest, "Unauthorized"))
                .Then(x => new LoginResponse { Token = Jwt.CreateToken(_secret, x.UserId.ToString().PadLeft(16), req.Email)});
        
        private class AccountRecord
        {
            public int UserId { get; set; }
            public string Password { get; set; }
        }
    }
}
