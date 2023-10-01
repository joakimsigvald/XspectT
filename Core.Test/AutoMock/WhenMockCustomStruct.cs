﻿using XspecT.Fixture;
using XspecT.Verification;

namespace XspecT.Test.AutoMock;

public class WhenMockCustomStruct : SubjectSpec<StaticValueService, MyValue<int>>
{
    public WhenMockCustomStruct() => When(_ => _.GetValue());
    public class GivenItWasNotProvided : WhenMockCustomStruct
    {
        [Fact] public void Then_It_Has_FirstCustomStruct() => Then().Result.Is(A<MyValue<int>>());
    }

    public class GivenItWasProvided : WhenMockCustomStruct
    {
        [Fact]
        public void Then_It_Has_ProvidedValue()
            => Given(A<MyValue<int>>()).Then().Result.Is(The<MyValue<int>>());
    }
}

public class StaticValueService
{
    private readonly MyValue<int> _value;
    public StaticValueService(MyValue<int> value) => _value = value;
    public MyValue<int> GetValue() => _value;
}

public struct MyValue<TValue> where TValue : struct
{ 
    public TValue Value { get; set; }
}