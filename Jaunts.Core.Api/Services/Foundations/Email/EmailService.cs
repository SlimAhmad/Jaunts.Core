using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Email;
using Jaunts.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
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
        private readonly IEmailTemplateSender emailTemplateSender;
        private readonly IConfiguration configuration;

        public EmailService(
            IEmailBroker emailBroker,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailTemplateSender emailTemplateSender,
            IDateTimeBroker dateTimeBroker)
        {
            this.emailBroker = emailBroker;
            this.configuration = configuration;
            this.userManager = userManager;
            this.emailTemplateSender = emailTemplateSender;
            this.dateTimeBroker = dateTimeBroker;

        }

     
        public async ValueTask<SendEmailResponse> PostVerificationMailRequestAsync(
            ApplicationUser user, string subject = "Verify Your Email - Jaunts")
        {

            string verificationUrl = await GenerateConfirmationUrlAsync(user);
            SendEmailDetails emailDetails =  ConvertEmailDetailsRequest(user, subject);
            SendEmailDetails emailDetailResponse =  ConvertToVerificationTokenEmailAsync(
                emailDetails,
                "Verify Email",
                $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
                "Verify Email",
                verificationUrl
                );
            return await emailBroker.PostMailAsync(emailDetails);

        }

        public async ValueTask<SendEmailResponse> PostForgetPasswordMailRequestAsync(ApplicationUser user, string subject = "Password Verification Token")
        {

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            SendEmailDetails emailDetails = ConvertEmailDetailsRequest(user, "Forgot Password Verification Token - Jaunts");
            SendEmailDetails emailDetailResponse =  ConvertToForgotPasswordCodeEmailAsync(
                emailDetails,
                "Verify Token",
                $"Hi {user.FirstName + " " + user.LastName ?? "stranger"},",
                "Thanks for creating an account with us.<br/>To continue please use token verify with us.",
                $"Verification Token : {token}",
                token
                );
            return await emailBroker.PostMailAsync(emailDetails);

        }



        public SendEmailDetails ConvertToVerificationTokenEmailAsync(
          SendEmailDetails details, string title, string content1,
          string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Jaunts.Core.Api.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = reader.ReadToEnd();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Html = templateText;
        
       
            // Send email
            return details;
        }

        public SendEmailDetails ConvertToForgotPasswordCodeEmailAsync(
            SendEmailDetails details, string title, string content1,
            string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Jaunts.Core.Api.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = reader.ReadToEnd();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Html = templateText;
    

            // Send email
            return details;
        }

        private SendEmailDetails ConvertEmailDetailsRequest(ApplicationUser user, string subject)
        {
  
            return new SendEmailDetails
            {
                From = new SendEmailDetails.FromResponse
                {
                    Email = this.configuration.GetSection("MailTrap:Email").Value,
                    Name = this.configuration.GetSection("MailTrap:Name").Value,
                },
                To = new List<SendEmailDetails.ToResponse> { new SendEmailDetails.ToResponse
               {
                    Name = user.FirstName + " " + user.LastName,
                    Email = user.Email,
               } },
               Subject = subject

            };
        }

        


        #region EmailSender

        public async ValueTask<string> GenerateConfirmationUrlAsync(ApplicationUser user)
        {

            var userIdentity = await userManager.FindByNameAsync(user.UserName);
            var emailVerificationCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = $"http://localhost:7040/api/verify/email/?userId={HttpUtility.UrlEncode(userIdentity.Id.ToString())}&emailToken={HttpUtility.UrlEncode(emailVerificationCode)}";

            return confirmationUrl;

        }


        #endregion



    }
}
