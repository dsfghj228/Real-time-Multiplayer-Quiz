using Back_Quiz.MediatR.Commands;
using FluentValidation;

namespace Back_Quiz.FluentValidation.Validation.Account;

public class MakeMoveCommandValidator : AbstractValidator<MakeMoveCommand>
{
    public MakeMoveCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}