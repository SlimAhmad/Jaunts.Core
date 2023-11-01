using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Models.Email;
using RESTFulSense.Clients;
using System.Net.Http.Headers;



namespace Jaunts.Core.Api.Brokers.EmailBroker
{
    public partial class EmailBroker : IEmailBroker
    {

        private readonly IConfiguration emailConfigurations;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;


        public EmailBroker(IConfiguration configuration)
        {
            this.emailConfigurations = configuration;
            this.httpClient = SetupHttpClient();
            this.apiClient = SetupApiClient();
        }

        private async ValueTask<TResult> PostAsync<TRequest, TResult>(string relativeUrl, TRequest content)
        {
            return await this.apiClient.PostContentAsync<TRequest, TResult>(
                relativeUrl,
                content,
                mediaType: "application/json",
                ignoreDefaultValues: true);
        }

        private async ValueTask<TResult> PostFormAsync<TRequest, TResult>(string relativeUrl, TRequest content)
            where TRequest : class
        {
            return await this.apiClient.PostFormAsync<TRequest, TResult>(
                relativeUrl,
                content);
        }

  

        private HttpClient SetupHttpClient()
        {
            
            var httpClient = new HttpClient()
            {

                BaseAddress =
                    new Uri(uriString: this.emailConfigurations.GetSection("MailTrap:TestUrl").Value),
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: this.emailConfigurations.GetSection("MailTrap:Token").Value);

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(this.httpClient);

      
    }
}

