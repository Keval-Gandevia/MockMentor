using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using System.Diagnostics;
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
            Debug.WriteLine("Add request is received");
            return await _questionService.AddQuestionAsync(question);
        }
    }
}
