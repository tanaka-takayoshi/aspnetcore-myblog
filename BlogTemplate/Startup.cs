using My_Blog.Data;
using My_Blog.Models;
using My_Blog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace My_Blog
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
            var appinsightsKey = Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
            if (!string.IsNullOrEmpty(appinsightsKey))
                services.AddApplicationInsightsTelemetry(options =>
                {
                    options.ApplicationVersion = "v0.1.0-theme-b";
                    options.InstrumentationKey = appinsightsKey;
                });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["BLOGDB_CONNECTION_STRING"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSingleton<IFileSystem, PhysicalFileSystem>();
            services.AddSingleton<BlogDataStore>();

            services.AddSingleton<SlugGenerator>();
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);
            services.AddSingleton<ExcerptGenerator>(excerptGenerator);
            services.AddSingleton<MarkdownRenderer>();
        }

        private const string XForwardedPathBase = "X-Forwarded-PathBase";
        private const string XForwardedProto = "X-Forwarded-Proto";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //workaround https://github.com/aspnet/Docs/issues/2384#issuecomment-286146843
            app.Use((context, next) =>
            {
                if (context.Request.Headers.TryGetValue(XForwardedPathBase, out StringValues pathBase))
                {
                    context.Request.PathBase = new PathString(pathBase);

                }

                if (context.Request.Headers.TryGetValue(XForwardedProto, out StringValues proto))
                {
                    context.Request.Protocol = proto;
                }

                return next();
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
