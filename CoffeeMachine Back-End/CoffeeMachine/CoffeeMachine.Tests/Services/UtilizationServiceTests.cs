using CoffeeMachine.Core.Interfaces;
using CoffeeMachine.Core.Models;
using CoffeeMachine.Services;
using Moq;

namespace CoffeeMachine.Tests.Services
{
    public class UtilizationServiceTests
    {
        private Mock<ICoffeeActionLogService> _mockLogUtilityService;
        private UtilizationService _utilizationServiceMoq;

        [SetUp]
        public void Setup()
        {
            _mockLogUtilityService = new Mock<ICoffeeActionLogService>();
            _utilizationServiceMoq = new UtilizationService(_mockLogUtilityService.Object);
        }

        [Test]
        public async Task GetFirstAndLastCupTimesPerDayOfWeekAsync_ShouldReturnCorrectData()
        {
            // Given
            var logs = new List<CoffeeActionLogDto>
            {
                new CoffeeActionLogDto { Timestamp = new DateTime(2025,3, 3, 8, 0, 0), ActionTypeId = 3 }, // Monday
                new CoffeeActionLogDto { Timestamp = new DateTime(2025,3, 3, 14, 0, 0), ActionTypeId = 3 }, // Monday
                new CoffeeActionLogDto { Timestamp = new DateTime(2025,3, 3, 10, 0, 0), ActionTypeId = 1 }, // Monday, non-coffee
                new CoffeeActionLogDto { Timestamp = new DateTime(2025,3, 4, 10, 0, 0), ActionTypeId = 3 }, //Tuesday
            };

            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(logs);

            // When
            var result = await _utilizationServiceMoq.GetFirstAndLastCupTimesPerDayOfWeekAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2)); // 2x Monday & Tuesday
            Assert.That(result["Monday"], Is.EqualTo("First Cup: 08:00:00, Last Cup: 14:00:00"));
            Assert.That(result["Tuesday"], Is.EqualTo("First Cup: 10:00:00, Last Cup: 10:00:00"));

        }

        [Test]
        public async Task GetAverageCupsPerHourAsync_ShouldReturnCorrectData()
        {
            // Given
            var logs = new List<CoffeeActionLogDto>
            {
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 8, 0, 0), ActionTypeId = 3 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 8, 30, 0), ActionTypeId = 3 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 8, 15, 0), ActionTypeId = 3 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 9, 0, 0), ActionTypeId = 3 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 10, 0, 0), ActionTypeId = 3 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 10, 30, 0), ActionTypeId = 3 }
            };

            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(logs);

            // When
            var result = await _utilizationServiceMoq.GetAverageCupsPerHourAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(0.5, result["08:00-08:59"]);
            Assert.AreEqual(0.17, result["09:00-09:59"], 0.01);
            Assert.AreEqual(0.33, result["10:00-10:59"], 0.01);
        }

        [Test]
        public async Task GetAverageCupsPerHourAsync_ShouldHandleEmptyLogs()
        {
            // Given
            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(new List<CoffeeActionLogDto>());

            // When
            var result = await _utilizationServiceMoq.GetAverageCupsPerHourAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetFirstAndLastCupTimesPerDayOfWeekAsync_ShouldHandleEmptyLogs()
        {
            // Given
            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(new List<CoffeeActionLogDto>());

            // When
            var result = await _utilizationServiceMoq.GetFirstAndLastCupTimesPerDayOfWeekAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetFirstAndLastCupTimesPerDayOfWeekAsync_ShouldHandleNoCoffeeLogs()
        {
            // Given
            var logs = new List<CoffeeActionLogDto>
            {
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 8, 0, 0), ActionTypeId = 1 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 4, 9, 0, 0), ActionTypeId = 2 }
            };

            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(logs);

            // When
            var result = await _utilizationServiceMoq.GetFirstAndLastCupTimesPerDayOfWeekAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAverageCupsPerHourAsync_ShouldHandleNoCoffeeLogs()
        {
            // Given
            var logs = new List<CoffeeActionLogDto>
            {
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 3, 8, 0, 0), ActionTypeId = 1 },
                new CoffeeActionLogDto { Timestamp = new DateTime(2025, 3, 4, 9, 0, 0), ActionTypeId = 2 }
            };

            _mockLogUtilityService.Setup(service => service.GetCoffeeActionLogsAsync())
                .ReturnsAsync(logs);

            // When
            var result = await _utilizationServiceMoq.GetAverageCupsPerHourAsync();

            // Then
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}