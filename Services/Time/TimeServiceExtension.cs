//  регистрация службы

using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace ASP_P42.Services.Time
{
    public static class TimeServiceExtension
    {
        public static IServiceCollection AddTimeService (
            this IServiceCollection services)
        {
            services.AddSingleton<ITimeService, TimeService>();

            return services;
        }
    }
}
