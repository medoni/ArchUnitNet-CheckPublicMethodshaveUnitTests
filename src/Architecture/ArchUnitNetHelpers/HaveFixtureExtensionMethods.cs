﻿using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;

namespace ArchUnitNetHelpers;
public static class HaveFixtureExtensionMethods
{
    /// <summary>
    /// Checks that the given methods have public unit tests
    /// </summary>
    public static IArchRule HaveUnitTest(
        this ClassesShould should,
        IObjectProvider<Class> unitTestClasses,
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
                    unitTestClasses,
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
        IObjectProvider<Class> unitTestClasses,
        Class classItem,
        string classFixtureName
    )
    {
        var fixtureType = GetFixtureTypeFromClass(architecture, classItem, unitTestClasses, classFixtureName);
        if (fixtureType is null)
        {
            return new ConditionResult(classItem, false, $"Class '{classItem.FullName}' has no corresponding '{classItem.Name}{classFixtureName}' class.");
        }

        return new ConditionResult(classItem, true);
    }

    internal static IType? GetFixtureTypeFromClass(
        Architecture architecture,
        IType type,
        IObjectProvider<Class> unitTestClasses,
        string classFixtureName
    )
    {
        var matchingName = type.Name + classFixtureName;

        return unitTestClasses.GetObjects(architecture)
            .FirstOrDefault(x => x.NameMatches(matchingName));
    }
}