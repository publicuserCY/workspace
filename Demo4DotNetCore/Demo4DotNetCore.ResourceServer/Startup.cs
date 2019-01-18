using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo4DotNetCore.ResourceServer
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
            var identityConnection = Configuration.GetConnectionString("IdentityConnection");
            services.AddIdentityServer()
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlite(identityConnection);
                 })
                 .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlite(identityConnection);
                 });
            services.AddDbContext<Identity.Model.AspNetIdentityContext>(options => options.UseSqlite(identityConnection));
            services.AddDbContext<IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext>(options => options.UseSqlite(identityConnection));
            services.AddDbContext<Books.Model.ResourceContext>(options => { options.UseSqlite(Configuration.GetConnectionString("BooksConnection")); });
            services.AddScoped<Books.Service.IBookService, Books.Service.BookService>();
            services.AddScoped<Identity.Service.IApiResourceService, Identity.Service.ApiResourceService>();
            services.AddScoped<Identity.Service.IApiScopeService, Identity.Service.ApiScopeService>();
            services.AddScoped<Identity.Service.IApiSecretService, Identity.Service.ApiSecretService>();
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
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressConsumesConstraintForFormFileParameters = true;
                //options.SuppressInferBindingSourcesForParameters = true;
                //options.SuppressModelStateInvalidFilter = true;
                //options.SuppressMapClientErrors = true;
            });
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration.GetValue<string>("Authority:Issue");
                    options.ApiName = Configuration.GetValue<string>("Authority:ApiName");
                });
            services.AddCors(options =>
            {
                options.AddPolicy("default", corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseCors("default");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                //routes.MapRoute(name: "areas", template: "api/{area:exists}/{controller}/{action}");
                //routes.MapRoute("default", "api/{controller}/{action}");
            });
        }
    }
}
