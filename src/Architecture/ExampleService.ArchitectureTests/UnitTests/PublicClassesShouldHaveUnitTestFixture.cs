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
        PublicMethodsShouldHaveUnitTestRule.Check(ExampleServiceArchitecture.Architecture);
    }

    private static readonly IArchRule PublicMethodsShouldHaveUnitTestRule =
        Classes()
            .That()
            .Are(ExampleServiceArchitecture.ClassesThatShouldBeTested)
            .As("Public Classes")
        .Should()
            .HaveUnitTest(ExampleServiceArchitecture.TestClasses);
}
