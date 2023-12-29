using ExampleService;

namespace ExampleServiceUnitTests;

[TestFixture]
[Category("Unit")]
public class WeatherForecastFixture
{
    private WeatherForecast Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new WeatherForecast();
    }
}
