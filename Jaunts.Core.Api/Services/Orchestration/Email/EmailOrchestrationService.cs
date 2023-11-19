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
        private readonly IEmailService emailService;
        

        public EmailOrchestrationService(
            IEmailProcessingService emailProcessingService,
            IUserProcessingService userProcessingService,
            ILoggingBroker loggingBroker,
            IEmailService emailService
           )
        {
            this.emailProcessingService = emailProcessingService;
            this.userProcessingService = userProcessingService;
            this.loggingBroker = loggingBroker;
            this.emailService = emailService;
        }

        public ValueTask<SendEmailResponse> VerificationMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.EmailConfirmationTokenAsync(user);
            return await emailProcessingService.PostVerificationMailRequestAsync(user,token);
        });

        public ValueTask<SendEmailResponse> PasswordResetMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.PasswordResetTokenAsync(user);
            return await emailProcessingService.PostForgetPasswordMailRequestAsync(user, token);
        });

        public ValueTask<SendEmailResponse> TwoFactorMailAsync(
                ApplicationUser user) =>
        TryCatch(async () =>
        {
            string token = await userProcessingService.TwoFactorTokenAsync(user);
            return await emailProcessingService.PostOTPVerificationMailRequestAsync(user, token);
        });
    }
}
