using System.Threading.Tasks;
using DAL.Entities;
using DAL.Paciak;

namespace Common.Services
{
    public interface ISettingsService
    {
        Task<Option> GetOrDefault(string name, Option @default);
        Task<bool> Upsert(Option option);
        Task<bool> Delete(string name);
        Task<bool> Delete(Option option);
    }
}