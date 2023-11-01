using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jaunts.Core.Models.Email
{
    /// <summary>
    /// A response from a SendEmail call for any <see cref="IEmailSender"/> implementation
    /// </summary>
    public class SendEmailResponse
    {
        /// <summary>
        /// True if the email was sent successfully
        /// </summary>
        public bool Successful => !(Errors?.Count > 0);

        public List<string> MessageIds { get; set; }
        /// <summary>
        /// The error message if the sending failed
        /// </summary>
        public List<string> Errors { get; set; }

    }
}
