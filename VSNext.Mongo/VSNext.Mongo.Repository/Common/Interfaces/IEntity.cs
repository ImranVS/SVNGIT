﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Repository
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
        int TenantId { get; set; }      
        [BsonIgnore]
        ObjectId ObjectId { get; }
        [BsonIgnore]
        DateTime CreatedOn { get; }
        DateTime ModifiedOn { get; }
    }
}
