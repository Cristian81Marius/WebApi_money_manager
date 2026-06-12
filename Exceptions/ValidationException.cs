namespace WebApi_money_management.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string message)
        : base(400, "VALIDATION_ERROR", message) { }
}
