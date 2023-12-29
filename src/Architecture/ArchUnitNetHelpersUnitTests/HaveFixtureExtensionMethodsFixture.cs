using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNetHelpers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNetHelpersUnitTests;

[TestFixture]
[Category("Unit")]
public class HaveFixtureExtensionMethodsFixture
{
    private Architecture Architecture { get; set; }

    [SetUp]
    public void SetUp()
    {
        Architecture = new ArchLoader().LoadAssemblies(
            typeof(HaveUnitTestExtensionMethodsFixture).Assembly
        ).Build();
    }

    [Test]
    public void HaveUnitTest_Should_Check_For_Correct_Services()
    {
        // arrange
        var classesThatShouldHaveFixtures = Classes()
            .That()
            .AreNestedIn(typeof(HaveFixtureExtensionMethodsFixture)).And()
            .DoNotHaveNameEndingWith("Fixture");

        var fixtureClasses = Classes()
            .That()
            .AreNestedIn(typeof(HaveFixtureExtensionMethodsFixture)).And()
            .HaveNameEndingWith("Fixture");

        // act
        var result = classesThatShouldHaveFixtures
            .Should()
            .HaveUnitTest()
            .Evaluate(Architecture);

        // assert
        Assert.That(
            result.Select(x => (((dynamic)x.EvaluatedObject).Name, x.Passed)),
            Is.EquivalentTo(new[]
            {
                ("ServiceClassWithUnitTests", true),
                ("ServiceClassWithoutUnitTests", false)
            })
        );
    }

    public class ServiceClassWithUnitTests
    {
        public void PublicServiceMethodWithTest() { }
    }

    public class ServiceClassWithUnitTestsFixture
    {
        public void PublicServiceMethodWithTest_Should() { }
    }

    public class ServiceClassWithoutUnitTests
    {
        public void PublicServiceMethodWithoutTest() { }
    }
}
