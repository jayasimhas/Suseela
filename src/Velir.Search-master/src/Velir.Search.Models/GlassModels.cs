





#pragma warning disable 1591
#pragma warning disable 0108
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Team Development for Sitecore.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;   
using System.Collections.Generic;   
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Jabberwocky.Glass.Models;
using Sitecore.Globalization;
using Sitecore.Data;



namespace Velir.Search.Models
{


 	/// <summary>
	/// IRefinements_Folder Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Refinements Folder</para>	
	/// <para>ID: 0056b1ab-7661-4271-96c5-153cc73d3c6f</para>	
	/// </summary>
	[SitecoreType(TemplateId=IRefinements_FolderConstants.TemplateIdString)]
	public partial interface IRefinements_Folder : IGlassBase 
	{
				}


	public static partial class IRefinements_FolderConstants{

			public const string TemplateIdString = "0056b1ab-7661-4271-96c5-153cc73d3c6f";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Refinements Folder";

			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// ISorts_Folder Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Sorts/Sorts Folder</para>	
	/// <para>ID: 0941e9eb-06e9-415b-aab1-6362dd21afd5</para>	
	/// </summary>
	[SitecoreType(TemplateId=ISorts_FolderConstants.TemplateIdString)]
	public partial interface ISorts_Folder : IGlassBase 
	{
				}


	public static partial class ISorts_FolderConstants{

			public const string TemplateIdString = "0941e9eb-06e9-415b-aab1-6362dd21afd5";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Sorts Folder";

			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IFacet Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Facet</para>	
	/// <para>ID: 2463de7d-4c6e-4e02-ab46-45833e0408c3</para>	
	/// </summary>
	[SitecoreType(TemplateId=IFacetConstants.TemplateIdString)]
	public partial interface IFacet : IGlassBase , global::Velir.Search.Models.I_Refinement
	{
								/// <summary>
					/// The And Filter field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: f0b316e1-2d87-4433-9634-40685172dd3c</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(IFacetConstants.And_FilterFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool And_Filter  {get; set;}
			
								/// <summary>
					/// The Is Multi Value field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: 524cf7f5-8ed0-4cd5-9aac-ff92e5bd6fbd</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(IFacetConstants.Is_Multi_ValueFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Is_Multi_Value  {get; set;}
			
				}


	public static partial class IFacetConstants{

			public const string TemplateIdString = "2463de7d-4c6e-4e02-ab46-45833e0408c3";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Facet";

					
			public static readonly ID And_FilterFieldId = new ID("f0b316e1-2d87-4433-9634-40685172dd3c");
			public const string And_FilterFieldName = "And Filter";
			
					
			public static readonly ID Is_Multi_ValueFieldId = new ID("524cf7f5-8ed0-4cd5-9aac-ff92e5bd6fbd");
			public const string Is_Multi_ValueFieldName = "Is Multi Value";
			
					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IText_Refinement Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Text Refinement</para>	
	/// <para>ID: 2cbb661f-2029-456e-9ae8-25194c3f425a</para>	
	/// </summary>
	[SitecoreType(TemplateId=IText_RefinementConstants.TemplateIdString)]
	public partial interface IText_Refinement : IGlassBase , global::Velir.Search.Models.I_Refinement
	{
								/// <summary>
					/// The Exact Match field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: 0c131541-0d60-46f4-9248-138ef257a026</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(IText_RefinementConstants.Exact_MatchFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Exact_Match  {get; set;}
			
				}


	public static partial class IText_RefinementConstants{

			public const string TemplateIdString = "2cbb661f-2029-456e-9ae8-25194c3f425a";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Text Refinement";

					
			public static readonly ID Exact_MatchFieldId = new ID("0c131541-0d60-46f4-9248-138ef257a026");
			public const string Exact_MatchFieldName = "Exact Match";
			
					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IYear_Refinement Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Year Refinement</para>	
	/// <para>ID: 3e39f45d-de43-410e-8e7e-f2f2b2be07db</para>	
	/// </summary>
	[SitecoreType(TemplateId=IYear_RefinementConstants.TemplateIdString)]
	public partial interface IYear_Refinement : IGlassBase , global::Velir.Search.Models.I_Refinement
	{
				}


	public static partial class IYear_RefinementConstants{

			public const string TemplateIdString = "3e39f45d-de43-410e-8e7e-f2f2b2be07db";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Year Refinement";

					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// I_Listing_Configuration Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Base/_Listing Configuration</para>	
	/// <para>ID: 5ccd0fbb-5349-460f-91f1-f1e960cb0e90</para>	
	/// </summary>
	[SitecoreType(TemplateId=I_Listing_ConfigurationConstants.TemplateIdString)]
	public partial interface I_Listing_Configuration : IGlassBase 
	{
								/// <summary>
					/// The Base Endpoint Url field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: ebbb5cdb-a469-46ae-ab89-fe1f11d676f0</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Base_Endpoint_UrlFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Base_Endpoint_Url  {get; set;}
			
