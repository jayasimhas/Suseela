using System.Linq;
using FuelSDK;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IExactTargetWrapper
    {
        ExactTargetResponse CreateEmail(ET_Email etEmail);
        ExactTargetResponse UpdateEmail(ET_Email etEmail);
    }

    [AutowireService]
    public class ExactTargetWrapper : IExactTargetWrapper
    {
        public ExactTargetResponse CreateEmail(ET_Email etEmail)
        {
            var client = new ET_Client();
            etEmail.AuthStub = client;
            var response = etEmail.Post();
            var result = response.Results.FirstOrDefault();
            return new ExactTargetResponse
            {
                Success = response.Status && result != null,
                ExactTargetEmailId = result?.NewID ?? 0,
                Message = response.Message
            };
        }

        public ExactTargetResponse UpdateEmail(ET_Email etEmail)
        {
            var client = new ET_Client();
            etEmail.AuthStub = client;
            var response = etEmail.Patch();

            return new ExactTargetResponse
            {
                ExactTargetEmailId = etEmail.ID,
                Success = response.Status,
                Message = response.Message
            };
        }
    }

    public class ExactTargetResponse
    {
        public int ExactTargetEmailId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}