using Newtonsoft.Json;

namespace Jaunts.Core.Models.Email
{
    /// <summary>
    /// The details about the email to send
    /// </summary>
    public class SendEmailMessage
    {


        [JsonProperty("to")]
        public List<ToResponse> To { get; set; }

        [JsonProperty("cc")]
        public List<CcResponse> Cc { get; set; }

        [JsonProperty("bcc")]
        public List<BccResponse> Bcc { get; set; }

        [JsonProperty("from")]
        public FromResponse From { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("custom_variables")]
        public CustomVariablesResponse CustomVariables { get; set; }

        [JsonProperty("headers")]
        public HeadersResponse Headers { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }


        public class Attachment
        {
            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("filename")]
            public string Filename { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("disposition")]
            public string Disposition { get; set; }
        }

        public class BccResponse
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class CcResponse
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class CustomVariablesResponse
        {
            [JsonProperty("user_id")]
            public string UserId { get; set; }

            [JsonProperty("batch_id")]
            public string BatchId { get; set; }
        }

        public class FromResponse
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class HeadersResponse
        {
            [JsonProperty("X-Message-Source")]
            public string XMessageSource { get; set; }
        }



        public class ToResponse
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }


        
    }
}
