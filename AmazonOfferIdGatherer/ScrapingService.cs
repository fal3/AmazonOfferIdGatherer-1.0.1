using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AmazonOfferIdGatherer
{
    public sealed class ScrapingService
    {
        private readonly string _url;
        private ScrapingBrowser _browser;
        private readonly string _amazonMerchantId = "ATVPDKIKX0DER";

        public ScrapingService(string sku, string productEndpoint)
        {
            if (String.IsNullOrEmpty(sku))
            {
                throw new ArgumentNullException();
            }
            _url = productEndpoint + sku;
            _browser = new ScrapingBrowser();
            

        }

        public async Task<string> GetOfferListingId()
        {
            string offerId = String.Empty;

            //in a future release update with dynamic proxy info

            //WebProxy myProxy = new WebProxy();
            //Uri newUri = new Uri("", false);
            //myProxy.Address = newUri;
            //myProxy.Credentials = new NetworkCredential("", "");
            //_browser.Proxy = myProxy;
            WebPage webpage = await _browser.NavigateToPageAsync(new Uri(_url));
            HtmlNode htmlNode = webpage.Html;
            IEnumerable<HtmlNode> formNode = htmlNode.CssSelect("form#addToCart");
            IEnumerable<HtmlNode> offerListingIdNodes = formNode.CssSelect("input#offerListingID");
            if (offerListingIdNodes.Any())
            {
                foreach (HtmlNode offerListingIdNode in offerListingIdNodes)
                {
                    HtmlNode merchantIdNode = formNode.CssSelect("input#merchantID").FirstOrDefault();
                    string merchantId = merchantIdNode.Attributes["value"].Value;
                    if (merchantId == _amazonMerchantId)
                    {
                        offerId = offerListingIdNode.Attributes["value"].Value;
                    }
                }
            }

            return offerId;
        }
    }
}
