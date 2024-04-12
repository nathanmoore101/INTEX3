using Microsoft.EntityFrameworkCore;
using INTEX3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INTEX3.Data;

public class EFUserRepository : IUserRepository
{
    private readonly Intex2Context _context;

    public EFUserRepository(Intex2Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AspNetUser>> GetAllUsersAsync()
    {
        return await _context.AspNetUsers.ToListAsync();
    }

    public async Task<AspNetUser> GetUserByIdAsync(string id)
    {
        return await _context.AspNetUsers.FindAsync(id);
    }

    public async Task UpdateUserAsync(AspNetUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(AspNetUser user)
    {
        _context.AspNetUsers.Remove(user);
        await _context.SaveChangesAsync();
    }
}
