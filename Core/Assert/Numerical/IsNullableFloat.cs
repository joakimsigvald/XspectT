﻿using FluentAssertions;

namespace XspecT.Assert.Numerical;

/// <summary>
/// Object that allows an assertions to be made on the provided nullable float
/// </summary>
public record IsNullableFloat : IsNullableNumerical<float, IsNullableFloat>
{
    internal IsNullableFloat(float? actual) : base(actual) { }
    [CustomAssertion] internal override FluentAssertions.Numeric.NullableNumericAssertions<float> Should() => Actual.Should();
}