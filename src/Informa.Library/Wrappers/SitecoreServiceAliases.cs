using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreServiceWeb : ISitecoreService { }
    public interface ISitecoreServiceMaster : ISitecoreService { }

    [AutowireService]
    public class SitecoreServiceWeb : SitecoreService, ISitecoreServiceWeb
    {
        public SitecoreServiceWeb() : base(Constants.WebDb) { }
    }

    [AutowireService]
    public class SitecoreServiceMaster : SitecoreService, ISitecoreServiceMaster
    {
        public SitecoreServiceMaster() : base(Constants.MasterDb) { }
    }
}
