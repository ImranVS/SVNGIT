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
    [CollectionName("status")]
    public class Status: Entity
    {

        public Status()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach(var property in props)
            {
                property.SetValue(this, null);
            }

        }


        [DataMember]
        [BsonElement("type")]
        [BsonIgnoreIfNullAttribute]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("secondary_role")]
        [BsonIgnoreIfNullAttribute]
        public string SecondaryRole { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }
        //"last_update" : "2015-08-05T08:40:51.620Z",
        [DataMember]
        [BsonElement("next_Scan")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? NextScan { get; set; }

        [DataMember]
        [BsonElement("last_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastUpdated { get; set; }

        [DataMember]
        [BsonElement("location")]
        [BsonIgnoreIfNullAttribute]
        public string Location { get; set; }

        [DataMember]
        [BsonElement("category")]
        [BsonIgnoreIfNullAttribute]
        public string Category { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNullAttribute]
        public string CurrentStatus { get; set; }

        [DataMember]
        [BsonElement("status_code")]
        [BsonIgnoreIfNullAttribute]
        public string StatusCode { get; set; }

        [DataMember]
        [BsonElement("details")]
        [BsonIgnoreIfNullAttribute]
        public string Details { get; set; }

        [DataMember]
        [BsonElement("description")]
        [BsonIgnoreIfNullAttribute]
        public string Description { get; set; }

        [DataMember]
        [BsonElement("pending_mail")]
        [BsonIgnoreIfNullAttribute]
        public int? PendingMail { get; set; }

        [DataMember]
        [BsonElement("dead_mail")]
        [BsonIgnoreIfNullAttribute]
        public int? DeadMail { get; set; }

        [DataMember]
        [BsonElement("mail_details")]
        [BsonIgnoreIfNullAttribute]
        public string MailDetails { get; set; }

        [DataMember]
        [BsonElement("up_count")]
        [BsonIgnoreIfNullAttribute]
        public int? UpCount { get; set; }

        [DataMember]
        [BsonElement("down_count")]
        [BsonIgnoreIfNullAttribute]
        public int? DownCount { get; set; }

        [DataMember]
        [BsonElement("up_percent")]
        [BsonIgnoreIfNullAttribute]
        public int? UpPercent { get; set; }

        [DataMember]
        [BsonElement("response_time")]
        [BsonIgnoreIfNullAttribute]
        public int? ResponseTime { get; set; }

        [DataMember]
        [BsonElement("response_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? ResponseThreshold { get; set; }

        [DataMember]
        [BsonElement("pending_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? PendingThreshold { get; set; }

        [DataMember]
        [BsonElement("held_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? HeldThreshold { get; set; }


        [DataMember]
        [BsonElement("dead_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? DeadThreshold { get; set; }

        [DataMember]
        [BsonElement("user_count")]
        [BsonIgnoreIfNullAttribute]
        public int? UserCount { get; set; }

        [DataMember]
        [BsonElement("domino_server_tasks")]
        [BsonIgnoreIfNullAttribute]
        public string DominoServerTasks { get; set; }

        [DataMember]
        [BsonElement("type_and_name")]
        [BsonIgnoreIfNullAttribute]
        public string TypeAndName { get; set; }

        [DataMember]
        [BsonElement("operating_system")]
        [BsonIgnoreIfNullAttribute]
        public string OperatingSystem { get; set; }
        [DataMember]
        [BsonElement("version")]
        [BsonIgnoreIfNullAttribute]
        public string version { get; set; }

        [DataMember]
        [BsonElement("cpu")]
        [BsonIgnoreIfNullAttribute]
        public double? CPU { get; set; }

        [DataMember]
        [BsonElement("Cpu_threshold")]
        [BsonIgnoreIfNullAttribute]
        public double? CPUthreshold { get; set; }

        [DataMember]
        [BsonElement("memory")]
        [BsonIgnoreIfNullAttribute]
        public double? Memory { get; set; }

        [DataMember]
        [BsonElement("elapsed_days")]
        [BsonIgnoreIfNullAttribute]
        public int? ElapsedDays { get; set; }

        [DataMember]
        [BsonElement("exjournal")]
        [BsonIgnoreIfNullAttribute]
        public int? Exjournal { get; set; }

        [DataMember]
        [BsonElement("exjournal1")]
        [BsonIgnoreIfNullAttribute]
        public int? Exjournal1 { get; set; }

        [DataMember]
        [BsonElement("exjournal2")]
        [BsonIgnoreIfNullAttribute]
        public int? Exjournal2 { get; set; }

        [DataMember]
        [BsonElement("exjournal_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? ExjournalDate { get; set; }


        #region ActiveDirectory

        [DataMember]
        [BsonElement("active_directory_replication_status")]
        [BsonIgnoreIfNullAttribute]
        public ActiveDirectoryReplicationStatus[] ActiveDirectoryReplicationStatus {get; set;}

        [DataMember]
        [BsonElement("logon_test")]
        [BsonIgnoreIfNullAttribute]
        public string LogonTest { get; set; }

        [DataMember]
        [BsonElement("query_test")]
        [BsonIgnoreIfNullAttribute]
        public string QueryTest { get; set; }

        [DataMember]
        [BsonElement("ldap_port_test")]
        [BsonIgnoreIfNullAttribute]
        public string LdapPortTest { get; set; }

        [DataMember]
        [BsonElement("advertising_test")]
        [BsonIgnoreIfNullAttribute]
        public string AdvertisingTest { get; set; }

        [DataMember]
        [BsonElement("frs_sys_vol_test")]
        [BsonIgnoreIfNullAttribute]
        public string FrsSysVolTest { get; set; }

        [DataMember]
        [BsonElement("replication_test")]
        [BsonIgnoreIfNullAttribute]
        public string ReplicationTest { get; set; }

        [DataMember]
        [BsonElement("services_test")]
        [BsonIgnoreIfNullAttribute]
        public string ServicesTest { get; set; }

        [DataMember]
        [BsonElement("dns_test")]
        [BsonIgnoreIfNullAttribute]
        public string DnsTest { get; set; }

        [DataMember]
        [BsonElement("fsmo_check_test")]
        [BsonIgnoreIfNullAttribute]
        public string FsmoCheckTest { get; set; }

        #endregion

        #region DAG

        [DataMember]
        [BsonElement("file_witness_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string FileWitnessSereverName { get; set; }

        [DataMember]
        [BsonElement("file_witness_server_status")]
        [BsonIgnoreIfNullAttribute]
        public string FileWitnessSereverStatus { get; set; }

        [DataMember]
        [BsonElement("dag_server")]
        [BsonIgnoreIfNullAttribute]
        public List<DagServers> DagServers { get; set; }

        [DataMember]
        [BsonElement("dag_database")]
        [BsonIgnoreIfNullAttribute]
        public List<DagDatabases> DagDatabases { get; set; }

        [DataMember]
        [BsonElement("dag_server_databases")]
        [BsonIgnoreIfNullAttribute]
        public List<DagServerDatabases> DagServerDatabases { get; set; }

        [DataMember]
        [BsonElement("mailbox_count")]
        [BsonIgnoreIfNullAttribute]
        public int? MailboxCount { get; set; }

        [DataMember]
        [BsonElement("database_count")]
        [BsonIgnoreIfNullAttribute]
        public int? DatabaseCount { get; set; }



        #endregion
    }


    [DataContract]
    [Serializable]
    [CollectionName("status_details")]
    public class StatusDetails :Entity
    {

        [DataMember]
        [BsonElement("device_id")]
        public int? DeviceId { get; set; }

        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("category")]
        public string category { get; set; }

        [DataMember]
        [BsonElement("test_name")]
        public string TestName { get; set; }

        [DataMember]
        [BsonElement("result")]
        public string Result { get; set; }

        [DataMember]
        [BsonElement("details")]
        public string Details { get; set; }

    }


    [DataContract]
    [Serializable]
    [CollectionName("active_directory_replication_status")]
    public class ActiveDirectoryReplicationStatus : Entity
    {
        public ActiveDirectoryReplicationStatus()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }


        [DataMember]
        [BsonElement("source_server")]
        [BsonIgnoreIfNullAttribute]
        public string SoureServer { get; set; }


        [DataMember]
        [BsonElement("largest_delta")]
        [BsonIgnoreIfNullAttribute]
        public string LargestDelta { get; set; }

        [DataMember]
        [BsonElement("fails")]
        [BsonIgnoreIfNullAttribute]
        public string Fails { get; set; }

        [DataMember]
        [BsonElement("directory_partitions")]
        [BsonIgnoreIfNullAttribute]
        public string DirectoryPartitions { get; set; }
    }

    public class DagServers : Entity 
    {

        public DagServers()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("dag_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string DAGServerName { get; set; }

        [DataMember]
        [BsonElement("cluster_service_test")]
        [BsonIgnoreIfNullAttribute]
        public string ClusterServiceTest { get; set; }

        [DataMember]
        [BsonElement("replay_service_test")]
        [BsonIgnoreIfNullAttribute]
        public string ReplayServiceTest { get; set; }

        [DataMember]
        [BsonElement("active_manager_test")]
        [BsonIgnoreIfNullAttribute]
        public string ActiveManagerTest { get; set; }

        [DataMember]
        [BsonElement("tasks_rpc_listener_test")]
        [BsonIgnoreIfNullAttribute]
        public string TasksRPCListenerTest { get; set; }

        [DataMember]
        [BsonElement("tcp_listener_test")]
        [BsonIgnoreIfNullAttribute]
        public string TCPListenerTest { get; set; }

        [DataMember]
        [BsonElement("dag_members_up_test")]
        [BsonIgnoreIfNullAttribute]
        public string DAGMembersUp { get; set; }

        [DataMember]
        [BsonElement("cluster_network_test")]
        [BsonIgnoreIfNullAttribute]
        public string ClusterNetworkTest { get; set; }

        [DataMember]
        [BsonElement("quorum_group_test")]
        [BsonIgnoreIfNullAttribute]
        public string QuorumGroupTest { get; set; }

        [DataMember]
        [BsonElement("file_share_quorum_test")]
        [BsonIgnoreIfNullAttribute]
        public string FileShareQuorumTest { get; set; }

        [DataMember]
        [BsonElement("db_copy_suspend_test")]
        [BsonIgnoreIfNullAttribute]
        public string DBCopySuspendTest { get; set; }

        [DataMember]
        [BsonElement("db_disconnected_test")]
        [BsonIgnoreIfNullAttribute]
        public string DBDisconnectedTest { get; set; }

        [DataMember]
        [BsonElement("db_log_copy_keeping_up_test")]
        [BsonIgnoreIfNullAttribute]
        public string DBLogCopyKeepingUpTest { get; set; }

        [DataMember]
        [BsonElement("db_log_replay_keeping_up_test")]
        [BsonIgnoreIfNullAttribute]
        public string DBLogReplayKeepingUpTest { get; set; }

    }


    public class DagDatabases : Entity
    {

        public DagDatabases()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }
        
        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseName { get; set; }

        

    }

    public class DagServerDatabases : Entity
    {

        public DagServerDatabases()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseName { get; set; }

        [DataMember]
        [BsonElement("server_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("action_preference")]
        [BsonIgnoreIfNullAttribute]
        public int? ActionPreference { get; set; }

        [DataMember]
        [BsonElement("is_active")]
        [BsonIgnoreIfNullAttribute]
        public string IsActive { get; set; }

        [DataMember]
        [BsonElement("copy_queue")]
        [BsonIgnoreIfNullAttribute]
        public string CopyQueue { get; set; }

        [DataMember]
        [BsonElement("replay_queue")]
        [BsonIgnoreIfNullAttribute]
        public string ReplayQueue { get; set; }

        [DataMember]
        [BsonElement("replay_lagged")]
        [BsonIgnoreIfNullAttribute]
        public string ReplayLagged { get; set; }

        [DataMember]
        [BsonElement("truncation_lagged")]
        [BsonIgnoreIfNullAttribute]
        public string TruncationLagged { get; set; }

        [DataMember]
        [BsonElement("contend_index")]
        [BsonIgnoreIfNullAttribute]
        public string ContendIndex { get; set; }


    }

}
