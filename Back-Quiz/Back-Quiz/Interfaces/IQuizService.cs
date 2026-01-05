using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Enums;
using Back_Quiz.Quiz;

namespace Back_Quiz.Interfaces;

public interface IQuizService
{
    Task<StartQuizResponse> CreateQuizAsync(string category, Difficulty difficulty, int numberOfQuestions, string userId);
    Task<StartQuizResponse> GetCurrentQuestionAsync(string sessionId, string userId);
    Task<StartQuizResponse> MakeMoveAsync(string sessionId, string userId, Guid selectedOptionId);
    Task<QuizResultDto> FinishQuizAsync(string sessionId, string userId);
    Task<QuizResultDto> ReturnResults(string sessionId, string userId);
    Task<List<string>> GetCategoriesAsync();
}