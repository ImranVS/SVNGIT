using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("O365Health")]
    public class O365Heath : Entity
    {
        [DataMember]
        public string VSId { get; set; }
        [DataMember]
        public string Account { get; set; }
    }
    [DataContract]
    [Serializable]
    [CollectionName("Statistics")]
    public class Statistics : Entity
    {
        [DataMember]
        public string LocationId
        {
            get;
            set;
        }
        [DataMember]
        public string StatName
        {
            get;
            set;
        }
        [DataMember]
        public string StatValue
        {
            get;
            set;
        }
       
    }


    //WS Moved this to Location.cs since it is a location that will be used elsewhere in the project and to make it more obvious where it is

    //[DataContract]
    //[Serializable]
    //[CollectionName("Location")]
    //public class Location : Entity
    //{
    //    [DataMember]
    //    public string O365Id
    //    {
    //        get;
    //        set;
    //    }
    //    [DataMember]
    //    public string City { get; set; }
    //    [DataMember]
    //    public string Region { get; set; }
    //    [DataMember]
    //    public string Country { get; set; }
    //    [DataMember]
    //    public string OptimalDataCenter { get; set; }
    //    [DataMember]
    //    public string ActiveDataCenter { get; set; }
    //}
}
