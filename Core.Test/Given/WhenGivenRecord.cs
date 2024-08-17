﻿using XspecT.Assert;

namespace XspecT.Test.Given;

public record MyRecord(int Id, string Name);

public class WhenGivenRecord : Spec<MyService, MyRecord>
{
    [Fact]
    public void GivenSetup_ThenReturnSetupValue()
    {
        Given<MyRecord>(_ => _ with { Name = A<string>() })
            .When(_ => MyService.Echo(The<MyRecord>()))
            .Then().Result.Name.Is(The<string>());
        Specification.Is(
            """
            Given MyRecord { Name = a string }
            When MyService.Echo(the MyRecord)
            Then Result.Name is the string
            """);
    }

    [Fact]
    public void GivenTwoSetup_ThenReturnSecondSetupAppliedToFirstSetup()
    {
        Given<MyRecord>(_ => _ with { Name = A<string>(), Id = 1 })
            .And<MyRecord>(_ => _ with { Name = _.Name + ASecond<string>() })
            .When(_ => MyService.Echo(The<MyRecord>()))
            .Then().Result.Name.Does().StartWith(The<string>())
            .And.EndWith(TheSecond<string>())
            ;
        Specification.Is(
            """
            Given MyRecord { Name = a string, Id = 1 }
              and MyRecord { Name = _.Name + ASecond<string>() }
            When MyService.Echo(the MyRecord)
            Then Result.Name starts with the string
                and ends with the second string
            """);
    }

    [Fact]
    public void GivenThatSetup_ThenReturnSetupValue()
    {
        Given().A<MyRecord>(_ => _ with { Name = A<string>() })
            .When(_ => MyService.Echo(The<MyRecord>()))
            .Then().Result.Name.Is(The<string>());
        Specification.Is(
            """
            Given a MyRecord { Name = a string }
            When MyService.Echo(the MyRecord)
            Then Result.Name is the string
            """);
    }

    [Fact]
    public void GivenTwoThatSetup_ThenReturnFirstSetupAppliedToSecondSetup()
    {
        Given().A<MyRecord>(_ => _ with { Name = _.Name + ASecond<string>() })
            .And().A<MyRecord>(_ => _ with { Name = A<string>() })
            .When(_ => MyService.Echo(The<MyRecord>()))
            .Then().Result.Name.Does().StartWith(The<string>()).And.EndWith(TheSecond<string>());
        Specification.Is(
            """
            Given a MyRecord { Name = a string }
              and a MyRecord { Name = _.Name + ASecond<string>() }
            When MyService.Echo(the MyRecord)
            Then Result.Name starts with the string
                and ends with the second string
            """);
    }
}