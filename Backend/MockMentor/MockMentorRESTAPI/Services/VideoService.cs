using MockMentorRESTAPI.Domain.DTOs;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using System.Net;
using System.Text.Json;

namespace MockMentorRESTAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ISQSService _sqsservice;
        private readonly string QUEUE_NAME = "answer-request-queue";

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

            string videoJson = JsonSerializer.Serialize(video);

            string queueUrl = await _sqsservice.GetQueueUrlAsync(QUEUE_NAME);

            await _sqsservice.SendMessage(queueUrl, videoJson);

            return new Response() { statusCode = HttpStatusCode.Created, message = "Video added successfully.", payload = res };

        }
    }
}
