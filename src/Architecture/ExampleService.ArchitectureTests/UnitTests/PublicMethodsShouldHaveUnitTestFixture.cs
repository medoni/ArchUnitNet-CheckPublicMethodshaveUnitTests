using ArchUnitNET.Fluent;
using ArchUnitNET.NUnit;
using ArchUnitNetHelpers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ExampleService.ArchitectureTests.UnitTests;

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
            .As("Public methods")
        .Should()
            .HaveUnitTest();
}
