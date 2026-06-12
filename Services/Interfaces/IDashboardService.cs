using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface IDashboardService
{
    Task<DashboardResponse> GetSummaryAsync(Guid userId, string? month);
}
