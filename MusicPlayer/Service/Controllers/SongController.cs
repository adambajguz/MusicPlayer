﻿using MusicPlayer.Core.CQRS;
using MusicPlayer.Service.Controllers.Song;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.Service.Controllers
{
    public class SongController
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public SongController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetSongs()
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Result>>(new GetSongs.Query());
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetSong.Query, Result>(new GetSong.Query() { ID = id });
        }

        public async Task Create(int score, string title, DateTime creationDate, string filePath)
        {
            await _commandDispatcher.Dispatch<CreateSong.Command>(new CreateSong.Command
            {
                _data = new CreateSong.Data(score, title, creationDate, filePath)

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteSong.Command>(new DeleteSong.Command
            {
                ID = id
            });
        }
    }
}