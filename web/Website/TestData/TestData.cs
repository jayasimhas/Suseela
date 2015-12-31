using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glass.Mapper.Sc.Fields;
using Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface;
using Informa.Web.Areas.Account.Models;
using Sitecore.Mvc.Extensions;

namespace Informa.Web.TestData
{
    public static class TestData
    {
        public static T TestModel<T>(this WebViewPage<T> page)
        {   
            return GetTestData<T>(page.Model);
        }

        private static T GetTestData<T>(T model) 
        {
            throw new NotImplementedException();
        }

        public static string TestRichText(this WebViewPage page)
        {
            return _employees.Aggregate((s, s1) => s.Append(s1, ','));
        }

        public static string TestSingleLineText(this WebViewPage page)
        {
            return GetRandomEmployee();
        }                     

        public static Guid GetRandomTopics()
        {
            throw new NotImplementedException();
        }

        public static string GetRandomTitle()
        {
            int index = rnd.Next(_employees.Count);

            return _employees[index];
        }

        public static string GetRandomEmployee()
        {
            int index = rnd.Next(_employees.Count);

            return _employees[index];
        }

        public static string GetRandomSummary()
        {
            throw new NotImplementedException();
        }


        public static LoginViewModel LoginModel => new LoginViewModel();    

        public static ListableModel Article => new ListableModel
        {
            ListableAuthor = null,
            ListableDate = DateTime.Now,
            ListableImage = new Image(),
            ListableTitle = GetRandomTitle(),
            ListableTopics = GetRandomTopics(),
            ListableUrl = new Link { Anchor = "#", Text = "Text", Title = "Title" },
            ListableSummary = GetRandomSummary()
        };

   

        public static List<string> _articleTitles => new List<string>
        {
            "Stockwatch: Big Pharma And big Biotech Are The New Black",
            "1st U.S. Biosimilar Arrives; Zarcio Launches"
        };

        public static List<string> _summaries => new List<string>
        {
            "Amgen fails to keep Novartis unit Sandoz from launching its biosimilar Zarxio. Novartis talks to SCRIP about pricing."
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
}