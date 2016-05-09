using System.Linq;
using FuelSDK;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IExactTargetWrapper
    {
        CreateResult CreateEmail(FuelSDK.Email etEmail, out string status);
        UpdateResult UpdateEmail(FuelSDK.Email etEmail, out string status);
    }

    [AutowireService]
    public class ExactTargetWrapper : IExactTargetWrapper
    {
        public CreateResult CreateEmail(FuelSDK.Email etEmail, out string status)
        {
            string requestId;
            var proxy = new FuelSDK.SoapClient();
            CreateResult[] results = proxy.Create(new CreateOptions(), new APIObject[] { etEmail }, out requestId, out status);

            return results.FirstOrDefault();
        }

        public UpdateResult UpdateEmail(FuelSDK.Email etEmail, out string status)
        {
            string requestId;
            var soapClient = new SoapClient();
            UpdateResult[] results = soapClient.Update(new UpdateOptions(), new APIObject[] { etEmail }, out requestId, out status);

            return results.FirstOrDefault();
        }
    }
}