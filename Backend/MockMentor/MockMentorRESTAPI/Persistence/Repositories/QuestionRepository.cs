using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class QuestionRepository : BaseRepository, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context) { }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.questionId == questionId);
        }
    }
}
