﻿using System;
using Informa.Library.User.Newsletter;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Newsletter;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Newsletter
{
	public class SalesforceUpdateNewsletterUserOptIns : IUpdateNewsletterUserOptIns
	{
		protected readonly ISalesforceServiceContext Service;
	    protected readonly INewsletterUserOptInsContext NewsletterContext;
        public SalesforceUpdateNewsletterUserOptIns(
			ISalesforceServiceContext service,
            INewsletterUserOptInsContext newsletterContext)
		{
			Service = service;
            NewsletterContext = newsletterContext;
		}

		public bool Update(IEnumerable<INewsletterUserOptIn> newsletterOptIns, string userName)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return false;
			}

            //we can't send just the ones we want to update. we have to send all newsletter optins that a user has
		    List<string> updateTypes = newsletterOptIns.Select(n => n.NewsletterType.ToLower()).ToList();
            var updatedOptIns = NewsletterContext.OptIns.Where(a => !updateTypes.Contains(a.NewsletterType.ToLower())).Concat(newsletterOptIns);
		    
            var optIns = updatedOptIns.Select(noi => new EBI_EmailNewsLetterOptin
			{
				optinName = noi.NewsletterType,
				IsReceivingEmailNewsletter = noi.OptIn,
				IsReceivingEmailNewsletterSpecified = true
			}).ToArray();

			var response = Service.Execute(s => s.updateEmailNewsletterOptIns(userName, optIns));
            if(!response.IsSuccess() 
                && response.errors != null 
                && response.errors.Any())
                Sitecore.Diagnostics.Log.Error($"Newsletter Opt In Error: {string.Join(",", response.errors.Select(a => a.message))}", this);
            
			return response.IsSuccess();
		}
	}
}