								/// <summary>
					/// The Index Name field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 8538e64b-69b6-44c0-829b-0826f26231dc</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Index_NameFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Index_Name  {get; set;}
			
								/// <summary>
					/// The Available Sort Options field.
					/// <para></para>
					/// <para>Field Type: Multilist</para>		
					/// <para>Field ID: f66f3aaf-25fa-40ab-bbf7-2a3399caf9ed</para>
					/// <para>Custom Data: generic=ISort</para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Available_Sort_OptionsFieldName, Setting = SitecoreFieldSettings.InferType)]
					IEnumerable<ISort> Available_Sort_Options  {get; set;}
			
								/// <summary>
					/// The Default Sort Order field.
					/// <para></para>
					/// <para>Field Type: Droplink</para>		
					/// <para>Field ID: 040c0194-6c6e-43fd-ac5c-0aa3daa35a87</para>
					/// <para>Custom Data: type=ISort</para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Default_Sort_OrderFieldName, Setting = SitecoreFieldSettings.InferType)]
					ISort Default_Sort_Order  {get; set;}
			
								/// <summary>
					/// The Hidden Expression field.
					/// <para></para>
					/// <para>Field Type: Rules</para>		
					/// <para>Field ID: d02579da-5de5-43a8-86c7-934cbcfe1819</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Hidden_ExpressionFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Hidden_Expression  {get; set;}
			
								/// <summary>
					/// The Items Per Page field.
					/// <para></para>
					/// <para>Field Type: Integer</para>		
					/// <para>Field ID: 9f76d8df-ef1c-4b6d-af33-b2655fd0cc74</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_Listing_ConfigurationConstants.Items_Per_PageFieldName, Setting = SitecoreFieldSettings.InferType)]
					int Items_Per_Page  {get; set;}
			
				}


	public static partial class I_Listing_ConfigurationConstants{

			public const string TemplateIdString = "5ccd0fbb-5349-460f-91f1-f1e960cb0e90";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "_Listing Configuration";

					
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
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// I_Refinement Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Base/_Refinement</para>	
	/// <para>ID: 6c886c92-a367-4757-ae05-423ff9cf394a</para>	
	/// </summary>
	[SitecoreType(TemplateId=I_RefinementConstants.TemplateIdString)]
	public partial interface I_Refinement : IGlassBase 
	{
								/// <summary>
					/// The Facet Title field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 3162cd52-3d1c-41f1-88a1-e5d7c4e89215</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_RefinementConstants.Facet_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Facet_Title  {get; set;}
			
								/// <summary>
					/// The Field Name field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 605fcdd4-db1f-43b1-a138-fc22f5c563f8</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_RefinementConstants.Field_NameFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Field_Name  {get; set;}
			
								/// <summary>
					/// The Filter By Label field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 0c554f2d-abed-43c5-80bb-3236edb7884a</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_RefinementConstants.Filter_By_LabelFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Filter_By_Label  {get; set;}
			
								/// <summary>
					/// The Is Hidden field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: 002cefb3-c0f2-444a-8422-30854bdaca08</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_RefinementConstants.Is_HiddenFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Is_Hidden  {get; set;}
			
								/// <summary>
					/// The Key field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: d1c9128f-881b-4951-a9a3-96f7d60cf743</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(I_RefinementConstants.KeyFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Key  {get; set;}
			
				}


	public static partial class I_RefinementConstants{

			public const string TemplateIdString = "6c886c92-a367-4757-ae05-423ff9cf394a";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "_Refinement";

					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// ISearch_Refinements Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Components/Search Refinements</para>	
	/// <para>ID: 887c07b8-a6a7-4960-9b09-f78f69580e50</para>	
	/// </summary>
	[SitecoreType(TemplateId=ISearch_RefinementsConstants.TemplateIdString)]
	public partial interface ISearch_Refinements : IGlassBase 
	{
								/// <summary>
					/// The Refinements Title field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 77f09046-0a92-4378-b160-965f9e78fd39</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISearch_RefinementsConstants.Refinements_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Refinements_Title  {get; set;}
			
								/// <summary>
					/// The Refinements field.
					/// <para></para>
					/// <para>Field Type: Multilist</para>		
					/// <para>Field ID: 3dd8aa26-2b10-4305-831c-7fcc78a674be</para>
					/// <para>Custom Data: generic=I_Refinement</para>
					/// </summary>
					[SitecoreField(ISearch_RefinementsConstants.RefinementsFieldName, Setting = SitecoreFieldSettings.InferType)]
					IEnumerable<I_Refinement> Refinements  {get; set;}
			
				}


