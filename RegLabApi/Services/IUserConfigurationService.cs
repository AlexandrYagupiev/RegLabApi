using RegLabApi.Models;

namespace RegLabApi.Services
{
    public interface IConfigurationService
    {
        IEnumerable<Configuration> GetAll();
        Configuration GetById(int id);
        void Create(Configuration configuration);
        void Update(Configuration configuration);
        void Delete(int id);
    }
}
