﻿using FluentAssertions;
using System.Linq.Expressions;

namespace XspecT.Assert;

/// <summary>
/// Object that allows an assertions to be made on the provided enumerable
/// </summary>
public record HasEnumerable<TItem> : Constraint<HasEnumerable<TItem>, IEnumerable<TItem>>
{
    internal HasEnumerable(IEnumerable<TItem> actual, string actualExpr) : base(actual, actualExpr, "has") { }

    /// <summary>
    /// actual.Should().ContainSingle()
    /// </summary>
    [CustomAssertion]
    public ContinueWith<HasEnumerable<TItem>> Single()
    {
        Actual.Should().ContainSingle();
        return And();
    }

    /// <summary>
    /// actual.Should().ContainSingle()
    /// </summary>
    [CustomAssertion]
    public ContinueWith<HasEnumerable<TItem>> Single(Expression<Func<TItem, bool>> predicate)
    {
        Actual.Should().ContainSingle(predicate);
        return And();
    }

    /// <summary>
    /// actual.Should().HaveCount(expected)
    /// </summary>
    public ContinueWith<HasEnumerable<TItem>> Count(int expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string expectedExpr = null)
    {
        AddAssert([CustomAssertion] () => Actual.Should().HaveCount(expected), expectedExpr);
        Actual.Should().HaveCount(expected);
        return And();
    }

    /// <summary>
    /// collection.Select((it, i) => (it, i)).Should().OnlyContain(t => predicate(t.it, t.i))
    /// </summary>
    [CustomAssertion]
    public ContinueWith<HasEnumerable<TItem>> All(
        Func<TItem, int, bool> predicate, string because = "", params object[] becauseArgs)
    {
        Actual.Select((it, i) => (it, i)).Should().OnlyContain(t => predicate(t.it, t.i), because, becauseArgs);
        return And();
    }

    /// <summary>
    /// collection.Select((it, i) => (it, i)).Should().OnlyContain(t => predicate(t.it, t.i))
    /// </summary>
    [CustomAssertion]
    public ContinueWith<HasEnumerable<TItem>> All(
        Func<TItem, bool> predicate, string because = "", params object[] becauseArgs)
    {
        Actual.Should().OnlyContain(it => predicate(it), because, becauseArgs);
        return And();
    }

    /// <summary>
    /// Applies the given assertion to all element of the enumerable
    /// </summary>
    /// <param name="assert"></param>
    /// <returns></returns>
    public ContinueWith<HasEnumerable<TItem>> All(Action<TItem, int> assert)
    {
        Actual.Select((it, i) => (it, i)).ToList().ForEach(t => assert(t.it, t.i));
        return And();
    }

    /// <summary>
    /// Applies the given assertion to all element of the enumerable
    /// </summary>
    /// <param name="assert"></param>
    /// <returns></returns>
    public ContinueWith<HasEnumerable<TItem>> All(Action<TItem> assert)
    {
        Actual.ToList().ForEach(assert);
        return And();
    }
}