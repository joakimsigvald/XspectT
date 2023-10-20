﻿using XspecT.Fixture;
using XspecT.Verification;

namespace XspecT.Test.Verification;

public class WhenTimeSpan : StaticSpec<TimeSpan>
{
    [Fact] public void IsSame() => Given(A<TimeSpan>()).When(Echo).Then().Result.Is(The<TimeSpan>());
    [Fact]
    public void IsNot()
        => Given(A<TimeSpan>()).When(Echo).Then().Result.Is().Not(Another<TimeSpan>());
    [Fact]
    public void IsLessThanEtc()
        => Given(A<TimeSpan>()).When(Echo)
        .Then().Result.Is().LessThan(2 * The<TimeSpan>())
        .And.GreaterThan(The<TimeSpan>() / 2)
        .And.NotLessThan(The<TimeSpan>())
        .And.NotGreaterThan(The<TimeSpan>());

    private static TimeSpan Echo(TimeSpan ts) => ts;
}