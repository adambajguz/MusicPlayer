﻿using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Band
{
    public class GetBand
    {
        public class Query : IQuery
        {
            public int ID;
        }

        public class Handler : IQueryHandler<Query, Result>
        {
            private IUnitOfWork _uow;
            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Result> Handle(Query query)
            {
                var result = await _uow.BandRepository.Query().Where(x => x.Id == query.ID).Select(x => new Result()
                {
                    name=x.name,
                    CreationData=x.CreationData,
                    EndDate=x.EndDate,
                    Description=x.Description

                }
                ).FirstOrDefaultAsync();
                return result;
            }
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
                var result = await _uow.BandRepository.Query().Select(x => new Result()
                {
                    name = x.name,
                    CreationData = x.CreationData,
                    EndDate = x.EndDate,
                    Description = x.Description
                }
                ).ToListAsync();

                return result;
            }

        }

        public class Result
        {
            public string name { get; set; }
            public DateTime CreationData { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }

        }
    }
}
