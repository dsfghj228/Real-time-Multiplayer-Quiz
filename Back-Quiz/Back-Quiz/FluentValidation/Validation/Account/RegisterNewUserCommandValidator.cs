using Back_Quiz.MediatR.Commands;
using FluentValidation;

namespace Back_Quiz.FluentValidation.Validation.Account;

public class RegisterNewUserCommandValidator : AbstractValidator<RegisterNewUserCommand>
{
    public RegisterNewUserCommandValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character");
        RuleFor(request => request.Username).NotEmpty().WithMessage("UserName is required");
    }
}