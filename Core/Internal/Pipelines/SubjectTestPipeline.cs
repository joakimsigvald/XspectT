﻿namespace XspecT.Internal.Pipelines;

internal abstract class SubjectTestPipeline<TSUT, TResult>
    : TestPipeline<TResult, SubjectSpec<TSUT, TResult>>, ISubjectTestPipeline<TSUT, TResult>
    where TSUT : class
{
    internal SubjectTestPipeline(SubjectSpec<TSUT, TResult> parent)
        : base(parent) { }

    /// <summary>
    /// Provide the method-under-test to the test-pipeline
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> When(Action<TSUT> act)
        => Parent.When(act);

    /// <summary>
    /// Provide the method-under-test to the test-pipeline
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> When(Func<TSUT, TResult> act)
        => Parent.When(act);

    /// <summary>
    /// Provide the method-under-test to the test-pipeline
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> When(Func<TSUT, Task> action)
        => Parent.When(action);

    /// <summary>
    /// Provide the method-under-test to the test-pipeline
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> When(Func<TSUT, Task<TResult>> func)
        => Parent.When(func);

    /// <summary>
    /// Provide setUp to the test-pipeline
    /// </summary>
    /// <param name="setUp"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> After(Action<TSUT> setUp)
        => Parent.After(setUp);

    /// <summary>
    /// Provide setUp to the test-pipeline
    /// </summary>
    /// <param name="setUp"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> After(Func<TSUT, Task> setUp)
        => Parent.After(setUp);

    /// <summary>
    /// Provide tearDown to the test-pipeline
    /// </summary>
    /// <param name="tearDown"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> Before(Action<TSUT> tearDown)
        => Parent.Before(tearDown);

    /// <summary>
    /// Provide tearDown to the test-pipeline
    /// </summary>
    /// <param name="tearDown"></param>
    /// <returns></returns>
    public ISubjectTestPipeline<TSUT, TResult> Before(Func<TSUT, Task> tearDown)
        => Parent.Before(tearDown);

    public IGivenContinuation<TSUT, TResult, TService> Given<TService>() where TService : class
        => Parent.Given<TService>();

    public IGivenContinuation<TSUT, TResult> Given() => Parent.Given();

    public IGivenSubjectTestPipeline<TSUT, TResult> Given<TValue>(Action<TValue> setup) where TValue : class
        => Parent.Given(setup);

    public IGivenSubjectTestPipeline<TSUT, TResult> Given<TValue>(Func<TValue> value) => Parent.Given(value);

    public IGivenSubjectTestPipeline<TSUT, TResult> Given<TValue>(TValue value) => Parent.Given(value);

}