﻿using AutoMapper;
using FluentAssertions;
using SoundSphere.Core.Services;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Integration.Services
{
    public class AuthorityServiceIntegrationTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly IMapper _mapper;

        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = AuthorityMock.GetMockedAuthorityDto1();
        private readonly IList<AuthorityDto> _authorityDtos = AuthorityMock.GetMockedAuthorityDtos();

        public AuthorityServiceIntegrationTest(DbFixture fixture)
        {
            _fixture = fixture;
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Authority, AuthorityDto>();
                config.CreateMap<AuthorityDto, Authority>();
            }).CreateMapper();
        }

        private void Execute(Action<AuthorityService, SoundSphereContext> action)
        {
            using var context = _fixture.CreateContext();
            var authorityService = new AuthorityService(new AuthorityRepository(context), _mapper);
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(_authorities);
            context.SaveChanges();
            action(authorityService, context);
        }

        [Fact] public void FindAll_Test() => Execute((authorityService, context) => authorityService.FindAll().Should().BeEquivalentTo(_authorityDtos));

        [Fact] public void FindById_Test() => Execute((authorityService, context) => authorityService.FindById(Constants.ValidAuthorityGuid).Should().BeEquivalentTo(_authorityDto1));

        [Fact] public void Save_Test() => Execute((authorityService, context) =>
        {
            AuthorityDto newAuthorityDto = AuthorityMock.GetMockedAuthorityDto1();
            authorityService.Invoking(service => service.Save(newAuthorityDto)).Should().Throw<InvalidOperationException>();
        });
    }
}