namespace WebApi_money_management.DTOs;

public record UploadStatementResponse(
    Guid StatementId,
    string Bank,
    int ImportedCount
);
