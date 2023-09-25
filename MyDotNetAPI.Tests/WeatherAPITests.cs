using Microsoft.Extensions.Logging;
using Moq;
using MyDotNetAPI.Controllers;

namespace MyDotNetAPI.Tests;

public class WeatherApiTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGetRequest()
    {
        var mockLogger = Mock.Of<ILogger<WeatherForecastController>>();
        // Arrange
        var controller = new WeatherForecastController(mockLogger);
        
        // Act
        var controllerResult = controller.Get();
        
        // Assert
        const int expectedNumOfResults = 5;
        Assert.That(controllerResult.Count(), Is.EqualTo(expectedNumOfResults), "Exactly 5 results should be returned");
    }
}
