using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNext.Mongo.Entities
{
    class CustomSerializers
    {
        public class StringToListStringSerializer : EnumerableSerializerBase<List<String>>
        {
            public override List<String> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                if (context.Reader.CurrentBsonType == BsonType.String)
                {
                    var s = context.Reader.ReadString();
                    return new List<String>() { s };//null;
                }

                return base.Deserialize(context, args);
            }

            protected override void AddItem(object accumulator, object item)
            {
                ((List<String>)accumulator).Add((String)item);
            }

            protected override object CreateAccumulator()
            {
                return new List<String>();
            }

            protected override IEnumerable EnumerateItemsInSerializationOrder(List<String> value)
            {
                return value;
            }

            protected override List<String> FinalizeResult(object accumulator)
            {
                return (List<String>)accumulator;
            }
        }
    }
}
