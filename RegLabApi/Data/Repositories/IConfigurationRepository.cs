using System.Collections.Generic;
using RegLabApi.Models;


namespace RegLabApi.Data.Repositories
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<Configuration>> GetAllAsync();
        Task<Configuration> GetByIdAsync(int id);
        Task AddAsync(Configuration configuration);
        Task UpdateAsync(Configuration configuration);
        Task RemoveAsync(Configuration configuration);
    }
}
