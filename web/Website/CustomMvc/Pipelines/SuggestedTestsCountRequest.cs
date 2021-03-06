﻿using Sitecore.ContentTesting.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ContentTesting;

namespace Informa.Web.CustomMvc.Pipelines
{
    public class SuggestedTestsCountRequest : PipelineProcessorRequest<ItemContext>
    {
        private readonly IContentTestStore contentTestStore;

        public SuggestedTestsCountRequest() : this(ContentTestingFactory.Instance.ContentTestStore)
        {
        }

        public SuggestedTestsCountRequest(IContentTestStore contentTestStore)
        {
            Assert.ArgumentNotNull(contentTestStore, "contentTestStore");
            this.contentTestStore = contentTestStore;
        }

        public override PipelineProcessorResponseValue ProcessRequest()
        {
            //IEnumerable<SuggestedTestSearchResultItem> suggestedTests = this.contentTestStore.GetSuggestedTests(null, null);
            //return new PipelineProcessorResponseValue { Value = suggestedTests.Count<SuggestedTestSearchResultItem>() };
            return new PipelineProcessorResponseValue { Value = 0 };
        }
    }
}