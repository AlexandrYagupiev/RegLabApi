using System.Collections.Generic;
using System.Linq;
using RegLabApi.Models;
using RegLabApi.Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using RegLabApi.SignalR;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Configuration>> GetAllAsync(string nameFilter = null, DateTime? dateFilter = null)
        {

            var configurations = await _configurationRepository.GetAllAsync().Select(config => new Configuration
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

        public async Task<Configuration> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Идентификатор конфигурации должен быть положительным числом.");
            }

            var configuration =await _configurationRepository.GetByIdAsync(id);
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

        public async Task CreateAsync(Configuration configuration)
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
            await _configurationRepository.AddAsync(entity);

            // Отправка уведомления о создании конфигурации
            await _hubContext.Clients.All.SendAsync("ConfigurationCreated", configuration.Name);
        }

        public async Task UpdateAsync(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var existingConfig = await _configurationRepository.GetByIdAsync(configuration.Id);

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
                await _configurationRepository.UpdateAsync(existingConfig);

                // Отправка уведомления об обновлении конфигурации
                await _hubContext.Clients.All.SendAsync("ConfigurationUpdated", configuration.Name);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var configuration =await _configurationRepository.GetByIdAsync(id);
            if (configuration != null)
            {
                await _configurationRepository.RemoveAsync(configuration);

                // Отправка уведомления об удалении конфигурации
                await _hubContext.Clients.All.SendAsync("ConfigurationDeleted", configuration.Name);
            }
        }
    }
}
