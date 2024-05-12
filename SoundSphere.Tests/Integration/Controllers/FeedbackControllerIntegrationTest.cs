using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class FeedbackControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();
        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();
        private readonly IList<FeedbackDto> _paginatedFeedbackDtos = FeedbackMock.GetMockedPaginatedFeedbackDtos();
        private readonly FeedbackPaginationRequest _paginationRequest = FeedbackMock.GetMockedPaginationRequest();
        private readonly IList<User> _users = UserMock.GetMockedUsers();

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

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiFeedback);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_feedbackDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiFeedback}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedFeedbackDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<FeedbackDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_feedbackDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            FeedbackDto newFeedbackDto = FeedbackMock.GetMockedFeedbackDto37();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiFeedback, new StringContent(JsonConvert.SerializeObject(newFeedbackDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<FeedbackDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().BeEquivalentTo(newFeedbackDto, options => options.Excluding(feedback => feedback.SentAt));

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiFeedback);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<FeedbackDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newFeedbackDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = ToDto(updatedFeedback);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}", new StringContent(JsonConvert.SerializeObject(updatedFeedbackDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<FeedbackDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().Be(updatedFeedbackDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<FeedbackDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(updatedFeedbackDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_feedbackDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var getResponseBody = JsonConvert.DeserializeObject<ProblemDetails>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.FeedbackNotFound, Constants.ValidFeedbackGuid) });
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.FeedbackNotFound, Constants.InvalidGuid) });
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