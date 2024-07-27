using MockMentorRESTAPI.Domain.DTOs;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using System.Net;

namespace MockMentorRESTAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }
        public async Task<Response> AddVideoAsync(AddVideoRequest addVideoRequest)
        {
            if(addVideoRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding video" };
            }

            Video video = new Video() { questionId = addVideoRequest.questionId, videoUrl = addVideoRequest.videoUrl};

            var res = await _videoRepository.AddVideosAsync(video);

            return new Response() { statusCode = HttpStatusCode.Created, message = "Video added successfully.", payload = res };

        }
    }
}
