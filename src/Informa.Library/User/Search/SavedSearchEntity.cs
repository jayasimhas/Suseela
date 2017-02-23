using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Library.User.Search
{
    public class SavedSearchEntity : SavedSearchItemId, ISavedSearchEntity
    {
        public string Id { get; set; }
        public virtual string SearchString { get; set; }
        public bool HasAlert { get; set; }
        public DateTime DateCreated { get; set; }
        public string UnsubscribeToken { get; set; }
        public string Publication { get; set; }
        public string PublicationCode { get; set; }
        public string PublicationName { get; set; }
        public string VerticalName { get; set; }
    }

    public class SavedSearchItemId : ISavedSearchItemId, IEquatable<ISavedSearchItemId>
    {
        public virtual string Username { get; set; }
        public virtual string Name { get; set; }
        public bool Equals(ISavedSearchItemId other)
        {
            return Username == other.Username && Name == other.Name;
        }
    }

    public class SearchStringEqualityComparer : IEqualityComparer<ISavedSearchEntity>
    {
        public bool Equals(ISavedSearchEntity x, ISavedSearchEntity y)
        {
            var xParams = x.SearchString.Split('&');
            var yParams = y.SearchString.Split('&');

            if (xParams.Length != yParams.Length) return false;

            Dictionary<string, List<string>> d = new Dictionary<string, List<string>>();
            foreach (string s in xParams)
            {
                string[] kvp = s.Split('=');
                string key = (kvp.Length > 0) ? kvp[0] : string.Empty;
                string value = (kvp.Length > 1) ? kvp[1] : string.Empty;
                List<string> l = new List<string>();
                foreach (string v in value.Split(';'))
                {
                    l.Add(HttpUtility.UrlDecode(v));
                }
                if (!string.IsNullOrEmpty(key) && !d.ContainsKey(key))
                    d.Add(key, l);
            }

            foreach (string p in yParams)
            {
                string[] kvp = p.Split('=');
                string key = (kvp.Length > 0) ? kvp[0] : string.Empty;
                if (!d.ContainsKey(key))
                    return false;

                string value = (kvp.Length > 1) ? kvp[1] : string.Empty;
                List<string> l = new List<string>();
                string[] list = value.Split(';');
                if (d[key].Count != list.Length)
                    return false;

                foreach (string v in list)
                {
                    if (!d[key].Contains(HttpUtility.UrlDecode(v)))
                        return false;
                }
            }

            return true;
        }

        public int GetHashCode(ISavedSearchEntity obj)
        {
            return obj.SearchString.Split('&').Select(p => p.GetHashCode()).Sum();
        }
    }
}