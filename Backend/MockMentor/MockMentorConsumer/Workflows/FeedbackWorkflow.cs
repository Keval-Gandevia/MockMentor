using MockMentorConsumer.Domain.Workflows;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Services;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockMentorConsumer.Workflows
{
    public class FeedbackWorkflow : IFeedbackWorkflow
    {
        public async Task HandleFeedbackResponse(FeedbackQueueResponse feedbackQueueResponse, IFeedbackService feedbackService, IAnswerService answerService, IQuestionService questionService, IVideoService videoService)
        {
            var addFeedbackRequest = new AddFeedbackRequest()
            {
                answerId = feedbackQueueResponse.answerId,
                feedbackText = feedbackQueueResponse.feedbackText
            };

            await feedbackService.AddFeedbackAsync(addFeedbackRequest);
        }
    }
}
