using System;
using System.Web;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Wrappers
{
    public interface IHttpContextProvider
    {
        HttpContextBase Current { get; }
        Uri RequestUri { get; }
    }

    [AutowireService]
    public class HttpContextProvider : IHttpContextProvider
    {
        public HttpContextBase Current => new HttpContextWrapper(HttpContext.Current);

        public Uri RequestUri => HttpContext.Current?.Request.Url;
    }
}