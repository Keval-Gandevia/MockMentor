using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MockMentorConsumer.Domain.Workflows;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Services;
using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MockMentorConsumer.Workflows
{
    internal class RekognitionWorkflow : IRekognitionWorkflow
    {
        public async Task HandleRekognitionResponse(RekognitionQueueResponse rekognitionQueueResponse, IEmotionService emotionService)
        {
            var addEmotionRequest = new AddEmotionRequest()
            {
                videoId = rekognitionQueueResponse.videoId,
                emotionValue = rekognitionQueueResponse.emotionValue
            };

            await emotionService.AddEmotionAsync(addEmotionRequest);           
        }
    }
}
