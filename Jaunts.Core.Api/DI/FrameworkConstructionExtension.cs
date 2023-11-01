using FluentAssertions.Common;
using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Email.Templates;
using Jaunts.Core.Api.Services.Foundations.Auth;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Email;

namespace Jaunts.Core.Api.DI
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
       
        public static IServiceCollection AddBrokers(this IServiceCollection services)
        {
            services.AddTransient<IUserManagementBroker, UserManagementBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IEmailBroker, EmailBroker>();
            services.AddTransient<IRoleManagementBroker, RoleManagementBroker>();



            return services;
        }


        public static IServiceCollection AddFoundationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddEmailTemplateSender(this IServiceCollection services)
        {
            // Inject the SendGridEmailSender
            services.AddScoped<IEmailTemplateSender, EmailTemplateSender>();

            // Return collection for chaining
            return services;
        }
    }
}
