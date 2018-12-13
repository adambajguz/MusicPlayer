﻿using FluentValidation;
using MusicPlayer.Core.CQRS;
using MusicPlayer.Core.Data;
using System;
using System.Threading.Tasks;

namespace MusicPlayer.UWP.Controllers.Artist
{
    public class CreateArtist
    {
        public class Command : ICommand
        {
            public Data _data { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task Execute(Command command)
            {
                _uow.ArtistRepository.Insert(command._data.GetEntity());
                await _uow.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {

            public Validator()
            {
                //RuleFor(x => x._data.Value).GreaterThan(0);
            }
        }

        // Private Class Data Pattern
        public class Data : ICreateResultData<Core.Entities.Artist>
        {
            private Core.Entities.Artist artist;

            public Data(string name, string surname, string pseudonym, DateTime birthdate, string description)
            {
                artist = new Core.Entities.Artist();

                //artist.Band = x.Band;
                //artist.Photo = x.Photo;
                artist.Name = name;
                artist.Surname = surname;
                artist.Pseudonym = pseudonym;
                artist.Birthdate = birthdate;
                artist.Description = description;
            }

            public Core.Entities.Artist GetEntity()
            {
                return artist;
            }
        }
    }
}

