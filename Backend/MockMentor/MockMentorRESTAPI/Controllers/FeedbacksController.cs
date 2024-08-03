using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Controllers
{
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet(APIRoutes.GET_FEEDBACK)]
        public async Task<Response> GetFeedback(int questionId)
        {
            return await _feedbackService.GetFeedbackAsync(questionId);
        }
    }
}
