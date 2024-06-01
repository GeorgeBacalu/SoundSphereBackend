using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Newtonsoft.Json.JsonConvert;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.FeedbackMock;
using static SoundSphere.Tests.Mocks.UserMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class FeedbackControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Feedback _feedback1 = GetMockedFeedback1();
        private readonly Feedback _feedback2 = GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = GetMockedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = GetMockedFeedbacksPaginationRequest();
        private readonly IList<User> _users = GetMockedUsers();

        public FeedbackControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Users.AddRangeAsync(_users);
            await context.Feedbacks.AddRangeAsync(_feedbacks);
            await context.SaveChangesAsync();
            await action();
            context.Feedbacks.RemoveRange(context.Feedbacks);
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact] public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(ApiFeedback);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_feedbackDtos);
        });

        [Fact] public async Task GetAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiFeedback}/pagination", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedFeedbackDtos);
        });

        [Fact] public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiFeedback}/{ValidFeedbackGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<FeedbackDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_feedbackDto1);
        });

        [Fact] public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiFeedback}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(FeedbackNotFound, InvalidGuid) });
        });

        [Fact] public async Task Add_Test() => await Execute(async () =>
        {
            FeedbackDto newFeedbackDto = GetMockedFeedbackDto37();
            var saveResponse = await _httpClient.PostAsync(ApiFeedback, new StringContent(SerializeObject(newFeedbackDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<FeedbackDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().BeEquivalentTo(newFeedbackDto, options => options.Excluding(feedback => feedback.SentAt));

            var getAllResponse = await _httpClient.GetAsync(ApiFeedback);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<FeedbackDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newFeedbackDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = ToDto(updatedFeedback);
            var updateResponse = await _httpClient.PutAsync($"{ApiFeedback}/{ValidFeedbackGuid}", new StringContent(SerializeObject(updatedFeedbackDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<FeedbackDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedFeedbackDto);

            var getResponse = await _httpClient.GetAsync($"{ApiFeedback}/{ValidFeedbackGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<FeedbackDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedFeedbackDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiFeedback}/{InvalidGuid}", new StringContent(SerializeObject(_feedbackDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(FeedbackNotFound, InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiFeedback}/{ValidFeedbackGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(NoContent);

            var getResponse = await _httpClient.GetAsync($"{ApiFeedback}/{ValidFeedbackGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(NotFound);
            var getResponseBody = DeserializeObject<ProblemDetails>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(FeedbackNotFound, ValidFeedbackGuid) });
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiFeedback}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(FeedbackNotFound, InvalidGuid) });
        });

        private FeedbackDto ToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            SentAt = feedback.SentAt
        };
    }
}