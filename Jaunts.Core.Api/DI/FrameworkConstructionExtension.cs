using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Email.Templates;
using Jaunts.Core.Api.Services.Aggregations.Account;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.Role;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.Jwt;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.Email;

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
            services.AddTransient<ISignInManagementBroker, SignInManagementBroker>();


            return services;
        }


        public static IServiceCollection AddFoundationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
          

            return services;
        }

        public static IServiceCollection AddProcessingServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailProcessingService, EmailProcessingService>();
            services.AddTransient<IUserProcessingService, UserProcessingService>();
            services.AddTransient<IJwtProcessingService, JwtProcessingService>();
            services.AddTransient<IRoleProcessingService, RoleProcessingService>();

            return services;
        }

        public static IServiceCollection AddOrchestrationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailOrchestrationService, EmailOrchestrationService>();
            services.AddTransient<IRoleOrchestrationService, RoleOrchestrationService>();
            services.AddTransient<IUserOrchestrationService, UserOrchestrationService>();
            services.AddTransient<IJwtOrchestrationService, JwtOrchestrationService>();


            return services;
        }

        public static IServiceCollection AddAggregationServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountAggregationService, AccountAggregationService>();

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
