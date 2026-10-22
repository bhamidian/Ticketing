namespace Ticketing.Application.Results;

public static class ApplicationErrors
{
    public static ApplicationError RegistrationFailed(string message) =>
        new("auth.registration_failed", message, ApplicationErrorType.Validation);

    public static ApplicationError InvalidCredentials =>
        new("auth.invalid_credentials", "اطلاعات ورود یا نشست معتبر نیست.", ApplicationErrorType.Unauthorized);

    public static ApplicationError Forbidden(string message = "اجازه انجام این عملیات را ندارید.") =>
        new("access.forbidden", message, ApplicationErrorType.Forbidden);

    public static ApplicationError TicketNotFound(Guid ticketId) =>
        new("ticket.not_found", $"تیکت «{ticketId}» پیدا نشد.", ApplicationErrorType.NotFound);

    public static ApplicationError TicketMessageNotFound(Guid ticketMessageId) =>
        new("ticketmessage.not_found", $"پیام تیک «{ticketMessageId}» پیدا نشد",ApplicationErrorType.NotFound);

    public static ApplicationError TicketClosed =>
        new("ticket.closed", "امکان ارسال پیام برای تیکت بسته‌شده وجود ندارد.", ApplicationErrorType.Conflict);

    public static ApplicationError TicketAlreadyClosed =>
        new("ticket.already_closed", "این تیکت قبلاً بسته شده است.", ApplicationErrorType.Conflict);

    public static ApplicationError TicketAlreadyAssigned =>
        new("ticket.already_assigned", "این تیکت به عامل دیگری واگذار شده است.", ApplicationErrorType.Conflict);

    public static ApplicationError ClosedTicketCannotBeAssigned =>
        new("ticket.closed_assignment", "امکان واگذاری تیکت بسته‌شده وجود ندارد.", ApplicationErrorType.Conflict);

    public static ApplicationError TicketMustBeClaimed =>
        new("ticket.must_be_claimed", "برای ویرایش، ابتدا تیکت را بپذیرید.", ApplicationErrorType.Conflict);
}
