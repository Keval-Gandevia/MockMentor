using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Persistence.Contexts;
using MockMentorRESTAPI.Persistence.Repositories;
using MockMentorRESTAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Connect to the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Register services
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IEmotionService, EmotionService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISQSService, SQSService>();
builder.Services.AddScoped<IVideoAnalysisService, VideoAnalysisService>();
builder.Services.AddScoped<IVideoService, VideoService>();

// Register repositories
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IEmotionRepository, EmotionRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IVideoAnalysisRepository, VideoAnalysisRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();

// Enable cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
