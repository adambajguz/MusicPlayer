using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Album;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class AlbumController : IAlbumController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AlbumController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetAlbums.Query, List<Result>>(new GetAlbums.Query());
        }

        public async Task<List<Result>> GetAllDescending()
        {
            return await _queryDispatcher.Dispatch<GetAlbumsDescending.Query, List<Result>>(new GetAlbumsDescending.Query());
        }

        public async Task<List<Result>> GetAllByDate()
        {
            return await _queryDispatcher.Dispatch<GetAlbumsByDate.Query, List<Result>>(new GetAlbumsByDate.Query());
        }

        public async Task<List<Result>> GetAllByDateDescending()
        {
            return await _queryDispatcher.Dispatch<GetAlbumsByDateDescending.Query, List<Result>>(new GetAlbumsByDateDescending.Query());
        }

        public async Task<List<Result>> GetAllByPublicationDate()
        {
            return await _queryDispatcher.Dispatch<GetAlbumsByPublicationDate.Query, List<Result>>(new GetAlbumsByPublicationDate.Query());
        }

        public async Task<List<Result>> GetAllByPublicationDateDescending()
        {
            return await _queryDispatcher.Dispatch<GetAlbumsByPublicationDateDescending.Query, List<Result>>(new GetAlbumsByPublicationDateDescending.Query());
        }

        public async Task<List<Result>> Search(string name)
        {
            return await _queryDispatcher.Dispatch<SearchAlbums.Query, List<Result>>(new SearchAlbums.Query() { Name = name });
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetAlbum.Query, Result>(new GetAlbum.Query() { ID = id });
        }

        public async Task<List<Song.Result>> GetSongs(int albumId)
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Song.Result>>(new GetSongs.Query() { ID = albumId });
        }

        public async Task Create(string title, string description, DateTime publicationDate, int imageId)
        {
            await _commandDispatcher.Dispatch<CreateAlbum.Command>(new CreateAlbum.Command
            {
                _data = new CreateAlbum.Data(title, description, publicationDate, imageId)

            });

        }

        public async Task Update(int id,string title, string description, DateTime publicationDate, int imageId)
        {
                await _commandDispatcher.Dispatch<UpdateAlbum.Command>(new UpdateAlbum.Command
            {
                ID = id,
                Title=title,
                Description=description,
                PublicationDate=publicationDate,
                ImageId=imageId
                
            });

        }

        public async Task AddSong(int albumId,int songId, int trackNumber)
        {
            await _commandDispatcher.Dispatch<AddSong.Command>(new AddSong.Command
            {
                _data=new AddSong.Data(albumId,songId,trackNumber)
            });
        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteAlbum.Command>(new DeleteAlbum.Command
            {
                ID = id
            });
        }

        public async Task DeleteSong(int songId, int albumId)
        {
            await _commandDispatcher.Dispatch<DeleteSong.Command>(new DeleteSong.Command
            {
                SongId = songId,
                AlbumId = albumId
            });
        }
    }
}
