namespace WebApi_money_management.Exceptions;

public class FileTooLargeException : AppException
{
    public FileTooLargeException()
        : base(413, "FILE_TOO_LARGE", "File exceeds the maximum allowed size of 10 MB.") { }
}
