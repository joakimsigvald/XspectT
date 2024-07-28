﻿using XspecT.Test.AutoFixture;

namespace XspecT.Test.AutoMock;

public class WhenReturnsDefaultInt : Spec<MyDefaultService, int>
{
    [Fact]
    public void ThenReturnZero()
        => Given<IDefaultRetriever>().That(_ => _.GetInt()).ReturnsDefault()
        .When(_ => _.GetInt()).Then().Result.Is(0);
}

public class WhenReturnsDefaultModel : Spec<MyDefaultService, MyModel>
{
    [Fact]
    public void ThenReturnNull()
        => Given<IDefaultRetriever>().That(_ => _.GetModel()).ReturnsDefault()
        .When(_ => _.GetModel()).Then().Result.Is().Null();
}

public interface IDefaultRetriever
{
    int GetInt();
    object GetObject();
    MyModel GetModel();
}

public class MyDefaultService(IDefaultRetriever retriever)
{
    private readonly IDefaultRetriever _retriever = retriever;
    public int GetInt() => _retriever.GetInt();
    public object GetObject() => _retriever.GetObject();
    public MyModel GetModel() => _retriever.GetModel();
}