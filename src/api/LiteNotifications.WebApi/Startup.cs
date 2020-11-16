using Carvana;
using LiteMediator;
using LiteNotifications.WebApi.Auth;
using LiteNotifications.WebApi.Domain;
using LiteNotifications.WebApi.Email;
using LiteNotifications.WebApi.Infrastructure.Slack;
using LiteNotifications.WebApi.Infrastructure.Sql;
using LiteNotifications.WebApi.Persistence;
using LiteNotifications.WebApi.Tenancy;
using LiteNotifications.WebApi.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LiteNotifications.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection svc)
        {
            svc.AddControllers();
            svc.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LiteNotifications" });
            });
            svc.AddSingleton(s => new SlackClient(new EnvironmentVariable("SlackToken").Get()));
            svc.AddSingleton<SlackPostMessage>();
            svc.AddSingleton<SlackGetChannel>();
            svc.AddSingleton<SlackGetUser>();
            svc.AddSingleton<SlackChannelChannel>();
            svc.AddSingleton<SlackUserChannel>();

            svc.AddScoped<Io>(s => new JsonStoreIo());
            svc.AddScoped(s => new UserOutletsPersistence(s.GetRequiredService<Io>(), "88de76ffc0fabba642d0836b4a986c261952a0e1aefbaa8863e5dd89c595275e"));
            svc.AddScoped(s => new SubscriptionsPersistence(s.GetRequiredService<Io>(), "567db3f221ac7e95567e47413d4cec051e4fa031db09ca52d86f369ec4dd09f8"));
            svc.AddSingleton(s => new Channels
            {
                //{"sms", new TwilioSmsChannel(new SmsClient())},
                {"email", new EmailChannel(new EmailClient(new EnvironmentVariablesConfig()))},
                {"slackchannel", s.GetRequiredService<SlackChannelChannel>()},
                {"slackUser", s.GetRequiredService<SlackUserChannel>()},
            });
            svc.AddScoped(s => new UglyPublishNotificationFirstDraft("", s.GetRequiredService<SubscriptionsPersistence>(), // TODO: Configure public URL
                s.GetRequiredService<UserOutletsPersistence>(),
                s.GetRequiredService<Channels>()));

            svc.AddScoped(_ => new DapperSqlDb(new EnvironmentVariable("NotifyAppSqlConnection")));
            svc.AddScoped<AuthSql>(s => new AuthSql("S", s.GetRequiredService<DapperSqlDb>()));
            svc.AddScoped<TenancySql>();
            svc.AddScoped<CreateNewUserAccount>();
            svc.AddScoped<IExternal<RegisterUserRequest, int>>(s => s.GetRequiredService<AuthSql>());
            svc.AddScoped<IExternal<LoginUserRequest, LoginResponse>>(s => s.GetRequiredService<AuthSql>());
            svc.AddScoped<IExternal<RegisterUserRequest, Unit>, CreateNewUserAccount>();
            svc.AddScoped<IExternal<CreateGroupRequest, int>, TenancySql>();
            svc.AddScoped<IExternal<AddUserToGroupRequest, Unit>, TenancySql>();
            
            svc.AddScoped(s =>
            {
                var handler = new AsyncMediator();
                handler.Register<LoginUserRequest, Result<LoginResponse>>(r => s.GetRequiredService<IExternal<LoginUserRequest, LoginResponse>>().Get(r));
                handler.Register<RegisterUserRequest, Result<Unit>>(r => s.GetRequiredService<IExternal<RegisterUserRequest, Unit>>().Get(r));
                return handler;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
