﻿namespace XspecT.Assert.Numerical;

/// <summary>
/// Object that allows an assertions to be made on the provided long
/// </summary>
public record IsLong : IsNumerical<IsLong, long>
{
    internal IsLong(long actual, string actualExpr = null) : base(actual, actualExpr) { }
}