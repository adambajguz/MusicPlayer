//using SIP.Core.CQRS;
//using SIP.Core.Entities;
//using SIP.Core.Services.Schema;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SIP.API.Controllers.Configuration
//{
//    public class GetVersions
//    {
//        public class Query : IQuery
//        {
//        }

//        public class Handler : IQueryHandler<Query, List<Result>>
//        {
//            private ISchemaProvider _schemaProvider;

//            public Handler(ISchemaProvider schemaProvider)
//            {
//                _schemaProvider = schemaProvider;
//            }

//            public async Task<List<Result>> Handle(Query query)
//            {
//                WebSchema schema = await _schemaProvider.GetWebSchemaAsync();

//                return schema.Sitecores.Select(s => new Result()
//                {
//                    Id = s.Id,
//                    Name = s.Name
//                }).ToList();
//            }
//        }

//        public class Result
//        {
//            public string Id { get; set; }
//            public string Name { get; set; }
//        }
//    }


//public class Handler : IQueryHandler<Query, Result>
//{
//    private IUnitOfWork _uow;
//    public Handler(IUnitOfWork uow)
//    {
//        _uow = uow;
//    }

//    public async Task<Result> Handle(Query query)
//    {
//        var result = await _uow.ProductRepository.Query().Where(x => x.Id == query.ID).Select(x => new Result()
//        {
//            Name = x.Name,
//            Picture = x.Picture,
//            Description = x.Description,
//            Tags = x.Tags,
//            Count = x.Count,
//            CurrentPriceId = x.CurrentPriceId,
//            CategoryId = x.CategoryId
//        }
//        ).FirstOrDefaultAsync();
//        return result;
//    }
//}