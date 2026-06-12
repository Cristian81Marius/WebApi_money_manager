using WebApi_money_management.DTOs;
using WebApi_money_management.Repositories;

namespace WebApi_money_management.Services.UserPreferences;

public class UserPreferenceService(IUserPreferenceRepository repo) : IUserPreferenceService
{
    public async Task<UserPreferencesResponse> GetAsync(Guid userId)
    {
        var pref = await repo.GetOrCreateAsync(userId);
        return new UserPreferencesResponse
        {
            Language = pref.Language,
            Theme = pref.Theme,
            ProfilePicture = pref.ProfilePicture
        };
    }

    public async Task<UserPreferencesResponse> UpdateAsync(Guid userId, UpdatePreferencesRequest request)
    {
        var pref = await repo.GetOrCreateAsync(userId);

        if (request.Language is not null) pref.Language = request.Language;
        if (request.Theme is not null) pref.Theme = request.Theme;
        if (request.ProfilePicture is not null) pref.ProfilePicture = request.ProfilePicture;

        await repo.UpdateAsync(pref);
        return new UserPreferencesResponse
        {
            Language = pref.Language,
            Theme = pref.Theme,
            ProfilePicture = pref.ProfilePicture
        };
    }
}
