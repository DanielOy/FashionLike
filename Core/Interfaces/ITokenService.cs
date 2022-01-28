using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
