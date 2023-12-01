using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaunts.Core.Models.Email
{
    public interface IEmailTemplateSender
    {
        ValueTask<SendEmailMessage>  SendVerificationEmailAsync(
           SendEmailMessage details, string title, string content1,
            string content2, string buttonText, string buttonUrl);
    }
}
