using Back_Quiz.Data;
using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Enums;
using Back_Quiz.Interfaces;
using Back_Quiz.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Back_Quiz.Services;

public class QuizService : IQuizService
{
    private readonly IRedisService _redisService;
    private readonly ApplicationDbContext _context;
    
    public QuizService(IRedisService redisService, ApplicationDbContext context)
    {
        _redisService = redisService;
        _context = context;
    }
    
    public async Task<StartQuizResponse> CreateQuizAsync(string Category, Difficulty Difficulty, int NumberOfQuestions, string UserId)
    {
        var query = _context.Questions
            .Where(q => q.Category == Category && q.Difficulty == Difficulty)
            .Select(q => q.Id);

        int count = await query.CountAsync();
        var random = new Random();
        var skip = random.Next(0, Math.Max(0, count - NumberOfQuestions));

        var questionsIds = await query
            .Skip(skip)
            .Take(NumberOfQuestions)
            .ToListAsync();
        
        if(questionsIds.Count < NumberOfQuestions)
        {
            throw new Exception("Not enough questions available for the selected category and difficulty.");
        }
        
        var sessionId = Guid.NewGuid().ToString();

        var session = new QuizSession
        {
            UserId = UserId,
            QuestionIds = questionsIds,
            CurrentIndex = 0,
            Answers = new List<UserAnswer>(),
            StartedAt = DateTime.UtcNow
        };

        await _redisService.SetAsync(
            $"quiz:session:{sessionId}",
            session,
            TimeSpan.FromMinutes(30));
        
        var firstQuestion = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == questionsIds[0]);
        
        var questionDto = new QuestionDto
        {
            Id = firstQuestion.Id,
            Text = firstQuestion.Text,
            Options = firstQuestion.Options.Select(o => new QuestionOptionDto
            {
                Id = o.Id,
                Text = o.Text
            }).ToList()
        };

        return new StartQuizResponse
        {
            SessionId = sessionId,
            Question = questionDto
        };
    }
}