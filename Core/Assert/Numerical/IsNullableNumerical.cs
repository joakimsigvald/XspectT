﻿namespace XspecT.Assert.Numerical;

/// <summary>
/// base class that allows an assertions to be made on the provided nullable numerical
/// </summary>
/// <typeparam name="TActual"></typeparam>
/// <typeparam name="TContinuation"></typeparam>
/// <typeparam name="TValueContinuation"></typeparam>
public abstract record IsNullableNumerical<TActual, TContinuation, TValueContinuation>
    : IsNullableComparableStruct<TActual, TContinuation, TValueContinuation>
    where TActual : struct, IComparable<TActual>
    where TContinuation : IsNullableNumerical<TActual, TContinuation, TValueContinuation>
    where TValueContinuation : IsNumerical<TActual, TValueContinuation>, new();