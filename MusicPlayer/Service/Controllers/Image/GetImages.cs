﻿using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Image
{
    public class GetImages
    {
        public class Query : IQuery
        {
            public int ID;
        }
        public class HandlerList : IQueryHandler<Query, List<Result>>
        {


            private IUnitOfWork _uow;
            public HandlerList(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Result>> Handle(Query query)
            {
                var result = await _uow.ImageRepository.Query().Select(x => new Result()
                {
                    FilePath = x.FilePath
                }
                ).ToListAsync();

                return result;
            }

        }

        public class Result
        {
            public string FilePath { get; set; }

        }
    }
}
