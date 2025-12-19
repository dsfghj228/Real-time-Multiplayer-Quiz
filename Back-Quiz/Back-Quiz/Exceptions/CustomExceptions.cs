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
}