using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
	public interface IListableViewModel : IListable
	{
		bool DisplayImage { get; set; }
	}
}
