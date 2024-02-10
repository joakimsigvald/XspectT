﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using XspecT.Assert;

namespace XspecT.Test.AutoMock;

public class WhenGetRecordWithBsonIdFromMock : SubjectSpec<BsonIdService, RecordMongoDb>
{
    public WhenGetRecordWithBsonIdFromMock() => Given<RecordMongoDb>(_ => _.Value = "123").When(_ => _.GetRecord());
    [Fact] public void ThenGetRecord() => Then().Result.Is().NotNull();
}

public class BsonIdService(IBsonIdRepository repo)
{
    private readonly IBsonIdRepository _repo = repo;
    public Task<RecordMongoDb> GetRecord() => _repo.GetRecord();
}

public interface IBsonIdRepository
{
    Task<RecordMongoDb> GetRecord();
}

public class RecordMongoDb
{
    [BsonId] public ObjectId Id { get; set; }
    public string Value { get; set; }
}