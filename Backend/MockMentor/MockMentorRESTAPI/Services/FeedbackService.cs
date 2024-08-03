using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System.Diagnostics;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAnswerRepository _answerRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IAnswerRepository answerRepository)
        {
            _feedbackRepository = feedbackRepository;
            _answerRepository = answerRepository;
        }
        public async Task<Response> AddFeedbackAsync(AddFeedbackRequest addFeedbackRequest)
        {
            if(addFeedbackRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding feedback." };
            }

            Feedback feedback = new Feedback() 
            { 
                answerId = addFeedbackRequest.answerId,
                feedbackText = addFeedbackRequest.feedbackText
            };

            var res = await _feedbackRepository.AddFeedbackAsync(feedback);

            return new Response() { statusCode = HttpStatusCode.OK, message = "Feedback added successfully.", payload = res };
        }

        public async Task<Response> GetFeedbackAsync(int questionId)
        {
            var answer = await _answerRepository.GetAnswerByQuestionIdAsync(questionId);

            if(answer == null)
            {
                return new Response() { statusCode = HttpStatusCode.NotFound, message = "Error fetching answer." };
            }

            var feedback = await _feedbackRepository.GetFeedbackByAnswerIdAsync(answer.answerId);

            if(feedback == null)
            {
                return new Response() { statusCode = HttpStatusCode.NotFound, message = "Error fetching feedback." };
            }

            var getFeedbackResponse = new GetFeedbackResponse()
            {
                feedbackId = feedback.feedbackId,
                answerId = answer.answerId,
                feedbackText = feedback.feedbackText
            };

            return new Response() { statusCode = HttpStatusCode.OK, message = "Feedback retrieved successfully.", payload = getFeedbackResponse };
        }
    }
}
