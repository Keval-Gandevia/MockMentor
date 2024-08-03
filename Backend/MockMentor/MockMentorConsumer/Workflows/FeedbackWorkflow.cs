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
        public async Task HandleFeedbackResponse(FeedbackQueueResponse feedbackQueueResponse, IFeedbackService feedbackService, IAnswerService answerService, IQuestionService questionService, IVideoService videoService, IVideoAnalysisService videoAnalysisService)
        {
            var addFeedbackRequest = new AddFeedbackRequest()
            {
                answerId = feedbackQueueResponse.answerId,
                feedbackText = feedbackQueueResponse.feedbackText
            };

            var feedbackResponse = await feedbackService.AddFeedbackAsync(addFeedbackRequest);
            var feedback = (Feedback)feedbackResponse.payload;

            var answerResponse = await answerService.GetAnswerByIdAsync(feedbackQueueResponse.answerId);
            var answer = (Answer)answerResponse.payload;

            var questionResponse = await questionService.GetQuestionByIdAsync(answer.questionId);
            var question = (Question)questionResponse.payload;

            var videoResponse = await videoService.GetVideoByQuestionIdAsync(question.questionId);
            var video = (Video)videoResponse.payload;

            var getVideoAnalysisResponse = await videoAnalysisService.GetVideoAnalysisByVideoIdAsync(video.videoId);
            var videoAnalysis = (VideoAnalysis)getVideoAnalysisResponse.payload;

            

            if(getVideoAnalysisResponse.statusCode == HttpStatusCode.OK)
            {
                videoAnalysis.isFeedbackCompleted = true;
                await videoAnalysisService.UpdateVideoAnalysisAsync(videoAnalysis);
            }
            else if (getVideoAnalysisResponse.statusCode == HttpStatusCode.NotFound)
            {
                var addVideoAnalysisRequest = new AddVideoAnalysisRequest()
                {
                    videoId = video.videoId,
                    isFeedbackCompleted = true,
                };

                var res = await videoAnalysisService.AddVideoAnalysisAsync(addVideoAnalysisRequest);
            }
        }
    }
}
