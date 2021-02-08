using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Paciak
{
    public interface ISettingsRepository
    {
        Task<Option> GetOrDefault(string name, Option @default);
        Task<bool> Upsert(Option option);
        Task<bool> Delete(string name);
    }
}