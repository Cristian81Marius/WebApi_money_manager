using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories;

public interface IUserPreferenceRepository
{
    Task<UserPreference> GetOrCreateAsync(Guid userId);
    Task<UserPreference> UpdateAsync(UserPreference preference);
}
