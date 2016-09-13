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
        [BsonElement("device_type")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("secondary_role")]
        [BsonIgnoreIfNullAttribute]
        public string SecondaryRole { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNullAttribute]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNullAttribute]
        public string DeviceId { get; set; }

        //"last_update" : "2015-08-05T08:40:51.620Z",
        [DataMember]
        [BsonElement("next_scan")]
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
        [BsonElement("held_mail")]
        [BsonIgnoreIfNullAttribute]
        public int? HeldMail { get; set; }

        [DataMember]
        [BsonElement("my_percent")]
        [BsonIgnoreIfNullAttribute]
        public double? MyPercent { get; set; }

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
        public double? UpPercent { get; set; }

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
        [BsonElement("domino_server_tasks_status")]
        [BsonIgnoreIfNullAttribute]
        public string DominoServerTasksStatus { get; set; }

        [DataMember]
        [BsonElement("type_and_name")]
        [BsonIgnoreIfNullAttribute]
        public string TypeAndName { get; set; }

        [DataMember]
        [BsonElement("icon")]
        [BsonIgnoreIfNullAttribute]
        public string Icon { get; set; }

        [DataMember]
        [BsonElement("version")]
        [BsonIgnoreIfNullAttribute]
        public string Version { get; set; }

        [DataMember]
        [BsonElement("operating_system")]
        [BsonIgnoreIfNullAttribute]
        public string OperatingSystem { get; set; }

        [DataMember]
        [BsonElement("software_version")]
        [BsonIgnoreIfNullAttribute]
        public string SoftwareVersion { get; set; }

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
        public double? ElapsedDays { get; set; }

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
        [BsonElement("up_minutes")]
        [BsonIgnoreIfNullAttribute]
        public double? UpMinutes { get; set; }

        [DataMember]
        [BsonElement("down_minutes")]
        [BsonIgnoreIfNullAttribute]
        public double? DownMinutes { get; set; }

        [DataMember]
        [BsonElement("up_percent_minutes")]
        [BsonIgnoreIfNullAttribute]
        public double? UpPercentMinutes { get; set; }

        [DataMember]
        [BsonElement("version_architecture")]
        [BsonIgnoreIfNullAttribute]
        public string VersionArchitecture { get; set; }

        [DataMember]
        [BsonElement("cpu_count")]
        [BsonIgnoreIfNullAttribute]
        public int? CpuCount { get; set; }

        [DataMember]
        [BsonElement("disks")]
        [BsonIgnoreIfNull]
        public List<Disk> Disks { get; set; }

        [DataMember]
        [BsonElement("domino_version")]
        [BsonIgnoreIfNullAttribute]
        public string DominoVersion { get; set; }

        [DataMember]
        [BsonElement("up_percent_count")]
        [BsonIgnoreIfNullAttribute]
        public double? UpPercentCount { get; set; }

        [DataMember]
        [BsonElement("percentage_change")]
        [BsonIgnoreIfNullAttribute]
        public double? PercentageChange { get; set; }

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
        [BsonElement("asp_net_version")]
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

        #region LatencyTest

        [DataMember]
        [BsonElement("latency_results")]
        [BsonIgnoreIfNullAttribute]
        public List<LatencyResults> LatencyResults { get; set; }

        #endregion

        #region Domino

        [DataMember]
        [BsonElement("domino_server_tasks")]
        [BsonIgnoreIfNull]
        public List<DominoServerTask> DominoServerTasks { get; set; }

        [DataMember]
        [BsonElement("domain")]
        [BsonIgnoreIfNull]
        public string Domain { get; set; }


        [DataMember]
        [BsonElement("mailbox_performance_index")]
        [BsonIgnoreIfNull]
        public int? MailboxPerformanceIndex { get; set; }

        [DataMember]
        [BsonElement("mail_pending")]
        [BsonIgnoreIfNull]
        public int? MailPending { get; set; }

        [DataMember]
        [BsonElement("mail_dead")]
        [BsonIgnoreIfNull]
        public int? MailDead { get; set; }

        [DataMember]
        [BsonElement("mail_waiting")]
        [BsonIgnoreIfNull]
        public int? MailWaiting { get; set; }

        [DataMember]
        [BsonElement("mail_held")]
        [BsonIgnoreIfNull]
        public int? MailHeld { get; set; }

        [DataMember]
        [BsonElement("mail_max_size_delivered")]
        [BsonIgnoreIfNull]
        public int? MailMaximiumSizeDelivered { get; set; }

        [DataMember]
        [BsonElement("mail_peek_message_delivery_time")]
        [BsonIgnoreIfNull]
        public int? MailPeakMessageDeliveryTime { get; set; }

        [DataMember]
        [BsonElement("held_mail_threshold")]
        [BsonIgnoreIfNull]
        public int? HeldMailThreshold { get; set; }

        [DataMember]
        [BsonElement("mail_average_delivery_time")]
        [BsonIgnoreIfNull]
        public int? MailAverageDeliveryTime { get; set; }

        [DataMember]
        [BsonElement("mail_average_size_delivered")]
        [BsonIgnoreIfNull]
        public int? MailAverageSizeDelivered { get; set; }

        [DataMember]
        [BsonElement("mail_average_server_hopes")]
        [BsonIgnoreIfNull]
        public int? MailAverageServerHops { get; set; }

        [DataMember]
        [BsonElement("mail_transferred")]
        [BsonIgnoreIfNull]
        public int? MailTransferred { get; set; }

        [DataMember]
        [BsonElement("mail_delivered")]
        [BsonIgnoreIfNull]
        public int? MailDelivered { get; set; }

        [DataMember]
        [BsonElement("mail_transferred_nrpc")]
        [BsonIgnoreIfNull]
        public int? MailTransferredNRPC { get; set; }

        [DataMember]
        [BsonElement("mail_transferred_smtp")]
        [BsonIgnoreIfNull]
        public int? MailTransferredSMTP { get; set; }
        [DataMember]

        [BsonElement("mail_transfer_threads_active")]
        [BsonIgnoreIfNull]
        public int? MailTransferThreadsActive { get; set; }

        [DataMember]
        [BsonElement("mail_waiting_for_delivery_retry")]
        [BsonIgnoreIfNull]
        public int? MailWaitingForDeliveryRetry { get; set; }

        [DataMember]
        [BsonElement("mail_waiting_for_dir")]
        [BsonIgnoreIfNull]
        public int? MailWaitingForDIR { get; set; }

        [DataMember]
        [BsonElement("mail_waiting_for_dns")]
        [BsonIgnoreIfNull]
        public int? MailWaitingForDNS { get; set; }

        [DataMember]
        [BsonElement("mail_delivered_size_under_1kb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_Under_1KB { get; set; }

        [DataMember]
        [BsonElement("mail_delivered_size_1kb_to_10kb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_1KB_to_10KB { get; set; }

        [DataMember]
        [BsonElement("mail_delivered_size_10kb_to_100kb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_10KB_to_100KB { get; set; }
        
        [DataMember]
        [BsonElement("mail_delivered_size_100kb_to_1mb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_100KB_to_1MB { get; set; }

        [DataMember]
        [BsonElement("mail_delivered_size_1mb_to_10mb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_1MB_to_10MB { get; set; }

        [DataMember]
        [BsonElement("mail_delivered_size_10mb_to_100mb")]
        [BsonIgnoreIfNull]
        public int? MailDeliveredSize_10MB_to_100MB { get; set; }

        [DataMember]
        [BsonElement("mail_routed")]
        [BsonIgnoreIfNull]
        public int? MailRouted { get; set; }

        [DataMember]
        [BsonElement("mail_peek_messages_delivered")]
        [BsonIgnoreIfNull]
        public int? MailPeakMessagesDelivered { get; set; }

        [DataMember]
        [BsonElement("mail_peek_messages_transferred")]
        [BsonIgnoreIfNull]
        public int? MailPeakMessagesTransferred { get; set; }

        [DataMember]
        [BsonElement("mail_peek_message_transferred_time")]
        [BsonIgnoreIfNull]
        public int? MailPeakMessageTransferredTime { get; set; }

        [DataMember]
        [BsonElement("mail_recall_failures")]
        [BsonIgnoreIfNull]
        public int? MailRecallFailures { get; set; }

        [DataMember]
        [BsonElement("mail_waiting_recipients")]
        [BsonIgnoreIfNull]
        public int? MailWaitingRecipients { get; set; }

        [DataMember]
        [BsonElement("data_member")]
        [BsonIgnoreIfNull]
        public int? DataMember { get; set; }

        [DataMember]
        [BsonElement("cluster_name")]
        [BsonIgnoreIfNull]
        public string ClusterName { get; set; }

        [DataMember]
        [BsonElement("cluster_seconds_on_queue")]
        [BsonIgnoreIfNull]
        public int? ClusterSecondsOnQueue { get; set; }
        
        [DataMember]
        [BsonElement("cluster_seconds_on_queue_average")]
        [BsonIgnoreIfNull]
        public double? ClusterSecondsOnQueueAverage { get; set; }
        
        [DataMember]
        [BsonElement("cluster_seconds_on_queue_max")]
        [BsonIgnoreIfNull]
        public int? ClusterSecondsOnQueueMax { get; set; }
        
        [DataMember]
        [BsonElement("cluster_work_queue_depth")]
        [BsonIgnoreIfNull]
        public int? ClusterWorkQueueDepth { get; set; }

        [DataMember]
        [BsonElement("cluster_work_queue_depth_average")]
        [BsonIgnoreIfNull]
        public double? ClusterWorkQueueDepthAverage { get; set; }

        [DataMember]
        [BsonElement("cluster_work_queue_depth_max")]
        [BsonIgnoreIfNull]
        public int? ClusterWorkQueueDepthMax { get; set; }

        [DataMember]
        [BsonElement("cluster_availability")]
        [BsonIgnoreIfNull]
        public double? ClusterAvailability { get; set; }

        [DataMember]
        [BsonElement("cluster_availability_threshold")]
        [BsonIgnoreIfNull]
        public double? ClusterAvailabilityThreshold { get; set; }

        [DataMember]
        [BsonElement("cluster_analysis")]
        [BsonIgnoreIfNull]
        public string ClusterAnalysis { get; set; }



        #endregion

        #region Traveler

        [DataMember]
        [BsonElement("traveler_status")]
        [BsonIgnoreIfNull]
        public string TravelerStatus { get; set; }

        [DataMember]
        [BsonElement("traveler_details")]
        [BsonIgnoreIfNull]
        public string TravelerDetails { get; set; }

        [DataMember]
        [BsonElement("traveler_description")]
        [BsonIgnoreIfNull]
        public string TravelerDescription { get; set; }

        [DataMember]
        [BsonElement("traveler_users")]
        [BsonIgnoreIfNull]
        public int? TravelerUsers { get; set; }

        [DataMember]
        [BsonElement("traveler_device_count")]
        [BsonIgnoreIfNull]
        public int? TravelerDeviceCount { get; set; }

        [DataMember]
        [BsonElement("traveler_version")]
        [BsonIgnoreIfNull]
        public string TravelerVersion { get; set; }

        [DataMember]
        [BsonElement("traveler_incremental_syncs")]
        [BsonIgnoreIfNull]
        public int? TravelerIncrementalSyncs { get; set; }

        [DataMember]
        [BsonElement("traveler_availability_index")]
        [BsonIgnoreIfNull]
        public int? TravelerAvailabilityIndex { get; set; }
        
        [DataMember]
        [BsonElement("http_peak_connections")]
        [BsonIgnoreIfNull]
        public int? HttpPeakConnections { get; set; }

        [DataMember]
        [BsonElement("http_max_configured_connections")]
        [BsonIgnoreIfNull]
        public int? HttpMaxConfiguredConnections { get; set; }

        [DataMember]
        [BsonElement("http_status")]
        [BsonIgnoreIfNull]
        public string HttpStatus { get; set; }

        [DataMember]
        [BsonElement("http_details")]
        [BsonIgnoreIfNull]
        public string HttpDetails { get; set; }

        [DataMember]
        [BsonElement("resource_constraint")]
        [BsonIgnoreIfNull]
        public string ResourceConstraint { get; set; }

        [DataMember]
        [BsonElement("traveler_heartbeat")]
        [BsonIgnoreIfNull]
        public string TravelerHeartBeat { get; set; }

        [DataMember]
        [BsonElement("traveler_status_reasons")]
        [BsonIgnoreIfNull]
        public List<String> TravelerStatusReasons { get; set; }

        [DataMember]
        [BsonElement("traveler_ha")]
        [BsonIgnoreIfNull]
        public Boolean? TravelerHA { get; set; }

        [DataMember]
        [BsonElement("traveler_servlet")]
        [BsonIgnoreIfNull]
        public string TravelerServlet { get; set; }

        [DataMember]
        [BsonElement("traveler_devices_api_status")]
        [BsonIgnoreIfNull]
        public string TravelerDevicesAPIStatus { get; set; }

        #endregion

        #region WebSphere

        [DataMember]
        [BsonElement("jvm_count")]
        [BsonIgnoreIfNullAttribute]
        public int? JvmCount { get; set; }

        [DataMember]
        [BsonElement("jvm_monitored_count")]
        [BsonIgnoreIfNullAttribute]
        public int? JvmMonitoredCount { get; set; }

        #endregion




    }


    [DataContract]
    [Serializable]
    [CollectionName("status_details")]
    public class StatusDetails :Entity
    {

        [DataMember]
        [BsonElement("device_id")]
        public string DeviceId { get; set; }

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

    public class DagServers  
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


    public class DagDatabases 
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

        #region Configuration

        [DataMember]
        [BsonElement("copy_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? CopyThreshold { get; set; }

        [DataMember]
        [BsonElement("replay_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? ReplayThreshold { get; set; }

        [DataMember]
        [BsonElement("database_size_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? DatabaseSizeThreshold { get; set; }

        [DataMember]
        [BsonElement("database_white_space_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? DatabaseWhiteTSpaceThreshold { get; set; }

        #endregion
    }

    public class DagServerDatabases 
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

    public class SharePointWebApplication 
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


    public class SharePointSiteCollection 
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


    public class SharePointTimerJob 
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

    public class SharePointSiteActivity 
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



    public class Disk
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

        [DataMember]
        [BsonElement("average_queue_length")]
        [BsonIgnoreIfNullAttribute]
        public double? AverageQueueLength { get; set; }

        [DataMember]
        [BsonElement("disk_threshold")]
        [BsonIgnoreIfNullAttribute]
        public double? DiskThreshold { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("threshold")]
        public int? Threshold { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("threshold_type")]
        public string ThresholdType { get; set; }
    }

    public class DominoServerTask
    {
        public DominoServerTask()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("task_name")]
        [BsonIgnoreIfNullAttribute]
        public string TaskName { get; set; }

        [DataMember]
        [BsonElement("monitored")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? Monitored { get; set; }

        [DataMember]
        [BsonElement("status_summary")]
        [BsonIgnoreIfNullAttribute]
        public string StatusSummary { get; set; }

        [DataMember]
        [BsonElement("primary_status")]
        [BsonIgnoreIfNullAttribute]
        public string PrimaryStatus { get; set; }

        [DataMember]
        [BsonElement("secondary_status")]
        [BsonIgnoreIfNullAttribute]
        public string SecondaryStatus { get; set; }

        [DataMember]
        [BsonElement("last_updated")]
        [BsonIgnoreIfNullAttribute]
        public string LastUpdated { get; set; }

        [DataMember]
        [BsonElement("enabled")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? Enabled { get; set; }

        [DataMember]
        [BsonElement("send_load_cmd")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? SendLoadCmd { get; set; }

        [DataMember]
        [BsonElement("send_restart_cmd")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? SendRestartCmd { get; set; }

        [DataMember]
        [BsonElement("send_exit_cmd")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? SendExitCmd { get; set; }

        [DataMember]
        [BsonElement("send_restart_cmd_offhours")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? SendRestartCmdOffhours { get; set; }

        [DataMember]
        [BsonElement("console_string")]
        [BsonIgnoreIfNullAttribute]
        public String ConsoleString { get; set; }

        [DataMember]
        [BsonElement("freeze_detect")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? FreezeDetect { get; set; }

        [DataMember]
        [BsonElement("retry_count")]
        [BsonIgnoreIfNullAttribute]
        public int? RetryCount { get; set; }

        [DataMember]
        [BsonElement("max_busy_time")]
        [BsonIgnoreIfNullAttribute]
        public int? MaxBusyTime { get; set; }
    }


    [DataContract]
    [Serializable]
    [CollectionName("o365_account_stats")]
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
    [CollectionName("o365_service_details")]
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
    [CollectionName("o365_msol_users")]
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
    [CollectionName("o365_groups")]
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
    [CollectionName("o365_group_members")]
    public class Office365GroupMembers : Entity
    {
        [DataMember]
        [BsonElement("user_principle_name")]
        public string UserPrincipleName { get; set; }
    }

    [DataContract]
    [Serializable]
    [CollectionName("o365_lync_stats")]
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
    [CollectionName("o365_lync_devices")]
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
    [CollectionName("o365_lync_pav_time_report")]
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
    [CollectionName("o365_lync_p2p_session_report")]
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
    [CollectionName("o365_lync_conference_report")]
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

    [DataContract]
    [Serializable]
    [CollectionName("o365_users_license_services")]
    public class Office365UsersLicensesServices : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("display_name")]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("xml_configuration")]
        public string XMLConfiguration { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("network_device_details")]
    public class NetworkDeviceDetails : Entity
    {
        [DataMember]
        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [DataMember]
        [BsonElement("stat_name")]
        public string StatName { get; set; }

        [DataMember]
        [BsonElement("stat_value")]
        public double StatValue { get; set; }

    }

    public class LatencyResults
    {
        public LatencyResults()
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
        public string SourceServer { get; set; }

        [DataMember]
        [BsonElement("destination_server")]
        [BsonIgnoreIfNullAttribute]
        public string DestinationServer { get; set; }

        [DataMember]
        [BsonElement("latency")]
        [BsonIgnoreIfNullAttribute]
        public double? Latency { get; set; }

    }
}
