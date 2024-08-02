using MockMentorConsumer.Domain.Workflows;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using MockMentorRESTAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using MockMentorRESTAPI.Services;

namespace MockMentorConsumer.Workflows
{
    public class AnswerWorkflow : IAnswerWorkflow
    {
        private readonly IConfiguration _configuration;

        public AnswerWorkflow(IConfiguration configuration)
        {
            _configuration = configuration;
        }
           
        public async Task HandleAnswerResponse(AnswerQueueResponse answerQueueResponse, IAnswerService answerService, ISQSService sQSService, IQuestionService questionService)
        {
            var addAnswerRequest = new AddAnswerRequest()
            {
                questionId = answerQueueResponse.questionId,
                answerText = answerQueueResponse.answerText
            };
            await answerService.AddAnswerAsync(addAnswerRequest);

            var questionResponse = await questionService.GetQuestionByIdAsync(answerQueueResponse.questionId);

            var question = (Question)questionResponse.payload;

            var feedbackQueueRequest = new FeedbackQueueRequest()
            {
                questionText = question.questionText,
                answerText = answerQueueResponse.answerText,
                messageType = MessageType.GET_FEEDBACK
            };

            string feedbackMessageJson = JsonSerializer.Serialize(feedbackQueueRequest);

            string feedbackRequestQueueUrl = await sQSService.GetQueueUrlAsync(_configuration["AWS:SQS:FeedbackRequestQueue"]);

            await sQSService.SendMessage(feedbackRequestQueueUrl, feedbackMessageJson);
        }
    }
}
