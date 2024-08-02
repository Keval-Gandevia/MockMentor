using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using MockMentorConsumer;
using MockMentorConsumer.Domain.Workflows;
using MockMentorConsumer.Workflows;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Persistence.Contexts;
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
    services.AddScoped<ISQSService, SQSService>();
    services.AddScoped<IQuestionService, QuestionService>();

    services.AddScoped<IVideoConversionWorkflow, VideoConversionWorkflow>();
    services.AddScoped<IAnswerWorkflow, AnswerWorkflow>();

}).Build();

host.Run();
