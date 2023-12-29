using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ExampleServiceUnitTests.Controllers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ExampleService.ArchitectureTests;

internal static class ExampleServiceArchitecture
{
    public static Architecture Architecture { get; }
    public static IObjectProvider<Class> PublicClasses { get; }
    public static IObjectProvider<Class> ClassesThatShouldBeTested { get; }

    public static IObjectProvider<Class> TestClasses { get; }

    static ExampleServiceArchitecture()
    {
        Architecture =
            new ArchLoader().LoadAssemblies(
                typeof(Program).Assembly,
                typeof(WeatherForecastControllerFixture).Assembly
            ).Build();

        TestClasses =
            Classes()
            .That()
            .ResideInAssembly(@"UnitTests\W", true)
            .As("Test Classes");

        PublicClasses =
            Classes()
            .That()
            .AreNot(TestClasses).And()
            .ArePublic().And()
            .AreNot(TestClasses)
            .As("Public Classes");

        ClassesThatShouldBeTested =
            Classes()
            .That()
            .Are(PublicClasses).And()
            .DoNotHaveName("Program").And()
            .DoNotImplementInterface(@"Swashbuckle\.AspNetCore\.Filters\.IExamplesProvider\W", true)
            .As("Public classes that should be tested");
    }
}
