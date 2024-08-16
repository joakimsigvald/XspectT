﻿namespace XspecT.Test.Given;

public class WhenGivenZipCode : Spec<MyValueTypeModel, MyZipCode>
{
    [Fact]
    public void GivenAutoGeneratedPrimitiveIsRestrictedInt_ThenValueIsUsed()
    {
        Given(A<MyZipCode>).When(_ => _.ZipCode)
            .Then().Result.Is(The<MyZipCode>())
            .And(Result).Primitive.Is().NotLessThan(0).And.LessThan(100_000);
        Specification.Is(
            """
            Given a MyZipCode
            When _.ZipCode
            Then Result is the MyZipCode
              and Result's Primitive is not less than 0
                and less than 100_000
            """);
    }
}