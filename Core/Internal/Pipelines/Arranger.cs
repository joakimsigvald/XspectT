﻿namespace XspecT.Internal.Pipelines;

internal class Arranger
{
    private readonly List<Action> _arrangements = [];
    internal void Push(Action arrangement) => _arrangements.Insert(0, arrangement);
    internal void Arrange() => _arrangements.ForEach(_ => _());
}