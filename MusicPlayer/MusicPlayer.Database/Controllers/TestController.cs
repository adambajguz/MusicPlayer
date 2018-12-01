//using Microsoft.AspNetCore.Mvc;
//using Test.API.Controllers.Configuration;
//using Test.Core.CQRS;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Test.API.Controllers
//{
//    [Route("api/[controller]")]
//    public class ConfigurationController : Controller
//    {
//        private IQueryDispatcher _queryDispatcher;
//        private ICommandDispatcher _commandDispatcher;

//        public ConfigurationController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
//        {
//            _queryDispatcher = queryDispatcher;
//            _commandDispatcher = commandDispatcher;
//        }

//        [HttpGet("[action]")]
//        public async Task<List<GetVersions.Result>> Versions()
//        {
//            return await _queryDispatcher.Dispatch<GetVersions.Query, List<GetVersions.Result>>(new GetVersions.Query());
//        }

//    }
//}