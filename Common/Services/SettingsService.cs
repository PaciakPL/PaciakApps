using System.Threading.Tasks;
using DAL.Entities;
using DAL.Paciak;

namespace Common.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }
        
        public async Task<Option> GetOrDefault(string name, Option @default = default)
        {
            return await settingsRepository.GetOrDefault(name, @default);
        }

        public async Task<bool> Upsert(Option option)
        {
            return await settingsRepository.Upsert(option);
        }

        public async Task<bool> Delete(string name)
        {
            return await settingsRepository.Delete(name);
        }

        public async Task<bool> Delete(Option option)
        {
            return await Delete(option.Name);
        }
    }
}