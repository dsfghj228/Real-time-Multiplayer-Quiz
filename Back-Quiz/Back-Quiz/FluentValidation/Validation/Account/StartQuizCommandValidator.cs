using Back_Quiz.MediatR.Commands;
using FluentValidation;

namespace Back_Quiz.FluentValidation.Validation.Account;

public class StartQuizCommandValidator : AbstractValidator<StartQuizCommand>
{
    public StartQuizCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Difficulty).IsInEnum().NotEmpty();
        RuleFor(x => x.NumberOfQuestions).GreaterThan(0).LessThanOrEqualTo(30);
    }
}