using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("schedule_reports")]
    public class ScheduledReports : Entity
    {
        [DataMember]
        [BsonElement("report_name")]
        [BsonIgnoreIfNullAttribute]
        public string ReportName { get; set; }

        [DataMember]
        [BsonElement("report_subject")]
        [BsonIgnoreIfNullAttribute]
        public string ReportSubject { get; set; }

        [DataMember]
        [BsonElement("report_body")]
        [BsonIgnoreIfNullAttribute]
        public string ReportBody { get; set; }

        [DataMember]
        [BsonElement("report_frequency")]
        [BsonIgnoreIfNullAttribute]
        public string Frequency { get; set; }

        [DataMember]
        [BsonElement("send_to")]
        [BsonIgnoreIfNullAttribute]
        public string SendTo { get; set; }

        [DataMember]
        [BsonElement("copy_to")]
        [BsonIgnoreIfNullAttribute]
        public string CopyTo { get; set; }


        [DataMember]
        [BsonElement("blind_copy_to")]
        [BsonIgnoreIfNullAttribute]
        public string BlindCopyTo { get; set; }

        [DataMember]
        [BsonElement("file_format")]
        [BsonIgnoreIfNullAttribute]
        public string FileFormat { get; set; }

        [DataMember]
        [BsonElement("frequency_days_list")]
        [BsonIgnoreIfNullAttribute]
        public List<String> FrequencyDayList { get; set; }


        [DataMember]
        [BsonElement("report_frequency_value")]
        [BsonIgnoreIfNullAttribute]
        public string FrequencyeportValue { get; set; }

        [DataMember]
        [BsonElement("repeat")]
        [BsonIgnoreIfNullAttribute]
        public string Repeat { get; set; }

        [DataMember]
        [BsonElement("selected_reports")]
        [BsonIgnoreIfNullAttribute]
        public List<string> SelectedReports { get; set; }
        
    }
}
