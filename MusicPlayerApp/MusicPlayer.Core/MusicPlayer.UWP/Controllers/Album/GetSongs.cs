﻿using Microsoft.EntityFrameworkCore;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Album
{
    public class GetSongs
    {
        public class Query : IQuery
        {
            public int ID;
        }
        public class HandlerList : IQueryHandler<Query, List<Song.Result>>
        {
            private IUnitOfWork _uow;
            public HandlerList(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<List<Song.Result>> Handle(Query query)
            {
                var songs = await _uow.SongAlbumRepository.Query().Where(x => x.AlbumId == query.ID).Select(x => x.SongId).ToListAsync();
                var result = await _uow.SongRepository.Query().Where(x => songs.Contains(x.Id)).Select(x => new Song.Result(x)).ToListAsync();

                return result;
            }

        }
    }
}
