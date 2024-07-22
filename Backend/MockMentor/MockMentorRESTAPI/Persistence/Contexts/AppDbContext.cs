using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
    }
}
