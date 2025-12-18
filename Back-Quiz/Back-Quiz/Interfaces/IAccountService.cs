using Back_Quiz.MediatR.Commands;

namespace Back_Quiz.Interfaces;

public interface IAccountService
{
    Task CheckUser(RegisterNewUserCommand command);
}