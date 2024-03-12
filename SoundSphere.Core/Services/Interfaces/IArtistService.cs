﻿using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<Artist> FindAll();

        Artist FindById(Guid id);

        Artist Save(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DisableById(Guid id);
    }
}