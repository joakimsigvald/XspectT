﻿namespace XspecT.Test.AutoFixture;

public class WhenMany : Spec<MyRetriever, MyModel[]>
{
    public WhenMany() => When(_ => _.List());

    public class GivenReferringManyTwice : WhenMany
    {
        public GivenReferringManyTwice() => Given(Many<MyModel>());

        [Fact]
        public void ThenCanRetrieveThatArray()
        {
            Result.Is(Many<MyModel>());
            VerifyDescription(
@"Given many MyModel
When List()
Then Result is many MyModel");
        }

        [Fact]
        public void ThenArrayHasThreeElements()
        {
            Result.Has().Count(3);
            VerifyDescription(
@"Given many MyModel
When List()
Then Result has count 3");
        }

        [Fact]
        public void ThenDifferentReferencesToMany_AreTheSameArray()
        {
            Then(Many<MyModel>()).Is(Many<MyModel>());
            VerifyDescription(
@"Given many MyModel
When List()
Then many MyModel is many MyModel");
        }
    }

    public class GivenReferringManyOfHigherCountSecondTime : WhenMany
    {
        public GivenReferringManyOfHigherCountSecondTime() => Given(Two<MyModel>());

        [Fact]
        public void ThenItIsDifferentFromFirst()
        {
            Result.Is().Not(Three<MyModel>());
            VerifyDescription(
@"Given two MyModel
When List()
Then Result is not three MyModel");
        }

        [Fact]
        public void ThenArrayHasOriginalCount()
        {
            Result.Has().Count(2);
            VerifyDescription(
@"Given two MyModel
When List()
Then Result has count 2");
        }

        [Fact]
        public void ThenLastElementIsCreated()
        {
            Then(TheThird<MyModel>()).Is(Three<MyModel>().Last());
            VerifyDescription(
@"Given two MyModel
When List()
Then the third MyModel is Last() of three MyModel");
        }

        [Fact]
        public void ThenDifferentReferencesToManyOfSameCount_HaveSameElements()
        {
            Then(Three<MyModel>()).Is().EqualTo(Three<MyModel>());
            VerifyDescription(
@"Given two MyModel
When List()
Then three MyModel is equal to three MyModel");
        }
    }

    public class GivenReferringManyOfLowerCountSecondTime : WhenMany
    {
        public GivenReferringManyOfLowerCountSecondTime() => Given(Four<MyModel>());

        [Fact]
        public void ThenItIsDifferentFromFirst()
        {
            Result.Is().Not(Three<MyModel>());
            VerifyDescription(
@"Given four MyModel
When List()
Then Result is not three MyModel");
        }

        [Fact]
        public void ThenArrayHasOriginalCount()
        {
            Result.Has().Count(4);
            VerifyDescription(
@"Given four MyModel
When List()
Then Result has count 4");
        }

        [Fact]
        public void ThenDifferentReferencesToManyOfSameCount_HaveSameElements()
        {
            Then(Three<MyModel>()).Is().EqualTo(Three<MyModel>());
            VerifyDescription(
@"Given four MyModel
When List()
Then three MyModel is equal to three MyModel");
        }
    }

    public class GivenMentionManyAfterTwo : WhenMany
    {
        [Fact]
        public void ThenReturnTwoAsMany()
        {
            Given<IMyRepository>().That(_ => _.List()).Returns(Many<MyModel>)
                    .And(Two<MyModel>).Then().Result.Has().Count(2);
            VerifyDescription(
@"Given two MyModel
 and IMyRepository.List() returns many MyModel
When List()
Then Result has count 2");
        }
    }

    public class GivenMentionManyAfterFour : WhenMany
    {
        [Fact]
        public void ThenReturnFourAsMany()
        {
            Given<IMyRepository>().That(_ => _.List()).Returns(() => Many<MyModel>())
                    .And(Four<MyModel>).Then().Result.Has().Count(4);
            VerifyDescription(
@"Given four MyModel
 and IMyRepository.List() returns many MyModel
When List()
Then Result has count 4");
        }
    }

    public class GivenMentionManyAfterOne : WhenMany
    {
        [Fact]
        public void ThenReturnThreeAsMany()
        {
            Given<IMyRepository>().That(_ => _.List()).Returns(Many<MyModel>)
                    .And(One<MyModel>).Then().Result.Has().Count(3);
            VerifyDescription(
@"Given one MyModel
 and IMyRepository.List() returns many MyModel
When List()
Then Result has count 3");
        }
    }
}

public class WhenMockReturnsFewerElementsThanPreviouslyMentioned : Spec<MyRetriever, MyModel[]>
{
    public WhenMockReturnsFewerElementsThanPreviouslyMentioned()
        => When(_ => _.Create(An<int>()));

    [Fact]
    public void ThenItIsDifferentFromFirst()
    {
        Given(3)
            .And<IMyRepository>().That(_ => _.Create(Three<MyModel>().Length))
            .Returns(Two<MyModel>)
            .Then().Result.Has().Count(2);
        VerifyDescription(
@"Given 3
 and IMyRepository.Create(Length of three MyModel) returns two MyModel
When Create(an int)
Then Result has count 2");
    }
}