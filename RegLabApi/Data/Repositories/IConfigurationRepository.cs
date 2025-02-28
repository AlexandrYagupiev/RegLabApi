using System.Collections.Generic;
using RegLabApi.Models;


namespace RegLabApi.Data.Repositories
{
    public interface IConfigurationRepository
    {
        IEnumerable<Configuration> GetAll();
        Configuration GetById(int id);
        void Add(Configuration configuration);
        void Update(Configuration configuration);
        void Remove(Configuration configuration);
    }
}
