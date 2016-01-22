using System.Linq;

namespace Informa.Models.FieldSources
{


    public class FactoryInterfaceDataSource : Sitecore.Buckets.FieldTypes.IDataSource
    {
        public Sitecore.Data.Items.Item[] ListQuery(Sitecore.Data.Items.Item item)
        {
            item.Fields.ReadAll();

            return item.Fields.Select(x => x.InnerItem).ToArray();

            //List<TemplateItem> templates = new List<TemplateItem>();
            //var template = item.Template;
            //if (template != null)
            //{
            //    if (template.ID == new ID("f4dbbc93-2591-4aa8-b71a-1dc4acd5c941"))
            //    {
            //        templates.Add(template);
            //    }

            //    foreach (var t in template.BaseTemplates) 

            //}

            //    return item.Parent.Children.ToArray();

        }
    }
}
//				  namespace Informa.Models.InterfaceFactory
//{

// /// <summary>
//	/// IInterfaceTemplate Interface
//	/// <para></para>
//	/// <para>Path: /sitecore/templates/Velir/FactoryInterface/InterfaceTemplate</para>	
//	/// <para>ID: f4dbbc93-2591-4aa8-b71a-1dc4acd5c941</para>	
//	/// </summary>
//	[GlassFactoryInterface]
//	public partial interface IInterfaceFactoryTemplate 
//	{
//				}
//				} 
//namespace Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface
//{


// 	/// <summary>
//	/// ILinkable Interface
//	/// <para></para>
//	/// <para>Path: /sitecore/templates/Velir/FactoryInterface/Linkable</para>	
//	/// <para>ID: 3efeaf26-8572-417d-a8a8-6cb3a28eaf82</para>	
//	/// </summary>
//	[GlassFactoryInterface]
//	public partial interface ILinkable : IInterfaceFactoryTemplate , global::Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface.IInterfaceTemplate
//	{

//								/// <summary>
//					/// The LinkableText field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 529c867f-2816-43a9-964a-3f16c014a4f7</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					string LinkableText  {get;}
//								/// <summary>
//					/// The LinkableUrl field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: d156cd07-71a4-4daa-b52a-5ce602d6d3bb</para>
//					/// <para>Custom Data: type=Link</para>
//					/// </summary>

//					Link LinkableUrls  {get;}
//				}

//	public partial class LinkableModel : ILinkable{

//    public Guid Item_Id { get; set; }
//								/// <summary>
//					/// The LinkableText field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 529c867f-2816-43a9-964a-3f16c014a4f7</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					public virtual string LinkableText  {get; set;}
//								/// <summary>
//					/// The LinkableUrl field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: d156cd07-71a4-4daa-b52a-5ce602d6d3bb</para>
//					/// <para>Custom Data: type=Link</para>
//					/// </summary>

//					public virtual Link LinkableUrls  {get; set;}

//	}

