﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicPlayer.Data;

namespace MusicPlayer.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MusicPlayer.Core.Entities.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CoverImageId");

                    b.Property<DateTime>("DBCreationDate");

                    b.Property<string>("Description");

                    b.Property<DateTime>("PublicationDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CoverImageId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BandId");

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("Pseudonym");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.HasIndex("BandId");

                    b.HasIndex("PhotoId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Band", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationData");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.ToTable("Bands");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DBCreationDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.PlayQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SongId");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("PlayQueues");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumId");

                    b.Property<int?>("ArtistId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("DBCreationDate");

                    b.Property<string>("FilePath");

                    b.Property<int?>("GenreId");

                    b.Property<int?>("ImageId");

                    b.Property<TimeSpan>("Length");

                    b.Property<long>("PlayTimes");

                    b.Property<int>("Score");

                    b.Property<string>("Title");

                    b.Property<long>("bitrate");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("ArtistId");

                    b.HasIndex("GenreId");

                    b.HasIndex("ImageId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongAlbum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumId");

                    b.Property<int?>("SongId");

                    b.Property<int>("TrackNumber");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("SongId");

                    b.ToTable("SongAlbums");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongArtist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArtistId");

                    b.Property<int?>("SongId");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("SongId");

                    b.ToTable("SongArtists");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongPlaylist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Order");

                    b.Property<int?>("PlaylistId");

                    b.Property<int?>("SongId");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("SongId");

                    b.ToTable("SongPlaylists");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Album", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Image", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Artist", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Band", "Band")
                        .WithMany()
                        .HasForeignKey("BandId");

                    b.HasOne("MusicPlayer.Core.Entities.Image", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.PlayQueue", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.Song", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumId");

                    b.HasOne("MusicPlayer.Core.Entities.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");

                    b.HasOne("MusicPlayer.Core.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");

                    b.HasOne("MusicPlayer.Core.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongAlbum", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumId");

                    b.HasOne("MusicPlayer.Core.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongArtist", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");

                    b.HasOne("MusicPlayer.Core.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId");
                });

            modelBuilder.Entity("MusicPlayer.Core.Entities.SongPlaylist", b =>
                {
                    b.HasOne("MusicPlayer.Core.Entities.Playlist", "Playlist")
                        .WithMany()
                        .HasForeignKey("PlaylistId");

                    b.HasOne("MusicPlayer.Core.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId");
                });
#pragma warning restore 612, 618
        }
    }
}
