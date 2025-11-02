using System;
using Common.Entities;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Users;

public class UserRequestFluent : AbstractValidator<User>
{
    public UserRequestFluent()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("Username cannot be empty.").MinimumLength(6).WithMessage("Username must be at least 6 characters long.");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password cannot be empty.").MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First Name cannot be empty.");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last Name cannot be empty.");
    }
}
