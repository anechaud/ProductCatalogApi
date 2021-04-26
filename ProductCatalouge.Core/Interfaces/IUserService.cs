using ProductCatalouge.Core.Models;
using System.Threading.Tasks;

namespace ProductCatalouge.Core.Interfaces
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}
