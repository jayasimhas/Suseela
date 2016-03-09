using AutoMapper;

namespace Informa.Library.Utilities.AutoMapper.Resolvers
{
    public class AutoMapResolver<TIn, TOut> : BaseValueResolver<TIn, TOut>
    {
        protected override TOut Resolve(TIn source, ResolutionContext context)
        {
            return context.Engine.Mapper.Map<TIn, TOut>(source);
        }
    }
}
