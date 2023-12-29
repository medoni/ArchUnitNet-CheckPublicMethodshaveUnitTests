using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNetHelpers;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNetHelpersUnitTests;

[TestFixture]
[Category("Unit")]
public class HaveUnitTestExtensionMethodsFixture
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
        var testClasses = Classes()
            .That()
            .AreNestedIn(typeof(HaveUnitTestExtensionMethodsFixture));

        var fixtureClasses = Classes()
            .That()
            .HaveNameEndingWith("Fixture");

        var methodsThatShouldHaveTests = MethodMembers()
            .That()
            .ArePublic().And()
            .AreDeclaredIn(testClasses).And()
            .AreNotDeclaredIn(fixtureClasses).And()
            .AreNoConstructors()
            .As("Public methods");

        // act
        var result = methodsThatShouldHaveTests
            .Should()
            .HaveUnitTests()
            .Evaluate(Architecture);

        // assert
        Assert.That(
            result.Select(x => (((dynamic)x.EvaluatedObject).Name, x.Passed)),
            Is.EquivalentTo(new[]
            {
                ("PublicServiceMethodWithTest()", true),
                ("PublicServiceMethodWithoutTest()", false)
            })
        );
    }

    public class ServiceClassWithUnitTests
    {
        public void PublicServiceMethodWithTest() { }
        public void PublicServiceMethodWithoutTest() { }
        private void PrivateServiceMethod() { }
    }

    public class ServiceClassWithUnitTestsFixture
    {
        public void PublicServiceMethodWithTest_Should() { }
    }
}
