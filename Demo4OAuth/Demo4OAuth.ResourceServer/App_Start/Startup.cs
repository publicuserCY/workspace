using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(Demo4OAuth.ResourceServer.Startup))]
namespace Demo4OAuth.ResourceServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationOptions());
            //JsonSerializer
            var serializer = new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.None,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            // Web API 路由
            var config = new HttpConfiguration();
            //config.Filters.Add(new AuthorizeAttribute());
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //DI容器
            var apiBuilder = new ContainerBuilder();
            apiBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            apiBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            apiBuilder.Register((c) => serializer);
            apiBuilder.RegisterType<Model.ResourceContext>().InstancePerRequest();
            apiBuilder.RegisterType<Service.BookService>().As<Service.IBookService>().InstancePerRequest();
            apiBuilder.RegisterType<Service.AttachmentsService>().As<Service.IAttachmentService>().InstancePerRequest();
            var apiContainer = apiBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(apiContainer);
            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,and finally the standard Web API middleware.
            app.UseAutofacMiddleware(apiContainer);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}