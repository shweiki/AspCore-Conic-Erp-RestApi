using FluentValidation;
using MediatR;
using Application.Common.Constants;
using Application.Common.Helpers;

namespace Application.Features.Member.Commands.AddMember;

public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    private readonly ISender _mediator;

    public AddMemberCommandValidator(ISender mediator)
    {
        _mediator = mediator;

        //RuleFor(v => v.Email)
        //    .NotEmpty().WithMessage("Email is required")
        //    .MaximumLength(DefaultSizeConstants.EmailMaxLength).WithMessage($"Email must not exceed {DefaultSizeConstants.EmailMaxLength} characters.")
        //    .Must(IsEmailValid).When(v => !string.IsNullOrWhiteSpace(v.Email), ApplyConditionTo.CurrentValidator).WithMessage("Email is invalid")
        //    .MustAsync(EmailUnique).When(v => !string.IsNullOrWhiteSpace(v.Email), ApplyConditionTo.CurrentValidator).WithMessage(v => $"Email {v.Email} exists.");

        //RuleFor(v => v.FullName)
        //    .NotEmpty().WithMessage("Full Name is required.")
        //    .MaximumLength(DefaultSizeConstants.FullNameMaxLength).WithMessage($"Full Name must not exceed {DefaultSizeConstants.FullNameMaxLength} characters.")
        //    .MustAsync(FullNameUnique).When(v => !string.IsNullOrWhiteSpace(v.FullName), ApplyConditionTo.CurrentValidator).WithMessage(v => $"Full Name {v.FullName} exists.");

        ////RuleFor(v => v.Organization)
        ////    .NotEmpty().WithMessage("Organization is required.")
        ////    .MaximumLength(DefaultSizeConstants.UserTitleMaxLength).WithMessage($"Organization must not exceed {DefaultSizeConstants.UserTitleMaxLength} characters.");


        //RuleFor(v => v.Phone)
        //    .NotEmpty().WithMessage("Phone is required.")
        //    .MaximumLength(DefaultSizeConstants.PhoneMaxLength).WithMessage($"Phone must not exceed {DefaultSizeConstants.PhoneMaxLength} characters.")
        //    .Must(IsPhoneValid).WithMessage("Please enter a valid phone number");

        //RuleFor(v => v.Country)
        //    .NotEmpty().WithMessage("Country is required.")
        //    .MaximumLength(DefaultSizeConstants.CountryMaxLength).WithMessage($"Country must not exceed {DefaultSizeConstants.CountryMaxLength} characters.");

        //RuleFor(v => v.State)
        //    .NotEmpty().WithMessage("State is required.")
        //    .MaximumLength(DefaultSizeConstants.StateMaxLength).WithMessage($"State must not exceed {DefaultSizeConstants.StateMaxLength} characters.");


    }


    private async Task<bool> EmailUnique(AddMemberCommand command, string email, CancellationToken cancellationToken)
    {
        return true;
        //var internalUser = await _mediator.Send(new User.Queries.GetUser.GetUserByEmailQuery { Email = email }, cancellationToken);
        //if (internalUser is null)
        //{
        //    var externalUser = await _mediator.Send(new ExternalUser.Queries.GetUserByEmail.GetUserByEmailQuery { Email = email }, cancellationToken);
        //    return externalUser is null;
        //}
        //else
        //{
        //    return false;
        //}
    }
    private async Task<bool> FullNameUnique(AddMemberCommand command, string fullname, CancellationToken cancellationToken)
    {
        return true;
        //var externalUser = await _mediator.Send(new ExternalUser.Queries.GetUser.GetUserByFullNameQuery { FullName = fullname }, cancellationToken);
        //return externalUser is null;
    }



    public static bool IsEmailValid(AddMemberCommand command, string email)
    {
        return true; // EmailValidation.IsEmailValid(email);
    }

    public static bool IsPhoneValid(string phone)
    {
        return true;  //PhoneValidation.IsPhoneValid(phone);
    }
}
