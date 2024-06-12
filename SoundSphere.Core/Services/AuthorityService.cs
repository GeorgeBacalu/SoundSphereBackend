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

        public IList<AuthorityDto> GetAll()
        {
            IList<AuthorityDto> authorityDtos = _authorityRepository.GetAll().ToDtos(_mapper);
            return authorityDtos;
        }

        public AuthorityDto GetById(Guid id)
        {
            AuthorityDto authorityDto = _authorityRepository.GetById(id).ToDto(_mapper);
            return authorityDto;
        }

        public AuthorityDto Add(AuthorityDto authorityDto)
        {
            Authority authorityToCreate = authorityDto.ToEntity(_mapper);
            AuthorityDto createdAuthorityDto = _authorityRepository.Add(authorityToCreate).ToDto(_mapper);
            return createdAuthorityDto;
        }
    }
}