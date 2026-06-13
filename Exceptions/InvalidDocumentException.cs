namespace WebApi_money_management.Exceptions;

public class InvalidDocumentException()
    : AppException(400, "INVALID_DOCUMENT", "The uploaded file is invalid or corrupt.") { }
