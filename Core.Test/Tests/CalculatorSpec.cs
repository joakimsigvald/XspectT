﻿using XspecT.Fixture;

using static XspecT.Test.Subjects.Calculator;
using XspecT.Verification;

namespace XspecT.Test.Tests;

public class CalculatorSpec : StaticSpec<int>
{
    [Fact] public void WhenAddZeros_ThenSumIsZero() => When<int, int>(Add).Then().Result.Is(0);

    [Fact] public void WhenAdd_1_and_2_ThenSumIs_3() => Given(1, 2).When(Add).Then().Result.Is(3);

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(3, 4, 7)]
    public void WhenAdd_ThenReturnSum(int x, int y, int sum)
        => When<int, int>(Add).Given(x, y).Then().Result.Is(sum);

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(3, 4, 12)]
    public void WhenMultiply_ThenReturnProduct(int x, int y, int product)
        => When(() => Multiply(x, y)).Then().Result.Is(product);
}

public abstract class WhenAdd : StaticSpec<int>
{
    public WhenAdd() => When<int, int>(Add);

    public class Given_1_1 : WhenAdd
    {
        public Given_1_1() => Given(1, 1);
        [Fact] public void Then_Return_2() => Result.Is(2);
        [Fact] public void Then_Return_Between_1_And_3() => Result.Is().GreaterThan(1).And.BeLessThan(3);
    }

    public class Given_2_3 : WhenAdd
    {
        public Given_2_3() => Given(2, 3);
        [Fact] public void Then_Return_5() => Result.Is(5);
    }
}