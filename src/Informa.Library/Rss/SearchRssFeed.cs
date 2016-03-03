using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Search.Results;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Informa.Library.Rss
{
    public class SearchResultsRequest
    {
        //   "pageId":"0ff66777-7ec7-40be-abc4-6a20c8ed1ef0",
        //"page":1,
        //"perPage":10,
        //"sortBy":"relevance",
        //"sortOrder":"asc",
        //"queryParameters":{      

        public string page { get; set; }
        public string pageId { get; set; }
        public string perPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
      //  public List<string> queryParameters { get; set; }
      
    }

    public class SearchResults
    {
        public SearchResultsRequest request { get; set; }
        public string totalResults { get; set; }
        public List<InformaSearchResultItem> results { get; set; }
    }

    public class SearchRssFeed : Sitecore.Syndication.PublicFeed
    {

        public override IEnumerable<Item> GetSourceItems()
        {

            if (!Sitecore.Context.RawUrl.Contains("?"))
            {
                return new List<Item>();
            }

            string[] urlParts = Sitecore.Context.RawUrl.Split('?');

            if (urlParts.Length != 2)
            {
                return new List<Item>();
            }

            string url =
                "http://informa.gabe.dev/api/informasearch?pId=0ff66777-7ec7-40be-abc4-6a20c8ed1ef0&" + urlParts[1];

            var client = new WebClient();
            var content = client.DownloadString(url);

            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(content);

            List<Item> resultItems = new List<Item>();

            foreach (var searchResult in results.results)
            {
                Item theItem = Sitecore.Context.Database.GetItem(searchResult.ItemId);

                if (theItem == null)
                {
                    continue;
                }

                resultItems.Add(theItem);
            }

            return resultItems;
        }

        //private async Task<string> GetResults()
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri("http://localhost:9000/");
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        // New code:
            //        HttpResponseMessage response = await client.GetAsync("api/products/1");
            //        //if (response.IsSuccessStatusCode)
            //        //{
            //        //    Product product = await response.Content.ReadAsAsync > Product > ();
            //        //    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
            //        //}
            //    }

            //    return string.Empty;
            //}
        }
}
