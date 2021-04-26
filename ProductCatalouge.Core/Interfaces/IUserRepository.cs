using ProductCatalouge.EntityFramework.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalouge.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<TblUser> GetUser(string userId, string password);
        Task<List<TblUser>> GetUsers();
        Task<List<TblUserRole>> GetRoles(string userId);
    }
}
