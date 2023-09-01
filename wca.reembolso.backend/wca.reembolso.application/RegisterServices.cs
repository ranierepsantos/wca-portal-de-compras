using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using wca.reembolso.application.Common;

namespace wca.reembolso.application
{
    public static class RegisterServices
    {
        public static void ConfigureApplication(this IServiceCollection services,
                                                IConfiguration configuration)
        {
            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            HandleFile.Configure(configuration);
        }

        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }

}