	public static partial class ISearch_RefinementsConstants{

			public const string TemplateIdString = "887c07b8-a6a7-4960-9b09-f78f69580e50";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Search Refinements";

					
			public static readonly ID Refinements_TitleFieldId = new ID("77f09046-0a92-4378-b160-965f9e78fd39");
			public const string Refinements_TitleFieldName = "Refinements Title";
			
					
			public static readonly ID RefinementsFieldId = new ID("3dd8aa26-2b10-4305-831c-7fcc78a674be");
			public const string RefinementsFieldName = "Refinements";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IDate_Range_Refinement Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Date Range Refinement</para>	
	/// <para>ID: 89a08d1f-18dc-432a-9ce3-afcba7845e4d</para>	
	/// </summary>
	[SitecoreType(TemplateId=IDate_Range_RefinementConstants.TemplateIdString)]
	public partial interface IDate_Range_Refinement : IGlassBase , global::Velir.Search.Models.I_Refinement
	{
				}


	public static partial class IDate_Range_RefinementConstants{

			public const string TemplateIdString = "89a08d1f-18dc-432a-9ce3-afcba7845e4d";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Date Range Refinement";

					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IHierarchical_Facet Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Hierarchical Facet</para>	
	/// <para>ID: 981413b1-9651-4b49-b356-d7b7fb9dd8f6</para>	
	/// </summary>
	[SitecoreType(TemplateId=IHierarchical_FacetConstants.TemplateIdString)]
	public partial interface IHierarchical_Facet : IGlassBase , global::Velir.Search.Models.IFacet
	{
								/// <summary>
					/// The Root Item field.
					/// <para></para>
					/// <para>Field Type: Droptree</para>		
					/// <para>Field ID: 733222cf-b934-4dcc-a3b9-b608fffbdabd</para>
					/// <para>Custom Data: type=IGlassBase</para>
					/// </summary>
					[SitecoreField(IHierarchical_FacetConstants.Root_ItemFieldName, Setting = SitecoreFieldSettings.InferType)]
					IGlassBase Root_Item  {get; set;}
			
								/// <summary>
					/// The Valid Templates field.
					/// <para></para>
					/// <para>Field Type: Treelist</para>		
					/// <para>Field ID: 3c02e5a7-bc18-4621-8ab6-2daa34b1ea20</para>
					/// <para>Custom Data: generic=Guid</para>
					/// </summary>
					[SitecoreField(IHierarchical_FacetConstants.Valid_TemplatesFieldName, Setting = SitecoreFieldSettings.InferType)]
					IEnumerable<Guid> Valid_Templates  {get; set;}
			
				}


	public static partial class IHierarchical_FacetConstants{

			public const string TemplateIdString = "981413b1-9651-4b49-b356-d7b7fb9dd8f6";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Hierarchical Facet";

					
			public static readonly ID Root_ItemFieldId = new ID("733222cf-b934-4dcc-a3b9-b608fffbdabd");
			public const string Root_ItemFieldName = "Root Item";
			
					
			public static readonly ID Valid_TemplatesFieldId = new ID("3c02e5a7-bc18-4621-8ab6-2daa34b1ea20");
			public const string Valid_TemplatesFieldName = "Valid Templates";
			
					
			public static readonly ID And_FilterFieldId = new ID("f0b316e1-2d87-4433-9634-40685172dd3c");
			public const string And_FilterFieldName = "And Filter";
			
					
			public static readonly ID Is_Multi_ValueFieldId = new ID("524cf7f5-8ed0-4cd5-9aac-ff92e5bd6fbd");
			public const string Is_Multi_ValueFieldName = "Is Multi Value";
			
					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// ISearch_Listing Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Components/Search Listing</para>	
	/// <para>ID: 9c7e6d73-f5ab-4c81-8feb-ae3377612af2</para>	
	/// </summary>
	[SitecoreType(TemplateId=ISearch_ListingConstants.TemplateIdString)]
	public partial interface ISearch_Listing : IGlassBase , global::Velir.Search.Models.I_Listing_Configuration
	{
								/// <summary>
					/// The Listing Title field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 24dede84-4b6e-453f-9d84-2c7bee6029c4</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISearch_ListingConstants.Listing_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Listing_Title  {get; set;}
			
				}


	public static partial class ISearch_ListingConstants{

			public const string TemplateIdString = "9c7e6d73-f5ab-4c81-8feb-ae3377612af2";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Search Listing";

					
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
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// ISort Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Sorts/Sort</para>	
	/// <para>ID: a216f849-a384-46e8-afac-05860d8c69c1</para>	
	/// </summary>
	[SitecoreType(TemplateId=ISortConstants.TemplateIdString)]
	public partial interface ISort : IGlassBase 
	{
								/// <summary>
					/// The Field Name field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 32e84ec7-0ac5-4da0-a122-7e18c4c4812b</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISortConstants.Field_NameFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Field_Name  {get; set;}
			
