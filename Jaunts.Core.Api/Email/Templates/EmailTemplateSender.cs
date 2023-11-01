using Jaunts.Core.Api.Brokers.EmailBroker;
using Jaunts.Core.Email;
using Jaunts.Core.Models.Email;
using System.Reflection;
using System.Text;

namespace Jaunts.Core.Api.Email.Templates
{
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public  SendEmailDetails SendGeneralEmailAsync(
            SendEmailDetails details, string title, string content1,
            string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Jaunts.Core.Api.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText =  reader.ReadToEnd();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Text = templateText;

            // Send email
            return details; 
        }

    }
}
