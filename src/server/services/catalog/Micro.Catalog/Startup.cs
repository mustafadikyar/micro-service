using Micro.Catalog.Services;
using Micro.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Micro.Catalog
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.Authority = Configuration["IdentityServerURL"];
                option.Audience = "resource_catalog"; //Token içerisinde olmak zorunda.
                option.RequireHttpsMetadata = false; //Ssl zorunluluðunu kaldýrýr.
            });

            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICourseService, CourseManager>();

            services.AddAutoMapper(c => c.AddMaps("Micro.Catalog"));

            services.AddControllers(option =>
            {
                option.Filters.Add(new AuthorizeFilter()); //Tüm endpointlere authorize ekler.
            });

            services.Configure<DatabaseSetting>(Configuration.GetSection("DatabaseSetting"));
            services.AddSingleton<IDatabaseSetting>(provider => provider.GetRequiredService<IOptions<DatabaseSetting>>().Value);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Micro.Catalog", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Micro.Catalog v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
