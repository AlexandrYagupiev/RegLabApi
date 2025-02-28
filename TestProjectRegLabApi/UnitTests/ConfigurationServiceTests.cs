using Xunit;
using Moq;
using RegLabApi.Services;
using RegLabApi.Data.Repositories;
using RegLabApi.Models;


namespace TestProjectRegLabApi.UnitTests
{
    public class ConfigurationServiceTests
    {
        [Fact]
        public void GetAll_ReturnsAllConfigurations()
        {           
            var mockRepo = new Mock<IConfigurationRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(new[]
            {
                new Configuration { Id = 1, Name = "Config1", Value = "Value1" },
                new Configuration { Id = 2, Name = "Config2", Value = "Value2" }
            });

            var service = new ConfigurationService(mockRepo.Object);
           
            var result = service.GetAll();
           
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Config1");
            Assert.Contains(result, c => c.Name == "Config2");
        }

        [Fact]
        public void GetById_ReturnsCorrectConfiguration()
        {           
            var mockRepo = new Mock<IConfigurationRepository>();
            mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(new Configuration { Id = 1, Name = "Config1", Value = "Value1" });

            var service = new ConfigurationService(mockRepo.Object);
           
            var result = service.GetById(1);
           
            Assert.NotNull(result);
            Assert.Equal("Config1", result.Name);
        }

        [Fact]
        public void Create_SavesConfiguration()
        {          
            var mockRepo = new Mock<IConfigurationRepository>();
            var service = new ConfigurationService(mockRepo.Object);
            var configuration = new Configuration { Name = "TestConfig", Value = "TestValue" };
           
            service.Create(configuration);
          
            mockRepo.Verify(repo => repo.Add(It.Is<Configuration>(c => c.Name == "TestConfig")), Times.Once);
        }

        [Fact]
        public void Update_UpdatesExistingConfiguration()
        {           
            var mockRepo = new Mock<IConfigurationRepository>();
            mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(new Configuration { Id = 1, Name = "OldConfig", Value = "OldValue" });

            var service = new ConfigurationService(mockRepo.Object);
            var updatedConfig = new Configuration { Id = 1, Name = "UpdatedConfig", Value = "UpdatedValue" };
            
            service.Update(updatedConfig);
           
            mockRepo.Verify(repo => repo.Update(It.Is<Configuration>(c => c.Name == "UpdatedConfig")), Times.Once);
        }

        [Fact]
        public void Delete_RemovesConfiguration()
        {          
            var mockRepo = new Mock<IConfigurationRepository>();
            mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(new Configuration { Id = 1, Name = "ConfigToDelete", Value = "DeleteMe" });

            var service = new ConfigurationService(mockRepo.Object);
 
            service.Delete(1);
           
            mockRepo.Verify(repo => repo.Remove(It.Is<Configuration>(c => c.Id == 1)), Times.Once);
        }
    }
}