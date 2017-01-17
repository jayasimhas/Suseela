using Glass.Mapper.Sc.Fields;

namespace Informa.Library.AgrowBuyers
{
    interface IAgrowTile
    {
        Image AgrowLogo { get; set; }
        string AgrowTitle { get; set; }
        string AgrowShortDescription { get; set; }
        string AgrowDetailUrl { get; set; }
        string AgrowDetailText { get; set; }
        Image AgrowBanner { get; set; }
    }
}
