﻿using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using XspecT.Continuations;

namespace XspecT.Internal.Pipelines;

internal class GivenServiceContinuation<TSUT, TResult, TService> : IGivenServiceContinuation<TSUT, TResult, TService>
    where TService : class
{
    private readonly Spec<TSUT, TResult> _spec;

    internal GivenServiceContinuation(Spec<TSUT, TResult> spec) => _spec = spec;

    public IGivenThatReturnsContinuation<TSUT, TResult, TService> Returns<TReturns>(
        Func<TReturns> value,
        [CallerArgumentExpression(nameof(value))] string valueExpr = null)
    {
        var theValue = value();
        _spec.ArrangeLast(() => _spec.GetMock<TService>().SetReturnsDefault(theValue));
        _spec.ArrangeLast(() => _spec.GetMock<TService>().SetReturnsDefault(Task.FromResult(theValue)));
        return new GivenThatReturnsContinuation<TSUT, TResult, TService>(_spec);
    }

    public IGivenTestPipeline<TSUT, TResult> Throws<TException>() where TException : Exception, new()
        => Throws(_spec.Another<TException>);

    public IGivenTestPipeline<TSUT, TResult> Throws(Func<Exception> ex)
    {
        _spec.SetupThrows<TService>(ex);
        return new GivenTestPipeline<TSUT, TResult>(_spec);
    }

    public IGivenThatContinuation<TSUT, TResult, TService, TReturns> That<TReturns>(
        Expression<Func<TService, TReturns>> call,
        [CallerArgumentExpression(nameof(call))] string callExpr = null)
        => new GivenThatContinuation<TSUT, TResult, TService, TReturns, TReturns>(_spec, call, callExpr);

    public IGivenThatContinuation<TSUT, TResult, TService, TReturns> That<TReturns>(
        Expression<Func<TService, Task<TReturns>>> call,
        [CallerArgumentExpression(nameof(call))] string callExpr = null)
        => new GivenThatContinuation<TSUT, TResult, TService, TReturns, Task<TReturns>>(_spec, call, callExpr);
}