using Demo.BLL.Interfaces;
using Demo.BLL.Resitories;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
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
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddControllersWithViews();
            services.AddDbContext<MvcProjectDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("default")
                    );
            },ServiceLifetime.Scoped);

            services.AddScoped<Services.ProductService>();
            services.AddScoped<OrderService>();
            services.AddScoped<PaymentService>();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddScoped<CartService>();

            //services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            //StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            //////
            //services.AddAuthentication();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MvcProjectDbContext>()
                .AddDefaultTokenProviders();

            //Dendancy Injection
            services.AddTransient<IProductRepo, ProductRepo>(); //configure services
            services.AddTransient<ICategoryRepo, CategoryRepo>();
            services.AddTransient<ICourseRepo, CourseRepo>();




            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(Options =>
               {
                   Options.LoginPath = "Account/Login";
                   Options.AccessDeniedPath = "Home/Error";
               });
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

            app.UseSession();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var stripeSettings = Configuration.GetSection("Stripe").Get<StripeSettings>();
            //StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            StripeConfiguration.ApiKey = stripeSettings.ApiKey;
        }
    }
}
