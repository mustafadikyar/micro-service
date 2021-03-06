using Micro.Shared.Services;
using Micro.WebUI.Handlers;
using Micro.WebUI.Helpers;
using Micro.WebUI.Models;
using Micro.WebUI.Services;
using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Micro.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));

            services.AddHttpClient<IIdentityService, IdentityManager>();
            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenManager>();
            
            services.AddScoped<ISharedIdentityService, SharedIdentityManager>();
            services.AddScoped<PhotoHelper>();
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();


            ServiceApiSettings serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
            services.AddHttpClient<IUserService, UserManager>(option => option.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri))
                    .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICatalogService, CatalogManager>(option => option.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}"))
                    .AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IPhotoStockService, PhotoStockManager>(option => option.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}"))
                    .AddHttpMessageHandler<ClientCredentialTokenHandler>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                    {
                        option.LoginPath = "/Account/SignIn";
                        option.ExpireTimeSpan = TimeSpan.FromDays(60);
                        option.SlidingExpiration = true;
                        option.Cookie.Name = "mycookie";
                    });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
