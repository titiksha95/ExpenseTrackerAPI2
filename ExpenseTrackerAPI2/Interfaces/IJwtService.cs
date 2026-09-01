using ExpenseTrackerAPI2.Models;

namespace ExpenseTrackerAPI2.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}