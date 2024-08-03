using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Persistence.Repositories;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class VideoAnalysisService : IVideoAnalysisService
    {
        private readonly IVideoAnalysisRepository _videoAnalysisRepository;

        public VideoAnalysisService(IVideoAnalysisRepository videoAnalysisRepository)
        {
            _videoAnalysisRepository = videoAnalysisRepository;
        }
        public async Task<Response> AddVideoAnalysisAsync(AddVideoAnalysisRequest addVideoAnalysisRequest)
        {
            if (addVideoAnalysisRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding video analysis." };
            }
            
            VideoAnalysis videoAnalysis = new VideoAnalysis()
            {
                videoId = addVideoAnalysisRequest.videoId,
                isFeedbackCompleted = addVideoAnalysisRequest.isFeedbackCompleted,
                isRekognitionCompleted = addVideoAnalysisRequest.isRekognitionCompleted
            };

            var res = await _videoAnalysisRepository.AddVideoAnalysisAsync(videoAnalysis);

            return new Response() { statusCode = HttpStatusCode.OK, message = "Feedback added successfully.", payload = res };
        }

        public async Task<Response> GetVideoAnalysisByVideoIdAsync(int videoId)
        {
            var videoAnalysis = _videoAnalysisRepository.GetVideoAnalysisByVideoIdAsync(videoId);

            if (videoAnalysis == null)
            {
                return new Response() { statusCode = HttpStatusCode.NotFound, message = "VideoAnalysis does not exist" };
            }

            return new Response() { statusCode = HttpStatusCode.OK, message = "Video retrieved successfully", payload = videoAnalysis };

        }

        public async Task<Response> UpdateVideoAnalysisAsync(VideoAnalysis videoAnalysis)
        {
            var res = await _videoAnalysisRepository.UpdateVideoAnalysisAsync(videoAnalysis);

            if(res == true)
            {
                return new Response() { statusCode = HttpStatusCode.OK, message = "Video Analysis updated successfully", payload = videoAnalysis };
            }
            else
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error updating video analysis" };
            }
        }
    }
}
