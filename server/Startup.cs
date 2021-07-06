using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShamblesCom.Server.Converters;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Seeders;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
#if DEBUG
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
#endif
                });
            });
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new UTCDateTimeConverter());
            });
            services.AddEntityFrameworkSqlite().AddDbContext<ShamblesDBContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.Cookie.Name = "shambles_auth";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/denied";
                options.LoginPath = "/login";
                options.LogoutPath = "/";
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using(var client = new ShamblesDBContext()) {
                client.Database.Migrate();
                client.Database.EnsureCreated();
                
                Task.Run(async () =>  { await UserManager.Seed(client);
                    await GameSeeder.SeedF12020(client);
                    await GameSeeder.SeedProjectCars2(client);
                }).Wait();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            SPAMiddleware.UseSPAMiddleware(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
        }
    }
}
