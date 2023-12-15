// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Api.Services.Orchestration.Email;
using Jaunts.Core.Api.Services.Processings.Email;
using Jaunts.Core.Api.Services.Processings.User;
using Jaunts.Core.Models.Email;

namespace Jaunts.Core.Api.Services.Orchestration.Email
{
    public partial class EmailOrchestrationService : IEmailOrchestrationService
    {
        private readonly IEmailProcessingService emailProcessingService;
        private readonly IUserProcessingService userProcessingService;
        private readonly ILoggingBroker loggingBroker;
        

        public EmailOrchestrationService(
            IEmailProcessingService emailProcessingService,
            IUserProcessingService userProcessingService,
            ILoggingBroker loggingBroker
  
           )
        {
            this.emailProcessingService = emailProcessingService;
            this.userProcessingService = userProcessingService;
            this.loggingBroker = loggingBroker;
  
        }

        public ValueTask<SendEmailResponse> VerificationMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.EmailConfirmationTokenAsync(user);
            return await emailProcessingService.SendVerificationMailRequestAsync(user,token);
        });

        public ValueTask<SendEmailResponse> PasswordResetMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.PasswordResetTokenAsync(user);
            return await emailProcessingService.SendForgetPasswordMailRequestAsync(user, token);
        });

        public ValueTask<UserAccountDetailsResponse> TwoFactorMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.TwoFactorTokenAsync(user);
            await emailProcessingService.SendOtpVerificationMailRequestAsync(user, token);
            return ConvertTo2FAResponse(user) ;
        });
        private UserAccountDetailsResponse ConvertTo2FAResponse(ApplicationUser user)
        {
            return new UserAccountDetailsResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
            };
        }
    }
}
