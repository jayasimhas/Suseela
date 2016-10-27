using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Services.Captcha
{
	
	// Single instance because of the HttpClient, which is designed to be shared. This one
	// client will serve all the captcha verification requests
	[AutowireService(LifetimeScope = LifetimeScope.SingleInstance)]
	public class RecaptchaService : IRecaptchaService
	{
		private readonly HttpClient httpClient;
		private readonly Uri VerifyUri = new Uri("https://www.google.com/recaptcha/api/siteverify");

		public RecaptchaService()
		{
			httpClient = new HttpClient();
		}

		public string SecretKey => ConfigurationManager.AppSettings["Recaptcha.SecretKey"] ?? "";		
		public string SiteKey => ConfigurationManager.AppSettings["Recaptcha.SiteKey"] ?? "";
        public string EnableCaptcha => ConfigurationManager.AppSettings["EnableCaptcha"] ?? "";

        public bool Verify(string userInput)
		{
            if (EnableCaptcha == "false")
                return true;
			var formContent = new FormUrlEncodedContent(new[]
			{
					new KeyValuePair<string, string>("secret", SecretKey),
					new KeyValuePair<string, string>("response", userInput)
			});

			var response = httpClient.PostAsync(VerifyUri, formContent).Result;
			if (!response.IsSuccessStatusCode)
			{
				return false;
			}
			var content = response.Content.ReadAsStringAsync().Result;
			dynamic responseObject = (JObject)JsonConvert.DeserializeObject(content);
			return responseObject["success"];
		}
	}
	

	public interface IRecaptchaService
	{
		string SiteKey { get; }
		string SecretKey { get;  }
		bool Verify(string input);
	}
}
