# ArchUnit - Check that public methods have unit tests
This is an example to show how unit tests can be enforced for a given list of classes.

## How To
The following snippet shows how fixture classes can be enforced for public classes:
```csharp
using static ArchUnitNET.Fluent.ArchRuleDefinition;

[TestFixture]
[Category("Architecture")]
public class PublicClassesShouldHaveUnitTestFixture
{
    [Test]
    public void Verify()
    {
        ClassesThatShouldBeTestedRule.Check(ExampleServiceArchitecture.Architecture);
    }

    private static readonly IArchRule ClassesThatShouldBeTestedRule =
        Classes()
            .That()
            .Are(ExampleServiceArchitecture.ClassesThatShouldBeTested)
            .As("Classes that should be tested needs a Fixture class. See LINK_TO_WIKI.")
        .Should()
            .HaveFixtures();
}
```
See [PublicClassesShouldHaveUnitTestFixture.cs](https://github.com/medoni/ArchUnitNet-CheckPublicMethodshaveUnitTests/blob/main/src/Architecture/ExampleService.ArchitectureTests/UnitTests/PublicClassesShouldHaveUnitTestFixture.cs)

If a fixture is missing or misspelled, the test will fail with the following message:
```
Classes that should be tested needs a Fixture class. See LINK_TO_WIKI. should Have unit tests Classes that should be tested needs a Fixture class. See LINK_TO_WIKI." failed:
   ExampleService.WeatherForecast
```

---

The following snippet shows how unit tests methods can be enforced for each public method:
```csharp
using static ArchUnitNET.Fluent.ArchRuleDefinition;

[TestFixture]
[Category("Architecture")]
public class PublicMethodsShouldHaveUnitTestFixture
{
    [Test]
    public void Verify()
    {
        PublicMethodsShouldHaveUnitTestRule.Check(ExampleServiceArchitecture.Architecture);
    }

    private static readonly IArchRule PublicMethodsShouldHaveUnitTestRule =
        MethodMembers()
            .That()
            .ArePublic().And()
            .AreDeclaredIn(ExampleServiceArchitecture.ClassesThatShouldBeTested).And()
            .AreNoConstructors().And()
            .DoNotHaveNameStartingWith("get_").And()
            .DoNotHaveNameStartingWith("set_")
            .As("Public methods needs Unit Tests. See LINK_TO_WIKI.")
        .Should()
            .HaveUnitTests();
}
```
See [PublicMethodsShouldHaveUnitTestFixture.cs](https://github.com/medoni/ArchUnitNet-CheckPublicMethodshaveUnitTests/blob/main/src/Architecture/ExampleService.ArchitectureTests/UnitTests/PublicMethodsShouldHaveUnitTestFixture.cs)

If a fixture is missing or misspelled, the test will fail with the following message:
```
Public methods needs Unit Tests. See LINK_TO_WIKI. should Have unit tests Public methods needs Unit Tests. See LINK_TO_WIKI." failed:
   System.Collections.Generic.IEnumerable`1<ExampleService.WeatherForecast> ExampleService.Controllers.WeatherForecastController::Get()
```

The necessary architecture definition:
```csharp
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
```
See[ExampleServiceArchitecture.cs](https://github.com/medoni/ArchUnitNet-CheckPublicMethodshaveUnitTests/blob/main/src/Architecture/ExampleService.ArchitectureTests/ExampleServiceArchitecture.cs)

## Implementation
The following extensions methods have been added to the `.Should()` method:
- [HaveFixtures](https://github.com/medoni/ArchUnitNet-CheckPublicMethodshaveUnitTests/blob/main/src/Architecture/ArchUnitNetHelpers/HaveFixtureExtensionMethods.cs#L13)
- [HaveUnitTests](https://github.com/medoni/ArchUnitNet-CheckPublicMethodshaveUnitTests/blob/main/src/Architecture/ArchUnitNetHelpers/HaveUnitTestExtensionMethods.cs#L15)

Required packages:
- [ArchUnitNet](https://github.com/TNG/ArchUnitNET)

## Motivation

## Other
