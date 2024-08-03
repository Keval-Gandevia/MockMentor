using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using MockMentorConsumer;
using MockMentorConsumer.Domain.Workflows;
using MockMentorConsumer.Workflows;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Persistence.Contexts;
using MockMentorRESTAPI.Persistence.Repositories;
using MockMentorRESTAPI.Services;

var builder = Host.CreateDefaultBuilder(args);

IHost host = builder.ConfigureServices((hostContext, services) =>
{
    string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

    services.AddHostedService<Worker>();

    // Register services
    services.AddAWSService<IAmazonSQS>();
    services.AddScoped<IAnswerService, AnswerService>();
    services.AddScoped<IEmotionService, EmotionService>();
    services.AddScoped<IQuestionService, QuestionService>();
    services.AddScoped<IFeedbackService, FeedbackService>();
    services.AddScoped<ISQSService, SQSService>();
    services.AddScoped<IVideoAnalysisService, VideoAnalysisService>();
    services.AddScoped<IVideoService, VideoService>();

    // Register repositories
    services.AddScoped<IAnswerRepository, AnswerRepository>();
    services.AddScoped<IEmotionRepository, EmotionRepository>();
    services.AddScoped<IFeedbackRepository, FeedbackRepository>();
    services.AddScoped<IQuestionRepository, QuestionRepository>();
    services.AddScoped<IVideoAnalysisRepository, VideoAnalysisRepository>();
    services.AddScoped<IVideoRepository, VideoRepository>();

    services.AddScoped<IVideoConversionWorkflow, VideoConversionWorkflow>();
    services.AddScoped<IAnswerWorkflow, AnswerWorkflow>();
    services.AddScoped<IFeedbackWorkflow, FeedbackWorkflow>();
    services.AddScoped<IRekognitionWorkflow, RekognitionWorkflow>();

}).Build();

host.Run();
