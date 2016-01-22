using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
    public interface IPersonModel
    {
        string Name { get; }
        string Twitter { get; }
        string Email_Address { get; }
        string Image { get; }
    }
}