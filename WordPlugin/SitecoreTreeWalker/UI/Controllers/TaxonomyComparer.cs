using System.Collections.Generic;
using PluginModels;

namespace InformaSitecoreWord.UI.Controllers
{
    /// <summary>
    /// Comparer used to sort the list of results on a search for a taxonomy. 
    /// </summary>
    class TaxonomyComparer : IComparer<TaxonomyStruct>
    {
        private readonly string _substring;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="substring">Substring which the comparison will be based on.</param>
        public TaxonomyComparer(string substring)
        {
            _substring = substring;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>A negative number if the substring appears earlier in Taxonomy x
        /// A positive number if the substring appears earlier in Taxonomy y
        /// If the substring appears at the same index, a negative number if Taxonomy x has fewer characters
        /// If the substring appears at the same index, a positive number if Taxonomy y has fewer characters
        /// 0 if the substring appears at the same index for both Taxonomy x and Taxonomy y and they both have the same length</returns>
        public int Compare(TaxonomyStruct x, TaxonomyStruct y)
        {
            if(x==null)
            {
                if(y==null)
                {
                    return 0;
                }

                return -1;
            }
            if(y==null)
            {
                return 1;
            }
            string xname = x.Name.ToLower();
            string yname = y.Name.ToLower();
            int retVal = xname.IndexOf(_substring).CompareTo(yname.IndexOf(_substring));
            if(retVal != 0)
            {
                return retVal;
            }
            //if the strings have the substring at the same index
            //compare the lengths of the strings instead
            return x.Name.Length - y.Name.Length;
        }
    }
}
