using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;

        public AuthorityService(IAuthorityRepository authorityRepository) => _authorityRepository = authorityRepository;

        public IList<Authority> FindAll() => _authorityRepository.FindAll();

        public Authority FindById(Guid id) => _authorityRepository.FindById(id);

        public Authority Save(Authority authority)
        {
            if (authority == null) throw new Exception("Can't persist null authority to DB!");
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            return _authorityRepository.Save(authority);
        }
    }
}