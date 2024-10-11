using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YGZ.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assemply = Assembly.GetExecutingAssembly();

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assemply));

            return services;
        }
    }
}
