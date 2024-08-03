using MockMentorConsumer.Domain.Workflows;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using MockMentorRESTAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockMentorRESTAPI.Services;
using System.Text.Json;
using MockMentorRESTAPI.Domain.Services;

namespace MockMentorConsumer.Workflows
{
    public class VideoConversionWorkflow : IVideoConversionWorkflow
    {
        private readonly IConfiguration _configuration;

        public VideoConversionWorkflow(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task HandleVideoConversionResponse(VideoConversionQueueResponse videoConversionQueueResponse, ISQSService sQSService)
        {

            var RekognitionQueueRequest = new RekognitionQueueRequest()
            {
                questionId = videoConversionQueueResponse.questionId,
                videoUrl = videoConversionQueueResponse.videoUrl,
                messageType = MessageType.GET_EMOTION
            };

            string rekognitionMessageJson = JsonSerializer.Serialize(RekognitionQueueRequest);

            string rekognitionRequestQueueUrl = await sQSService.GetQueueUrlAsync(_configuration["AWS:SQS:EmotionRequestQueue"]);

            await sQSService.SendMessage(rekognitionRequestQueueUrl, rekognitionMessageJson);

        }
    }
}
