using RegLabApi.Models;

namespace RegLabApi.Services
{
    public interface IConfigurationService
    {
        Task<IEnumerable<Configuration>> GetAllAsync(string nameFilter = null, DateTime? dateFilter = null);
        Task<Configuration> GetByIdAsync(int id);
        Task CreateAsync(Configuration configuration);
        Task UpdateAsync(Configuration configuration);
        Task DeleteAsync(int id);
        Task<IEnumerable<Configuration>> GetHistoryAsync(int configurationId);
        Task RestoreVersionAsync(int configurationId, int version);
    }
}
