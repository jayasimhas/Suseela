using Glass.Mapper.Sc.Fields;

namespace Informa.Library.AgrowBuyers
{
    public class AgrowTile: IAgrowTile
    {
        public Image AgrowLogo { get; set; }
        public string AgrowTitle { get; set; }
        public string AgrowShortDescription { get; set; }
        public string AgrowDetailUrl { get; set; }
        public Image AgrowBanner { get; set; }
    }
}
