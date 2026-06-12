using Microsoft.AspNetCore.Http;
using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface IStatementService
{
    Task<UploadStatementResponse> UploadAsync(Guid userId, string bank, IFormFile file, string? startDate, string? endDate);
}
