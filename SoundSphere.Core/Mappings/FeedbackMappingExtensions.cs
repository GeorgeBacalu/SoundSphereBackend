using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class FeedbackMappingExtensions
    {
        public static IList<FeedbackDto> ToDtos(this IList<Feedback> feedbacks, IMapper mapper)
        {
            IList<FeedbackDto> feedbackDtos = feedbacks.Select(feedback => feedback.ToDto(mapper)).ToList();
            return feedbackDtos;
        }

        public static IList<Feedback> ToEntities(this IList<FeedbackDto> feedbackDtos, IUserRepository userRepository, IMapper mapper)
        {
            IList<Feedback> feedbacks = feedbackDtos.Select(feedbackDto => feedbackDto.ToEntity(userRepository, mapper)).ToList();
            return feedbacks;
        }

        public static FeedbackDto ToDto(this Feedback feedback, IMapper mapper)
        {
            FeedbackDto feedbackDto = mapper.Map<FeedbackDto>(feedback);
            return feedbackDto;
        }

        public static Feedback ToEntity(this FeedbackDto feedbackDto, IUserRepository userRepository, IMapper mapper)
        {
            Feedback feedback = mapper.Map<Feedback>(feedbackDto);
            feedback.User = userRepository.GetById(feedbackDto.UserId);
            return feedback;
        }
    }
}