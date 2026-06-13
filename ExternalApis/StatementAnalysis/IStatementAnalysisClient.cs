using WebApi_money_management.DTOs.StatementAnalysis;

namespace WebApi_money_management.ExternalApis.StatementAnalysis;

public interface IStatementAnalysisClient
{
    Task<StatementAnalysisResponse> AnalyzeAsync(
        byte[] pdfBytes,
        string? bankName,
        DateOnly startDate,
        DateOnly endDate,
        List<CategoryDto> categories);
}
