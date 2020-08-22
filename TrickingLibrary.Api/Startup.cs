using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrickingLibrary.Api.BackgroundServices;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Data;

namespace TrickingLibrary.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private const string AllCors = "All";

        public Startup(IWebHostEnvironment env)
        {
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
                .AddSingleton<VideoManager>()
                .AddCors(options => options.AddPolicy(AllCors, build => build.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(AllCors);

            app.UseRouting();

            app.UseAuthentication();

            app.UseIdentityServer();

            // auth here

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
            });

            var identityServerBuilder = services.AddIdentityServer();

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();

            if (_env.IsDevelopment())
            {
                identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                });

                identityServerBuilder.AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "web-client",
                        AllowedGrantTypes = GrantTypes.Code,

                        RedirectUris = new[] {"http://localhost:3000"},
                        PostLogoutRedirectUris = new[] {"http://localhost:3000"},
                        AllowedCorsOrigins = new[] {"http://localhost:3000"},

                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,
                        RequireClientSecret = false,
                    },
                });

                identityServerBuilder.AddDeveloperSigningCredential();
            }
        }
    }
}