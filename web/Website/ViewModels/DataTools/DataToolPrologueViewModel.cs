using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.DataTools
{
    [AutowireService(LifetimeScope.PerScope)]
    public class DataToolPrologueViewModel : IDataToolPrologueViewModel
    {
        protected readonly IDataToolProloguePrintViewModel DataToolProloguePrintViewModel;
        protected readonly IDataToolPrologueEmailViewModel DataToolPrologueEmailViewModel;
        protected readonly IDataToolPrologueShareViewModel DataToolPrologueShareViewModel;
        protected readonly IDataToolTagsViewModel DataToolTagsViewModel;
        public DataToolPrologueViewModel(
            IDataToolProloguePrintViewModel dataToolProloguePrintViewModel,
            IDataToolPrologueEmailViewModel dataToolPrologueEmailViewModel,
            IDataToolPrologueShareViewModel dataToolPrologueShareViewModel,
            IDataToolTagsViewModel dataToolTagsViewModel)
        {
            DataToolProloguePrintViewModel = dataToolProloguePrintViewModel;
            DataToolPrologueEmailViewModel = dataToolPrologueEmailViewModel;
            DataToolPrologueShareViewModel = dataToolPrologueShareViewModel;
            DataToolTagsViewModel = dataToolTagsViewModel;
        }

        public IDataToolProloguePrintViewModel PrintViewModel => DataToolProloguePrintViewModel;

        public IDataToolPrologueEmailViewModel EmailViewModel => DataToolPrologueEmailViewModel;

        public IDataToolPrologueShareViewModel ShareViewModel => DataToolPrologueShareViewModel;

        public IDataToolTagsViewModel TagsViewModel => DataToolTagsViewModel;
    }
}