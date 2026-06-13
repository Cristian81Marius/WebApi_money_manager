using System.Net.Http.Headers;
using System.Text.Json;
using WebApi_money_management.DTOs.StatementAnalysis;
using WebApi_money_management.Exceptions;

namespace WebApi_money_management.ExternalApis.StatementAnalysis;

public class StatementAnalysisClient(HttpClient httpClient) : IStatementAnalysisClient
{
    public async Task<StatementAnalysisResponse> AnalyzeAsync(
        byte[] fileBytes,
        string fileName,
        string contentType,
        string? bankName,
        DateOnly startDate,
        DateOnly endDate,
        List<CategoryDto> categories)
    {
        using var form = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        form.Add(fileContent, "file", fileName);

        if (bankName is not null)
            form.Add(new StringContent(bankName), "bank_name");

        form.Add(new StringContent(startDate.ToString("yyyy-MM-dd")), "start_date");
        form.Add(new StringContent(endDate.ToString("yyyy-MM-dd")), "end_date");
        form.Add(new StringContent(JsonSerializer.Serialize(categories)), "categories");

        var response = await httpClient.PostAsync("/api/v1/analyze", form);

        if (!response.IsSuccessStatusCode)
        {
            throw (int)response.StatusCode switch
            {
                400 => new InvalidDocumentException(),
                401 => new StatementAnalysisAuthException(),
                413 => new FileTooLargeException(),
                415 => new UnsupportedFormatException(),
                422 => new ValidationException("Invalid or missing request fields."),
                502 => new ExternalServiceException(),
                _   => new StatementParsingException()
            };
        }

        return await response.Content.ReadFromJsonAsync<StatementAnalysisResponse>()
               ?? throw new StatementParsingException();
    }
}
