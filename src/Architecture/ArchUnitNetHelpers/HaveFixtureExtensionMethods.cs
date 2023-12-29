using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;

namespace ArchUnitNetHelpers;
public static class HaveFixtureExtensionMethods
{
    /// <summary>
    /// Checks that the given classes have corresponding fixture class
    /// </summary>
    public static IArchRule HaveFixtures(
        this ClassesShould should,
        string classFixtureName = "Fixture"
    )
    {
        var description = "Have unit tests " + should.Description;
        return should.FollowCustomCondition(
            new ArchitectureCondition<Class>(Condition, description)
        );

        IEnumerable<ConditionResult> Condition(IEnumerable<Class> classes, Architecture architecture)
        {
            foreach (var classItem in classes)
            {
                var result = GetClassHasFixtureResult(
                    architecture,
                    classItem,
                    classFixtureName
                );
                if (result is null) continue;

                yield return result;
            }
        }
    }

    private static ConditionResult? GetClassHasFixtureResult(
        Architecture architecture,
        Class classItem,
        string classFixtureName
    )
    {
        var fixtureType = GetFixtureTypeFromClass(architecture, classItem, classFixtureName);
        if (fixtureType is null)
        {
            return new ConditionResult(classItem, false, $"Class '{classItem.FullName}' has no corresponding '{classItem.Name}{classFixtureName}' class.");
        }

        return new ConditionResult(classItem, true);
    }

    internal static IType? GetFixtureTypeFromClass(
        Architecture architecture,
        IType type,
        string classFixtureName
    )
    {
        var matchingName = type.Name + classFixtureName;

        return architecture.Classes
            .FirstOrDefault(x => x.NameMatches(matchingName));
    }
}
