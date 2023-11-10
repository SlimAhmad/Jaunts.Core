using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.UserManagement;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Jaunts.Core.Api.Services.Foundations.Email
{
    public partial class EmailService : IEmailService
    {
        private readonly IEmailBroker emailBroker;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IUserManagementBroker userManagementBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IEmailTemplateSender  emailTemplateSender;

        private readonly IConfiguration configuration;

        public EmailService(
            IEmailBroker emailBroker,
            IUserManagementBroker userManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IEmailTemplateSender emailTemplateSender

            )
        {
            this.emailBroker = emailBroker;
            this.userManagementBroker = userManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.emailTemplateSender = emailTemplateSender;
            
        }

     
        public  ValueTask<SendEmailResponse> PostVerificationMailRequestAsync(
            ApplicationUser user, 
            string subject ,
            string token,
            string from,
            string fromName) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateSendMail(subject);
            ValidateSendMail(token);
            ValidateSendMail(from);
            ValidateSendMail(fromName);
            string verificationUrl = GenerateConfirmationUrlAsync(user.Id.ToString(), token);
            SendEmailDetails emailDetails = ConvertEmailDetailsRequest(user, subject,from, fromName);
             SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                 emailDetails,
                "Verify Email",
                $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
                "Verify Email",
                verificationUrl
                );
            ValidateMail(sendEmailDetails);
            return await emailBroker.PostMailAsync(sendEmailDetails);

        });

        public ValueTask<SendEmailResponse> PostForgetPasswordMailRequestAsync(
            ApplicationUser user,
            string subject,
            string token,
            string from,
            string fromName) =>
        TryCatch(async () => {

            ValidateUser(user);
            ValidateSendMail(subject);
            ValidateSendMail(token);
            ValidateSendMail(from);
            ValidateSendMail(fromName);
            string verificationUrl = GenerateResetPasswordUrlAsync(user.Id.ToString(), token);
            SendEmailDetails emailDetails = ConvertEmailDetailsRequest(user, 
                "Forgot Password Verification Token - Jaunts", from, fromName);
            SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                  emailDetails,
                "Verify Token",
                $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please use token verify with us.",
                $"Verification Token",
                verificationUrl
                );
            ValidateMail(sendEmailDetails);
            return await emailBroker.PostMailAsync(sendEmailDetails);

        });

        public  ValueTask<SendEmailResponse> PostOTPVerificationMailRequestAsync(
          ApplicationUser user, string subject, string token, string from, string fromName) =>
        TryCatch(async () =>
        {
            ValidateUser(user);
            ValidateSendMail(subject);
            ValidateSendMail(token);
            ValidateSendMail(from);
            ValidateSendMail(fromName);
            string verificationUrl = GenerateOTPLoginUrlAsync(user.Id.ToString(), token);
            SendEmailDetails emailDetails = ConvertEmailDetailsRequest(user, subject, from, fromName);
            SendEmailDetails sendEmailDetails = await this.emailTemplateSender.SendVerificationEmailAsync(
                emailDetails,
                "Two Factor Authentication",
                $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                "Thanks .<br/>To continue please use the OTP code to login.",
                $"OTP-{token}",
                verificationUrl
                );
            ValidateMail(sendEmailDetails);
            return await emailBroker.PostMailAsync(sendEmailDetails);

        });

        private SendEmailDetails ConvertEmailDetailsRequest(ApplicationUser user, string subject,string from,string fromName)
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

        public string GenerateConfirmationUrlAsync(string id,string token) =>
                $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)}&emailToken={HttpUtility.UrlEncode(token)}";
        public string GenerateResetPasswordUrlAsync(string id, string token) =>
             $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)} &emailToken= {HttpUtility.UrlEncode(token)}";
        public string GenerateOTPLoginUrlAsync(string id,string code) =>
             $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(id)}&emailToken={HttpUtility.UrlEncode(code)}";

          
        
        #endregion



    }
}
