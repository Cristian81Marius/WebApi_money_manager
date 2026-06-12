using Microsoft.AspNetCore.Http;
using WebApi_money_management.DTOs;
using WebApi_money_management.Entities;
using WebApi_money_management.Exceptions;
using WebApi_money_management.Repositories;

namespace WebApi_money_management.Services.Statements;

public class StatementService(
    IStatementRepository statementRepository,
    ITransactionRepository transactionRepository) : IStatementService
{
    private static readonly HashSet<string> SupportedBanks =
        ["BCR", "BRD", "ING", "Raiffeisen", "BT", "UniCredit", "CEC", "Other"];

    private static readonly string[] AllowedMimeTypes =
        ["application/pdf", "text/csv"];

    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    public async Task<UploadStatementResponse> UploadAsync(
        Guid userId, string bank, IFormFile file, string? startDate, string? endDate)
    {
        if (!SupportedBanks.Contains(bank))
            throw new ValidationException(
                $"bank must be one of: {string.Join(", ", SupportedBanks)}.");

        if (file.Length > MaxFileSizeBytes)
            throw new FileTooLargeException();

        if (!AllowedMimeTypes.Contains(file.ContentType))
            throw new UnsupportedFormatException();

        DateOnly? from = null, to = null;

        if (startDate is not null)
        {
            if (!DateOnly.TryParseExact(startDate, "yyyy-MM-dd", out var d))
                throw new ValidationException("startDate must be in YYYY-MM-DD format.");
            from = d;
        }

        if (endDate is not null)
        {
            if (!DateOnly.TryParseExact(endDate, "yyyy-MM-dd", out var d))
                throw new ValidationException("endDate must be in YYYY-MM-DD format.");
            to = d;
        }

        if (from.HasValue && to.HasValue && from > to)
            throw new ValidationException("startDate must not be after endDate.");

        var imported = await ParseAndSaveTransactionsAsync(userId, file, from, to);

        var statement = await statementRepository.CreateAsync(new BankStatement
        {
            UserId        = userId,
            Bank          = bank,
            FileName      = file.FileName,
            ImportedCount = imported
        });

        return new UploadStatementResponse(statement.Id, statement.Bank, statement.ImportedCount);
    }

    // Placeholder — add bank-specific parsers here (CSV/PDF per bank format).
    private async Task<int> ParseAndSaveTransactionsAsync(
        Guid userId, IFormFile file, DateOnly? from, DateOnly? to)
    {
        if (!file.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase))
            return 0;

        var transactions = new List<Transaction>();

        using var reader = new System.IO.StreamReader(file.OpenReadStream());
        var header = await reader.ReadLineAsync();
        if (header is null) return 0;

        var columns = header.Split(',');
        int dateCol = Array.FindIndex(columns, c => c.Trim().Equals("date", StringComparison.OrdinalIgnoreCase));
        int amountCol = Array.FindIndex(columns, c => c.Trim().Equals("amount", StringComparison.OrdinalIgnoreCase));
        int typeCol = Array.FindIndex(columns, c => c.Trim().Equals("type", StringComparison.OrdinalIgnoreCase));
        int descCol = Array.FindIndex(columns, c => c.Trim().Equals("description", StringComparison.OrdinalIgnoreCase));
        int categoryCol = Array.FindIndex(columns, c => c.Trim().Equals("category", StringComparison.OrdinalIgnoreCase));

        if (dateCol < 0 || amountCol < 0) return 0;

        string? line;
        while ((line = await reader.ReadLineAsync()) is not null)
        {
            var fields = line.Split(',');
            if (fields.Length <= Math.Max(dateCol, amountCol)) continue;

            if (!DateOnly.TryParse(fields[dateCol].Trim(), out var date)) continue;
            if (!decimal.TryParse(fields[amountCol].Trim(), out var amount)) continue;
            if (amount <= 0) continue;
            if (from.HasValue && date < from) continue;
            if (to.HasValue && date > to) continue;

            transactions.Add(new Transaction
            {
                UserId      = userId,
                Type        = typeCol >= 0 ? fields[typeCol].Trim() : "expense",
                Description = descCol >= 0 && fields.Length > descCol ? fields[descCol].Trim() : "Imported",
                Amount      = Math.Abs(amount),
                Date        = date,
                Category    = categoryCol >= 0 && fields.Length > categoryCol ? fields[categoryCol].Trim() : "Other",
            });
        }

        foreach (var t in transactions)
            await transactionRepository.CreateAsync(t);

        return transactions.Count;
    }
}
