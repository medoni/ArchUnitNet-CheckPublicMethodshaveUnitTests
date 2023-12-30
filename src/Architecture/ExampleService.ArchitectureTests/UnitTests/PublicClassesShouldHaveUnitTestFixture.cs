using ArchUnitNET.Fluent;
using ArchUnitNET.NUnit;
using ArchUnitNetHelpers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ExampleService.ArchitectureTests.UnitTests;

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
