using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.System.Dictionary;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Pipelines.GetTranslation;
using Sitecore.SecurityModel;


namespace Informa.Library.CustomSitecore.Pipelines
{
    public class GenerateIfDictionaryKeyNotFound : IProcessor<GetTranslationArgs>
    {
        private readonly ISitecoreService SitecoreService;
        private readonly IItemReferences ItemReferences;

        public GenerateIfDictionaryKeyNotFound(Func<string, ISitecoreService> sitecoreFactory, IItemReferences itemReferences)
        {
            SitecoreService = sitecoreFactory(Constants.MasterDb);
            ItemReferences = itemReferences;
        }

        public void Process(GetTranslationArgs args)
        {
            if (!ShouldGenerateEntry(args))
            {
                return;
            }

            if (args.ContentDatabase.Name != Constants.MasterDb)
            {
                string pipelineName = "getTranslation";
                string result = null;
                if (CorePipelineFactory.GetPipeline(pipelineName, string.Empty) != null)
                {
                    GetTranslationArgs masterArgs = new GetTranslationArgs
                    {
                        Language = args.Language,
                        Key = args.Key,
                        Options = args.Options,
                        DomainName = args.DomainName,
                        Parameters = args.Parameters,
                        ContentDatabase = Database.GetDatabase(Constants.MasterDb)
                    };
                    CorePipeline.Run(pipelineName, masterArgs, false);
                }
                args.Result = args.Key;
                return;
            }          

            GenerateEntry(args);
        }

        protected void GenerateEntry(GetTranslationArgs args)
        {
            //Move to reference or config
            IDictionary_Domain generated =
                SitecoreService.GetItem<IDictionary_Domain>(ItemReferences.GeneratedDictionary);

            if (generated == null)
                return;
            
            var key = args.Key;
            var dictionaryKey = key.Split('.');

            IGlassBase lastItem = generated;
            IDictionary_Entry newEntry = null;
            int total = dictionaryKey.Length;
            using (new SecurityDisabler())
            {
                int i = 0;
                foreach(var k in dictionaryKey)
                {
                    i++;
                    var lookup = lastItem._ChildrenWithInferType.FirstOrDefault(x => x._Name == k);
                    if (lookup is IDictionary_Entry)
                    {
                        var entry = (lookup as IDictionary_Entry);
                        var phrase = entry.Phrase;

                        if (string.IsNullOrEmpty(phrase))
                        {
                            phrase = key;
                            entry.Phrase = phrase;
                            SitecoreService.Save(entry);
                        }

                        args.Result = string.IsNullOrEmpty(phrase) ? key : phrase;
                        return;
                    }
                    if (lookup != null)
                    {
                        lastItem = lookup;
                    }
                    else { 
                    if (i < total)
                        {
                            lastItem = SitecoreService.Create<IDictionary_Folder, IGlassBase>(lastItem, k);
                        }
                        else
                        {
                            newEntry = SitecoreService.Create<IDictionary_Entry, IGlassBase>(lastItem, k);
                        }
                    }
                }

                if (newEntry == null)
                {
                    return;
                }

                newEntry.Key = key;
                newEntry.Phrase = key;
                SitecoreService.Save(newEntry);
            }
            args.Result = key;
            return;
        }

        //
        protected virtual bool ShouldGenerateEntry(GetTranslationArgs args)
        {
            Assert.ArgumentNotNull(args, "args");                      
            return  args.Parameters != null && args.Parameters.Contains("GenerateIfDictionaryKeyNotFound") && !args.HasResult && string.IsNullOrEmpty(args.Result);
        }
    }

}         
