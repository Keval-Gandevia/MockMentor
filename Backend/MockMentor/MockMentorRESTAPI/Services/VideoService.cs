using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
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

            var answerQueueRequest = new AnswerQueueRequest()
            {
                questionId = addVideoRequest.questionId,
                videoUrl = addVideoRequest.videoUrl,
                messageType = MessageType.TRANSCRIBE_VIDEO
            };

            var videoConvertQueueRequest = new VideoConvertQueueRequest()
            {
                questionId = addVideoRequest.questionId,
                videoUrl = addVideoRequest.videoUrl,
                messageType = MessageType.CONVERT_VIDEO
            };

            string answerMessageJson = JsonSerializer.Serialize(answerQueueRequest);
            string convertVideoMessageJson = JsonSerializer.Serialize(videoConvertQueueRequest);

            string answerRequestQueueUrl = await _sqsservice.GetQueueUrlAsync(_configuration["AWS:SQS:AnswerRequestQueue"]);
            string videoConversionRequestQueueUrl = await _sqsservice.GetQueueUrlAsync(_configuration["AWS:SQS:VideoConversionRequestQueue"]);

            await _sqsservice.SendMessage(answerRequestQueueUrl, answerMessageJson);
            await _sqsservice.SendMessage(videoConversionRequestQueueUrl, convertVideoMessageJson);

            return new Response() { statusCode = HttpStatusCode.Created, message = "Video added successfully.", payload = res };
        }
    }
}
