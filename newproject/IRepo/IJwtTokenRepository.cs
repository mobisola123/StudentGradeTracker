using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace StudentGradeTracker.IRepo
{
    public interface IJwtTokenRepository
    {
        Task<string> GenerateJwtToken(IdentityUser identityUser);
    }
}
