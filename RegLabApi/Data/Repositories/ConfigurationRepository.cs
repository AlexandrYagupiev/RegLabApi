using System.Collections.Generic;
using System.Linq;
using RegLabApi.Data.Entities;
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

        public IEnumerable<Configuration> GetAll()
        {
            return _context.Configurations.ToList();
        }

        public Configuration GetById(int id)
        {
            return _context.Configurations.Find(id);
        }

        public void Add(Configuration configuration)
        {
            _context.Configurations.Add(configuration);
            _context.SaveChanges();
        }

        public void Update(Configuration configuration)
        {
            _context.Entry(configuration).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(Configuration configuration)
        {
            _context.Configurations.Remove(configuration);
            _context.SaveChanges();
        }
    }
}
