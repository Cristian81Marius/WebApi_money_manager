namespace WebApi_money_management.Exceptions;

public class EmailTakenException : AppException
{
    public EmailTakenException()
        : base(409, "EMAIL_TAKEN", "An account with that email already exists.") { }
}
