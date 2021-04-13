using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazonOfferIdGatherer
{
    class Program
    {

        private static WebhookService _webhookService;
        private static readonly string _discordWebhookUrl = "https://discordapp.com/api/webhooks/830939802388004864/TTk98usbeQdNzOfDdzeQf6efrYsVLBaQlwQasujzF-rNRc2kWowOo4C9nJwdPl7tgOZ6";
        private static readonly string _amazonProductEndpoint = @"https://www.amazon.com/gp/product/";

        private static string[] productIDs = { @"B08HR6ZBYJ", @"B08J5F3G18", @"B08HH5WF97", @"B08HBTJMLJ", @"B08J5F3G18", @"B08HJQ182D", @"B08HBVX53D", @"B08KTYZXR9", @"B08J61SS5R",
            @"B07C24VB8Z",
           @"B08239QS3H",
           @"B08HBJB7YD",
           @"B08HHDP9DW",
           @"B08HR7SV3M",
           @"B08HR3Y5GQ",
           @"B08HR3DPGW",
           @"B08HR55YB5",
           @"B08HR55YB5",
           @"B08HR4RJ3Q",
           @"B08HR6FMF3",
           @"B08HJTH61J",
           @"B08HJS2JLJ",
           @"B08HBJB7YD",
           @"B08HBF5L3K",
           @"B08HBJB7YD",
           @"B08HBF5L3K"
        };
        private static List<OfferIdDetails> offerIDs = new List<OfferIdDetails>();
        private static List<ScrapingService> scrapingServices = new List<ScrapingService>();

        private static List<Task> TaskList = new List<Task>();

        public static async Task Main(string[] args)
        {
 
            foreach (string product in productIDs)
            {
                offerIDs.Add(new OfferIdDetails { Sku = product, OfferId = String.Empty });
                scrapingServices.Add(new ScrapingService(product, _amazonProductEndpoint));
            }

            for (int i = 0; i < productIDs.Length; i++)
            {
                var details = offerIDs[i];
                var scrapingService = scrapingServices[i];

                TaskList.Add(Task.Run(async () => await GatherOfferId(details, scrapingService)));
            }

            await Task.WhenAll(TaskList);



        }

        private static async Task GatherOfferId(OfferIdDetails details, ScrapingService scrapingService)
        {

            try
            {

                while (String.IsNullOrEmpty(details.OfferId))
                {
                    details.OfferId = await scrapingService.GetOfferListingId();

                    if (String.IsNullOrEmpty(details.OfferId))
                    {
                        Console.WriteLine($"Item is not currently in stock and sold by Amazon: {details.Sku}");
                        await Task.Delay(5000);
                    }
                }
                _webhookService = new WebhookService(_discordWebhookUrl);
                await _webhookService.SendDiscordMessage(details.Sku, details.OfferId, _amazonProductEndpoint + details.Sku);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }


    }
}
