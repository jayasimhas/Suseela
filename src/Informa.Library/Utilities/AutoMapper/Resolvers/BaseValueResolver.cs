using AutoMapper;

namespace Informa.Library.Utilities.AutoMapper.Resolvers
{
    public abstract class BaseValueResolver<TIn, TOut> : IValueResolver
    {
        public virtual ResolutionResult Resolve(ResolutionResult source)
        {
            return source.New(Resolve((TIn) source.Value, source.Context));
        }

        protected abstract TOut Resolve(TIn source, ResolutionContext context);
    }
}
