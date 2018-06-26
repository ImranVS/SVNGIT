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
        //public class StringToListStringSerializer : EnumerableSerializerBase<List<String>>
        //{
        //    public override List<String> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        //    {
        //        if (context.Reader.CurrentBsonType == BsonType.String)
        //        {
        //            var s = context.Reader.ReadString();
        //            return new List<String>() { s };//null;
        //        }

        //        return base.Deserialize(context, args);
        //    }

        //    protected override void AddItem(object accumulator, object item)
        //    {
        //        ((List<String>)accumulator).Add((String)item);
        //    }

        //    protected override object CreateAccumulator()
        //    {
        //        return new List<String>();
        //    }

        //    protected override IEnumerable EnumerateItemsInSerializationOrder(List<String> value)
        //    {
        //        return value;
        //    }

        //    protected override List<String> FinalizeResult(object accumulator)
        //    {
        //        return (List<String>)accumulator;
        //    }
        //}

        //public class StringToObjectIdSerializer : MongoDB.Bson.Serialization.Serializers.SerializerBase<ObjectId>
        //{
        //    public override ObjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        //    {
        //        if (context.Reader.CurrentBsonType == BsonType.String)
        //        {
        //            var s = context.Reader.ReadString();
        //            return new ObjectId(s);//null;
        //        }

        //        return base.Deserialize(context, args);
        //    }
        //}
        

        //Converts a List<String> to a List<NameValuePair> with the String being the Name and the Value being set to Null
        public class ListStringToListNameValuePair : EnumerableSerializerBase<List<NameValuePair>>
        {
            public override List<NameValuePair> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                //Sets a bookmark to the front of the Reader so we can go back if its already converted
                var bookmark = context.Reader.GetBookmark();
                switch (context.Reader.CurrentBsonType)
                {
                    case BsonType.Array:
                        context.Reader.ReadStartArray();
                        var accumulator = CreateAccumulator();
                        do
                        {
                            var v = context.Reader.ReadBsonType();
                            switch (v)
                            {
                                case BsonType.String:
                                    //If the array is of strings, covnert to a new namevalue and add it
                                    AddItem(accumulator, new NameValuePair() { Name = context.Reader.ReadString(), Value = null });
                                    break;

                                case BsonType.EndOfDocument:
                                    break;

                                default:
                                    //if its not being handled, assume its a namevalue and call the base deserializer
                                    context.Reader.ReturnToBookmark(bookmark);
                                    return base.Deserialize(context, args);
                            }
                        } while (context.Reader.CurrentBsonType != BsonType.EndOfDocument);
                        context.Reader.ReadEndArray();
                        return FinalizeResult(accumulator);
                }
                
                return base.Deserialize(context, args);
            }

            protected override void AddItem(object accumulator, object item)
            {
                if(item.GetType() == typeof(System.Dynamic.ExpandoObject))
                    ((List<NameValuePair>)accumulator).Add(BsonSerializer.Deserialize<NameValuePair>(((System.Dynamic.ExpandoObject)item).ToBsonDocument()));
                else
                    ((List<NameValuePair>)accumulator).Add((NameValuePair)item);
            }

            protected override object CreateAccumulator()
            {
                return new List<NameValuePair>();
            }

            protected override IEnumerable EnumerateItemsInSerializationOrder(List<NameValuePair> value)
            {
                return value;
            }

            protected override List<NameValuePair> FinalizeResult(object accumulator)
            {
                return (List<NameValuePair>)accumulator;
            }

        }

    }
}
