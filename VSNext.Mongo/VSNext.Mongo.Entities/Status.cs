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
        [BsonElement("software_version")]
        [BsonIgnoreIfNullAttribute]
        public double SoftwareVersion { get; set; }

        [DataMember]
        [BsonElement("cpu")]
        [BsonIgnoreIfNullAttribute]
        public double? CPU { get; set; }

        [DataMember]
        [BsonElement("cpu_threshold")]
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

        [DataMember]
        [BsonElement("disks")]
        [BsonIgnoreIfNull]
        public List<Disk> Disks { get; set; }

        #region Exchange

        [DataMember]
        [BsonElement("submission_queue_count")]
        [BsonIgnoreIfNullAttribute]
        public int? SubmissionQueueCount { get; set; }

        [DataMember]
        [BsonElement("submission_queue_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? SubmissionQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("unreachable_queue_count")]
        [BsonIgnoreIfNullAttribute]
        public int? UnreachableQueueCount { get; set; }

        [DataMember]
        [BsonElement("unreachable_queue_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? UnreachableQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("shadow_queue_count")]
        [BsonIgnoreIfNullAttribute]
        public int? ShadowQueueCount { get; set; }

        [DataMember]
        [BsonElement("shadow_queue_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? ShadowQueueThreshold { get; set; }

        #endregion

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

        #region SharePoint

        [DataMember]
        [BsonElement("site_collections")]
        [BsonIgnoreIfNullAttribute]
        public List<SharePointSiteCollection> SharePointSiteCollections;

        [DataMember]
        [BsonElement("web_applications")]
        [BsonIgnoreIfNullAttribute]
        public List<SharePointWebApplication> SharePointWebApplications;

        [DataMember]
        [BsonElement("timer_jobs")]
        [BsonIgnoreIfNullAttribute]
        public List<SharePointTimerJob> SharePointTimerJobs;

        [DataMember]
        [BsonElement("file_upload_test")]
        [BsonIgnoreIfNullAttribute]
        public string FileUploadTest;

        [DataMember]
        [BsonElement("site_creation_test")]
        [BsonIgnoreIfNullAttribute]
        public string SiteCreationTest;


        #region Stats

        [DataMember]
        [BsonElement("ASP.NET Version")]
        [BsonIgnoreIfNullAttribute]
        public string AspNetVersion;

        [DataMember]
        [BsonElement("iis_service_state")]
        [BsonIgnoreIfNullAttribute]
        public string IISServiceState;

        [DataMember]
        [BsonElement("iis_version")]
        [BsonIgnoreIfNullAttribute]
        public string IISVersion;

        [DataMember]
        [BsonElement("iis_app_requests")]
        [BsonIgnoreIfNullAttribute]
        public string IISAppRequests;

        [DataMember]
        [BsonElement("iis_app_requests_rejected")]
        [BsonIgnoreIfNullAttribute]
        public string IISAppRequestsRejected;

        [DataMember]
        [BsonElement("iis_current_connections")]
        [BsonIgnoreIfNullAttribute]
        public string IISCurrentConnections;

        [DataMember]
        [BsonElement("network_bytes_received")]
        [BsonIgnoreIfNullAttribute]
        public string NetworkBytesReceived;

        [DataMember]
        [BsonElement("network_bytes_sent")]
        [BsonIgnoreIfNullAttribute]
        public string NetworkBytesSent;

        [DataMember]
        [BsonElement("web_services_bytes_received")]
        [BsonIgnoreIfNullAttribute]
        public string WebServiceBytesReceived;

        [DataMember]
        [BsonElement("web_services_bytes_sent")]
        [BsonIgnoreIfNullAttribute]
        public string WebServiceBytesSent;

        #endregion


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

        [DataMember]
        [BsonElement("last_update")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastUpdate { get; set; }
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

        [DataMember]
        [BsonElement("server_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("size_mb")]
        [BsonIgnoreIfNullAttribute]
        public double? SizeMB { get; set; }

        [DataMember]
        [BsonElement("white_space_mb")]
        [BsonIgnoreIfNullAttribute]
        public double? WhiteSpaceMB { get; set; }

        [DataMember]
        [BsonElement("mailbox_count")]
        [BsonIgnoreIfNullAttribute]
        public int? MailboxCount { get; set; }

        [DataMember]
        [BsonElement("disconnected_mailbox_count")]
        [BsonIgnoreIfNullAttribute]
        public int? DisconnectedMailboxCount { get; set; }

        [DataMember]
        [BsonElement("connected_mailbox_count")]
        [BsonIgnoreIfNullAttribute]
        public int? ConnectedMailboxCount { get; set; }


        #region DatabaseBackupDetails
        [DataMember]
        [BsonElement("storage_group")]
        [BsonIgnoreIfNullAttribute]
        public String StorageGroup { get; set; }

        [DataMember]
        [BsonElement("mounted")]
        [BsonIgnoreIfNullAttribute]
        public bool? Mounted { get; set; }

        [DataMember]
        [BsonElement("backup_in_progress")]
        [BsonIgnoreIfNullAttribute]
        public bool? BackupInProgress { get; set; }

        [DataMember]
        [BsonElement("online_maintenance_in_progress")]
        [BsonIgnoreIfNullAttribute]
        public bool? OnlineMaintenanceInProgress { get; set; }

        [DataMember]
        [BsonElement("last_full_backup_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastFullBackupDate { get; set; }

        [DataMember]
        [BsonElement("last_incremental_backup_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastIncrementalBackupDate { get; set; }

        [DataMember]
        [BsonElement("last_differential_backup_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastDifferentialBackupDate { get; set; }

        [DataMember]
        [BsonElement("last_copy_backup_date")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? LastCopyBackupDate { get; set; }

        #endregion
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

    public class SharePointWebApplication : Entity
    {

        public SharePointWebApplication()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("web_application_name")]
        [BsonIgnoreIfNullAttribute]
        public string WebApplicationName { get; set; }

        [DataMember]
        [BsonElement("content_database_name")]
        [BsonIgnoreIfNullAttribute]
        public string ContentDatabaseName { get; set; }

        [DataMember]
        [BsonElement("content_database_id")]
        [BsonIgnoreIfNullAttribute]
        public string ContentDatabaseId { get; set; }

        [DataMember]
        [BsonElement("database_site_count")]
        [BsonIgnoreIfNullAttribute]
        public int? DatabaseSiteCount { get; set; }

        [DataMember]
        [BsonElement("max_site_count_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? MaxSiteCountThreshold { get; set; }

        [DataMember]
        [BsonElement("warning_site_count_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? WarningSiteCountThreshold { get; set; }

        [DataMember]
        [BsonElement("content_database_read_only")]
        [BsonIgnoreIfNullAttribute]
        public bool? ContentDBReadOnly { get; set; }

        [DataMember]
        [BsonElement("database_server")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseServer { get; set; }

        
    }


    public class SharePointSiteCollection : Entity
    {

        public SharePointSiteCollection()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("url")]
        [BsonIgnoreIfNullAttribute]
        public string URL { get; set; }

        [DataMember]
        [BsonElement("size_mb")]
        [BsonIgnoreIfNullAttribute]
        public double? SizeMB { get; set; }

        [DataMember]
        [BsonElement("owner")]
        [BsonIgnoreIfNullAttribute]
        public string Owner { get; set; }

        [DataMember]
        [BsonElement("site_count")]
        [BsonIgnoreIfNullAttribute]
        public int? SiteCount { get; set; }

        [DataMember]
        [BsonElement("web_application")]
        [BsonIgnoreIfNullAttribute]
        public string WebApplication { get; set; }

    }


    public class SharePointTimerJob : Entity
    {
        public SharePointTimerJob()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("job_definition_title")]
        [BsonIgnoreIfNullAttribute]
        public string JobDefinitionTitle { get; set; }

        [DataMember]
        [BsonElement("web_application_name")]
        [BsonIgnoreIfNullAttribute]
        public string WebApplicationName { get; set; }

        [DataMember]
        [BsonElement("server_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNullAttribute]
        public string Status { get; set; }

        [DataMember]
        [BsonElement("start_time")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? StartTime { get; set; }

        [DataMember]
        [BsonElement("end_time")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? EndTime { get; set; }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNullAttribute]
        public string DatabaseName { get; set; }

        [DataMember]
        [BsonElement("error_message")]
        [BsonIgnoreIfNullAttribute]
        public string ErrorMessage { get; set; }

        [DataMember]
        [BsonElement("schedule")]
        [BsonIgnoreIfNullAttribute]
        public string Schedule { get; set; }
    }

    public class SharePointSiteActivity : Entity
    {
        public SharePointSiteActivity()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("server_relative_url")]
        [BsonIgnoreIfNullAttribute]
        public string ServerRelativeUrl { get; set; }

        [DataMember]
        [BsonElement("user_name")]
        [BsonIgnoreIfNullAttribute]
        public string UserName { get; set; }

        [DataMember]
        [BsonElement("hit_count")]
        [BsonIgnoreIfNullAttribute]
        public string HitCount { get; set; }

    }



    public class Disk : Entity
    {
        public Disk()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("disk_name")]
        [BsonIgnoreIfNullAttribute]
        public string DiskName { get; set; }

        [DataMember]
        [BsonElement("disk_free")]
        [BsonIgnoreIfNullAttribute]
        public double? DiskFree { get; set; }

        [DataMember]
        [BsonElement("disk_size")]
        [BsonIgnoreIfNullAttribute]
        public double? DiskSize { get; set; }

        [DataMember]
        [BsonElement("percent_free")]
        [BsonIgnoreIfNullAttribute]
        public double? PercentFree { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365AccountStats")]
    public class Office365 : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public string ServerId { get; set; }

        [DataMember]
        [BsonElement("total_active_user_mailboxes")]
        public string TotalActiveUserMailboxes { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("active_units")]
        public string ActiveUnits { get; set; }

        [DataMember]
        [BsonElement("warning_units")]
        public double WarningUnits { get; set; }

        [DataMember]
        [BsonElement("consumed_units")]
        public int ConsumedUnits { get; set; }

        [DataMember]
        [BsonElement("license_type")]
        public string LicenseType { get; set; }

        [DataMember]
        [BsonElement("street")]
        public string Street { get; set; }

        [DataMember]
        [BsonElement("preferred_language")]
        public string PreferredLanguage { get; set; }

        [DataMember]
        [BsonElement("city")]
        public string City { get; set; }

        [DataMember]
        [BsonElement("state")]
        public string State { get; set; }

        [DataMember]
        [BsonElement("country")]
        public string Country { get; set; }

        [DataMember]
        [BsonElement("postal_code")]
        public string PostalCode { get; set; }

        [DataMember]
        [BsonElement("telephone")]
        public string Telephone { get; set; }

        [DataMember]
        [BsonElement("technical_notification_email")]
        public string TechnicalNotificationEmails { get; set; }
    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365ServiceDetails")]
    public class Office365ServiceDetails : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("service_name")]
        public string ServiceName { get; set; }

        [DataMember]
        [BsonElement("service_id")]
        public string ServiceID { get; set; }

        [DataMember]
        [BsonElement("start_time")]
        public DateTime  StartTime { get; set; }

        [DataMember]
        [BsonElement("end_time")]
        public DateTime  EndTime { get; set; }

        [DataMember]
        [BsonElement("status")]
        public string Status { get; set; }

        [DataMember]
        [BsonElement("event_type")]
        public string EventType { get; set; }

        [DataMember]
        [BsonElement("message")]
        public string Message { get; set; }

       
    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365MSOLUsers")]
    public class Office365MSOLUsers : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("display_name")]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("first_name")]
        public string FirstName { get; set; }

        [DataMember]
        [BsonElement("last_name")]
        public string LastName { get; set; }

        [DataMember]
        [BsonElement("user_principal_name")]
        public string UserPrincipalName { get; set; }

        [DataMember]
        [BsonElement("user_type")]
        public string UserType { get; set; }

        [DataMember]
        [BsonElement("title")]
        public string Title { get; set; }

        [DataMember]
        [BsonElement("is_licensed")]
        public string IsLicensed { get; set; }

        [DataMember]
        [BsonElement("department")]
        public string Department { get; set; }

        [DataMember]
        [BsonElement("strong_password_required")]
        public string StrongPasswordRequired { get; set; }

        [DataMember]
        [BsonElement("passwprd_never_expires")]
        public string PasswordNeverExpires { get; set; }

        [DataMember]
        [BsonElement("group_member_type")]
        public string GroupMemberType { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365Groups")]
    public class Office365Groups : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("group_id")]
        public string GroupId { get; set; }

        [DataMember]
        [BsonElement("group_name")]
        public string GroupName { get; set; }

        [DataMember]
        [BsonElement("group_type")]
        public string GroupType { get; set; }

        [DataMember]
        [BsonElement("group_description")]
        public string GroupDescription { get; set; }

        [DataMember]
        [BsonElement("members")]
        public Office365GroupMembers[] Members { get; set; }

    }
    [DataContract]
    [Serializable]
    [CollectionName("Office365GroupMembers")]
    public class Office365GroupMembers : Entity
    {
        [DataMember]
        [BsonElement("user_principle_name")]
        public string UserPrincipleName { get; set; }
    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365LyncStats")]
    public class Office365LyncStats : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("active_users")]
        public int ActiveUsers { get; set; }

        [DataMember]
        [BsonElement("active_im_users")]
        public int ActiveIMUsers { get; set; }

        [DataMember]
        [BsonElement("active_audio_users")]
        public int  ActiveAudioUsers { get; set; }

        [DataMember]
        [BsonElement("active_video_users")]
        public int ActiveVideoUsers { get; set; }

        [DataMember]
        [BsonElement("active_application_sharing_users")]
        public int ActiveApplicationSharingUsers { get; set; }

        [DataMember]
        [BsonElement("active_file_transfer_users")]
        public int ActiveFileTransferUsers { get; set; }
    }
    [DataContract]
    [Serializable]
    [CollectionName("Office365LyncDevices")]
    public class Office365LyncDevices : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("windows_users")]
        public int WindowsUsers { get; set; }

        [DataMember]
        [BsonElement("windows_phone_users")]
        public int WindowsPhoneUsers { get; set; }

        [DataMember]
        [BsonElement("android_users")]
        public int AndroidUsers { get; set; }

        [DataMember]
        [BsonElement("iphone_users")]
        public int IphoneUsers { get; set; }

        [DataMember]
        [BsonElement("ipad_users")]
        public int IpadUsers { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365LyncPAVTimeReport")]
    public class Office365LyncPAVTimeReport : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("total_audio_minutes")]
        public int TotalAudioMinutes { get; set; }

        [DataMember]
        [BsonElement("total_video_minutes")]
        public int TotalVideoMinutes { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365LyncP2PSessionReport")]
    public class Office365LyncP2PSessionReport : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("total_p2p_sessions")]
        public int TotalP2PSessions { get; set; }

        [DataMember]
        [BsonElement("p2p_im_sessions")]
        public int P2PIMSessions { get; set; }

        [DataMember]
        [BsonElement("p2p_audio_sessions")]
        public int P2PAudioSessions { get; set; }

        [DataMember]
        [BsonElement("p2p_video_sessions")]
        public int P2PVideoSessions { get; set; }

        [DataMember]
        [BsonElement("p2p_application_sharing_sessions")]
        public int P2PApplicationSharingSessions { get; set; }

        [DataMember]
        [BsonElement("p2p_file_transfer_sessions")]
        public int P2PFileTransferSessions { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("Office365LyncConferenceReport")]
    public class Office365LyncConferenceReport : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("account_name")]
        public string AccountName { get; set; }

        [DataMember]
        [BsonElement("total_conferences")]
        public int TotalConferences { get; set; }

        [DataMember]
        [BsonElement("av_conferences")]
        public int AVConferences { get; set; }

        [DataMember]
        [BsonElement("im_conferences")]
        public int IMConferences { get; set; }

        [DataMember]
        [BsonElement("application_sharing_conferences")]
        public int ApplicationSharingConferences { get; set; }

        [DataMember]
        [BsonElement("web_conferences")]
        public int WebConferences { get; set; }

        [DataMember]
        [BsonElement("telephony_conferences")]
        public int TelephonyConferences { get; set; }

    }
}
