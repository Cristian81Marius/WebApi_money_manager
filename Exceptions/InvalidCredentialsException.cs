namespace WebApi_money_management.Exceptions;

public class InvalidCredentialsException : AppException
{
    public InvalidCredentialsException()
        : base(401, "INVALID_CREDENTIALS", "Invalid email or password.") { }
}
