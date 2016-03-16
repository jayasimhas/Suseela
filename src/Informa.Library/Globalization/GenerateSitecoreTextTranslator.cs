using System.Linq;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.System.Dictionary;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Globalization
{
    public class GenerateSitecoreTextTranslator : ITextTranslator
    {
        protected readonly ISitecoreService SitecoreService;

        public GenerateSitecoreTextTranslator(ISitecoreService sitecoreService)
        {
            SitecoreService = sitecoreService;
        }

        public string Translate(string key)
        {
            var translatedText = Sitecore.Globalization.Translate.Text(key);

            if (string.IsNullOrEmpty(translatedText) || translatedText == key)
            {
                IDictionary_Domain generated =
                    SitecoreService.GetItem<IDictionary_Domain>("{1B81B972-B282-46F0-89DF-6C1A25A68A92}");
                var dictionaryKey = key.Split('.').ToList();

                IGlassBase lastItem = generated;
                IDictionary_Entry newEntry = null;
                int total = dictionaryKey.Count;

                dictionaryKey.Select((x, i) =>
                {
                    i++;
                    var newItem = lastItem;
                    if (i < total)
                    {
                        newItem = SitecoreService.Create<IDictionary_Folder, IGlassBase>(lastItem, x);
                    }
                    else
                    {
                        newEntry = SitecoreService.Create<IDictionary_Entry, IGlassBase>(lastItem, x);
                    }

                    lastItem = newItem;
                    return newItem;
                });

                if (newEntry == null) return translatedText;

                newEntry.Key = key;
                SitecoreService.Save(newEntry);
                translatedText = Sitecore.Globalization.Translate.Text(key);
            }

            return translatedText;
        }
    }
}