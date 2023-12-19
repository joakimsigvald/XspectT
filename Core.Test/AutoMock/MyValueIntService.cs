﻿using XspecT.Test.Given;

namespace XspecT.Test.AutoMock;

public class MyValueIntService
{
    private readonly IMyValueIntRepo _repo;

    public MyValueIntService(IMyValueIntRepo repo) => _repo = repo;

    public void SetValue(MyValueInt value) => _repo.Set(value);
    public string GetValue(MyValueInt value) => _repo.Get(value);

    public Task SetValueAsync(MyValueInt value) => _repo.SetAsync(value);
    public Task<string> GetValueAsync(MyValueInt value) => _repo.GetAsync(value);
    public IMyValueIntRepo GetRepo() => _repo.GetMe();
    public Task<IMyValueIntRepo> GetRepoAsync() => _repo.GetMeAsync();
    public object GetObject() => _repo.GetObject();
    public Task<object> GetObjectAsync() => _repo.GetObjectAsync();
}