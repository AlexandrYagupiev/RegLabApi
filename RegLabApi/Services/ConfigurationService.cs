using System.Collections.Generic;
using System.Linq;
using RegLabApi.Models;
using RegLabApi.Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using RegLabApi.SignalR;

namespace RegLabApi.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ConfigurationService(IConfigurationRepository configurationRepository, IHubContext<NotificationHub> hubContext)
        {
            _configurationRepository = configurationRepository;
            _hubContext = hubContext;
        }

        public IEnumerable<Configuration> GetAll(string nameFilter = null, DateTime? dateFilter = null)
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

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(nameFilter))
            {
                configurations = configurations.Where(e => e.Name.Contains(nameFilter));
            }

            // Фильтрация по дате создания
            if (dateFilter.HasValue)
            {
                configurations = configurations.Where(e => e.CreatedAt >= dateFilter.Value);
            }

            return configurations;
        }

        public Configuration GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Идентификатор конфигурации должен быть положительным числом.");
            }

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
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var entity = new Configuration
            {
                Name = configuration.Name,
                Value = configuration.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Version = 1
            };
            _configurationRepository.Add(entity);

            // Отправка уведомления о создании конфигурации
            _hubContext.Clients.All.SendAsync("ConfigurationCreated", configuration.Name);
        }

        public void Update(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var existingConfig = _configurationRepository.GetById(configuration.Id);

            if (existingConfig == null)
            {
                throw new InvalidOperationException($"Конфигурация с идентификатором {configuration.Id} не найдена.");
            }

            if (existingConfig != null)
            {
                existingConfig.Name = configuration.Name;
                existingConfig.Value = configuration.Value;
                existingConfig.UpdatedAt = DateTime.Now;
                existingConfig.Version++;
                _configurationRepository.Update(existingConfig);

                // Отправка уведомления об обновлении конфигурации
                _hubContext.Clients.All.SendAsync("ConfigurationUpdated", configuration.Name);
            }
        }

        public void Delete(int id)
        {
            var configuration = _configurationRepository.GetById(id);
            if (configuration != null)
            {
                _configurationRepository.Remove(configuration);

                // Отправка уведомления об удалении конфигурации
                _hubContext.Clients.All.SendAsync("ConfigurationDeleted", configuration.Name);
            }
        }
    }
}
