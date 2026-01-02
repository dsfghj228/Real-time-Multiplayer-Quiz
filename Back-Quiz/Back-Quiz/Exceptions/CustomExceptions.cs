using System.Net;

namespace Back_Quiz.Exceptions;

public abstract class CustomExceptions: Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Type { get; }
    public string Title { get; }

    protected CustomExceptions(
        HttpStatusCode statusCode,
        string type,
        string title,
        string message) : base(message)
    {
        StatusCode = statusCode;
        Type = type;
        Title = title;
    }
    
    public class UserAlreadyExistsException() : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        "User already exists",
        "Such user already exists");
    
    public class InternalServerErrorException(string errors) : CustomExceptions(HttpStatusCode.InternalServerError,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Internal Server Error",
        $"An unexpected error has occurred on the server: @{errors}");
    
    public class UnauthorizedUsernameException() : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Authorization error",
        "An error occurred while trying to log in. User with such username does not exist");
    
    public class UnauthorizedPasswordException() : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Authorization error",
        "An error occurred while trying to log in. Wrong password");
    
    public class BusinessRuleViolationException() : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Business Rule Violation",
        "Not enough questions available for the selected category and difficulty.");
    
    public class AccessDeniedException() : CustomExceptions(HttpStatusCode.Forbidden,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Access Denied",
        "Invalid session or user.");
    
    public class QuizAlreadyCompletedException() : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Quiz Already Completed",
        "Quiz is already completed.");
    
    public class QuestionNotFoundException(Guid id) : CustomExceptions(HttpStatusCode.NotFound,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Question Not Found",
        $"Question with ID @{id} was not found.");
    
    public class SessionExpiredException(string id) : CustomExceptions(HttpStatusCode.BadRequest,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Session Expired",
        $"Session with ID @{id} expired.");
    
    public class QuestionAlreadyAnsweredException(Guid id) : CustomExceptions(HttpStatusCode.BadRequest,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Question Already Answered",
        $"Question with ID @{id} already answered.");
    
    public class InvalidOptionException(Guid id) : CustomExceptions(HttpStatusCode.BadRequest,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Invalid Option",
        $"Option with ID @{id} is not valid for the current question.");
    
    public class ConcurrentAccessException() : CustomExceptions(HttpStatusCode.Conflict,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Concurrent Access",
        "The request conflicts with an ongoing operation on the same resource. Please try again.");
    
    public class QuizIsNotCompletedException() : CustomExceptions(HttpStatusCode.BadRequest,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Quiz Not Completed",
        "Quiz is not yet completed.");
    
    public class ResultNotFoundException() : CustomExceptions(HttpStatusCode.NotFound,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        "Result not found",
        "Result was not found.");
    
    public class SecurityTokenException(string msg) : CustomExceptions(HttpStatusCode.Unauthorized,
        "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        $"@{msg}",
        "");
}