using MediatR;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRegistrationService registration,
    IJwtService jwtService) : IRequestHandler<RegisterCommand, Result<AuthTokenDto>>
{
    public async Task<Result<AuthTokenDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registered = await registration.RegisterNormalUserAsync(
            request.Email.Trim(), request.Password, cancellationToken);
        if (!registered.Succeeded)
        {
            var details = registered.ErrorCodes.Select(code => code switch
            {
                "DuplicateEmail" or "DuplicateUserName" => "این ایمیل قبلاً ثبت شده است.",
                "PasswordTooShort" => "گذرواژه باید حداقل ۸ نویسه باشد.",
                "PasswordRequiresDigit" => "گذرواژه باید دست‌کم یک عدد داشته باشد.",
                "PasswordRequiresUpper" => "گذرواژه باید دست‌کم یک حرف بزرگ انگلیسی داشته باشد.",
                "PasswordRequiresLower" => "گذرواژه باید دست‌کم یک حرف کوچک انگلیسی داشته باشد.",
                "PasswordRequiresNonAlphanumeric" => "گذرواژه باید دست‌کم یک نماد داشته باشد.",
                _ => "اطلاعات واردشده معتبر نیست."
            }).Distinct();
            return Result<AuthTokenDto>.Failure(ApplicationErrors.RegistrationFailed(string.Join(" ", details)));
        }

        var tokens = await jwtService.CreateTokenPairAsync(registered.User!, cancellationToken);
        return Result<AuthTokenDto>.Success(tokens);
    }
}
