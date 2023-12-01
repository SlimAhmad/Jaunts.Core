using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Foundations.Email;
using Jaunts.Core.Models.AppSettings;
using Jaunts.Core.Models.Email;
using Microsoft.Extensions.Options;
using System.Web;

namespace Jaunts.Core.Api.Services.Processings.Email
{
    public partial class EmailProcessingService : IEmailProcessingService
    {
      
            private readonly IEmailService  emailService;
            private readonly ILoggingBroker loggingBroker;
            private readonly IEmailTemplateSender emailTemplateSender;
            private readonly IOptions<MailTrap> configuration;

            public EmailProcessingService(
                IEmailService  emailService,
                ILoggingBroker loggingBroker,
                IEmailTemplateSender emailTemplateSender,
                IOptions<MailTrap> configuration

                )
            {
                this.emailService = emailService;
                this.loggingBroker = loggingBroker;
                this.emailTemplateSender = emailTemplateSender;
                this.configuration = configuration;

            }

            public ValueTask<SendEmailResponse> PostVerificationMailRequestAsync(
                ApplicationUser user,
                string token) =>
            TryCatch(async () =>
            {
                ValidateUser(user);
                ValidateSendMail(token);
                string verificationUrl = GenerateConfirmationUrlAsync(user.Id.ToString(), token);
                SendEmailDetails emailDetails = ConvertEmailDetailsRequest(
                    user,
                    "Verify Your Email - Jaunts", 
                    configuration.Value.Email,
                    configuration.Value.Name);
                SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                emailDetails,
               "Verify Email",
               $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
               "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
               "Verify Email",
               verificationUrl
               );
                ValidateMail(sendEmailDetails);
                return await emailService.SendEmailRequestAsync(sendEmailDetails);

            });

            public ValueTask<SendEmailResponse> PostForgetPasswordMailRequestAsync(
                ApplicationUser user,
                string token) =>
            TryCatch(async () => {

                ValidateUser(user);
                ValidateSendMail(token);
                string verificationUrl = GenerateResetPasswordUrlAsync(user.Id.ToString(), token);
                SendEmailDetails emailDetails = ConvertEmailDetailsRequest(user,
                    "Forgot Password Verification Token - Jaunts",
                    configuration.Value.Email, configuration.Value.Name);
                SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                      emailDetails,
                    "Verify Token",
                    $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                    "Thanks for creating an account with us.<br/>To continue please use token verify with us.",
                    $"Verification Token",
                    verificationUrl
                    );
                ValidateMail(sendEmailDetails);
                return await emailService.SendEmailRequestAsync(sendEmailDetails);


            });

            public ValueTask<SendEmailResponse> PostOTPVerificationMailRequestAsync(
              ApplicationUser user, string token) =>
            TryCatch(async () =>
            {
                ValidateUser(user);
                ValidateSendMail(token);
                string verificationUrl = GenerateOTPLoginUrlAsync(user.Id.ToString(), token);
                SendEmailDetails emailDetails = ConvertEmailDetailsRequest(
                    user,
                    "Verify Token - Jaunts", 
                    configuration.Value.Email,
                    configuration.Value.Name);
                SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                    emailDetails,
                    "Two Factor Authentication",
                    $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                    "Thanks .<br/>To continue please use the OTP code to login.",
                    $"OTP-{token}",
                    verificationUrl
                    );
                ValidateMail(sendEmailDetails);
                return await emailService.SendEmailRequestAsync(sendEmailDetails);

            });

            private SendEmailDetails ConvertEmailDetailsRequest(ApplicationUser user, string subject, string from, string fromName)
            {

                return new SendEmailDetails
                {
                    From = new SendEmailDetails.FromResponse
                    {
                        Email = from,
                        Name = fromName,
                    },
                    To = new List<SendEmailDetails.ToResponse> { new SendEmailDetails.ToResponse
               {
                    Name = user.FirstName + " " + user.LastName,
                    Email = user.Email,
               } },
                    Subject = subject

                };
            }

            #region EmailSender Url

            public string GenerateConfirmationUrlAsync(string id, string token) =>
                    $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)}&emailToken={HttpUtility.UrlEncode(token)}";
            public string GenerateResetPasswordUrlAsync(string id, string token) =>
                 $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)} &emailToken= {HttpUtility.UrlEncode(token)}";
            public string GenerateOTPLoginUrlAsync(string id, string code) =>
                 $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)}&emailToken={HttpUtility.UrlEncode(code)}";



            #endregion



        
    }
}
