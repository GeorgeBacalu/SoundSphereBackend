--CREATE TABLE [dbo].[__EFMigrationsHistory](
--    [MigrationId] [nvarchar](150) NOT NULL,
--    [ProductVersion] [nvarchar](32) NOT NULL,
--    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
--) ON [PRIMARY];

CREATE TABLE [dbo].[Roles](
    [Id] [uniqueidentifier] NOT NULL,
    [Type] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[Authorities](
    [Id] [uniqueidentifier] NOT NULL,
    [Type] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Authorities] PRIMARY KEY ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[Albums](
    [Id] [uniqueidentifier] NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [ImageUrl] [nvarchar](max) NOT NULL,
    [ReleaseDate] [date] NOT NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Albums] PRIMARY KEY ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[AlbumLinks](
    [AlbumId] [uniqueidentifier] NOT NULL,
    [SimilarAlbumId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_AlbumLinks] PRIMARY KEY ([AlbumId], [SimilarAlbumId]),
    CONSTRAINT [FK_AlbumLinks_Albums_AlbumId] FOREIGN KEY([AlbumId]) REFERENCES [dbo].[Albums] ([Id]),
    CONSTRAINT [FK_AlbumLinks_Albums_SimilarAlbumId] FOREIGN KEY([SimilarAlbumId]) REFERENCES [dbo].[Albums] ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[Artists](
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [ImageUrl] [nvarchar](max) NOT NULL,
    [Bio] [nvarchar](max) NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Artists] PRIMARY KEY ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[ArtistLinks](
    [ArtistId] [uniqueidentifier] NOT NULL,
    [SimilarArtistId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_ArtistLinks] PRIMARY KEY ([ArtistId], [SimilarArtistId]),
    CONSTRAINT [FK_ArtistLinks_Artists_ArtistId] FOREIGN KEY([ArtistId]) REFERENCES [dbo].[Artists] ([Id]),
    CONSTRAINT [FK_ArtistLinks_Artists_SimilarArtistId] FOREIGN KEY([SimilarArtistId]) REFERENCES [dbo].[Artists] ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[Songs](
    [Id] [uniqueidentifier] NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [ImageUrl] [nvarchar](max) NOT NULL,
    [Genre] [nvarchar](max) NOT NULL,
    [ReleaseDate] [date] NOT NULL,
    [DurationSeconds] [int] NOT NULL,
    [AlbumId] [uniqueidentifier] NOT NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Songs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Songs_Albums_AlbumId] FOREIGN KEY([AlbumId]) REFERENCES [dbo].[Albums] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[SongLinks](
    [SongId] [uniqueidentifier] NOT NULL,
    [SimilarSongId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_SongLinks] PRIMARY KEY ([SongId], [SimilarSongId]),
    CONSTRAINT [FK_SongLinks_Songs_SongId] FOREIGN KEY([SongId]) REFERENCES [dbo].[Songs] ([Id]),
    CONSTRAINT [FK_SongLinks_Songs_SimilarSongId] FOREIGN KEY([SimilarSongId]) REFERENCES [dbo].[Songs] ([Id])
) ON [PRIMARY];

CREATE TABLE [dbo].[ArtistSong](
    [ArtistsId] [uniqueidentifier] NOT NULL,
    [SongsId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_ArtistSong] PRIMARY KEY ([ArtistsId], [SongsId]),
    CONSTRAINT [FK_ArtistSong_Artists_ArtistsId] FOREIGN KEY([ArtistsId]) REFERENCES [dbo].[Artists] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ArtistSong_Songs_SongsId] FOREIGN KEY([SongsId]) REFERENCES [dbo].[Songs] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[Users](
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](450) NOT NULL UNIQUE,
	[Email] [nvarchar](450) NOT NULL UNIQUE,
    [PasswordHash] [nvarchar](max) NOT NULL,
    [PasswordSalt] [nvarchar](max) NOT NULL,
    [Mobile] [nvarchar](max) NOT NULL,
    [Address] [nvarchar](max) NOT NULL,
    [Birthday] [date] NOT NULL,
    [Avatar] [nvarchar](max) NOT NULL,
	[EmailNotifications] BIT NOT NULL,
	[Theme] [nvarchar](max) NOT NULL,
    [RoleId] [uniqueidentifier] NOT NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[UserArtists](
    [UserId] [uniqueidentifier] NOT NULL,
    [ArtistId] [uniqueidentifier] NOT NULL,
    [IsFollowing] [bit] NOT NULL,
    CONSTRAINT [PK_UserArtists] PRIMARY KEY ([UserId], [ArtistId]),
    CONSTRAINT [FK_UserArtists_Artists_ArtistId] FOREIGN KEY([ArtistId]) REFERENCES [dbo].[Artists] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserArtists_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[UserSongs](
    [UserId] [uniqueidentifier] NOT NULL,
    [SongId] [uniqueidentifier] NOT NULL,
    [PlayCount] [int] NOT NULL,
    CONSTRAINT [PK_UserSongs] PRIMARY KEY ([UserId], [SongId]),
    CONSTRAINT [FK_UserSongs_Songs_SongId] FOREIGN KEY([SongId]) REFERENCES [dbo].[Songs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSongs_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[AuthorityUser](
    [AuthoritiesId] [uniqueidentifier] NOT NULL,
    [UsersId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_AuthorityUser] PRIMARY KEY ([AuthoritiesId], [UsersId]),
    CONSTRAINT [FK_AuthorityUser_Authorities_AuthoritiesId] FOREIGN KEY([AuthoritiesId]) REFERENCES [dbo].[Authorities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AuthorityUser_Users_UsersId] FOREIGN KEY([UsersId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[Feedbacks](
    [Id] [uniqueidentifier] NOT NULL,
    [UserId] [uniqueidentifier] NOT NULL,
    [Type] [nvarchar](max) NOT NULL,
    [Message] [nvarchar](max) NOT NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Feedbacks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Feedbacks_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[Notifications](
    [Id] [uniqueidentifier] NOT NULL,
    [SenderId] [uniqueidentifier] NOT NULL,
	[ReceiverId] [uniqueidentifier] NOT NULL,
    [Type] [nvarchar](max) NOT NULL,
    [Message] [nvarchar](max) NOT NULL,
    [IsRead] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_Users_SenderId] FOREIGN KEY([SenderId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION,
	CONSTRAINT [FK_Notifications_Users_ReceiverId] FOREIGN KEY([ReceiverId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION
) ON [PRIMARY];

CREATE TABLE [dbo].[Playlists](
    [Id] [uniqueidentifier] NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [UserId] [uniqueidentifier] NOT NULL,
    [CreatedAt] [datetime2](7) NULL,
    [UpdatedAt] [datetime2](7) NULL,
    [DeletedAt] [datetime2](7) NULL,
    CONSTRAINT [PK_Playlists] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Playlists_Users_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];

CREATE TABLE [dbo].[PlaylistSong](
    [PlaylistsId] [uniqueidentifier] NOT NULL,
    [SongsId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_PlaylistSong] PRIMARY KEY ([PlaylistsId], [SongsId]),
    CONSTRAINT [FK_PlaylistSong_Playlists_PlaylistsId] FOREIGN KEY([PlaylistsId]) REFERENCES [dbo].[Playlists] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlaylistSong_Songs_SongsId] FOREIGN KEY([SongsId]) REFERENCES [dbo].[Songs] ([Id]) ON DELETE CASCADE
) ON [PRIMARY];