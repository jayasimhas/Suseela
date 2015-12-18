using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Social.Client.Mvc.Areas.Social.Controllers;
using Sitecore.Social.Client.MVC;

namespace Informa.Web.Controllers
{
    public partial class TaxonomyStruct : ITaxonomy
    {     
        public string Name { get; set; } 
        /// <remarks/>
        public System.Guid ID { get; set; }
    }

    public interface ITaxonomy
    {
        string Name { get; set; }
        Guid ID { get; set; }
    }

    [Route]
    public class TaxonomyController : ApiController
    {   
        // GET api/<controller>
        public async Task<JsonResult<List<TaxonomyStruct>>> Get()
        {
            List<TaxonomyStruct> result = new List<TaxonomyStruct>();

            using (var client = new HttpClient())
            {
                 HttpResponseMessage response = await client.GetAsync($"http://informa.miked.velir.com/api/Taxonomy/{Guid.NewGuid()}");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<TaxonomyStruct>>();
                }          
            }

            return Json(result);
        }

        // GET api/<controller>/5
        public JsonResult<List<TaxonomyStruct>> Get(Guid id)
        {
            var taxonomy = Enumerable.Range(0, 30).Select(x => new TaxonomyStruct {ID = Guid.NewGuid(), Name = TestData.TestData.GetRandomEmployee()}).ToList();
                //TestData.Employees.Select(x => new TaxonomyStruct {ID = Guid.NewGuid(), Name = x}).ToList();

            return Json(taxonomy);
        }

        // POST api/<controller>
        public void Post([FromBody]Guid value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}

//namespace Sitecore.Social.Twitter.Client.Mvc.Areas.Social.Controllers
//{
//    public class TwitterConnectorController : ConnectorController
//    {
//        public TwitterConnectorController()
//          : base("Twitter", ID.Parse("{78D8D914-51C8-41F3-8424-021262F148B8}"))
//        {
//        }

//        public ActionResult Index()
//        {
//            return (ActionResult)this.LoginPartialView(Translate.Text(Context.User.IsAuthenticated ? "Attach Twitter account" : "Login with Twitter"));
//        }
//    }
//}
