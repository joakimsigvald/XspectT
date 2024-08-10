﻿using System.Text;
using XspecT.Internal.TestData;

namespace XspecT.Internal;

internal class SpecificationBuilder
{
    private string _description;
    private readonly List<Action> _applications = [];
    private readonly StringBuilder _descriptionBuilder = new();
    private int _givenCount;
    private int _thenCount;
    private string _currentMockSetup;
    private bool _isThenReferencingSubject = false;

    public string Description => _description ??= Build();

    internal void Add(Action apply) => _applications.Add(apply);

    internal string Build()
    {
        foreach (var apply in _applications) apply();
        return _descriptionBuilder.ToString().Trim();
    }

    internal void AddMockSetup<TService>(string callExpr) 
        => AddPhraseOrSentence($"{Given} {GetMockName<TService>('.')}{callExpr.ParseCall()}");

    internal void AddMockReturnsDefault<TService>(string returnsExpr)
        => AddPhraseOrSentence($"{Given} {GetMockName<TService>(' ')}returns {returnsExpr.ParseValue()}");

    internal void AddMockReturns(string returnsExpr)
        => AddWord($"returns {returnsExpr.ParseValue()}");

    internal void AddWhen(string actExpr) 
        => AddSentence($"when {actExpr.ParseCall()}");

    internal void AddAssert(string actual, string verb, string expected)
    {
        AddWord(actual.ParseActual(), _isThenReferencingSubject ? "'s " : " ");
        AddWord(verb.AsWords());
        AddWord(expected.ParseValue());
    }

    internal void AddThen(string subjectExpr)
    {
        AddPhraseOrSentence(Then);
        AddWord(subjectExpr.ParseValue());
        _isThenReferencingSubject = !string.IsNullOrEmpty(subjectExpr);
    }

    internal void AddGiven(string valueExpr, ApplyTo applyTo)
    {
        _currentMockSetup = null;
        AddPhraseOrSentence(string.Join(' ', GetWords()));

        IEnumerable<string> GetWords()
        {
            yield return Given;
            if (applyTo == ApplyTo.Default)
                yield return "default";
            else if (applyTo == ApplyTo.Using)
                yield return "using";
            yield return valueExpr.ParseValue();
        }
    }

    internal void AddGivenSetup<TModel>(string setupExpr)
    {
        _currentMockSetup = null;
        AddPhraseOrSentence($"{Given} {NameOf<TModel>()} {{ {setupExpr.ParseValue()} }}");
    }

    internal void AddVerify<TService>(string expressionExpr)
        => AddWord($"{NameOf<TService>()}.{expressionExpr.ParseCall()}");

    internal void AddThrows<TError>()
        => AddSentence($"{Then} throws {NameOf<TError>()}");

    internal void AddTap(string expr) => AddWord($"tap({expr})");

    private string GetMockName<TService>(char binder)
    {
        var nextMockSetup = NameOf<TService>();
        var mockName = nextMockSetup == _currentMockSetup
            ? ""
            : $"{nextMockSetup}{binder}";
        _currentMockSetup = nextMockSetup;
        return mockName;
    }

    private void AddPhraseOrSentence(string phrase)
    {
        if (char.IsUpper(phrase[0]))
            AddSentence(phrase);
        else AddPhrase(phrase);
    }

    private void AddPhrase(string phrase)
        => _descriptionBuilder.Append($"{Environment.NewLine} {phrase}");

    private void AddSentence(string phrase)
        => _descriptionBuilder.Append($"{Environment.NewLine}{phrase.Capitalize()}");

    private void AddWord(string word, string binder = " ")
    {
        if (string.IsNullOrEmpty(word))
            return;
        _descriptionBuilder.Append(binder);
        _descriptionBuilder.Append(word);
    }

    private static string NameOf<T>() => typeof(T).Alias();

    private string Given => 0 == _givenCount++ ? "Given" : "and";

    private string Then => 0 == _thenCount++ ? "Then" : "and";
}