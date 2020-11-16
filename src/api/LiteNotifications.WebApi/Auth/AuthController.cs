using System.Threading.Tasks;
using LiteMediator;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi.Auth
{
    public sealed class RegisterUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginResponse
    {
        public string Token { get; set; }
    }
    
    [Route("api/[controller]")]
    public sealed class AuthController : Controller
    {
        private readonly AsyncMediator _handler;

        public AuthController(AsyncMediator handler) => _handler = handler;
        
        [HttpPost, Route("register")] public Task<IActionResult> Register([FromBody]RegisterUserRequest req) => _handler.Handle(req);
        [HttpPost, Route("login")] public Task<IActionResult> Register([FromBody]LoginUserRequest req) => _handler.Handle(req);
    }
}
