using System.Collections.Generic;
using System.Xml;
using Glass.Mapper.Sc;
using Sitecore.Rules;

namespace Velir.Search.Core.Rules
{
	public class SearchRuleFactory<TContext> where TContext : RuleContext
	{
		public static List<SearchRule<TContext>> ParseRules(ISitecoreService db, string p_RulesXml)
    {
      List<SearchRule<TContext>> list = new List<SearchRule<TContext>>();
			if (!string.IsNullOrEmpty(p_RulesXml))
			{
				Dictionary<string, string> dictionary = ParseRuleNames(p_RulesXml);
				foreach (Rule<TContext> rule in RuleFactory.ParseRules<TContext>(db.Database, p_RulesXml).Rules)
				{
					if (rule.Condition != null)
					{
						var p_Condition = SearchConditionFactory.GetCondition(rule.Condition);
						string p_Id = rule.UniqueId.ToString();
						list.Add(new SearchRule<TContext>(p_Id, dictionary[p_Id], p_Condition, rule.Actions));
					}
				}
			}
      
      return list;
    }

    private static Dictionary<string, string> ParseRuleNames(string p_RulesXml)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(p_RulesXml);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ruleset/rule");
      int num = 1;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        string index = xmlElement.Attributes["uid"].Value;
        string str = !xmlElement.HasAttribute("name") ? string.Format("{0} {1}", "Rule", (object) num) : xmlElement.Attributes["name"].Value;
        dictionary[index] = str;
        ++num;
      }
      return dictionary;
    }
	}
}