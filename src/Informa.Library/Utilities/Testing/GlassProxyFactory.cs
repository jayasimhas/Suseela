using System;
using System.Linq;
using Castle.DynamicProxy;
using Jabberwocky.Glass.Factory.Interfaces;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Utilities.Testing
{
	public static class GlassProxyFactory
	{
		private static readonly Lazy<ProxyGenerator> LazyProxyGenerator = new Lazy<ProxyGenerator>();
		private static ProxyGenerator ProxyGenerator { get { return LazyProxyGenerator.Value; } }

		/// <summary>
		/// Creates a new Glass Interface Factory model proxy
		/// </summary>
		/// <typeparam name="T">The type of the Glass Interface to proxy for</typeparam>
		/// <typeparam name="TGlassItem"></typeparam>
		/// <param name="innerItem">The type of the inner glass item to pass to the proxy</param>
		/// <param name="params">Optional constructor arguments to pass to the underlying Glass Interface Factory model's proxy implementation</param>
		/// <returns></returns>
		public static T For<T, TGlassItem>(TGlassItem innerItem, params object[] @params) where TGlassItem : class, IGlassBase where T : BaseInterface<TGlassItem>
		{
			return ProxyGenerator.CreateClassProxy(typeof(T), new object[] { innerItem }.Concat(@params).ToArray()) as T;
		}
	}
}
