using MusicPlayer.Core.CQRS;
using MusicPlayer.UWP.Controllers.Artist;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers
{
    public class ArtistController : IArtistController, IController<Result>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public ArtistController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<Result>> GetAll()
        {
            return await _queryDispatcher.Dispatch<GetArtists.Query, List<Result>>(new GetArtists.Query());
        }

        public async Task<List<Result>> GetAllDescending()
        {
            return await _queryDispatcher.Dispatch<GetArtistsDescending.Query, List<Result>>(new GetArtistsDescending.Query());
        }

        public async Task<List<Result>> Search(string name)
        {
            return await _queryDispatcher.Dispatch<SearchArtists.Query, List<Result>>(new SearchArtists.Query() { Name = name });
        }

        public async Task<Result> Get(int id)
        {
            return await _queryDispatcher.Dispatch<GetArtist.Query, Result>(new GetArtist.Query() { ID = id });
        }

        public async Task<List<Song.Result>> GetSongs(int artistId)
        {
            return await _queryDispatcher.Dispatch<GetSongs.Query, List<Song.Result>>(new GetSongs.Query() { ID = artistId });
        }

        public async Task AddSong(int artistId, int songId)
        {
            await _commandDispatcher.Dispatch<AddSong.Command>(new AddSong.Command
            {
                _data = new AddSong.Data(artistId, songId)
            });
        }

        public async Task Create(string name, string surname, string pseudonym, DateTime birthdate, string description, int bandId, int imageId)
        {
            await _commandDispatcher.Dispatch<CreateArtist.Command>(new CreateArtist.Command
            {
                _data = new CreateArtist.Data(name, surname, pseudonym, birthdate, description, bandId, imageId)

            });

        }

        public async Task Update(int id,string name, string surname, string pseudonym, DateTime birthdate, string description, int bandId, int imageId)
        {
            await _commandDispatcher.Dispatch<UpdateArtist.Command>(new UpdateArtist.Command
            {
                ID = id,
                Surname=surname,
                Name=name,
                Pseudonym=pseudonym,
                Birthdate=birthdate,
                Description=description,
                BandId=bandId,
                ImageId=imageId

            });

        }

        public async Task Delete(int id)
        {
            await _commandDispatcher.Dispatch<DeleteArtist.Command>(new DeleteArtist.Command
            {
                ID = id
            });
        }

        public async Task DeleteSong(int songId, int artistId)
        {
            await _commandDispatcher.Dispatch<DeleteSong.Command>(new DeleteSong.Command
            {
                SongId = songId,
                ArtistId = artistId
            });
        }
    }
}
