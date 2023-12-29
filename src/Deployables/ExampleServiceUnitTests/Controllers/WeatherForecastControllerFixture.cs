using ExampleService.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExampleServiceUnitTests.Controllers;

[TestFixture]
[Category("Unit")]
public class WeatherForecastControllerFixture
{
    private WeatherForecastController Sut { get; set; }

    private Mock<ILogger<WeatherForecastController>> LoggerMock { get; set; }

    [SetUp]
    public void SetUp()
    {
        LoggerMock = new Mock<ILogger<WeatherForecastController>>();

        Sut = new WeatherForecastController(
            LoggerMock.Object
        );
    }

    [Test]
    public void Get_Should_Contain_Correct_Forecasts()
    {
        // act
        var result = Sut.Get();

        // assert
        Assert.That(result, Is.Not.Empty);
    }
}
