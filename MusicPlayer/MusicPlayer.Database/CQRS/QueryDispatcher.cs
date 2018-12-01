using Autofac;
using FluentValidation;
using MusicPlayer.Core.CQRS;
using System.Threading.Tasks;

namespace MusicPlayer.DatabaseService.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _context;

        public QueryDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery
        {
            AbstractValidator<TQuery> validator;
            if (_context.TryResolve(out validator))
            {
                var validationResult = validator.Validate(query);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            IReadCacheHandler<TQuery, TResult> cache;
            if (_context.TryResolve(out cache))
            {
                TResult result = await cache.Read(query);
                if (result != null)
                {
                    return result;
                }
            }

            var handler = _context.Resolve<IQueryHandler<TQuery, TResult>>();
            var queryResult = await handler.Handle(query);

            IWriteCacheHandler<TQuery, TResult> cache2;
            if (_context.TryResolve(out cache2))
            {
                await cache2.Save(query, queryResult);
            }

            return queryResult;
        }
    }
}