﻿using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using XspecT.Internal;
using XspecT.Internal.TestData;
using Xunit.Sdk;

namespace XspecT;

/// <summary>
/// 
/// </summary>
public static class Specification
{
    //[ThreadStatic]
    [ThreadStatic]
    private static string _description;
    [ThreadStatic]
    private static List<Action<StringBuilder>> _applications;

    internal static void Clear()
    {
        _description = null;
        _applications = null;
    }

    /// <summary>
    /// 
    /// </summary>
    public static string Description
        => _description ??= Build();

    private static string Build()
    {
        StringBuilder sb = new();
        foreach (var apply in Applications)
            apply(sb);
        return sb.ToString().Trim(',').Trim().Capitalize();
    }

    internal static void AddMockSetup<TService>(string callExpr)
    {
        Add(AddMockSetup);

        void AddMockSetup(StringBuilder descriptionBuilder)
        {
            var sb = new StringBuilder();
            sb.Append($"given {typeof(TService).Name}.");
            sb.Append(callExpr.ParseCall());
            descriptionBuilder.AddPhrase(sb.ToString());
        }
    }

    internal static void AddMockReturns(string returnsExpr)
    {
        Add(AddMockReturns);

        void AddMockReturns(StringBuilder descriptionBuilder)
        {
            var sb = new StringBuilder();
            sb.Append("returns ");
            sb.Append(returnsExpr.ParseValue());
            descriptionBuilder.AddWord(sb.ToString());
        }
    }

    internal static void AddWhen<TDelegate>(Expression<TDelegate> act)
    {
        Add(AddWhen);

        void AddWhen(StringBuilder descriptionBuilder)
        {
            var sb = new StringBuilder();
            sb.Append($"when {act.GetMethodName()}");
            AddMethodArguments(sb, act.Body as MethodCallExpression);
            descriptionBuilder.AddPhrase(sb.ToString());
        }
    }

    internal static void AddAssert(
        Action assert,
        string actual = null,
        string expected = null,
        [CallerMemberName] string verb = "")
    {
        Add(AddAssert);
        try
        {
            assert();
        }
        catch (XunitException ex)
        {
            throw new XunitException(Description, ex);
        }

        void AddAssert(StringBuilder descriptionBuilder)
        {
            descriptionBuilder.AddWord(actual.ParseActual());
            descriptionBuilder.AddWord(verb.AsWords());
            descriptionBuilder.AddWord(expected.ParseValue());
        }
    }

    internal static void AddAnd() => Add(sb => sb.AddWord("and"));

    internal static void AddThen(string subjectExpr)
    {
        Add(sb => sb.AddPhrase("then"));
        if (subjectExpr != null)
            Add(sb => sb.AddWord(subjectExpr.ParseValue()));
    }

    internal static void AddGiven(string valueExpr, ApplyTo applyTo)
    {
        Add(sb => sb.AddPhrase(string.Join(' ', GetWords())));

        IEnumerable<string> GetWords()
        {
            yield return "given";
            if (applyTo == ApplyTo.Default)
                yield return "default";
            else if (applyTo == ApplyTo.Using)
                yield return "using";
            yield return valueExpr.ParseValue();
        }
    }

    internal static void AddVerify<TService>(string expressionExpr) 
        => Add(sb => sb.AddWord($"{typeof(TService).Name}.{expressionExpr.ParseCall()}"));

    internal static void AddThrows<TError>()
        => Add(sb => sb.AddPhrase($"then throws {typeof(TError).Name}"));

    private static void Add(Action<StringBuilder> apply) => Applications.Add(apply);

    private static void AddMethodArguments(StringBuilder sb, MethodCallExpression body)
    {
        if (body is null)
            return;
        sb.Append('(');
        foreach (var argument in body.Arguments)
            sb.Append(DescribeArgument(argument));
        sb.Append(')');
    }

    private static void AddPhrase(this StringBuilder sb, string phrase)
    {
        sb.Append($",{Environment.NewLine}");
        sb.AddWord(phrase);
    }

    private static void AddWord(this StringBuilder sb, string word)
    {
        if (string.IsNullOrEmpty(word))
            return;
        sb.Append(' ');
        sb.Append(word);
    }

    private static string DescribeArgument(Expression expr)
        => expr switch
        {
            MethodCallExpression mce => $"{mce.Method.Name.AsWords()} {mce.Method.ReturnType.Alias()}",
            UnaryExpression ue => DescribeArgument(ue.Operand),
            MemberExpression => "TODO",
            ParameterExpression => "TODO",
            ConstantExpression => "TODO",
            LambdaExpression le => DescribeLambdaExpression(le),
            _ => throw new SetupFailed($"Unknown argument expression: {expr.NodeType}")
        };

    private static string DescribeLambdaExpression(LambdaExpression expr)
    {
        var body = expr.Body as MethodCallExpression;
        var methodName = DescribeArgument(body);
        return body.Arguments.Count == 0
            ? methodName
            : $"{methodName} {{ {DescribeCriteria(body.Arguments[0] as UnaryExpression)} }}";
    }

    private static string DescribeCriteria(UnaryExpression criteria)
    {
        var operand = criteria.Operand as LambdaExpression;
        var body = operand.Body as BinaryExpression;
        var left = body.Left as MemberExpression;
        var prop = left.Member as PropertyInfo;
        return $"{prop.Name} = {DescribeArgument(body.Right)}";
    }

    private static List<Action<StringBuilder>> Applications => _applications ??= [];
}