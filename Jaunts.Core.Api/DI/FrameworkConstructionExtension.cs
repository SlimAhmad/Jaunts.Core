using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.RoleManagement;
using Jaunts.Core.Api.Brokers.SignInManagement;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Email.Templates;
using Jaunts.Core.Api.Services.Aggregations.Account;
using Jaunts.Core.Api.Services.Foundations.Adverts;
using Jaunts.Core.Api.Services.Foundations.Amenities;
using Jaunts.Core.Api.Services.Foundations.Customers;
using Jaunts.Core.Api.Services.Foundations.Drivers;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Foundations.Fleets;
using Jaunts.Core.Api.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Services.Foundations.Jwt;
using Jaunts.Core.Api.Services.Foundations.Packages;
using Jaunts.Core.Api.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Services.Foundations.ProviderCategories;
using Jaunts.Core.Api.Services.Foundations.Providers;
using Jaunts.Core.Api.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Services.Foundations.Rides;
using Jaunts.Core.Api.Services.Foundations.Role;
using Jaunts.Core.Api.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Services.Foundations.SignIn;
using Jaunts.Core.Api.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Services.Foundations.Transactions;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Services.Foundations.Wallets;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Orchestration.Jwt;
using Jaunts.Core.Api.Services.Orchestration.Role;
using Jaunts.Core.Api.Services.Orchestration.SignIn;
using Jaunts.Core.Api.Services.Orchestration.User;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.Jwt;
using Jaunts.Core.Api.Services.Processings.Role;
using Jaunts.Core.Api.Services.Processings.SignIn;
using Jaunts.Core.Api.Services.Processings.TransactionFees;
using Jaunts.Core.Api.Services.Processings.Transactions;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Api.Services.Processings.WalletBalances;
using Jaunts.Core.Api.Services.Processings.Wallets;
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
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IWalletBalanceService, WalletBalanceService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITransactionFeeService, TransactionFeeService>();
            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IAmenityService, AmenityService>();
            services.AddTransient<IProviderCategoryService, ProviderCategoryService>();
            services.AddTransient<IProvidersDirectorService, ProvidersDirectorService>();
            services.AddTransient<IDriverService, DriverService>();
            services.AddTransient<IFleetService, FleetService>();
            services.AddTransient<IFlightDealService, FlightDealService>();
            services.AddTransient<IPackageService, PackageService>();
            services.AddTransient<IPromosOfferService, PromosOfferService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IProviderServiceService, ProviderServiceService>();
            services.AddTransient<IRideService, RideService>();
            services.AddTransient<IShortLetService, ShortLetService>();
            services.AddTransient<ICustomerService, CustomerService>();


            return services;
        }

        public static IServiceCollection AddProcessingServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailProcessingService, EmailProcessingService>();
            services.AddTransient<IUserProcessingService, UserProcessingService>();
            services.AddTransient<IJwtProcessingService, JwtProcessingService>();
            services.AddTransient<ISignInProcessingService, SignInProcessingService>();
            services.AddTransient<IRoleProcessingService, RoleProcessingService>();
            services.AddTransient<IWalletProcessingService, WalletProcessingService>();
            services.AddTransient<IWalletBalanceProcessingService, WalletBalanceProcessingService>();
            services.AddTransient<ITransactionProcessingService, TransactionProcessingService>();
            services.AddTransient<ITransactionFeeProcessingService, TransactionFeeProcessingService>();

            return services;
        }

        public static IServiceCollection AddOrchestrationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailOrchestrationService, EmailOrchestrationService>();
            services.AddTransient<IRoleOrchestrationService, RoleOrchestrationService>();
            services.AddTransient<IUserOrchestrationService, UserOrchestrationService>();
            services.AddTransient<ISignInOrchestrationService, SignInOrchestrationService>();
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
