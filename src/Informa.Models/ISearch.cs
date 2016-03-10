using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Jabberwocky.Glass.Models;
using Sitecore.Data;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
    //NOTE NOTE NOTE NOTE NOTE
    //Code generation for the search page it turned off for now and these interfaces will be used.  The reasson is that the generation outputs
    //global::Informa.Models.Informa.Models.sitecore.templates.Velir.Search.Components.ISearch_Listing__Raw for the search page but that causes an issue with
    //the facets dissapearing sometimes, it needs to be global::Velir.Search.Models.ISearch_Listing (and the same for ISearch_Refinements)

    //If you need to add fields to the Search page, generate the code and move the search page intterfaces here manually

    /// <summary>
    /// ISearch Interface
    /// <para></para>
    /// <para>Path: /sitecore/templates/User Defined/Pages/Search</para>	
    /// <para>ID: 0175c257-55f5-47d3-b34d-45022f16f1b0</para>	
    /// </summary>
    [SitecoreType(TemplateId = ISearchConstants.TemplateIdString)]
    public partial interface ISearch : IGlassBase, global::Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates.I___BasePage, global::Velir.Search.Models.ISearch_Listing, global::Velir.Search.Models.ISearch_Refinements
    {
        /// <summary>
        /// The Search Tips Text field.
        /// <para></para>
        /// <para>Field Type: Rich Text</para>		
        /// <para>Field ID: f3c99235-b6ae-4181-aada-199155242ac6</para>
        /// <para>Custom Data: </para>
        /// </summary>
        [SitecoreField(ISearchConstants.Search_Tips_TextFieldName, Setting = SitecoreFieldSettings.InferType)]
        string Search_Tips_Text { get; set; }
        /// <summary>
        /// The Search Tips Title field.
        /// <para></para>
        /// <para>Field Type: Single-Line Text</para>		
        /// <para>Field ID: 29106a20-f739-465e-bd36-ea83af0935ea</para>
        /// <para>Custom Data: </para>
        /// </summary>
        [SitecoreField(ISearchConstants.Search_Tips_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
        string Search_Tips_Title { get; set; }
    }

    /// <summary>
    /// ISearch Interface
    /// <para></para>
    /// <para>Path: /sitecore/templates/User Defined/Pages/Search</para>	
    /// <para>ID: 0175c257-55f5-47d3-b34d-45022f16f1b0</para>	
    /// </summary>
    [SitecoreType]
    public partial interface ISearch__Raw : IGlassBase, global::Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates.I___BasePage__Raw, global::Velir.Search.Models.ISearch_Listing__Raw, global::Velir.Search.Models.ISearch_Refinements__Raw
    {
        /// <summary>
        /// The Search Tips Text field.
        /// <para></para>
        /// <para>Field Type: Rich Text</para>		
        /// <para>Field ID: f3c99235-b6ae-4181-aada-199155242ac6</para>
        /// <para>Custom Data: </para>
        /// </summary>
        [SitecoreField(ISearchConstants.Search_Tips_TextFieldName, Setting = SitecoreFieldSettings.RichTextRaw)]
        string Search_Tips_Text { get; set; }
        /// <summary>
        /// The Search Tips Title field.
        /// <para></para>
        /// <para>Field Type: Single-Line Text</para>		
        /// <para>Field ID: 29106a20-f739-465e-bd36-ea83af0935ea</para>
        /// <para>Custom Data: </para>
        /// </summary>
        [SitecoreField(ISearchConstants.Search_Tips_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
        string Search_Tips_Title { get; set; }
    }

    public static partial class ISearchConstants
    {

        public const string TemplateIdString = "0175c257-55f5-47d3-b34d-45022f16f1b0";
        public static readonly ID TemplateId = new ID(TemplateIdString);
        public const string TemplateName = "Search";


        public static readonly ID Search_Tips_TextFieldId = new ID("f3c99235-b6ae-4181-aada-199155242ac6");
        public const string Search_Tips_TextFieldName = "Search Tips Text";

        public static readonly ID Search_Tips_Text__RawFieldId = new ID("f3c99235-b6ae-4181-aada-199155242ac6");
        public const string Search_Tips_Text__RawFieldName = "Search Tips Text";


        public static readonly ID Search_Tips_TitleFieldId = new ID("29106a20-f739-465e-bd36-ea83af0935ea");
        public const string Search_Tips_TitleFieldName = "Search Tips Title";



        public static readonly ID Custom_Meta_TagsFieldId = new ID("58c1bb46-882f-4f72-8e76-72fca199706b");
        public const string Custom_Meta_TagsFieldName = "Custom Meta Tags";



        public static readonly ID Meta_DescriptionFieldId = new ID("15f619cd-b981-477f-b496-b88577615c11");
        public const string Meta_DescriptionFieldName = "Meta Description";



        public static readonly ID Meta_KeywordsFieldId = new ID("77b76c9b-2c0e-44cb-aa78-5b9cb1b535ba");
        public const string Meta_KeywordsFieldName = "Meta Keywords";



        public static readonly ID Meta_Title_OverrideFieldId = new ID("31603f25-6d9c-4954-bcde-342e1e184a30");
        public const string Meta_Title_OverrideFieldName = "Meta Title Override";



        public static readonly ID Navigation_TitleFieldId = new ID("8f4ee718-6fa0-42a5-85c4-6c8fe8b1fcd4");
        public const string Navigation_TitleFieldName = "Navigation Title";



        public static readonly ID BodyFieldId = new ID("446a339b-e1d9-4de1-8b5e-db7942ef1723");
        public const string BodyFieldName = "Body";

        public static readonly ID Body__RawFieldId = new ID("446a339b-e1d9-4de1-8b5e-db7942ef1723");
        public const string Body__RawFieldName = "Body";


        public static readonly ID Sub_TitleFieldId = new ID("f1d1dc93-63df-4afa-b60f-b3a3e9d36675");
        public const string Sub_TitleFieldName = "Sub Title";



        public static readonly ID TitleFieldId = new ID("2d750eff-9058-4abf-a7ff-57ffe5da1e3c");
        public const string TitleFieldName = "Title";



        public static readonly ID Include_In_SearchFieldId = new ID("f2b6ee8b-d5e1-49ae-92f0-bc966269b300");
        public const string Include_In_SearchFieldName = "Include In Search";



        public static readonly ID TaxonomiesFieldId = new ID("0212fc0f-953b-461c-b4d9-b7483a4d4f1b");
        public const string TaxonomiesFieldName = "Taxonomy";



        public static readonly ID Canonical_LinkFieldId = new ID("fd4b0a78-6cfe-4fbc-b802-1145e22e4222");
        public const string Canonical_LinkFieldName = "Canonical Link";



        public static readonly ID Listing_TitleFieldId = new ID("24dede84-4b6e-453f-9d84-2c7bee6029c4");
        public const string Listing_TitleFieldName = "Listing Title";



        public static readonly ID Base_Endpoint_UrlFieldId = new ID("ebbb5cdb-a469-46ae-ab89-fe1f11d676f0");
        public const string Base_Endpoint_UrlFieldName = "Base Endpoint Url";



        public static readonly ID Index_NameFieldId = new ID("8538e64b-69b6-44c0-829b-0826f26231dc");
        public const string Index_NameFieldName = "Index Name";



        public static readonly ID Available_Sort_OptionsFieldId = new ID("f66f3aaf-25fa-40ab-bbf7-2a3399caf9ed");
        public const string Available_Sort_OptionsFieldName = "Available Sort Options";



        public static readonly ID Default_Sort_OrderFieldId = new ID("040c0194-6c6e-43fd-ac5c-0aa3daa35a87");
        public const string Default_Sort_OrderFieldName = "Default Sort Order";



        public static readonly ID Hidden_ExpressionFieldId = new ID("d02579da-5de5-43a8-86c7-934cbcfe1819");
        public const string Hidden_ExpressionFieldName = "Hidden Expression";



        public static readonly ID Items_Per_PageFieldId = new ID("9f76d8df-ef1c-4b6d-af33-b2655fd0cc74");
        public const string Items_Per_PageFieldName = "Items Per Page";



        public static readonly ID Refinements_TitleFieldId = new ID("77f09046-0a92-4378-b160-965f9e78fd39");
        public const string Refinements_TitleFieldName = "Refinements Title";



        public static readonly ID RefinementsFieldId = new ID("3dd8aa26-2b10-4305-831c-7fcc78a674be");
        public const string RefinementsFieldName = "Refinements";




    }

}
