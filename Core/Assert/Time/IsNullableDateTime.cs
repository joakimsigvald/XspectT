﻿using FluentAssertions;

namespace XspecT.Assert.Time;

/// <summary>
/// Object that allows an assertions to be made on the provided nullable DateTime
/// </summary>
public record IsNullableDateTime : Constraint<IsNullableDateTime, DateTime?>
{
    internal IsNullableDateTime(DateTime? actual, string actualExpr = null) : base(actual, actualExpr, "is") { }

    /// <summary>
    /// Asserts that the dateTime is null or not equal to the given value
    /// </summary>
    /// <param name="unexpected"></param>
    /// <returns></returns>
    [CustomAssertion]
    public ContinueWith<IsNullableDateTime> Not(DateTime unexpected)
    {
        Actual.Should().NotBe(unexpected);
        return And();
    }

    /// <summary>
    /// Asserts that the dateTime is not null
    /// </summary>
    /// <returns></returns>
    [CustomAssertion]
    public ContinueWith<IsDateTime> NotNull()
    {
        Actual.Should().NotBeNull();
        return new(new(Actual.Value));
    }

    /// <summary>
    /// Asserts that the dateTime is null
    /// </summary>
    [CustomAssertion] public void Null() => Actual.Should().BeNull();
}