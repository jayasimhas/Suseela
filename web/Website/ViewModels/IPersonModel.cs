using Glass.Mapper.Sc.Fields;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
    public interface IPersonModel
    {
        string Name { get; }
        string Twitter { get; }
        string Email_Address { get; }
        Image Image { get; }
    }
}