using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Controllers
{
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    public class EmotionsController : ControllerBase
    {
        private readonly IEmotionService _emotionService;

        public EmotionsController(IEmotionService emotionService)
        {
            _emotionService = emotionService;
        }

        [HttpGet(APIRoutes.GET_EMOTION)]
        public async Task<Response> GetEmotion(int videoId)
        {
            return await _emotionService.GetEmotionByVideoIdAsync(videoId);
        }
    }
}
