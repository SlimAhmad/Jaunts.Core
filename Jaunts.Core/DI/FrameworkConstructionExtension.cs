using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Brokers.UserManagement;

namespace Jaunts.Core.Api.DI
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
       
        public static IServiceCollection AddBrokers(this IServiceCollection services)
        {
            services.AddScoped<IUserManagementBroker, UserManagementBroker>();
            services.AddScoped<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();

            return services;
        }


        public static IServiceCollection AddFoundationServices(this IServiceCollection services)
        {
            //services.AddTransient<IStudentService, StudentService>();

            return services;
        }
    }
}
