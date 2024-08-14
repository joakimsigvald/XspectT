﻿namespace XspecT.Assert.Numerical;

/// <summary>
/// Object that allows an assertions to be made on the provided sbyte
/// </summary>
public record IsSByte : IsNumerical<IsSByte, sbyte>
{
    internal IsSByte(sbyte actual, string actualExpr = null) : base(actual, actualExpr) { }
}