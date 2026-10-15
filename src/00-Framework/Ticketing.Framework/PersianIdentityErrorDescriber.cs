using Microsoft.AspNetCore.Identity;

namespace Ticketing.Framework;

public sealed class PersianIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => Error(nameof(DefaultError), "خطایی پیش‌بینی‌نشده رخ داد.");
    public override IdentityError ConcurrencyFailure() => Error(nameof(ConcurrencyFailure), "اطلاعات هم‌زمان تغییر کرده است؛ دوباره تلاش کنید.");
    public override IdentityError PasswordMismatch() => Error(nameof(PasswordMismatch), "گذرواژه واردشده درست نیست.");
    public override IdentityError InvalidToken() => Error(nameof(InvalidToken), "کد یا پیوند واردشده معتبر نیست.");
    public override IdentityError LoginAlreadyAssociated() => Error(nameof(LoginAlreadyAssociated), "این روش ورود پیش‌تر به حساب دیگری متصل شده است.");
    public override IdentityError InvalidUserName(string? userName) => Error(nameof(InvalidUserName), $"نام کاربری «{userName}» معتبر نیست.");
    public override IdentityError InvalidEmail(string? email) => Error(nameof(InvalidEmail), $"ایمیل «{email}» معتبر نیست.");
    public override IdentityError DuplicateUserName(string userName) => Error(nameof(DuplicateUserName), $"نام کاربری «{userName}» قبلاً ثبت شده است.");
    public override IdentityError DuplicateEmail(string email) => Error(nameof(DuplicateEmail), $"ایمیل «{email}» قبلاً توسط شخص دیگری ثبت شده است.");
    public override IdentityError InvalidRoleName(string? role) => Error(nameof(InvalidRoleName), $"نام نقش «{role}» معتبر نیست.");
    public override IdentityError DuplicateRoleName(string role) => Error(nameof(DuplicateRoleName), $"نقش «{role}» قبلاً ثبت شده است.");
    public override IdentityError UserAlreadyHasPassword() => Error(nameof(UserAlreadyHasPassword), "این حساب از قبل گذرواژه دارد.");
    public override IdentityError UserLockoutNotEnabled() => Error(nameof(UserLockoutNotEnabled), "قفل‌کردن حساب برای این کاربر فعال نیست.");
    public override IdentityError UserAlreadyInRole(string role) => Error(nameof(UserAlreadyInRole), $"این کاربر از قبل عضو نقش «{role}» است.");
    public override IdentityError UserNotInRole(string role) => Error(nameof(UserNotInRole), $"این کاربر عضو نقش «{role}» نیست.");
    public override IdentityError PasswordTooShort(int length) => Error(nameof(PasswordTooShort), $"گذرواژه باید دست‌کم {length} نویسه داشته باشد.");
    public override IdentityError PasswordRequiresNonAlphanumeric() => Error(nameof(PasswordRequiresNonAlphanumeric), "گذرواژه باید دست‌کم یک نماد داشته باشد.");
    public override IdentityError PasswordRequiresDigit() => Error(nameof(PasswordRequiresDigit), "گذرواژه باید دست‌کم یک عدد داشته باشد.");
    public override IdentityError PasswordRequiresLower() => Error(nameof(PasswordRequiresLower), "گذرواژه باید دست‌کم یک حرف کوچک انگلیسی داشته باشد.");
    public override IdentityError PasswordRequiresUpper() => Error(nameof(PasswordRequiresUpper), "گذرواژه باید دست‌کم یک حرف بزرگ انگلیسی داشته باشد.");
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Error(nameof(PasswordRequiresUniqueChars), $"گذرواژه باید دست‌کم {uniqueChars} نویسهٔ یکتا داشته باشد.");
    public override IdentityError RecoveryCodeRedemptionFailed() => Error(nameof(RecoveryCodeRedemptionFailed), "کد بازیابی قبلاً استفاده شده یا معتبر نیست.");

    private static IdentityError Error(string code, string description) => new()
    {
        Code = code,
        Description = description
    };
}
