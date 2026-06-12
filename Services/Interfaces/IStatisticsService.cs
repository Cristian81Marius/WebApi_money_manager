using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface IStatisticsService
{
    Task<StatisticsResponse> GetStatisticsAsync(Guid userId, string? fromMonth, string? toMonth);
}
