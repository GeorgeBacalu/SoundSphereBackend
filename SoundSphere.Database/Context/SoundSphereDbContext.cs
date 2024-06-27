using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Context
{
    public class SoundSphereDbContext : DbContext
    {
        public SoundSphereDbContext() { }

        public SoundSphereDbContext(DbContextOptions<SoundSphereDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Authority> Authorities { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<SongLink> SongLinks { get; set; }
        public virtual DbSet<ArtistLink> ArtistLinks { get; set; }
        public virtual DbSet<AlbumLink> AlbumLinks { get; set; }
        public virtual DbSet<UserSong> UserSongs { get; set; }
        public virtual DbSet<UserArtist> UserArtists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(user => user.Name).IsUnique();
                entity.HasIndex(user => user.Email).IsUnique();
                entity.Property(user => user.Theme).HasConversion(new EnumToStringConverter<Theme>());
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(role => role.Type).IsUnique();
                entity.Property(role => role.Type).HasConversion(new EnumToStringConverter<RoleType>());
            });
            modelBuilder.Entity<Authority>(entity =>
            {
                entity.HasIndex(authority => authority.Type).IsUnique();
                entity.Property(authority => authority.Type).HasConversion(new EnumToStringConverter<AuthorityType>());
            });
            modelBuilder.Entity<Feedback>(entity => entity.Property(feedback => feedback.Type).HasConversion(new EnumToStringConverter<FeedbackType>()));
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(notification => notification.Type).HasConversion(new EnumToStringConverter<NotificationType>());
                entity.HasOne(notification => notification.Sender)
                    .WithMany()
                    .HasForeignKey(notification => notification.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(notification => notification.Receiver)
                    .WithMany()
                    .HasForeignKey(notification => notification.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Song>(entity => entity.Property(song => song.Genre).HasConversion(new EnumToStringConverter<GenreType>()));
            modelBuilder.Entity<SongLink>(entity =>
            {
                entity.HasKey(song => new { song.SongId, song.SimilarSongId });
                entity.HasOne(song => song.Song)
                    .WithMany(song => song.SimilarSongs)
                    .HasForeignKey(song => song.SongId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(song => song.SimilarSong)
                    .WithMany()
                    .HasForeignKey(song => song.SimilarSongId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ArtistLink>(entity =>
            {
                entity.HasKey(artist => new { artist.ArtistId, artist.SimilarArtistId });
                entity.HasOne(artist => artist.Artist)
                    .WithMany(artist => artist.SimilarArtists)
                    .HasForeignKey(artist => artist.ArtistId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(artist => artist.SimilarArtist)
                    .WithMany()
                    .HasForeignKey(artist => artist.SimilarArtistId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<AlbumLink>(entity =>
            {
                entity.HasKey(album => new { album.AlbumId, album.SimilarAlbumId });
                entity.HasOne(album => album.Album)
                    .WithMany(album => album.SimilarAlbums)
                    .HasForeignKey(album => album.AlbumId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(album => album.SimilarAlbum)
                    .WithMany()
                    .HasForeignKey(album => album.SimilarAlbumId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<UserSong>(entity =>
            {
                entity.HasKey(userSong => new { userSong.UserId, userSong.SongId });
                entity.HasOne(userSong => userSong.User)
                    .WithMany(user => user.UserSongs)
                    .HasForeignKey(userSong => userSong.UserId);
                entity.HasOne(userSong => userSong.Song)
                    .WithMany()
                    .HasForeignKey(userSong => userSong.SongId);
            });
            modelBuilder.Entity<UserArtist>(entity =>
            {
                entity.HasKey(userArtist => new { userArtist.UserId, userArtist.ArtistId });
                entity.HasOne(userArtist => userArtist.User)
                    .WithMany(user => user.UserArtists)
                    .HasForeignKey(userArtist => userArtist.UserId);
                entity.HasOne(userArtist => userArtist.Artist)
                    .WithMany()
                    .HasForeignKey(userArtist => userArtist.ArtistId);
            });
        }
    }
}