								/// <summary>
					/// The Is Default field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: 71080aeb-d9fd-4ffe-b4e7-69c8d933fbe0</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISortConstants.Is_DefaultFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Is_Default  {get; set;}
			
								/// <summary>
					/// The Key field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 1818ab02-885d-41d4-8b71-59da29f81866</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISortConstants.KeyFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Key  {get; set;}
			
								/// <summary>
					/// The Sort Ascending field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: f25e88db-e274-4c5d-8841-0fa770141465</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISortConstants.Sort_AscendingFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Sort_Ascending  {get; set;}
			
								/// <summary>
					/// The Sort Title field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: f5fb1147-82f1-4c02-bff8-cc133618781f</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(ISortConstants.Sort_TitleFieldName, Setting = SitecoreFieldSettings.InferType)]
					string Sort_Title  {get; set;}
			
				}


	public static partial class ISortConstants{

			public const string TemplateIdString = "a216f849-a384-46e8-afac-05860d8c69c1";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Sort";

					
			public static readonly ID Field_NameFieldId = new ID("32e84ec7-0ac5-4da0-a122-7e18c4c4812b");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Is_DefaultFieldId = new ID("71080aeb-d9fd-4ffe-b4e7-69c8d933fbe0");
			public const string Is_DefaultFieldName = "Is Default";
			
					
			public static readonly ID KeyFieldId = new ID("1818ab02-885d-41d4-8b71-59da29f81866");
			public const string KeyFieldName = "Key";
			
					
			public static readonly ID Sort_AscendingFieldId = new ID("f25e88db-e274-4c5d-8841-0fa770141465");
			public const string Sort_AscendingFieldName = "Sort Ascending";
			
					
			public static readonly ID Sort_TitleFieldId = new ID("f5fb1147-82f1-4c02-bff8-cc133618781f");
			public const string Sort_TitleFieldName = "Sort Title";
			
			

	}

}
namespace Velir.Search.Models
{


 	/// <summary>
	/// IBoolean_Refinement Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Velir/Search/Refinements/Boolean Refinement</para>	
	/// <para>ID: ec9f61d5-4b55-4e71-8dfa-20a38d7499c2</para>	
	/// </summary>
	[SitecoreType(TemplateId=IBoolean_RefinementConstants.TemplateIdString)]
	public partial interface IBoolean_Refinement : IGlassBase , global::Velir.Search.Models.I_Refinement
	{
								/// <summary>
					/// The Apply When False field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: a6bc148a-b466-4c37-baa4-581d96d5d06f</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(IBoolean_RefinementConstants.Apply_When_FalseFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Apply_When_False  {get; set;}
			
								/// <summary>
					/// The Apply When True field.
					/// <para></para>
					/// <para>Field Type: Checkbox</para>		
					/// <para>Field ID: c1c73ebe-0e2b-470c-ab9b-9da77182e7c5</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField(IBoolean_RefinementConstants.Apply_When_TrueFieldName, Setting = SitecoreFieldSettings.InferType)]
					bool Apply_When_True  {get; set;}
			
				}


	public static partial class IBoolean_RefinementConstants{

			public const string TemplateIdString = "ec9f61d5-4b55-4e71-8dfa-20a38d7499c2";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Boolean Refinement";

					
			public static readonly ID Apply_When_FalseFieldId = new ID("a6bc148a-b466-4c37-baa4-581d96d5d06f");
			public const string Apply_When_FalseFieldName = "Apply When False";
			
					
			public static readonly ID Apply_When_TrueFieldId = new ID("c1c73ebe-0e2b-470c-ab9b-9da77182e7c5");
			public const string Apply_When_TrueFieldName = "Apply When True";
			
					
			public static readonly ID Facet_TitleFieldId = new ID("3162cd52-3d1c-41f1-88a1-e5d7c4e89215");
			public const string Facet_TitleFieldName = "Facet Title";
			
					
			public static readonly ID Field_NameFieldId = new ID("605fcdd4-db1f-43b1-a138-fc22f5c563f8");
			public const string Field_NameFieldName = "Field Name";
			
					
			public static readonly ID Filter_By_LabelFieldId = new ID("0c554f2d-abed-43c5-80bb-3236edb7884a");
			public const string Filter_By_LabelFieldName = "Filter By Label";
			
					
			public static readonly ID Is_HiddenFieldId = new ID("002cefb3-c0f2-444a-8422-30854bdaca08");
			public const string Is_HiddenFieldName = "Is Hidden";
			
					
			public static readonly ID KeyFieldId = new ID("d1c9128f-881b-4951-a9a3-96f7d60cf743");
			public const string KeyFieldName = "Key";
			
			

	}

}
