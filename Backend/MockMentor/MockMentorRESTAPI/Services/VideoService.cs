using MockMentorRESTAPI.Domain.DTOs;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace MockMentorRESTAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ISQSService _sqsservice;
        private readonly IConfiguration _configuration;
        private readonly string QUEUE_NAME = "answer-request-queue";

        public VideoService(IVideoRepository videoRepository, ISQSService sqsService, IConfiguration configuration)
        {
            _videoRepository = videoRepository;
            _sqsservice = sqsService;
            _configuration = configuration;
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

            Debug.WriteLine(videoJson);

            string answerRequestQueueUrl = await _sqsservice.GetQueueUrlAsync(_configuration["AWS:SQS:AnswerRequestQueue"]);
            string videoConversionRequestQueueUrl = await _sqsservice.GetQueueUrlAsync(_configuration["AWS:SQS:VideoConversionRequestQueue"]);

            await _sqsservice.SendMessage(answerRequestQueueUrl, videoJson);
            await _sqsservice.SendMessage(videoConversionRequestQueueUrl, videoJson);

            return new Response() { statusCode = HttpStatusCode.Created, message = "Video added successfully.", payload = res };

        }

    }
}
