using RegLabApi.Models;

namespace RegLabApi.Services
{
    public interface IConfigurationService
    {
        IEnumerable<Configuration> GetAll(string nameFilter = null, DateTime? dateFilter = null);
        Configuration GetById(int id);
        void Create(Configuration configuration);
        void Update(Configuration configuration);
        void Delete(int id);
    }
}
