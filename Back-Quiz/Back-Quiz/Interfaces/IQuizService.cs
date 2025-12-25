using Back_Quiz.Enums;
using Back_Quiz.Models;
using Back_Quiz.Quiz;

namespace Back_Quiz.Interfaces;

public interface IQuizService
{
    Task<StartQuizResponse> CreateQuizAsync(string category, Difficulty difficulty, int numberOfQuestions, string userId);
    Task<StartQuizResponse> GetCurrentQuestionAsync(string sessionId, string userId);
    Task<StartQuizResponse> MakeMoveAsync(string sessionId, string userId, Guid selectedOptionId);
}