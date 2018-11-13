CREATE TABLE [Album] (
	ID integer NOT NULL,
	Okladka integer NOT NULL,
	tytul text NOT NULL,
	opis integer NOT NULL,
	dataWydania datetime NOT NULL,
	wytwornia string NOT NULL,
	dataUtworzeniaWBazie datetime NOT NULL,
  CONSTRAINT [PK_ALBUM] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Utwor] (
	ID integer NOT NULL UNIQUE,
	Artysta integer NOT NULL,
	Album integer,
	Grafika integer,
	Gatunek integer NOT NULL,
	ocena integer NOT NULL DEFAULT '0',
	tytul text NOT NULL,
	dataPowstania datetime NOT NULL,
	sciezkaDoPliku text,
	dlugosc timestamp NOT NULL,
	bitrate integer NOT NULL,
	dataUtworzeniaWBazie datetime NOT NULL,
  CONSTRAINT [PK_UTWOR] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Gatunek] (
	ID integer NOT NULL,
	nazwa text NOT NULL,
	opis text NOT NULL,
  CONSTRAINT [PK_GATUNEK] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [KolejkaOdtwarzania] (
	ID integer NOT NULL,
	Utwor integer NOT NULL,
  CONSTRAINT [PK_KOLEJKAODTWARZANIA] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Playlista] (
	ID integer NOT NULL,
	nazwa text NOT NULL,
	dataUtworzeniaWBazie datetime NOT NULL,
	opis datetime NOT NULL,
  CONSTRAINT [PK_PLAYLISTA] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Artysta] (
	ID integer NOT NULL,
	Zespol integer,
	Zdjecie integer NOT NULL,
	imie text NOT NULL,
	nazwisko text NOT NULL,
	pseudonim text NOT NULL,
	dataUrodzenia datetime NOT NULL,
	opis text NOT NULL,
  CONSTRAINT [PK_ARTYSTA] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [UtworAlbum] (
	AlbumID integer NOT NULL,
	UtworID integer NOT NULL,
	numerSciezki integer NOT NULL,
  CONSTRAINT [PK_UTWORALBUM] PRIMARY KEY CLUSTERED
  (
  [AlbumID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [UtworArtysta] (
	ArtystaID integer NOT NULL,
	UtworID integer NOT NULL,
  CONSTRAINT [PK_UTWORARTYSTA] PRIMARY KEY CLUSTERED
  (
  [ArtystaID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Zespol] (
	ID integer NOT NULL,
	nazwa binary NOT NULL,
	dataZalozenia datetime NOT NULL,
	dataRozwiazania datetime,
	opis string NOT NULL,
  CONSTRAINT [PK_ZESPOL] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Obraz] (
	ID integer NOT NULL,
	sciezkaDoPliku text NOT NULL,
  CONSTRAINT [PK_OBRAZ] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [UtworPlaylista] (
	PlaylistaID integer NOT NULL,
	UtworID integer NOT NULL,
	kolejnosc integer NOT NULL,
  CONSTRAINT [PK_UTWORPLAYLISTA] PRIMARY KEY CLUSTERED
  (
  [PlaylistaID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
ALTER TABLE [Album] WITH CHECK ADD CONSTRAINT [Album_fk0] FOREIGN KEY ([Okladka]) REFERENCES [Obraz]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Album] CHECK CONSTRAINT [Album_fk0]
GO

ALTER TABLE [Utwor] WITH CHECK ADD CONSTRAINT [Utwor_fk0] FOREIGN KEY ([ID]) REFERENCES [UtworPlaylista]([UtworID])
ON UPDATE CASCADE
GO
ALTER TABLE [Utwor] CHECK CONSTRAINT [Utwor_fk0]
GO
ALTER TABLE [Utwor] WITH CHECK ADD CONSTRAINT [Utwor_fk1] FOREIGN KEY ([Artysta]) REFERENCES [UtworArtysta]([UtworID])
ON UPDATE CASCADE
GO
ALTER TABLE [Utwor] CHECK CONSTRAINT [Utwor_fk1]
GO
ALTER TABLE [Utwor] WITH CHECK ADD CONSTRAINT [Utwor_fk2] FOREIGN KEY ([Album]) REFERENCES [UtworAlbum]([UtworID])
ON UPDATE CASCADE
GO
ALTER TABLE [Utwor] CHECK CONSTRAINT [Utwor_fk2]
GO
ALTER TABLE [Utwor] WITH CHECK ADD CONSTRAINT [Utwor_fk3] FOREIGN KEY ([Grafika]) REFERENCES [Obraz]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Utwor] CHECK CONSTRAINT [Utwor_fk3]
GO
ALTER TABLE [Utwor] WITH CHECK ADD CONSTRAINT [Utwor_fk4] FOREIGN KEY ([Gatunek]) REFERENCES [Gatunek]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Utwor] CHECK CONSTRAINT [Utwor_fk4]
GO


ALTER TABLE [KolejkaOdtwarzania] WITH CHECK ADD CONSTRAINT [KolejkaOdtwarzania_fk0] FOREIGN KEY ([Utwor]) REFERENCES [Utwor]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [KolejkaOdtwarzania] CHECK CONSTRAINT [KolejkaOdtwarzania_fk0]
GO


ALTER TABLE [Artysta] WITH CHECK ADD CONSTRAINT [Artysta_fk0] FOREIGN KEY ([Zespol]) REFERENCES [Zespol]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Artysta] CHECK CONSTRAINT [Artysta_fk0]
GO
ALTER TABLE [Artysta] WITH CHECK ADD CONSTRAINT [Artysta_fk1] FOREIGN KEY ([Zdjecie]) REFERENCES [Obraz]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Artysta] CHECK CONSTRAINT [Artysta_fk1]
GO

ALTER TABLE [UtworAlbum] WITH CHECK ADD CONSTRAINT [UtworAlbum_fk0] FOREIGN KEY ([AlbumID]) REFERENCES [Album]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [UtworAlbum] CHECK CONSTRAINT [UtworAlbum_fk0]
GO

ALTER TABLE [UtworArtysta] WITH CHECK ADD CONSTRAINT [UtworArtysta_fk0] FOREIGN KEY ([ArtystaID]) REFERENCES [Artysta]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [UtworArtysta] CHECK CONSTRAINT [UtworArtysta_fk0]
GO



ALTER TABLE [UtworPlaylista] WITH CHECK ADD CONSTRAINT [UtworPlaylista_fk0] FOREIGN KEY ([PlaylistaID]) REFERENCES [Playlista]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [UtworPlaylista] CHECK CONSTRAINT [UtworPlaylista_fk0]
GO

