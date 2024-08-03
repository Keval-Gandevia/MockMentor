using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Persistence.Repositories;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly IEmotionRepository _emotionRepository;

        public EmotionService(IEmotionRepository emotionRepository)
        {
            _emotionRepository = emotionRepository;
        }
        public async Task<Response> AddEmotionAsync(AddEmotionRequest addEmotionRequest)
        {
            if(addEmotionRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding emotion" };
            }

            Emotion emotion = new Emotion()
            {
                videoId = addEmotionRequest.videoId,
                emotionValue = addEmotionRequest.emotionValue
            };

            var res = await _emotionRepository.AddEmotionAsync(emotion);

            return new Response() { statusCode = HttpStatusCode.OK, message = "Emotion added successfully.", payload = res };
        }

        public async Task<Response> GetEmotionByVideoIdAsync(int videoId)
        {
            var emotion = await _emotionRepository.GetEmotionByVideoIdAsync(videoId);

            if (emotion == null)
            {
                return new Response() { statusCode = HttpStatusCode.NotFound, message = "Emotion with given videoId does not exist." };
            }

            var getEmotionResponse = new GetEmotionResponse()
            {
                emotionId = emotion.emotionId,
                emotionValue = emotion.emotionValue
            };

            return new Response() { statusCode = HttpStatusCode.OK, message = "Emotion retrieved successfully.", payload = getEmotionResponse };

        }
    }
}
