using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IMapper _mapper;

        public AuthorityService(IAuthorityRepository authorityRepository, IMapper mapper) => (_authorityRepository, _mapper) = (authorityRepository, mapper);

        public IList<AuthorityDto> GetAll() => _authorityRepository.GetAll().ToDtos(_mapper);

        public AuthorityDto GetById(Guid id) => _authorityRepository.GetById(id).ToDto(_mapper);

        public AuthorityDto Add(AuthorityDto authorityDto) => _authorityRepository.Add(authorityDto.ToEntity(_mapper)).ToDto(_mapper);
    }
}