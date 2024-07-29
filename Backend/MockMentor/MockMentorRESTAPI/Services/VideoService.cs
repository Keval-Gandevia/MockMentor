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
        private readonly ISQSService _sqsservice;
        private readonly string _queueUrl = "https://sqs.us-east-1.amazonaws.com/103677046658/answer-request-queue";

        public VideoService(IVideoRepository videoRepository, ISQSService sqsService)
        {
            _videoRepository = videoRepository;
            _sqsservice = sqsService;
        }
        public async Task<Response> AddVideoAsync(AddVideoRequest addVideoRequest)
        {
            if(addVideoRequest == null)
            {
                return new Response() { statusCode = HttpStatusCode.BadRequest, message = "Error adding video" };
            }

            Video video = new Video() { questionId = addVideoRequest.questionId, videoUrl = addVideoRequest.videoUrl};

            var res = await _videoRepository.AddVideosAsync(video);

            await _sqsservice.SendMessage(_queueUrl, "Hello! first message to queue");

            return new Response() { statusCode = HttpStatusCode.Created, message = "Video added successfully.", payload = res };

        }
    }
}
