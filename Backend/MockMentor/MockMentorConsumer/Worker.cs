using MockMentorConsumer.Domain.Workflows;
using MockMentorConsumer.Workflows;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.ResponseModels;
using Newtonsoft.Json;

namespace MockMentorConsumer
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ISQSService _sqsService = scope.ServiceProvider.GetRequiredService<ISQSService>();

                List<string> queueUrls = new List<string>()
                {
                    await _sqsService.GetQueueUrlAsync(_configuration["AWS:SQS:AnswerResponseQueue"]),
                    await _sqsService.GetQueueUrlAsync(_configuration["AWS:SQS:VideoConversionResponseQueue"]),
                    await _sqsService.GetQueueUrlAsync(_configuration["AWS:SQS:FeedbackResponseQueue"]),
                    await _sqsService.GetQueueUrlAsync(_configuration["AWS:SQS:EmotionResponseQueue"])
                };

                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach(string queueUrl in queueUrls)
                    {
                        try
                        {
                            var messages = await _sqsService.ReceiveMessageAsync(queueUrl);

                            if (messages.Any())
                            {
                                _logger.LogInformation("Response received.");
                                foreach (var message in messages)
                                {
                                    dynamic body = JsonConvert.DeserializeObject(message.Body);

                                    if (body != null)
                                    {
                                        if (body.messageType == MessageType.TRANSCRIBE_VIDEO)
                                        {
                                            _logger.LogInformation("Transcrive video response received.");
                                            IAnswerService answerService = scope.ServiceProvider.GetRequiredService<IAnswerService>();
                                            ISQSService sQSService = scope.ServiceProvider.GetRequiredService<ISQSService>();
                                            IQuestionService questionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                                            IAnswerWorkflow answerWorkflow = scope.ServiceProvider.GetRequiredService<IAnswerWorkflow>();
                                            answerWorkflow.HandleAnswerResponse(JsonConvert.DeserializeObject<AnswerQueueResponse>(message.Body), answerService, sQSService, questionService);

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }
                    await Task.Delay(500, stoppingToken);
                }
            }
        }
    }
}
