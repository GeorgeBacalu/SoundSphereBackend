using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IMapper _mapper;

        public AuthorityService(IAuthorityRepository authorityRepository, IMapper mapper) => (_authorityRepository, _mapper) = (authorityRepository, mapper);

        public IList<AuthorityDto> FindAll() => _authorityRepository.FindAll().ToDtos(_mapper);

        public AuthorityDto FindById(Guid id) => _authorityRepository.FindById(id).ToDto(_mapper);

        public AuthorityDto Save(AuthorityDto authorityDto)
        {
            Authority authority = authorityDto.ToEntity(_mapper);
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            return _authorityRepository.Save(authority).ToDto(_mapper);
        }
    }
}