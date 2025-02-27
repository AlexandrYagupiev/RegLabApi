using System.Collections.Generic;
using System.Linq;
using RegLabApi.Models;
using RegLabApi.Data.Repositories;
using RegLabApi.Data.Entities;

namespace RegLabApi.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IEnumerable<Configuration> GetAll()
        {
            var configurations = _configurationRepository.GetAll().Select(config => new Configuration
            {
                Id = config.Id,
                Name = config.Name,
                Value = config.Value,
                CreatedAt = config.CreatedAt,
                UpdatedAt = config.UpdatedAt,
                Version = config.Version
            });
            return configurations;
        }

        public Configuration GetById(int id)
        {
            var configuration = _configurationRepository.GetById(id);
            return new Configuration
            {
                Id = configuration.Id,
                Name = configuration.Name,
                Value = configuration.Value,
                CreatedAt = configuration.CreatedAt,
                UpdatedAt = configuration.UpdatedAt,
                Version = configuration.Version
            };
        }

        public void Create(Configuration configuration)
        {
            var entity = new Configuration
            {
                Name = configuration.Name,
                Value = configuration.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Version = 1
            };
            _configurationRepository.Add(entity);
        }

        public void Update(Configuration configuration)
        {
            var existingConfig = _configurationRepository.GetById(configuration.Id);
            if (existingConfig != null)
            {
                existingConfig.Name = configuration.Name;
                existingConfig.Value = configuration.Value;
                existingConfig.UpdatedAt = DateTime.Now;
                existingConfig.Version++;
                _configurationRepository.Update(existingConfig);
            }
        }

        public void Delete(int id)
        {
            var configuration = _configurationRepository.GetById(id);
            if (configuration != null)
            {
                _configurationRepository.Remove(configuration);
            }
        }
    }
}
