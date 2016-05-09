using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreServiceWeb : ISitecoreService { }
    public interface ISitecoreServiceMaster : ISitecoreService { }

    public class SitecoreServiceWeb : SitecoreService, ISitecoreServiceWeb
    {
        public SitecoreServiceWeb() : base(Constants.WebDb) { }
    }

    public class SitecoreServiceMaster : SitecoreService, ISitecoreServiceMaster
    {
        public SitecoreServiceMaster() : base(Constants.MasterDb) { }
    }
}
