using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository) 
        { 
            _answerRepository = answerRepository; 
        }
        public async Task<Response> AddAnswerAsync(AddAnswerRequest answerRequest)
        {
            if(answerRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding answer." };
            }

            Answer answer = new Answer() { questionId = answerRequest.questionId, answerText = answerRequest.answerText };

            var res = await _answerRepository.AddAnswerAsync(answer);

            return new Response() { statusCode = HttpStatusCode.OK, message = "Answer added successfully.", payload = answer };
            
        }
    }
}
