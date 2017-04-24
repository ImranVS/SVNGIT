using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VSNext.Mongo.Repository
{
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public class Entity : IEntity
    {    
        [DataMember]
        [BsonElement("_id",Order = 0)]
        [BsonRepresentation(BsonType.ObjectId)]
        //[BsonIgnoreIfNull]
        public string Id { get; set; }

        [DataMember]
        [BsonElement("tenant_id",Order = 1)]   
        [BsonIgnoreIfNull]     
        public int? TenantId { get; set; }
            
        [DataMember]
        [BsonElement("modified_on",Order = 2)]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonIgnoreIfNull]
        public DateTime? ModifiedOn { get; set; }


        private DateTime? _createdOn;
        [BsonElement("created_on", Order = 3)]
        public DateTime CreatedOn
        {
            get
            {
                if (_createdOn.HasValue)
                    return _createdOn.Value;
                return ObjectId.CreationTime;
            }
            set
            {
                _createdOn = value;
            }
        }
        public ObjectId ObjectId
        {
            get
            {
                //Incase, this is required before inserted into db
                if (string.IsNullOrEmpty( Id))
                    Id = ObjectId.GenerateNewId().ToString();
                return ObjectId.Parse(Id);
            }
        }
    }
}
