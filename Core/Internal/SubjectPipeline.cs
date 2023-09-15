﻿using Moq;
using XspecT.Fixture.Exceptions;

namespace XspecT.Internal;

internal class SubjectPipeline<TSUT, TResult> : Pipeline<TResult>
    where TSUT : class
{
    private readonly Arranger _arranger = new();
    public TSUT SUT { get; private set; }

    protected override void Arrange()
    {
        _arranger.Arrange();
        SUT = CreateInstance<TSUT>();
    }

    internal TValue CreateInstance<TValue>() where TValue : class
            => _context.CreateInstance<TValue>();

    internal Mock<TObject> GetMock<TObject>() where TObject : class => _context.GetMock<TObject>();

    internal void Use<TService>(TService service)
    {
        var type = typeof(TService);
        _context.Use(typeof(TService), service);
        if (typeof(Task).IsAssignableFrom(type))
            return;
        if (typeof(Mock).IsAssignableFrom(type))
            return;
        Use(Task.FromResult(service));
    }

    internal void GivenThat(Action arrangement)
    {
        if (HasRun)
            throw new SetupFailed("GivenThat must be called before Then");
        _arranger.Push(arrangement);
    }

    internal void SetupMock<TService>(Action<Mock<TService>> setup) where TService : class
        => _arranger.Push(() => setup(GetMock<TService>()));

    internal void Using(params Action[] usings)
    {
        if (HasRun)
            throw new SetupFailed("Use must be called before Then");
        foreach (var use in usings)
            _arranger.Add(use);
    }
}