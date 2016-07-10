using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Models;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Factory;
using Jabberwocky.Glass.Models;
using Sitecore.ContentSearch.Linq.Extensions;
using Sitecore.Mvc.Extensions;

namespace Informa.Web.TestData
{
    public static class InterfaceFactoryExtension
    {
        //public static TType InterfaceFactory<TModel, TType>(this HtmlHelper<TModel> htmlHelper, TModel model) where TModel : class, Jabberwocky.Glass.Models.IGlassBase where TType : class
        //{
        //    ISitecoreContext context = htmlHelper.Glass().SitecoreContext;

        //    return context.GetItem<TType>(model._Id);
        //}

        //public static TType InterfaceFactory<TModel, TType>(this HtmlHelper htmlHelper, TModel model) where TModel : class, Jabberwocky.Glass.Models.IGlassBase where TType : class
        //{
        //    ISitecoreContext context = htmlHelper.Glass().SitecoreContext;

        //    return context.GetItem<TType>(model._Id);
        //}

        //public IListable GetIListable(IGlassInterfaceFactory factory,
        //ISitecoreContext context)
        //{


        //    IListable listable = factory.GetItem<IListable>(contextItem);

        //    return listable;
        //}
    }

    public static class TestData
    {
        //public static T TestModel<T>(this WebViewPage<T> page)
        //{   
        //    return GetTestData<T>(page.Model);
        //}

        //private static T GetTestData<T>(T model) 
        //{
        //    if(_dictionary.)
        //}

        
        public static IListableViewModel ListableModel(int numAuthors = 1, int numTaxonomy = 0, bool hasImage = true)
        {
            var authors = GetRandomEmployees(numAuthors);

            if (numAuthors == 0)
                numAuthors = rnd.Next(6);
            if (numTaxonomy == 0)
                numTaxonomy = rnd.Next(6);

            var model = new TestListableViewModel
            {
				DisplayImage = true,
                ListableTopics = GetRandomTopics(numTaxonomy),
                ListableAuthorByLine = string.Join(", ", GetRandomEmployees(numAuthors).Select(a => a.LinkableText)),
                ListableDate = DateTime.Now,
                ListableImage = "http://lorempixel.com/800/450/technics",
                ListableSummary = GetRandomSummary(),
                ListableTitle = GetRandomTitle(),
                ListableUrl = new Link { Anchor = "#" }
            };

            if (!hasImage)
                model.ListableImage = null;

            return model;
        }

        public static ILinkable LinkableModel(int numAuthors = 1, int numTaxonomy = 0, bool hasImage = true)
        {   
            return new LinkableModel
            {
                LinkableText = GetRandomEmployeeName(),
                LinkableUrl = "#"
            };        
        }

        public static string TestRichText(this WebViewPage page)
        {
            return _employees.Aggregate((s, s1) => s.Append(s1, ','));
        }

        public static string TestSingleLineText(this WebViewPage page)
        {
            return GetRandomStaff().LinkableText;
        }                     

        public static ILinkable GetRandomTopic()
        {
            int index = rnd.Next(_topics.Count);  

            return new LinkableModel
            {
                LinkableText = _topics[index],
                LinkableUrl = "#"
            };

        }

        public static IEnumerable<ILinkable> GetRandomTopics(int num = 1)
        {
            return Enumerable.Range(0, num).Select(x => GetRandomTopic());
        }

        public static string GetRandomTitle()
        {
            int index = rnd.Next(_articleTitles.Count);

            return _articleTitles[index];
        }

        public static IEnumerable<ILinkable> GetRandomEmployees(int num = 1)
        {
            return Enumerable.Range(0, num).Select(x => GetRandomStaff());
        }

        public static ILinkable GetRandomStaff()
        {
            int index = rnd.Next(_employees.Count);

            var employee = new LinkableModel();

            employee.LinkableText = _employees[index];
            employee.LinkableUrl = "#";


            return employee;
        }

