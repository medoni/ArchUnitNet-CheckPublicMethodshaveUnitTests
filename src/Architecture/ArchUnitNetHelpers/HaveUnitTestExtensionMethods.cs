using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using System.Text.RegularExpressions;

namespace ArchUnitNetHelpers;

public static class HaveUnitTestExtensionMethods
{
    /// <summary>
    /// Checks that the given methods have public unit tests
    /// </summary>
    public static IArchRule HaveUnitTests(
        this MethodMembersShould should,
        string classFixtureName = "Fixture"
    )
    {
        var description = "Have unit tests " + should.Description;
        return should.FollowCustomCondition(
            new ArchitectureCondition<MethodMember>(Condition, description)
        );

        IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methods, Architecture architecture)
        {
            foreach (var method in methods)
            {
                var result = GetMethodHasUnitTestResult(
                    architecture,
                    method,
                    classFixtureName
                );
                if (result is null) continue;

                yield return result;
            }
        }
    }

    private static ConditionResult? GetMethodHasUnitTestResult(
        Architecture architecture,
        MethodMember method,
        string classFixtureName
    )
    {
        if (method.DeclaringType is null) return null;

        var fixtureType = HaveFixtureExtensionMethods.GetFixtureTypeFromClass(architecture, method.DeclaringType, classFixtureName);
        if (fixtureType is null)
        {
            return new ConditionResult(method, false, $"Class '{method.DeclaringType.FullName}' has no corresponding '{method.DeclaringType}{classFixtureName}' class.");
        }

        if (!FixtureHasTestMethod(fixtureType, method))
        {
            return new ConditionResult(method, false);
        }

        return new ConditionResult(method, true);
    }

    private static bool FixtureHasTestMethod(
        IType fixtureType,
        MethodMember method
    )
    {
        var methodName = Regex.Match(method.Name, @"^\.?(\w+)\W").Groups[1].Value;

        return fixtureType
            .Members.OfType<MethodMember>()
            .Any(x => x.NameMatches(@$"^{methodName}(_|$)", true));
    }
}
