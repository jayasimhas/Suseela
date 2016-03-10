using AutoMapper;

namespace Informa.Library.Utilities.AutoMapper.Resolvers
{
    public abstract class BaseValueResolver<TIn, TOut> : IValueResolver
    {
        public virtual ResolutionResult Resolve(ResolutionResult source)
        {
            var result = Resolve((TIn) source.Value, source.Context);

            return result == null
                ? source.Ignore()
                : source.New(result);
        }

        protected abstract TOut Resolve(TIn source, ResolutionContext context);
    }
}
