using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspcoreIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspcoreIdentity
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
            services.AddMvc();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            var connectionString = Configuration.GetConnectionString("Default");
            var currentAsmName = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddDbContext<IdentityDbContext>(option =>
            {
                option.UseSqlServer(connectionString,obj=> obj.MigrationsAssembly(currentAsmName));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 3,
                    RequiredUniqueChars = 3,
                    RequireLowercase = false
                };
                option.User = new UserOptions
                {
                    RequireUniqueEmail = true

                };
                option.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = false,
                    RequireConfirmedPhoneNumber = false
                };
                option.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers = false,
                    DefaultLockoutTimeSpan = new TimeSpan(0, 15, 0),
                    MaxFailedAccessAttempts = 3
                };
            }).AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
