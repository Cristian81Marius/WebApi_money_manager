using System.Text.Json.Serialization;

namespace WebApi_money_management.DTOs.StatementAnalysis;

public class StatementAnalysisResponse
{
    [JsonPropertyName("expenses")]
    public List<AnalyzedTransactionDto> Expenses { get; set; } = [];

    [JsonPropertyName("income")]
    public List<AnalyzedTransactionDto> Income { get; set; } = [];

    [JsonPropertyName("total_expenses")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal TotalExpenses { get; set; }

    [JsonPropertyName("total_income")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal TotalIncome { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("bank_name")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("statement_period")]
    public string StatementPeriod { get; set; } = string.Empty;

    [JsonPropertyName("transaction_count")]
    public int TransactionCount { get; set; }

    [JsonPropertyName("parsing_confidence")]
    public double ParsingConfidence { get; set; }

    [JsonPropertyName("warnings")]
    public List<string> Warnings { get; set; } = [];
}
