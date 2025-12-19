namespace Back_Quiz.Dtos.Account;

public class ReturnUserDto
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string Token { get; set; }
}