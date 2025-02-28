using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RegLabApi.Data.Context;
using RegLabApi.Models;


namespace RegLabApi.Data.Repositories
{
   public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Configuration>> GetAllAsync()
        {
            return await _context.Configurations.ToListAsync();
        }

        public async Task<Configuration> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Идентификатор конфигурации должен быть положительным числом.");
            }

            var entity = await _context.Configurations.FindAsync(id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Конфигурация с идентификатором {id} не найдена.");
            }

            return entity;
        }

        public async Task AddAsync(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.Name))
            {
                throw new ArgumentException("Имя конфигурации не может быть пустым.", nameof(configuration.Name));
            }

            var entity = new Configuration
            {
                Name = configuration.Name,
                Value = configuration.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Version = 1
            };
            _context.Configurations.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.Name))
            {
                throw new ArgumentException("Имя конфигурации не может быть пустым.", nameof(configuration.Name));
            }

            var existingConfig =await _context.Configurations.FindAsync(configuration.Id);

            if (existingConfig == null)
            {
                throw new InvalidOperationException($"Конфигурация с идентификатором {configuration.Id} не найдена.");
            }

            existingConfig.Name = configuration.Name;
            existingConfig.Value = configuration.Value;
            existingConfig.UpdatedAt = DateTime.Now;
            existingConfig.Version++;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException($"Конфигурация с идентификатором {configuration.Id} не найдена.");
            }

            _context.Configurations.Remove(configuration);
            await _context.SaveChangesAsync();
        }
    }
}
