﻿using static XspecT.Test.Helper;
namespace XspecT.Test.AutoMock;

public class WhenMockDateTime : Spec<StaticDateService, DateTime>
{
    public WhenMockDateTime() => When(_ => _.GetDate());
    public class GivenItWasNotProvided : WhenMockDateTime
    {
        [Fact]
        public void Then_It_Has_RandomDateTime()
        {
            Then().Result.Is().Not(A<DateTime>()).And(Result).Ticks.Is().Not(0);
            VerifyDescription(
                """
                When GetDate()
                Then Result is not a DateTime
                 and Result.Ticks is not 0
                """);
        }
    }

    public class GivenItWasProvided : WhenMockDateTime
    {
        [Fact]
        public void Then_It_Has_ProvidedValue()
        {
            Given(A<DateTime>()).Then().Result.Is(The<DateTime>());
            VerifyDescription(
                """
                Given a DateTime
                When GetDate()
                Then Result is the DateTime
                """);
        }
    }
}

public class StaticDateService(DateTime date)
{
    private readonly DateTime _date = date;
    public DateTime GetDate() => _date;
}