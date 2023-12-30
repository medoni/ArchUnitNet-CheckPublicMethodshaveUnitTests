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

    public static IObjectProvider<Class> DomainLayer { get; }
    public static IObjectProvider<Class> PersistenceLayer { get; }
    public static IObjectProvider<Class> CommandHandler { get; }
    public static IObjectProvider<Class> EventHandler { get; }

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

        DomainLayer =
            Classes()
            .That()
            .Are(PublicClasses).And()
            .ResideInNamespace(@"\.Domain\.", true)
            .As("Domain Layer");

        PersistenceLayer = Classes()
            .That()
            .Are(PublicClasses).And()
            .ResideInNamespace(@"\.Persistence\.", true)
            .As("Persistence Layer");

        CommandHandler = Classes()
            .That()
            .Are(PublicClasses).And()
            .ImplementInterface("ICommandHandler", true)
            .As("Command handler");

        EventHandler = Classes()
            .That()
            .Are(PublicClasses).And()
            .ImplementInterface("IEventHandler", true)
            .As("Event handler");

        ClassesThatShouldBeTested =
            Classes()
            .That()
            .Are(DomainLayer).Or()
            .Are(PersistenceLayer).Or()
            .Are(CommandHandler).Or()
            .Are(EventHandler)
            .As("Public classes that should be tested");
    }
}
