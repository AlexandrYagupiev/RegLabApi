using System.Collections.Generic;

namespace RegLabApi.Models
{
    public class UserConfiguration
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<Configuration> Configurations { get; set; }
    }
}
