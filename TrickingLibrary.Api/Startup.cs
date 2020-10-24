using System.Threading.Channels;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Data;

namespace TrickingLibrary.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private const string NuxtJsApp = "NuxtJsApp";

        public Startup(
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));

            AddIdentity(services);

            services.AddControllers();

            services.AddRazorPages();

            services.AddHostedService<VideoEditingBackgroundService>()
                .AddSingleton(_ => Channel.CreateUnbounded<EditVideoMessage>())
                .AddScoped<VersionMigrationContext>()
                .AddTransient<CommentCreationContext>()
                .AddFileManager(_config)
                .AddCors(options => options.AddPolicy(NuxtJsApp, build => build
                    .AllowAnyHeader()
                    .WithOrigins("https://localhost:3000")
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

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });
        }

        private void AddIdentity(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(config =>
                config.UseInMemoryDatabase("DevIdentity"));

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
                        //todo configure for production
                    }
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/api/auth/logout";
            });

            var identityServerBuilder = services.AddIdentityServer();

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();

            if (_env.IsDevelopment())
            {
                identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResource(TrickingLibraryConstants.IdentityResources.RoleScope,
                        new[] {TrickingLibraryConstants.Claims.Role}),
                });

                identityServerBuilder.AddInMemoryApiScopes(new ApiScope[]
                {
                    new ApiScope(IdentityServerConstants.LocalApi.ScopeName,
                        new[]
                        {
                            JwtClaimTypes.PreferredUserName,
                            TrickingLibraryConstants.Claims.Role
                        }),
                });

                identityServerBuilder.AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "web-client",
                        AllowedGrantTypes = GrantTypes.Code,

                        RedirectUris = new[]
                        {
                            "https://localhost:3000/oidc/sign-in-callback.html",
                            "https://localhost:3000/oidc/sign-in-silent-callback.html"
                        },
                        PostLogoutRedirectUris = new[] {"https://localhost:3000"},
                        AllowedCorsOrigins = new[] {"https://localhost:3000"},

                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.LocalApi.ScopeName,
                            TrickingLibraryConstants.IdentityResources.RoleScope
                        },

                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,
                        RequireClientSecret = false,
                    },
                });

                identityServerBuilder.AddDeveloperSigningCredential();
            }

            services.AddLocalApiAuthentication();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TrickingLibraryConstants.Policies.Mod, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim(TrickingLibraryConstants.Claims.Role, TrickingLibraryConstants.Roles.Mod)
                );
            });
        }
    }
}