using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Emotion> Emotions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<VideoAnalysis> VideoAnalyses { get; set; }
    }
}