        public static string GetRandomSummary()
        {
            int index = rnd.Next(_summaries.Count);
            return _summaries[index];
        }

    
        public static IListable Article => new ListableModel
        {
            ListableAuthorByLine = string.Empty,
            ListableDate = DateTime.Now,
            ListableImage = "http://lorempixel.com/800/450/technics",
            ListableTitle = GetRandomTitle(),
            ListableTopics = GetRandomTopics(),
            ListableUrl = new Link { Anchor = "#", Text = "Text", Title = "Title" },
            ListableSummary = GetRandomSummary()
        };

        public static string GetRandomEmployeeName()
        {
            return GetRandomStaff().LinkableText;
        }

   

        public static List<string> _articleTitles => new List<string>
        {
            $"Trending: Fashon from {GetRandomEmployeeName()} And {GetRandomEmployeeName()} Are The New Black",
            $"{GetRandomEmployeeName()} fixes bug; {GetRandomEmployeeName()} Celebrates"
        };

        public static List<string> _summaries => new List<string>
        {
            $"{GetRandomEmployeeName()} fails to keep {GetRandomEmployeeName()} challenge from knocking him further down the ladder. {GetRandomEmployeeName()} talks to {GetRandomEmployeeName()} about a future match.",
            $"New research by {GetRandomEmployeeName()} has shown that levels of the DNA protein BRCA1 are increased in key parts of neurons after talking with {GetRandomEmployeeName()}. The findings are consistent with {GetRandomEmployeeName()}'s hypothesis and could prove to be an effective way to help reproduce benefits of {GetRandomEmployeeName()}’s syndrome and other perplexing mutation anomalies.",
            $"RNAi pioneer is getting closer to market with NDA filing in amyloidosis expected in 2017, but ample competition is coming along, albeit far behind.",
            $"{GetRandomEmployeeName()}’s shift toward targeting {GetRandomEmployeeName()} in ping-pong could cause unintended consequences, including creating hurdles for {GetRandomEmployeeName()}, says {GetRandomEmployeeName()}. The ladder changes also could hinder {GetRandomEmployeeName()} and increase other games for {GetRandomEmployeeName()}.",
            $"{GetRandomEmployeeName()} should decide how to create more trustful workplaces for {GetRandomEmployeeName()} in light of changes to how DoJ targets {GetRandomEmployeeName()} for corporate wrongdoing, says a food and drug lawyer. The changes, implemented through {GetRandomEmployeeName()}’s Manual, also could deter {GetRandomEmployeeName()} from working FDA-regulated industries.",
            $"As 2015 comes to a close, {GetRandomEmployeeName()} looks at the current state of the Unique Device Identification program, persistent challenges for {GetRandomEmployeeName()} and what to expect in the year ahead. UDI guru {GetRandomEmployeeName()} provides insight."



        };

        public static List<string> _topics => new List<string>
        {
            "Markets",
            "Stockwatch",
            "USA",
            "Sandoz"
        };

