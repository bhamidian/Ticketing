using FluentValidation;

namespace Ticketing.Application.Features.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryValidator : AbstractValidator<GetTicketsQuery>
{
    public GetTicketsQueryValidator()
    {
        RuleFor(query => query.UserId).NotEmpty();
        RuleFor(query => query.Status)
            .Must(status => !status.HasValue || Enum.IsDefined(status.Value))
            .WithMessage("وضعیت انتخاب‌شده معتبر نیست.");
        RuleFor(query => query.Priority)
            .Must(priority => !priority.HasValue || Enum.IsDefined(priority.Value))
            .WithMessage("اولویت انتخاب‌شده معتبر نیست.");
    }
}
