namespace WebApi_money_management.Exceptions;

public class ExternalServiceException()
    : AppException(502, "EXTERNAL_SERVICE_ERROR", "The AI analysis service is currently unavailable.") { }
