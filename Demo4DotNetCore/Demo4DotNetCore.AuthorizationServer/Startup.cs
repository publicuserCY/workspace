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
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            System.IO.Directory.SetCurrentDirectory(Environment.ContentRootPath);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<AspNetIdentityContext>(options => options.UseSqlite(connectionString));
            services.AddScoped<Service.IApiResourceService, Service.ApiResourceService>();
            services.AddScoped<Service.IApiScopeService, Service.ApiScopeService>();
            services.AddScoped<Service.IApiSecretService, Service.ApiSecretService>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(options =>
            {
                options.RequireHttpsPermanent = false;
                var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    {
                        //options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                        options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
                        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            //services.Configure<IISOptions>(options =>
            //    {
            //        options.AuthenticationDisplayName = "Windows";
            //        options.AutomaticAuthentication = false;
            //    });
            services.AddIdentityServer(options =>
                 {
                     //options.Events.RaiseErrorEvents = true;
                     //options.Events.RaiseInformationEvents = true;
                     //options.Events.RaiseFailureEvents = true;
                     //options.Events.RaiseSuccessEvents = true;
                     options.UserInteraction.LoginUrl = "/account/login";
                     options.UserInteraction.ErrorUrl = "/home/error";
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

                     // this enables automatic token cleanup. this is optional.
                     // options.EnableTokenCleanup = true;
                     // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                     // options.TokenCleanupInterval = 15; 
                 });
                // .AddResourceOwnerValidator<Service.ResourceOwnerPasswordValidator>();
            //services.AddAuthorization();
            services.AddAuthentication(IdentityServer4.AccessTokenValidation.IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    //options.ApiName = "resapi";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseExceptionHandler("/Error");
                //app.UseHsts();
                //app.UseHttpsRedirection();
            }


            app.UseCors(corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=account}/{action=login}/{id?}");
            });
        }
    }
}

