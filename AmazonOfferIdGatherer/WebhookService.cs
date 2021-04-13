using Discord;
using Discord.Webhook;
using System;
using System.Threading.Tasks;

namespace AmazonOfferIdGatherer
{
    public sealed class WebhookService
    {
        private string _webhookUrl;

        public WebhookService(string webhookUrl)
        {
            if (String.IsNullOrEmpty(webhookUrl))
            {
                throw new ArgumentNullException();
            }

            _webhookUrl = webhookUrl;
        }

        public async Task SendDiscordMessage(string title, string description, string messageText)
        {
            using (var client = new DiscordWebhookClient(_webhookUrl))
            {
                var embed = new EmbedBuilder
                {
                    Title = title,
                    Description = description
                };
                // Webhooks are able to send multiple embeds per message
                // As such, your embeds must be passed as a collection.
                await client.SendMessageAsync(text: messageText, embeds: new[] { embed.Build() });
            }
        }
    }
}
