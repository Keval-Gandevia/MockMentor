using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Services;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockMentorConsumer.Domain.Workflows
{
    public interface IFeedbackWorkflow
    {
        Task HandleFeedbackResponse(FeedbackQueueResponse feedbackQueueResponse, IFeedbackService feedbackService, IAnswerService answerService, IQuestionService questionService, IVideoService videoService);
    }
}
