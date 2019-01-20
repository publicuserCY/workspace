using Demo4DotNetCore.AuthorizationServer.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Demo4DotNetCore.AuthorizationServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            System.IO.Directory.SetCurrentDirectory(HostingEnvironment.ContentRootPath);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AspNetIdentityContext>(options => options.UseSqlite(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(options =>
                {
                    options.RequireHttpsPermanent = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<IISOptions>(options =>
                {
                    options.AuthenticationDisplayName = "Windows";
                    options.AutomaticAuthentication = false;
                });
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer(options =>
                 {
                     //options.Events.RaiseErrorEvents = true;
                     //options.Events.RaiseInformationEvents = true;
                     //options.Events.RaiseFailureEvents = true;
                     //options.Events.RaiseSuccessEvents = true;
                     //options.UserInteraction.LoginUrl = "/account/login";
                     //options.UserInteraction.ErrorUrl = "/home/error";
                 })
                 .AddDeveloperSigningCredential()
                 .AddAspNetIdentity<ApplicationUser>()
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                 })
                 .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                 });
            services.AddCors(options =>
            {
                options.AddPolicy("default", corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseCors("default");
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}

