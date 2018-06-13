using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Department.Data;
using Department.Models;
using Department.Services;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Hangfire;
using Hangfire.PostgreSql;

namespace Department
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(config =>
              config.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddSession();

            // 发送邮件服务
            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    //发送方信息
                    Server = "smtp.office365.com",
                    Port = 587,
                    SenderName = "社团服务",
                    SenderEmail = "yujohn1997@outlook.com",
                    Security = true,

                    //账号密码
                    Account = "yujohn1997@outlook.com",
                    Password = "YUjun4815162342"
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "Application",
                    template: "{controller=Applications}/{action}/{id?}");
                routes.MapRoute(
                    name: "Activity",
                    template: "{controller=Activities}/{action}/{id?}");
                routes.MapRoute(
                    name: "Account",
                    template: "{controller=Account}/{action}/{id?}");
            });
        }
    }
}
