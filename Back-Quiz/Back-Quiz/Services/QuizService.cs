using Back_Quiz.Data;
using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Enums;
using Back_Quiz.Exceptions;
using Back_Quiz.Interfaces;
using Back_Quiz.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Back_Quiz.Services;

//СДЕЛАТЬ РЕФАКТОРИНГ ПОВТОРЯЮЩЕГОСЯ КОДА В GETCURRENTQUESTIONASYNC И MAKEMOVEASYNC
public class QuizService : IQuizService
{
    private readonly IRedisService _redisService;
    private readonly ApplicationDbContext _context;
    
    public QuizService(IRedisService redisService, ApplicationDbContext context)
    {
        _redisService = redisService;
        _context = context;
    }
    
    public async Task<StartQuizResponse> CreateQuizAsync(string category, Difficulty difficulty, int numberOfQuestions, string userId)
    {
        var query = _context.Questions
            .Where(q => q.Category == category && q.Difficulty == difficulty)
            .Select(q => q.Id);

        int count = await query.CountAsync();
        var random = new Random();
        var skip = random.Next(0, Math.Max(0, count - numberOfQuestions));

        var questionsIds = await query
            .Skip(skip)
            .Take(numberOfQuestions)
            .ToListAsync();
        
        if(questionsIds.Count < numberOfQuestions)
        {
            throw new CustomExceptions.BusinessRuleViolationException();
        }
        
        var sessionId = Guid.NewGuid().ToString();

        var session = new QuizSession
        {
            UserId = userId,
            QuestionIds = questionsIds,
            CurrentIndex = 0,
            Answers = new List<UserAnswer>(),
            StartedAt = DateTime.UtcNow,
            TimeLimitSeconds = 30 * 60
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

    public async Task<StartQuizResponse> GetCurrentQuestionAsync(string sessionId, string userId)
    {
        var session = await _redisService.GetAsync<QuizSession>($"quiz:session:{sessionId}");
        
        if (session == null || session.UserId != userId)
        {
            throw new CustomExceptions.AccessDeniedException();
        }
        
        if (session.CurrentIndex < 0 || session.CurrentIndex >= session.QuestionIds.Count)
        {
            throw new CustomExceptions.QuizAlreadyCompletedException();
        }
        
        var currentQuestionId = session.QuestionIds[session.CurrentIndex];
        var currentQuestion = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == currentQuestionId);
        
        if (currentQuestion == null)
        {
            throw new CustomExceptions.QuestionNotFoundException(currentQuestionId);
        }
        
        var questionDto = new QuestionDto
        {
            Id = currentQuestion.Id,
            Text = currentQuestion.Text,
            Options = currentQuestion.Options.Select(o => new QuestionOptionDto
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

    public async Task<StartQuizResponse> MakeMoveAsync(string sessionId, string userId, Guid selectedOptionId)
    {
        var lockKey = $"quiz:session:{sessionId}:lock";
        var sessionKey = $"quiz:session:{sessionId}";
        var token = await _redisService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(60));
        if (token == null)
        {
            throw new CustomExceptions.ConcurrentAccessException();
        }
        
        try
        {
            var session = await _redisService.GetAsync<QuizSession>(sessionKey);
            if (session == null || session.UserId != userId)
            {
                throw new CustomExceptions.AccessDeniedException();
            }

            if (session.CurrentIndex < 0 || session.CurrentIndex >= session.QuestionIds.Count)
            {
                throw new CustomExceptions.QuizAlreadyCompletedException();
            }

            var currentQuestionId = session.QuestionIds[session.CurrentIndex];
            if (session.Answers.Any(a => a.QuestionId == currentQuestionId))
            {
                throw new CustomExceptions.QuestionAlreadyAnsweredException(currentQuestionId);
            }

            var question = await _context.Questions
                .Include(q => q.Options)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == currentQuestionId);

            if (question == null)
            {
                throw new CustomExceptions.QuestionNotFoundException(currentQuestionId);
            }

            if (question.Options.All(o => o.Id != selectedOptionId))
            {
                throw new CustomExceptions.InvalidOptionException(selectedOptionId);
            }
            
            var expiresAt = session.StartedAt.AddSeconds(session.TimeLimitSeconds);

            var ttl = expiresAt - DateTime.UtcNow;
            if (ttl < TimeSpan.Zero)
            {
                throw new CustomExceptions.SessionExpiredException(sessionId);
            }

            session.Answers.Add(new UserAnswer
            {
                QuestionId = currentQuestionId,
                SelectedOptionId = selectedOptionId
            });

            session.CurrentIndex++;

            await _redisService.SetAsync(sessionKey, session, ttl);

            if (session.CurrentIndex >= session.QuestionIds.Count)
            {
                return new StartQuizResponse
                {
                    SessionId = sessionId,
                    Question = null
                };
            }

            var nextQuestionId = session.QuestionIds[session.CurrentIndex];
            var nextQuestion = await _context.Questions
                .Include(q => q.Options)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == nextQuestionId);

            if (nextQuestion == null)
            {
                throw new CustomExceptions.QuestionNotFoundException(nextQuestionId);
            }

            var nextQuestionDto = new QuestionDto
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.Text,
                Options = nextQuestion.Options.Select(o => new QuestionOptionDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList()
            };

            return new StartQuizResponse
            {
                SessionId = sessionId,
                Question = nextQuestionDto
            };
        }
        finally
        {
            await _redisService.ReleaseLockAsync(lockKey, token);
        }
    }
}