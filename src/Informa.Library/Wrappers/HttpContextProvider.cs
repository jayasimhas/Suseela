using System;
using System.Web;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Wrappers
{
    public interface IHttpContextProvider
    {
        HttpContextBase Current { get; }
        Uri RequestUrl { get; }
    }

    [AutowireService]
    public class HttpContextProvider : IHttpContextProvider
    {
        public HttpContextBase Current => new HttpContextWrapper(HttpContext.Current);

        public Uri RequestUrl => HttpContext.Current?.Request.Url;
    }
}