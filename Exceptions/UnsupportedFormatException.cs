namespace WebApi_money_management.Exceptions;

public class UnsupportedFormatException : AppException
{
    public UnsupportedFormatException()
        : base(422, "UNSUPPORTED_FORMAT", "File must be a PDF or CSV.") { }
}
