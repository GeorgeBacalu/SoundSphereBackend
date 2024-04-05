using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly SoundSphereContext _context;

        public ArtistRepository(SoundSphereContext context) => _context = context;

        public IList<Artist> FindAll() => _context.Artists
            .Include(artist => artist.SimilarArtists)
            .ToList();

        public IList<Artist> FindAllActive() => _context.Artists
            .Where(artist => artist.IsActive)
            .Include(artist => artist.SimilarArtists)
            .ToList();

        public Artist FindById(Guid id) => _context.Artists
            .Include(artist => artist.SimilarArtists)
            .FirstOrDefault(artist => artist.Id == id)
            ?? throw new ResourceNotFoundException($"Artist with id {id} not found!");

        public Artist Save(Artist artist)
        {
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return artist;
        }

        public Artist UpdateById(Artist artist, Guid id)
        {
            Artist artistToUpdate = FindById(id);
            artistToUpdate.Name = artist.Name;
            artistToUpdate.ImageUrl = artist.ImageUrl;
            artistToUpdate.Bio = artist.Bio;
            artistToUpdate.SimilarArtists.Clear();
            artist.SimilarArtists.ToList().ForEach(artistToUpdate.SimilarArtists.Add);
            _context.SaveChanges();
            return artistToUpdate;
        }

        public Artist DisableById(Guid id)
        {
            Artist artistToDisable = FindById(id);
            artistToDisable.IsActive = false;
            _context.SaveChanges();
            return artistToDisable;
        }

        public void AddArtistLink(Artist artist) =>
            artist.SimilarArtists = artist.SimilarArtists
                            .Select(similarArtist => _context.Artists.Find(similarArtist.SimilarArtistId))
                            .Where(similarArtist => similarArtist != null)
                            .Select(similarArtist => new ArtistLink
                            {
                                Artist = artist,
                                SimilarArtist = similarArtist
                            })
                            .ToList();

        public void AddUserArtist(Artist artist) =>
            _context.AddRange(_context.Users
                            .Select(user => new UserArtist
                            {
                                User = user,
                                Artist = artist,
                                IsFollowing = false
                            })
                            .ToList());
    }
}