        static Random rnd = new Random();
        public static List<string> _employees => new List<string>
        {
            "Aakash Shah",
            "Aaron Fredette",
            "Adam Lamarre",
            "Adam Ribaudo",
            "Akshay Mahajan",
            "Alessandro Faniuolo",
            "Alex Jackson",
            "Andrea Gillespie",
            "Andrew Rodgers",
            "Angie Yang Gregor",
            "Anna Mekerian",
            "Anna Yatskovskaya",
            "Arpit Gupta",
            "Arunabh Arora",
            "Ashwin Keshwaram",
            "Barron Wernick",
            "Ben Lipson",
            "Benjamin Garfield",
            "Bennett Grizzard",
            "Brad Ettinger",
            "Brendon Buckley",
            "Brett Borgeson",
            "Brittany DiCapua",
            "Brooke Talbot",
            "Bryan Mills",
            "Bryce Dooley",
            "Caitlin Portrie",
            "Cara Steinborn",
            "Caroline Cardiasmenos",
            "Charlie Spellman",
            "Chetna Kadam",
            "Chris Brady",
            "Chris Smith",
            "Chris Sulham",
            "Christina Guertin",
            "Christopher Grimm",
            "Cindy Lee",
            "Corey Caplette",
            "Dan Murphy",
            "Daniel DeLay",
            "Danielle Wooding",
            "Dave Valliere",
            "David Peet",
            "Dawn Erickson",
            "Diane OConnell",
            "Divya Mathew",
            "Dzmitry Siaukovich",
            "Ed Schwehm",
            "Edwina Nowicki",
            "Emily LeBarron",
            "Emily Tremonte",
            "Errol Silver",
            "Eugene Kim",
            "Felix Steiny",
            "Gabe Boys",
            "George Bica",
            "Gil Zamir",
            "Girija Ramapriya",
            "Greg Ellis",
            "Huxu Tian",
            "Ishan Kumar",
            "Jamie Michalski",
            "Jean Gao",
            "Jeff Holmes",
            "Jeff King",
            "Jillian Menoche",
            "Joel Douglass",
            "Jonathan Dallas",
            "Jordan Warren",
            "Jorge Pratt",
            "Joseph Curley",
            "Juan Osorio",
            "Judge DiCesaro",
            "Juquan Wu",
            "Justin Pietrella",
            "Kai Rasmussen",
            "Kate Hux",
            "Katie Spinello",
            "Ken Kopin",
            "Kenneth Smith",
            "Kerry Robert",
            "Kevin Hannon",
            "Kevin Mazzoni",
            "Kevin Quillen",
            "Kimiko Tanaka Vecchione",
            "Kristi Albright",
            "Kyle Cedrone",
            "Kyle LeRoy",
            "Ladan Ashrafi",
            "Laura Francis",
            "Lyudmila Mikhaylova",
            "Manisha Minde",
            "Maria Laina Del Pico",
            "Mark Gregor",
            "Mark Saad",
            "Mark Stiles",
            "Mark Tomlinson",
            "Mary Kate McArdle",
            "Mary Matthews",
            "Matt Gelfand",
            "Matt Goetz",
            "Michael Lambert",
            "Michael Saleeb",
            "Micquella Anthony",
            "Mike Distaula",
            "Mike Dolan",
            "Mike Gintz",
            "Mohsin Shaikh",
            "Molly Jagiello",
            "Nabil Lamriben",
            "Nancy Decker",
            "Nathan Martin",
            "Nedret Sahin",
            "Niamh O'Driscoll",
            "Nick Dorrough",
            "Nicole Bryant",
            "Nicole DuRand",
            "Nicole O'Keeffe",
            "Nicole Scianna Barka",
            "Nikki Gillespie",
            "Olivia Yetten",
            "Rebecca Martin",
            "Rebecca Mazur",
            "Richard Rosowski",
            "Rondel Ward",
            "Rose Heydt",
            "Ryan Connors",
            "Sameer Dongare",
            "Samuel Fine",
            "Sandipan Chakravarty",
            "Sanjay Joshi",
            "Sarena Douglass",
            "Sean Dailey",
            "Sean Fleming",
            "Sean Weber",
            "Sebastian Badon",
            "Shelby Staab",
            "Shu-Zhen Chen",
            "Solomon Robson",
            "Stephanie Allen",
            "Suma Deepika Kollipara",
            "Sunil Nagpal",
            "Teresa Bailey",
            "Umesh Upadhayay",
            "Vlad Ivashin",
            "Yehia Ibrahim",
            "Yogi Shridhare"
        };     
    }

	public class TestListableViewModel : ListableModel, IListableViewModel
	{
		public bool DisplayImage { get; set; }
        public bool IsUserAuthenticated { get; set; }
        public bool IsArticleBookmarked { get; set; }
        public string BookmarkText { get; set; }
        public string BookmarkedText { get; set; }
		public Guid ID { get; set; }
		public string PageTitle { get; set; }
	}
}