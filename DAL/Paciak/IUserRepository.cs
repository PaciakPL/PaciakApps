using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Paciak
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersByUids(int[] uids);
    }
}