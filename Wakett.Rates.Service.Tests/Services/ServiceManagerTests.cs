using Moq;
using System;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;
using Wakett.Rates.Service.Core.Services;
using Wakett.Rates.Service.Infrastructure.Repositories;
using Xunit;

namespace MyScheduledTaskService.Tests.Services
{
    public class ServiceManagerTests
    {
        private readonly Mock<IConfigurationRepository> _mockConfigRepo;
        private readonly Mock<ILogRepository> _mockLogRepo;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<ICryptocurrencyRepository> _mockCryptoCurrencyRepo;
        private readonly ServiceManager _serviceManager;

        public ServiceManagerTests()
        {
            _mockConfigRepo = new Mock<IConfigurationRepository>();
            _mockLogRepo = new Mock<ILogRepository>();
            _mockApiService = new Mock<IApiService>();
            _mockCryptoCurrencyRepo = new Mock<ICryptocurrencyRepository>();

            _serviceManager = new ServiceManager(_mockConfigRepo.Object, _mockLogRepo.Object, _mockApiService.Object, _mockCryptoCurrencyRepo.Object);
        }

        [Fact]
        public void ExecuteTasks_ShouldLogError_WhenNoConfigurationFound()
        {
            // Arrange
            _mockConfigRepo.Setup(repo => repo.GetTaskConfiguration()).Returns((TaskConfiguration)null);

            // Act
            _serviceManager.ExecuteTasks();

            // Assert
            _mockLogRepo.Verify(log => log.Log(It.IsAny<int>(), "No task configuration found.", "Error"), Times.Once);
        }

        [Fact]
        public void ExecuteTasks_ShouldExecuteTask_WhenDue()
        {
            // Arrange
            var config = new TaskConfiguration { TaskId = 2, TickTime = 3000 };
            _mockConfigRepo.Setup(repo => repo.GetTaskConfiguration()).Returns(config);
            //TODO
            _mockApiService.Setup(s => s.GetLatestCryptoCurrencyAsync());

            // Act
            _serviceManager.ExecuteTasks();

            // Assert
            _mockApiService.Verify(api => api.GetLatestCryptoCurrencyAsync(), Times.Once);
            _mockConfigRepo.Verify(repo => repo.UpdateLastRunTime(config.TaskId, It.IsAny<DateTime>()), Times.Once);
            _mockLogRepo.Verify(log => log.Log(config.TaskId, "Task executed successfully.", "Info"), Times.Once);
        }

        [Fact]
        public void ExecuteTasks_ShouldNotExecuteTask_WhenNotDue()
        {
            // Arrange
            var config = new TaskConfiguration { TaskId = 1, TickTime = 10, LastRunTime = DateTime.Now };
            _mockConfigRepo.Setup(repo => repo.GetTaskConfiguration()).Returns(config);

            // Act
            _serviceManager.ExecuteTasks();

            // Assert
            _mockApiService.Verify(api => api.GetLatestCryptoCurrencyAsync(), Times.Never);
            _mockLogRepo.Verify(log => log.Log(It.IsAny<int>(), "Task executed successfully.", "Info"), Times.Never);
        }
    }
}
