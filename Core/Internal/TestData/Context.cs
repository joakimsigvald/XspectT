﻿using Moq;

namespace XspecT.Internal.TestData;

internal class Context
{
    [Obsolete]
    private readonly Dictionary<Type, Dictionary<string, object>> _labeledMentions = new();
    private readonly DataProvider _dataProvider = new();

    internal TSUT CreateSUT<TSUT>()
    {
        var sutType = typeof(TSUT);
        return sutType.IsClass && sutType != typeof(string)
            ? _dataProvider.Instantiate<TSUT>()
            : Create<TSUT>();
    }

    internal TValue Mention<TValue>(int index, bool asDefault = false)
        => Produce<TValue>(index, asDefault);

    internal TValue Mention<TValue>(int index, Action<TValue> setup)
        => ApplyTo(setup, Produce<TValue>(index));

    internal TValue Mention<TValue>(int index, Func<TValue, TValue> setup)
        => (TValue)Mention(typeof(TValue), setup.Invoke(Produce<TValue>(index)), index);

    internal static TValue ApplyTo<TValue>(Action<TValue> setup, TValue value)
    {
        setup.Invoke(value);
        return value;
    }

    internal void SetDefault<TModel>(Action<TModel> setup) where TModel : class
        => _dataProvider.AddDefaultSetup(
            typeof(TModel),
            obj =>
            {
                if (obj is TModel model)
                    setup(model);
                return obj;
            });

    internal void SetDefault<TValue>(Func<TValue, TValue> setup)
        => _dataProvider.AddDefaultSetup(typeof(TValue), _ => setup((TValue)_));

    [Obsolete]
    internal TValue Mention<TValue>(string label)
    {
        var mentions = ProduceMentions(typeof(TValue));
        return mentions.TryGetValue(label, out var val)
            ? (TValue)val
            : (TValue)(mentions[label] = Create<TValue>());
    }

    internal TValue Mention<TValue>(TValue value, int index = 0, bool asDefault = false)
    {
        if (asDefault)
            Use(value);
        Mention(typeof(TValue), value, index);
        return value;
    }

    internal TValue[] MentionMany<TValue>(int count, int? minCount)
    {
        var (val, found) = _dataProvider.Retreive(typeof(TValue[]));
        return found && val is TValue[] arr
            ? Reuse(arr, count, minCount)
            : MentionMany<TValue>(count);
    }

    internal TValue[] MentionMany<TValue>(Action<TValue> setup, int count)
        => Mention(Enumerable.Range(0, count).Select(i => Mention(i, setup)).ToArray());

    internal TValue[] MentionMany<TValue>(Action<TValue, int> setup, int count)
        => Mention(Enumerable.Range(0, count).Select(i => Mention<TValue>(i, _ => setup(_, i))).ToArray());

    internal TValue Create<TValue>() => _dataProvider.Create<TValue>();

    internal Mock<TObject> GetMock<TObject>() where TObject : class 
        => _dataProvider.GetMock<TObject>();

    internal void Use<TService>(TService service) => _dataProvider.Use(service);

    [Obsolete]
    private Dictionary<string, object> ProduceMentions(Type type)
        => _labeledMentions.TryGetValue(type, out var mentions) ? mentions : _labeledMentions[type] = new();

    private TValue[] MentionMany<TValue>(int count)
        => Mention(Enumerable.Range(0, count).Select(i => Mention<TValue>(i)).ToArray());

    private TValue Produce<TValue>(int index, bool asDefault = false)
    {
        var (val, found) = _dataProvider.Retreive(typeof(TValue), index);
        return (TValue)(found ? val : Mention(Create<TValue>(), index, asDefault));
    }

    private object Mention(Type type, object value, int index = 0)
        => _dataProvider.GetMentions(type)[index] = value;

    private TValue[] Reuse<TValue>(TValue[] arr, int count, int? minCount)
        => arr.Length >= minCount || arr.Length == count ? arr
        : arr.Length > count ? arr[..count]
        : Extend(arr, count);

    private TValue[] Extend<TValue>(TValue[] arr, int count)
    {
        var oldLen = arr.Length;
        var newArr = new TValue[count];
        Array.Copy(arr, newArr, oldLen);
        for (var i = oldLen; i < count; i++)
            newArr[i] = Mention<TValue>(i);
        return newArr;
    }
}