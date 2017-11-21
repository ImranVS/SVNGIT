using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{
    [DataContract]
        [Serializable]
        [CollectionName("sitemap")]
        public class SiteMap : Entity
        {

            public SiteMap()
            {
                System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                foreach (var property in props)
                {
                    property.SetValue(this, null);
                }

            }
         [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("nodes")]
        public ICollection<SiteMapNode> Nodes { get; set; }
    }
}
