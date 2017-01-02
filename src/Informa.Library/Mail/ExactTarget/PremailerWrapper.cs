using System.Linq;
using Informa.Library.Logging;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IPremailerWrapper
    {
        string InlineCss(string html);
    }

    [AutowireService]
    public class PremailerWrapper : IPremailerWrapper
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            ILogWrapper LogWrapper { get; }
        }

        public PremailerWrapper(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string InlineCss(string html)
        {
            var inliner = new PreMailer.Net.PreMailer(html);
            var result = inliner.MoveCssInline(ignoreElements: "#ignore", removeComments: false);

            if (result.Warnings.Any())
            {
                _dependencies.LogWrapper.SitecoreWarn(
                    $"Email CSS inliner processed with {result.Warnings.Count} warnings.  Warnings log at Debug.");
                result.Warnings.ForEach(warn => _dependencies.LogWrapper.SitecoreDebug($"CSS Inliner Warning: {warn}"));
            }

            return result.Html;
        }
    }
}