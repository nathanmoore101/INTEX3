using INTEX3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INTEX3.Models
{
    public interface IUserRepository
    {
        Task<IEnumerable<AspNetUser>> GetAllUsersAsync();
        Task<AspNetUser> GetUserByIdAsync(string id);
        Task UpdateUserAsync(AspNetUser user);
        Task DeleteUserAsync(AspNetUser user);
    }
}
