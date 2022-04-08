using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ArkdBarV1.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Http;
using ArkdBarV1.Entities;
using ArkdBarV1.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ArkdBarV1
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true; //default = false
                options.Password.RequireNonAlphanumeric = false; //default = true
                options.Password.RequireUppercase = false; //default = true
                options.Password.RequireLowercase = false; //default = true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //default = 3
                options.Lockout.MaxFailedAccessAttempts = 3; //default = 5
                options.SignIn.RequireConfirmedAccount = false; //default = false
                options.SignIn.RequireConfirmedEmail = false; //Habilitar para deixar a verificação de email funcional
                options.SignIn.RequireConfirmedPhoneNumber = false; // default = false
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
             
            //Configuração do cookie de autenticação
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
                options.SlidingExpiration = true;
            });


            services.AddAuthorization(options =>
            {
                //adiciona uma política de acesso chamada isAdmin
                options.AddPolicy("isAdmin", policy =>
                    policy.RequireRole("admin"));
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Admin", "isAdmin");
                options.Conventions.AuthorizeFolder("/ProdutoCRUD", "isAdmin");
                options.Conventions.AuthorizeFolder("/PedidoAdmin", "isAdmin");
            }).AddCookieTempDataProvider(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Ativar para deixar o serviço de envio de email funcional
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);
        }
    }
}
