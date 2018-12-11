﻿using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers.Album
{
    public class GetAlbums
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
                var result = await _uow.AlbumRepository.Query().Select(x => new Result(x)).OrderBy(y => y.Title).ToListAsync();

                return result;
            }

        }
    }
}