using WebApi_money_management.DTOs;

namespace WebApi_money_management.Services;

public interface IUserPreferenceService
{
    Task<UserPreferencesResponse> GetAsync(Guid userId);
    Task<UserPreferencesResponse> UpdateAsync(Guid userId, UpdatePreferencesRequest request);
}
