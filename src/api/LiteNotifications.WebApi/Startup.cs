using LiteMediator;
using LiteNotifications.WebApi._Common;
using LiteNotifications.WebApi.Controllers;
using LiteNotifications.WebApi.Domain;
using LiteNotifications.WebApi.Email;
using LiteNotifications.WebApi.Infrastructure.Slack;
using LiteNotifications.WebApi.Persistence;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LiteNotifications" });
            });
            services.AddSingleton(s => new SlackClient(new EnvironmentVariable("SlackToken").Get()));
            services.AddSingleton<SlackPostMessage>();
            services.AddSingleton<SlackGetChannel>();
            services.AddSingleton<SlackGetUser>();
            services.AddSingleton<SlackChannelChannel>();
            services.AddSingleton<SlackUserChannel>();

            services.AddScoped<Io>(s => new JsonStoreIo());
            services.AddScoped(s => new UserOutletsPersistence(s.GetRequiredService<Io>(), "88de76ffc0fabba642d0836b4a986c261952a0e1aefbaa8863e5dd89c595275e"));
            services.AddScoped(s => new SubscriptionsPersistence(s.GetRequiredService<Io>(), "567db3f221ac7e95567e47413d4cec051e4fa031db09ca52d86f369ec4dd09f8"));
            services.AddSingleton(s => new Channels
            {
                //{"sms", new TwilioSmsChannel(new SmsClient())},
                {"email", new EmailChannel(new EmailClient(new EnvironmentVariablesConfig()))},
                {"slackchannel", s.GetRequiredService<SlackChannelChannel>()},
                {"slackUser", s.GetRequiredService<SlackUserChannel>()},
            });
            services.AddScoped(s => new UglyPublishNotificationFirstDraft("", s.GetRequiredService<SubscriptionsPersistence>(), // TODO: Configure public URL
                s.GetRequiredService<UserOutletsPersistence>(),
                s.GetRequiredService<Channels>()));
           
            
            services.AddSingleton(_ =>
            {
                var handler = new AsyncMediator();
                handler.Register<LoginUserRequest, LoginResponse>(s => new LoginResponse {Token = "SampleToken"});
                handler.Register<RegisterUserRequest, LoginResponse>(s => new LoginResponse {Token = "SampleToken"});
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
