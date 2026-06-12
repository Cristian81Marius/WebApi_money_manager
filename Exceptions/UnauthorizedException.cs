namespace WebApi_money_management.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException()
        : base(401, "UNAUTHORIZED", "Authentication required.") { }
}
