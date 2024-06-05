using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Integration.Services
{
    public class AuthorityServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly IList<Authority> _authorities = GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = GetMockedAuthorityDtos();

        public AuthorityServiceIntegrationTest(DbFixture fixture) => (_fixture, _mapper) = (fixture, new MapperConfiguration(config => { config.CreateMap<Authority, AuthorityDto>(); config.CreateMap<AuthorityDto, Authority>(); }).CreateMapper());

        private void Execute(Action<AuthorityService, SoundSphereDbContext> action)
        {
            using var context = _fixture.CreateContext();
            var authorityService = new AuthorityService(new AuthorityRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_authorities);
            context.SaveChanges();
            action(authorityService, context);
            transaction.Rollback();
        }

        [Fact] public void GetAll_Test() => Execute((authorityService, context) => authorityService.GetAll().Should().BeEquivalentTo(_authorityDtos));

        [Fact] public void GetById_Test() => Execute((authorityService, context) => authorityService.GetById(ValidAuthorityGuid).Should().BeEquivalentTo(_authorityDto1));

        [Fact] public void Add_Test() => Execute((authorityService, context) => authorityService
            .Invoking(service => service.Add(_authorityDto1))
            .Should().Throw<InvalidOperationException>());
    }
}