﻿using XspecT.Test.Subjects.RecordStructDefaults;

namespace XspecT.Test.Tests.KeyService;

public class KeyServiceSpec : Spec<Subjects.RecordStructDefaults.KeyService, Key>
{
}

public class WhenKeyKey : KeyServiceSpec
{
    public WhenKeyKey() => When(_ => _.GetKey());

    [Fact]
    public void ThenGetsKey()
    {
        Result.A.Is().NotNullOrEmpty();
        Specification.Is(
            """
            When add 1, 1
            Then Result is 2
            """);
    }
}