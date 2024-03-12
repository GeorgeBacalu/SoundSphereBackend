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

        public Authority Save(Authority authority) => _authorityRepository.Save(authority);
    }
}