//	public class LinkableConfig : SitecoreGlassMap<ILinkable> 
//    {
//        public override void Configure()
//        {
//            Map(
//                x => x.AutoMap()
//				,                                 
//				x =>  x.Delegate(y =>  y.LinkableText).GetValue((context) => 
//				{
//					if(!string.IsNullOrEmpty(context.Item.Fields["LinkableText"]?.Value)){
//						return context.Item[context.Item.Fields["LinkableText"]?.Value];
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.LinkableUrls).GetValue((context) => 
//				{
//                    return new Link(); //TODO: Fix this
//                    if (!string.IsNullOrEmpty(context.Item.Fields["LinkableUrl"]?.Value)){
//						return context.Item[context.Item.Fields["LinkableUrl"]?.Value];
//					}					                

//					return null;
//				})
//				                );
//        }
//    }

//}
//namespace Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface
//{


// 	/// <summary>
//	/// IListable Interface
//	/// <para></para>
//	/// <para>Path: /sitecore/templates/Velir/FactoryInterface/Listable</para>	
//	/// <para>ID: a3ec5da5-c37c-44a5-83f3-96d45b53389f</para>	
//	/// </summary>
//	[GlassFactoryInterface]
//	public partial interface IListable : IInterfaceFactoryTemplate , global::Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface.IInterfaceTemplate
//	{
//        						/// <summary>
//					/// The ListableAuthors field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 5f51fa66-5326-4aa6-9671-57a48aedeaa4</para>
//					/// <para>Custom Data: generic=ILinkable</para>
//					/// </summary>

//					IEnumerable<ILinkable> ListableAuthors  {get;}
//								/// <summary>
//					/// The ListableDate field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 73f216e4-c244-4a3b-95ee-07d9ffd2d4f7</para>
//					/// <para>Custom Data: type=DateTime?</para>
//					/// </summary>

//					DateTime? ListableDate  {get;}
//								/// <summary>
//					/// The ListableImage field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 6d481d3a-7c71-488f-88ff-9c53de9b4ac3</para>
//					/// <para>Custom Data: type=Image</para>
//					/// </summary>

//					Image ListableImage  {get;}
//								/// <summary>
//					/// The ListableSummary field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 241b01c9-b518-4182-9b94-c40db3fb9cb2</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					string ListableSummary  {get;}
//								/// <summary>
//					/// The ListableTitle field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: a92e4bfb-3010-4332-a7a0-c788348993ce</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					string ListableTitle  {get;}
//								/// <summary>
//					/// The ListableTopics field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 629bddb3-5e8e-49bb-a323-6e1c46bc7cb5</para>
//					/// <para>Custom Data: generic=ILinkable</para>
//					/// </summary>

//					IEnumerable<ILinkable> ListableTopics  {get;}
//								/// <summary>
//					/// The ListableUrl field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 5083ca52-6de6-4be9-a1e6-30e9579e782d</para>
//					/// <para>Custom Data: type=Link</para>
//					/// </summary>

//					Link ListableUrl  {get;}
//				}

//	public partial class ListableModel : IListable{

//    public Guid Item_Id { get; set; }
//								/// <summary>
//					/// The ListableAuthors field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 5f51fa66-5326-4aa6-9671-57a48aedeaa4</para>
//					/// <para>Custom Data: generic=ILinkable</para>
//					/// </summary>

//					public virtual IEnumerable<ILinkable> ListableAuthors  {get; set;}
//								/// <summary>
//					/// The ListableDate field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 73f216e4-c244-4a3b-95ee-07d9ffd2d4f7</para>
//					/// <para>Custom Data: type=DateTime?</para>
//					/// </summary>

//					public virtual DateTime? ListableDate  {get; set;}
//								/// <summary>
//					/// The ListableImage field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 6d481d3a-7c71-488f-88ff-9c53de9b4ac3</para>
//					/// <para>Custom Data: type=Image</para>
//					/// </summary>

//					public virtual Image ListableImage  {get; set;}
//								/// <summary>
//					/// The ListableSummary field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 241b01c9-b518-4182-9b94-c40db3fb9cb2</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					public virtual string ListableSummary  {get; set;}
//								/// <summary>
//					/// The ListableTitle field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: a92e4bfb-3010-4332-a7a0-c788348993ce</para>
//					/// <para>Custom Data: type=string</para>
//					/// </summary>

//					public virtual string ListableTitle  {get; set;}
//								/// <summary>
//					/// The ListableTopics field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 629bddb3-5e8e-49bb-a323-6e1c46bc7cb5</para>
//					/// <para>Custom Data: generic=ILinkable</para>
//					/// </summary>

//					public virtual IEnumerable<ILinkable> ListableTopics  {get; set;}
//								/// <summary>
//					/// The ListableUrl field.
//					/// <para></para>
//					/// <para>Field Type: Multilist</para>		
//					/// <para>Field ID: 5083ca52-6de6-4be9-a1e6-30e9579e782d</para>
//					/// <para>Custom Data: type=Link</para>
//					/// </summary>

//					public virtual Link ListableUrl  {get; set;}

//	}

//	public class ListableConfig : SitecoreGlassMap<IListable> 
//    {
//        public override void Configure()
//        {
//            Map(
//                x => x.AutoMap()
//				,
//				x =>  x.Field(y =>  y.ListableAuthors).FieldId(IArticleConstants.AuthorsFieldId)
//				,
//				x =>  x.Delegate(y =>  y.ListableDate).GetValue((context) =>
//				{
//				    return DateTime.Now;
//					if(!string.IsNullOrEmpty(context.Item.Fields["ListableDate"]?.Value))
//					{
//					    DateTime dateTime;
//						var date = DateTime.TryParse(context.Item[context.Item.Fields["ListableDate"]?.Value], out dateTime);

//                        return dateTime;
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.ListableImage).GetValue((context) =>
//				{
//				    return new Image {Src = "http://placehold.it/787x443"};


//                    if (!string.IsNullOrEmpty(context.Item.Fields["ListableImage"]?.Value)){
//						return context.Item[context.Item.Fields["ListableImage"]?.Value];
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.ListableSummary).GetValue((context) => 
//				{
//					if(!string.IsNullOrEmpty(context.Item.Fields["ListableSummary"]?.Value)){
//						return context.Item[context.Item.Fields["ListableSummary"]?.Value];
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.ListableTitle).GetValue((context) => 
//				{
//					if(!string.IsNullOrEmpty(context.Item.Fields["ListableTitle"]?.Value)){
//						return context.Item[context.Item.Fields["ListableTitle"]?.Value];
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.ListableTopics).GetValue((context) => 
//				{
//					if(!string.IsNullOrEmpty(context.Item.Fields["ListableTopics"]?.Value)){
//						return context.Item[context.Item.Fields["ListableTopics"]?.Value];
//					}					                

//					return null;
//				})
//				,
//				x =>  x.Delegate(y =>  y.ListableUrl).GetValue((context) => 
//				{
//                    //if(!string.IsNullOrEmpty(context.Item.Fields["ListableUrl"]?.Value)){
//                    //	return context.Item[context.Item.Fields["ListableUrl"]?.Value];
//                    //}					                

//                    return new Link();
//				})
//				                );
//        }
//    }

//}
//namespace Informa.Models.Glass.Models.sitecore.templates.Velir.FactoryInterface
//{


// 	/// <summary>
//	/// IInterfaceTemplate Interface
//	/// <para></para>
//	/// <para>Path: /sitecore/templates/Velir/FactoryInterface/InterfaceTemplate</para>	
//	/// <para>ID: f4dbbc93-2591-4aa8-b71a-1dc4acd5c941</para>	
//	/// </summary>
//	[GlassFactoryInterface]
//	public partial interface IInterfaceTemplate : IInterfaceFactoryTemplate 
//	{   
//	 Guid Item_Id { get; }
//				}

//	public partial class InterfaceTemplateModel : IInterfaceTemplate{

//    public Guid Item_Id { get; set; }

//	}

//	public class InterfaceTemplateConfig : SitecoreGlassMap<IInterfaceTemplate> 
//    {
//        public override void Configure()
//        {
//            Map(
//                x => x.AutoMap(),
//                x => x.Id(y => y.Item_Id)
//				                );
//        }
//    }

//}
