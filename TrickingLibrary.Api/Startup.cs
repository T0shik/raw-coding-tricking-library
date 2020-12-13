using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrickingLibrary.Api.BackgroundServices.SubmissionVoting;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Services.Email;
using TrickingLibrary.Api.Services.Storage;
using TrickingLibrary.Data;
using TrickingLibrary.Data.VersionMigrations;

namespace TrickingLibrary.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private const string NuxtJsApp = "NuxtJsApp";

        public Startup(IConfiguration config,
            IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_config.GetConnectionString("Default")));

            AddIdentity(services);

            services.AddControllers()
                .AddFluentValidation(x => x
                    .RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.AddRazorPages();

            services.Configure<ModerationItemReviewContext.ModerationSettings>(_config.GetSection(nameof(ModerationItemReviewContext.ModerationSettings)));
            services.Configure<SendGridOptions>(_config.GetSection(nameof(SendGridOptions)));


            services.AddHostedService<VideoEditingBackgroundService>()
                .AddSingleton(_ => Channel.CreateUnbounded<EditVideoMessage>())
                .AddScoped<VersionMigrationContext>()
                .AddScoped<EmailClient>()
                .AddSingleton<ModerationItemReviewContext>()
                .AddSingleton<ISubmissionVoteSink, SubmissionVotingService>()
                .AddHostedService(provider => (SubmissionVotingService) provider.GetRequiredService<ISubmissionVoteSink>())
                .AddTransient<CommentCreationContext>()
                .AddFileServices(_config)
                .AddCors(options => options.AddPolicy(NuxtJsApp, build => build
                    .AllowAnyHeader()
                    .WithOrigins("https://localhost:3000", "https://app.raw-coding.net")
                    .AllowAnyMethod()
                    .AllowCredentials()));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(NuxtJsApp);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });
        }

        private void AddIdentity(IServiceCollection services)
        {
            // services.AddDbContext<IdentityDbContext>(config =>
            //     config.UseInMemoryDatabase("DevIdentity"));
            services.AddDbContext<ApiIdentityDbContext>(config =>
                config.UseNpgsql(_config.GetConnectionString("Default")));

            services.AddDataProtection()
                .SetApplicationName("TrickingLibrary")
                .PersistKeysToDbContext<ApiIdentityDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    if (_env.IsDevelopment())
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 4;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                    else
                    {
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                })
                .AddEntityFrameworkStores<ApiIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/api/auth/logout";
                config.Cookie.Domain = _config["CookieDomain"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TrickingLibraryConstants.Policies.Mod, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim(TrickingLibraryConstants.Claims.Role,
                        TrickingLibraryConstants.Roles.Mod,
                        TrickingLibraryConstants.Roles.Admin)
                );
                options.AddPolicy(TrickingLibraryConstants.Policies.Admin, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim(TrickingLibraryConstants.Claims.Role,
                        TrickingLibraryConstants.Roles.Admin)
                );
            });
        }
    }
}