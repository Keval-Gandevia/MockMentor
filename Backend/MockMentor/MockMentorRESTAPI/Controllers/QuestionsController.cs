using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using System.Net;

namespace MockMentorRESTAPI.Controllers
{
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    public class QuestionsController : ControllerBase
    {

        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost(APIRoutes.ADD_QUESTION)]
        public async Task<Response> AddQuestion([FromBody] Question question)
        {
            return await _questionService.AddQuestionAsync(question);
        }

        [HttpGet(APIRoutes.HEALTH_CHECK)]
        public async Task<Response> HealthCheck()
        {
            return new Response()
            {
                statusCode = HttpStatusCode.OK,
                message = "Health check up is done."
            };
        }
    }
}
