using Microsoft.EntityFrameworkCore;
using WebApi_money_management.Data;
using WebApi_money_management.Entities;

namespace WebApi_money_management.Repositories.UserPreferences;

public class UserPreferenceRepository(AppDbContext db) : IUserPreferenceRepository
{
    public async Task<UserPreference> GetOrCreateAsync(Guid userId)
    {
        var preference = await db.UserPreferences.FindAsync(userId);
        if (preference is not null) return preference;

        preference = new UserPreference { UserId = userId };
        db.UserPreferences.Add(preference);
        await db.SaveChangesAsync();
        return preference;
    }

    public async Task<UserPreference> UpdateAsync(UserPreference preference)
    {
        db.UserPreferences.Update(preference);
        await db.SaveChangesAsync();
        return preference;
    }
}
