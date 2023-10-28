﻿using XspecT.Fixture;
using XspecT.Verification;

namespace XspecT.Test.AutoMock;

public class WhenMockTuple : SubjectSpec<StaticTupleService, (int, string)>
{
    public WhenMockTuple() => Given(A<(int, string)>).When(_ => _.GetValue());
    public class UsingAValue : WhenMockTuple
    {
        [Fact] public void Then_It_Has_TheValue() => Then().Result.Is(The<(int, string)>());
    }

    public class GivenItWasProvided : WhenMockTuple
    {
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "")]
        [InlineData(2, "hej")]
        public void Then_It_Has_ProvidedValue(int v1, string v2)
            => Given((v1, v2)).Then().Result.Is((v1, v2));
    }
}

public class StaticTupleService
{
    private readonly (int, string) _value;
    public StaticTupleService((int, string) value) => _value = value;
    public (int, string) GetValue() => _value;
}