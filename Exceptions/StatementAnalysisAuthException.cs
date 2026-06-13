namespace WebApi_money_management.Exceptions;

public class StatementAnalysisAuthException()
    : AppException(502, "ANALYSIS_UNAUTHORIZED", "Statement analysis service rejected the API key.") { }
