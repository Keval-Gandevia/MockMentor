using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Response> AddQuestionAsync(Question question)
        {
            if (question == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding question" };
            }

            var res = await _questionRepository.AddQuestionAsync(question);

            return new Response() { statusCode = HttpStatusCode.Created, message = "Question added successfully", payload = res};
        }

        public async Task<Response> GetQuestionByIdAsync(int questionId)
        {
            var question = await _questionRepository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error fetching question." };
            }
            return new Response() { statusCode = HttpStatusCode.OK, message = "Question retrieved successfully", payload = question };
        }
    }
}
