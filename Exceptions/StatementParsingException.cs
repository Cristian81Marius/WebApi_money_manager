namespace WebApi_money_management.Exceptions;

public class StatementParsingException()
    : AppException(500, "PARSING_ERROR", "An error occurred while parsing the bank statement.") { }
