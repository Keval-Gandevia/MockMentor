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
        public async Task HandleRekognitionResponse(RekognitionQueueResponse rekognitionQueueResponse, IEmotionService emotionService, IVideoAnalysisService videoAnalysisService)
        {
            var addEmotionRequest = new AddEmotionRequest()
            {
                videoId = rekognitionQueueResponse.videoId,
                emotionValue = rekognitionQueueResponse.emotionValue
            };

            var emotionResponse = await emotionService.AddEmotionAsync(addEmotionRequest);
            var emotion = (Emotion)emotionResponse.payload;

            var getVideoAnalysisResponse = await videoAnalysisService.GetVideoAnalysisByVideoIdAsync(emotion.videoId);
            var videoAnalysis = (VideoAnalysis)getVideoAnalysisResponse.payload;

            if (getVideoAnalysisResponse.statusCode == HttpStatusCode.OK)
            {
                videoAnalysis.isRekognitionCompleted = true;
                await videoAnalysisService.UpdateVideoAnalysisAsync(videoAnalysis);
            }
            else if (getVideoAnalysisResponse.statusCode == HttpStatusCode.NotFound)
            {
                var addVideoAnalysisRequest = new AddVideoAnalysisRequest()
                {
                    videoId = emotion.videoId,
                    isRekognitionCompleted = true,
                };

                var res = await videoAnalysisService.AddVideoAnalysisAsync(addVideoAnalysisRequest);
            }
        }
    }
}
