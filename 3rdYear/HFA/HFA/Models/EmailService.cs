using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using HFA.Models.ViewModels;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Client;


namespace HFA.Models
{
    public class EmailService
    {
        private readonly GraphServiceClient _graphServiceClient;
        private string _emailAddress;
        public EmailService() { }
        public EmailService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }
        public EmailService(GraphServiceClient graphServiceClient, IConfiguration iConfig)
        {
            _graphServiceClient = graphServiceClient;
            _emailAddress = iConfig.GetSection("CustomConfiguration").GetSection("EmailAddress").Value;
        }
        public async Task<bool> SetUpAsync(EmailContent body)
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();

            Message message = new()
            {
                Subject = body.Subject,
                Body = new ItemBody
                {
                    Content = $"{user.DisplayName} is reaching out to the Heritage Faculty Association\n\nMessage sent:\n{body.Message}",
                    ContentType = BodyType.Text,
                },
                ToRecipients = new List<Recipient>
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {

                        Address = _emailAddress

                    },
                },
            },
            };

            if (_graphServiceClient != null && message != null)
            {

                await _graphServiceClient.Me.SendMail(message, true).Request().PostAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

