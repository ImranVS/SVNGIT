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
    [CollectionName("server")]
    public class Server: Entity
    {

        public Server()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach(var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("device_name")]     
        [BsonIgnoreIfNull]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNull]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("description")]
        [BsonIgnoreIfNull]
        public string Description { get; set; }

        [DataMember]
        [BsonElement("location_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LocationId { get ; set; }

        [DataMember]
        [BsonElement("ip_address")]
        [BsonIgnoreIfNull]
        public string IPAddress { get; set; }

        [DataMember]
        [BsonElement("profile_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProfileId { get; set; }

        [DataMember]
        [BsonElement("business_hours_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BusinessHoursId { get; set; }

        [DataMember]
        [BsonElement("monthly_operating_cost")]
        [BsonIgnoreIfNull]
        public int? MonthlyOperatingCost { get; set; }

        [DataMember]
        [BsonElement("ideal_user_count")]
        [BsonIgnoreIfNull]
        public int? IdealUserCount { get; set; }

        [DataMember]
        [BsonElement("is_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? IsEnabled { get; set; }

        [DataMember]
        [BsonElement("scan_interval")]
        [BsonIgnoreIfNull]
        public int? ScanInterval { get; set; }

        [DataMember]
        [BsonElement("retry_interval")]
        [BsonIgnoreIfNull]
        public int? RetryInterval { get; set; }

        [DataMember]
        [BsonElement("off_hours_scan_interval")]
        [BsonIgnoreIfNull]
        public int? OffHoursScanInterval { get; set; }

        [DataMember]
        [BsonElement("category")]
        [BsonIgnoreIfNull]
        public string Category { get; set; }

        [DataMember]
        [BsonElement("cpu_threshold")]
        [BsonIgnoreIfNull]
        public double? CpuThreshold { get; set; }

        [DataMember]
        [BsonElement("memory_threshold")]
        [BsonIgnoreIfNull]
        public double? MemoryThreshold { get; set; }

        [DataMember]
        [BsonElement("response_time")]
        [BsonIgnoreIfNull]
        public int? ResponseTime { get; set; }

        [DataMember]
        [BsonElement("consecutive_failures_before_alert")]
        [BsonIgnoreIfNull]
        public int? ConsecutiveFailuresBeforeAlert { get; set; }

        [DataMember]
        [BsonElement("consecutive_over_threshold_before_alert")]
        [BsonIgnoreIfNull]
        public int? ConsecutiveOverThresholdBeforeAlert { get; set; }

        [DataMember]
        [BsonElement("credentials_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CredentialsId { get; set; }

        [DataMember]
        [BsonElement("disk_info")]
        [BsonIgnoreIfNull]
        public List<DiskSetting> DiskInfo { get; set; }

        [DataMember]
        [BsonElement("server_roles")]
        [BsonIgnoreIfNull]
        public List<String> ServerRoles { get; set; }

        [DataMember]
        [BsonElement("software_version")]
        [BsonIgnoreIfNull]
        public double? SoftwareVersion { get; set; }

        [DataMember]
        [BsonElement("maintenance_windows")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> MaintenanceWindows { get; set; }

        [DataMember]
        [BsonElement("scan_now")]
        [BsonIgnoreIfNull]
        public Boolean? ScanNow { get; set; }

        #region Domino

        [DataMember]
        [BsonElement("pending_mail_threshold")]
        [BsonIgnoreIfNull]
        public int? PendingMailThreshold { get; set; }

        [DataMember]
        [BsonElement("dead_mail_threshold")]
        [BsonIgnoreIfNull]
        public int? DeadMailThreshold { get; set; }

        [DataMember]
        [BsonElement("mail_directory")]
        [BsonIgnoreIfNull]
        public string MailDirectory{ get; set; }

        [DataMember]
        [BsonElement("dead_mail_delete_threshold")]
        [BsonIgnoreIfNull]
        public int? DeadMailDeleteThreshold { get; set; }

        [DataMember]
        [BsonElement("held_mail_threshold")]
        [BsonIgnoreIfNull]
        public int? HeldMailThreshold { get; set; }

        [DataMember]
        [BsonElement("scan_db_health")]
        [BsonIgnoreIfNull]
        public Boolean? ScanDBHealth { get; set; }

        [DataMember]
        [BsonElement("cluster_replication_delay_threshold")]
        [BsonIgnoreIfNull]
        public int? ClusterReplicationDelayThreshold { get; set; }

        [DataMember]
        [BsonElement("cluster_replication_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? ClusterReplicationQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("server_days_alert")]
        [BsonIgnoreIfNull]
        public int? ServerDaysAlert { get; set; }

        [DataMember]
        [BsonElement("require_ssl")]
        [BsonIgnoreIfNull]
        public Boolean? RequireSSL { get; set; }

        [DataMember]
        [BsonElement("traveler_external_alias")]
        [BsonIgnoreIfNull]
        public string TravelerExternalAlias { get; set; }

        [DataMember]
        [BsonElement("check_mail_threshold")]
        [BsonIgnoreIfNull]
        public Boolean? CheckMailThreshold { get; set; }

        [DataMember]
        [BsonElement("scan_log")]
        [BsonIgnoreIfNull]
        public Boolean? ScanLog { get; set; }

        [DataMember]
        [BsonElement("scan_agent_log")]
        [BsonIgnoreIfNull]
        public Boolean? ScanAgentLog { get; set; }

        [DataMember]
        [BsonElement("send_router_restart")]
        [BsonIgnoreIfNull]
        public Boolean? SendRouterRestart { get; set; }

        [DataMember]
        [BsonElement("exjournal_start_time")]
        [BsonIgnoreIfNull]
        public DateTime? EXJournalStartTime { get; set; }

        [DataMember]
        [BsonElement("exjournal_duration")]
        [BsonIgnoreIfNull]
        public int? EXJournalDuration { get; set; }

        [DataMember]
        [BsonElement("exjournal_lookback_duration")]
        [BsonIgnoreIfNull]
        public int? EXJournalLookbackDuration { get; set; }

        [DataMember]
        [BsonElement("exjournal_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? EXJournalEnabled { get; set; }

        [DataMember]
        [BsonElement("availability_index_threshold")]
        [BsonIgnoreIfNull]
        public int? AvailabilityIndexThreshold { get; set; }

        [DataMember]
        [BsonElement("server_tasks")]
        [BsonIgnoreIfNull]
        public List<DominoServerTask> ServerTasks { get; set; }

        [DataMember]
        [BsonElement("database_count")]
        [BsonIgnoreIfNull]
        public int? DatabaseCount { get; set; }

        [DataMember]
        [BsonElement("scan_traveler_server")]
        [BsonIgnoreIfNull]
        public Boolean? ScanTravelerServer { get; set; }

        [DataMember]
        [BsonElement("domino_custom_stats")]
        [BsonIgnoreIfNull]
        public List<DominoCustomStat> DominoCustomStats { get; set; }

        #endregion

        #region TravelerHA

        //[DataMember]
        //[BsonElement("traveler_service_pool_name")]
        //[BsonIgnoreIfNull]
        //public string TravelerServicePoolName { get; set; }

        [DataMember]
        [BsonElement("traveler_service_server_name")]
        [BsonIgnoreIfNull]
        public string TravelerServiceServerName { get; set; }

        [DataMember]
        [BsonElement("datastore")]
        [BsonIgnoreIfNull]
        public string Datastore { get; set; }

        [DataMember]
        [BsonElement("test_scan_server")]
        [BsonIgnoreIfNull]
        public string TestScanServer { get; set; }

        [DataMember]
        [BsonElement("used_by_servers")]
        [BsonIgnoreIfNull]
        public string UsedByServers { get; set; }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNull]
        public string DatabaseName { get; set; }

        #endregion

        #region Exchange

        [DataMember]
        [BsonElement("are_prerequisites_done")]
        [BsonIgnoreIfNull]
        public Boolean? ArePrerequisitesDone { get; set; }

        [DataMember]
        [BsonElement("submission_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? SubmissionQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("poison_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? PosionQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("unreachable_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? UnreachableQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("shadow_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? ShadowQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("total_queue_threshold")]
        [BsonIgnoreIfNull]
        public int? TotalQueueThreshold { get; set; }

        [DataMember]
        [BsonElement("enable_latency_test")]
        [BsonIgnoreIfNull]
        public Boolean? EnableLatencyTest { get; set; }

        [DataMember]
        [BsonElement("authentication_type")]
        [BsonIgnoreIfNull]
        public string AuthenticationType { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("cas_tests")]
        public List<CASTest> CASTests { get; set; }

        #endregion

        #region URL

        [DataMember]
        [BsonElement("search_string")]
        [BsonIgnoreIfNull]
        public string SearchString { get; set; }

        [DataMember]
        [BsonElement("alert_if_string_found")]
        [BsonIgnoreIfNull]
        public Boolean? AlertIfStringFound { get; set; }

        [DataMember]
        [BsonElement("username")]
        [BsonIgnoreIfNull]
        public string Username { get; set; }

        [DataMember]
        [BsonElement("password")]
        [BsonIgnoreIfNull]
        public string Password { get; set; }

        #endregion

        #region Sametime

        [DataMember]
        [BsonElement("platform")]
        [BsonIgnoreIfNull]
        public string Platform { get; set; }

        [DataMember]
        [BsonElement("test_chat_simulation")]
        [BsonIgnoreIfNull]
        public Boolean? TestChatSimulation { get; set; }

        [DataMember]
        [BsonElement("user1_credentials_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string User1CredentialsId { get; set; }

        [DataMember]
        [BsonElement("user2_credentials_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string User2CredentialsId { get; set; }

        [DataMember]
        [BsonElement("proxy_server_type")]
        [BsonIgnoreIfNull]
        public string ProxyServerType { get; set; }

        [DataMember]
        [BsonElement("proxy_server_protocol")]
        [BsonIgnoreIfNull]
        public string ProxyServerprotocol { get; set; }

        [DataMember]
        [BsonElement("dbms_host_name")]
        [BsonIgnoreIfNull]
        public string DbmsHostName { get; set; }

        [DataMember]
        [BsonElement("dbms_name")]
        [BsonIgnoreIfNull]
        public string DbmsName { get; set; }

        [DataMember]
        [BsonElement("dbms_port")]
        [BsonIgnoreIfNull]
        public int? DbmsPort { get; set; }

        [DataMember]
        [BsonElement("db2_settings_credentials_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Db2SettingsCredentialsId { get; set; }


        [DataMember]
        [BsonElement("collect_extended_statistics")]
        [BsonIgnoreIfNull]
        public Boolean? CollectExtendedStatistics { get; set; }

        [DataMember]
        [BsonElement("extended_statistics_port")]
        [BsonIgnoreIfNull]
        public int? ExtendedStatisticsPort{ get; set; }

        [DataMember]
        [BsonElement("collect_meeting_statistics")]
        [BsonIgnoreIfNull]
        public Boolean? CollectMeetingStatistics { get; set; }

        [DataMember]
        [BsonElement("collect_conference_statistics")]
        [BsonIgnoreIfNull]
        public Boolean? CollectConferenceStatistics { get; set; }

        [DataMember]
        [BsonElement("meeting_host_name")]
        [BsonIgnoreIfNull]
        public string MeetingHostName { get; set; }

        [DataMember]
        [BsonElement("meeting_port")]
        [BsonIgnoreIfNull]
        public int? MeetingPort { get; set; }

        [DataMember]
        [BsonElement("meeting_require_ssl")]
        [BsonIgnoreIfNull]
        public Boolean? MeetingRequireSSL { get; set; }

        //[DataMember]
        //[BsonElement("collect_conference_statistics")]
        //[BsonIgnoreIfNull]
        //public Boolean? CollectConferenceStatistics { get; set; }

        [DataMember]
        [BsonElement("conference_host_name")]
        [BsonIgnoreIfNull]
        public string ConferenceHostName { get; set; }

        [DataMember]
        [BsonElement("conference_port")]
        [BsonIgnoreIfNull]
        public int? ConferencePort { get; set; }

        [DataMember]
        [BsonElement("conference_require_ssl")]
        [BsonIgnoreIfNull]
        public Boolean? ConferenceRequireSSL { get; set; }

        #endregion

        #region SharePoint

        [DataMember]
        [BsonElement("site_collection_health_check")]
        [BsonIgnoreIfNull]
        public List<string> SiteCollectionHealthCheck { get; set; }

        #endregion

        #region SharePointFarm

        [DataMember]
        [BsonElement("log_on_test_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? LogOnTestEnabled { get; set; }

        [DataMember]
        [BsonElement("site_creation_test_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? SiteCreationTestEnabled { get; set; }

        [DataMember]
        [BsonElement("file_upload_test_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? FileUploadTestEnabled { get; set; }

        [DataMember]
        [BsonElement("application_url_test_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? ApplicationUrlTestEnabled { get; set; }

        #endregion

        #region Mail

        [DataMember]
        [BsonElement("port_number")]
        [BsonIgnoreIfNull]
        public int? PortNumber { get; set; }

        #endregion

        #region NetworkDevice

        //In Cloud
        //[DataMember]
        //[BsonElement("image_url")]
        //[BsonIgnoreIfNull]
        //public string ImageUrl { get; set; }

        [DataMember]
        [BsonElement("network_type")]
        [BsonIgnoreIfNull]
        public string NetworkType { get; set; }

        [DataMember]
        [BsonElement("display_on_dashboard")]
        [BsonIgnoreIfNull]
        public Boolean? DisplayOnDashboard { get; set; }

        //In URL
        //[DataMember]
        //[BsonElement("username")]
        //[BsonIgnoreIfNull]
        //public string Username { get; set; }

        //[DataMember]
        //[BsonElement("password")]
        //[BsonIgnoreIfNull]
        //public string Password { get; set; }


        #endregion

        #region NotesDatabase

        [DataMember]
        [BsonElement("domino_server_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DominoServerId { get; set; }

        [DataMember]
        [BsonElement("domino_server_name")]
        [BsonIgnoreIfNull]
        public string DominoServerName { get; set; }

        [DataMember]
        [BsonElement("database_file_name")]
        [BsonIgnoreIfNull]
        public string DatabaseFileName { get; set; }

        [DataMember]
        [BsonElement("trigger_type")]
        [BsonIgnoreIfNull]
        public string TriggerType { get; set; }

        [DataMember]
        [BsonElement("trigger_value")]
        [BsonIgnoreIfNull]
        public int? TriggerValue { get; set; }

        [DataMember]
        [BsonElement("initiate_replication")]
        [BsonIgnoreIfNull]
        public bool? InitiateReplication { get; set; }

        [DataMember]
        [BsonElement("replication_destination")]
        [BsonIgnoreIfNull]
        public List<String> ReplicationDestination { get; set; }

        #endregion

        #region NotesDatabaseReplica

        [DataMember]
        [BsonElement("difference_threshold")]
        [BsonIgnoreIfNull]
        public int? DifferenceThreshold { get; set; }

        [DataMember]
        [BsonElement("domino_server_a")]
        [BsonIgnoreIfNull]
        public string DominoServerA { get; set; }

        [DataMember]
        [BsonElement("domino_server_a_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerAFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_a_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerAExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("domino_server_b")]
        [BsonIgnoreIfNull]
        public string DominoServerB { get; set; }

        [DataMember]
        [BsonElement("domino_server_b_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerBFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_b_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerBExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("domino_server_c")]
        [BsonIgnoreIfNull]
        public string DominoServerC { get; set; }

        [DataMember]
        [BsonElement("domino_server_c_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerCFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_c_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerCExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("first_alert_threshold")]
        [BsonIgnoreIfNull]
        public int? FirstAlertThreshold { get; set; }



        #endregion

        #region NotesMailProbe

        [DataMember]
        [BsonElement("send_to_echo_service")]
        [BsonIgnoreIfNullAttribute]
        public Boolean? SendToEchoService { get; set; }

        [DataMember]
        [BsonElement("source_server")]
        [BsonIgnoreIfNullAttribute]
        public string SourceServer { get; set; }

        [DataMember]
        [BsonElement("send_to_address")]
        [BsonIgnoreIfNullAttribute]
        public string SendToAddress { get; set; }

        [DataMember]
        [BsonElement("reply_to_address")]
        [BsonIgnoreIfNullAttribute]
        public string ReplyToAddress { get; set; }

        [DataMember]
        [BsonElement("target_server")]
        [BsonIgnoreIfNullAttribute]
        public string TargetServer { get; set; }

        [DataMember]
        [BsonElement("target_database")]
        [BsonIgnoreIfNullAttribute]
        public string TargetDatabase { get; set; }

        [DataMember]
        [BsonElement("file_name")]
        [BsonIgnoreIfNullAttribute]
        public string FileName { get; set; }

        [DataMember]
        [BsonElement("delivery_threshold")]
        [BsonIgnoreIfNullAttribute]
        public int? DeliveryThreshold { get; set; }

        #endregion

        #region Cloud

        [DataMember]
        [BsonElement("image_url")]
        [BsonIgnoreIfNullAttribute]
        public string ImageUrl { get; set; }

        [DataMember]
        [BsonElement("search_text")]
        [BsonIgnoreIfNullAttribute]
        public string SearchText { get; set; }

        //In URL
        //[DataMember]
        //[BsonElement("username")]
        //[BsonIgnoreIfNullAttribute]
        //public string Username { get; set; }

        //[DataMember]
        //[BsonElement("password")]
        //[BsonIgnoreIfNullAttribute]
        //public string Password { get; set; }

        #endregion

        #region DAG

        [DataMember]
        [BsonElement("primary_server_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PrimaryServerId { get; set; }

        [DataMember]
        [BsonElement("backup_server_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BackupServerId { get; set; }

        [DataMember]
        [BsonElement("database_info")]
        [BsonIgnoreIfNullAttribute]
        public List<DagDatabases> DatabaseInfo { get; set; }

        #endregion

        #region Office365

        //In Cloud
        //[DataMember]
        //[BsonElement("image_url")]
        //[BsonIgnoreIfNullAttribute]
        //public string ImageUrl { get; set; }

        [DataMember]
        [BsonElement("cost_per_user")]
        [BsonIgnoreIfNullAttribute]
        public int? CostPerUser { get; set; }

        [DataMember]
        [BsonElement("mode")]
        [BsonIgnoreIfNullAttribute]
        public string Mode { get; set; }

        [DataMember]
        [BsonElement("directory_sync_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string DirectorySyncServerName { get; set; }

        [DataMember]
        [BsonElement("directory_sync_credentials_id")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DirectorySyncCredentialsId { get; set; }

        //In URL
        //[DataMember]
        //[BsonElement("username")]
        //[BsonIgnoreIfNullAttribute]
        //public string Username { get; set; }

        //[DataMember]
        //[BsonElement("password")]
        //[BsonIgnoreIfNullAttribute]
        //public string Password { get; set; }

        [DataMember]
        [BsonElement("node_ids")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> NodeIds { get; set; }

        [DataMember]
        [BsonElement("pass_fail_tests")]
        [BsonIgnoreIfNullAttribute]
        public List<string> PassFailTests { get; set; }

        #endregion

        #region WebSphereCell

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("connection_type")]
        public string ConnectionType { get; set; }


        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("cell_host_name")]
        public string CellHostName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("global_security")]
        public bool GlobalSecurity { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("realm")]
        public string Realm { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("nodes")]
        public List<WebSphereNode> Nodes { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("sametime_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SametimeId { get; set; }

        #endregion

        #region WebSphereNode

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("cell_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CellId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("server_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ServerId { get; set; }

        #endregion

        #region WebSphereServer

        //InWebSphereNode
        //[DataMember]
        //[BsonIgnoreIfNull]
        //[BsonElement("cell_id")]
        //public ObjectId? CellId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("node_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NodeId { get; set; }

        
        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("node_name")]
        public String NodeName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("cell_name")]
        public String CellName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("average_thread_pool")]
        public int? AverageThreadPool { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("heap_current")]
        public int? HeapCurrent { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("up_current")]
        public int? UPCurrent { get; set; }


        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("dump_generator")]
        public int? DumpGenerator { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("active_thread_count")]
        public int? ActiveThreadCount { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("maximum_heap")]
        public int? MaximumHeap { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("hung_thread_count")]
        public int? HungThreadCount { get; set; }

        #endregion

        #region NetworkLatency

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("test_duration")]
        public int? TestDuration { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("network_latency_servers")]
        public List<NetworkLatencyServer> NetworkLatencyServers { get; set; }

        #endregion

        #region IbmConnections

        //In Domino
        //[DataMember]
        //[BsonIgnoreIfNull]
        //[BsonElement("require_ssl")]
        //public Boolean? RequireSSL { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("database_settings_host_name")]
        public string DatabaseSettingsHostName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("database_settings_credentials_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatabaseSettingsCredentialsId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("database_settings_port")]
        public int? DatabaseSettingsPort{ get; set; }


        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("simulation_tests")]
        public List<NameValuePair> SimulationTests { get; set; }

        #endregion

        #region DominoLogScanning

        [DataMember]
        [BsonElement("log_file_keywords")]
        [BsonIgnoreIfNull]
        public List<LogFileKeyword> LogFileKeywords { get; set; }

        [DataMember]
        [BsonElement("log_file_servers")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> LogFileServers { get; set; }

        #endregion

        [DataMember]
        [BsonElement("farm_servers")]
        [BsonIgnoreIfNull]
        public List<String> FarmServers { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("windows_services")]
        public List<WindowServices> WindowServices { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("notifications")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> NotificationList { get; set; }

        [DataMember]
        [BsonElement("assigned_node")]
        [BsonIgnoreIfNull]
        public string AssignedNode { get; set; }

        [DataMember]
        [BsonElement("current_node")]
        [BsonIgnoreIfNull]
        public string CurrentNode { get; set; }

        [DataMember]
        [BsonElement("license_cost")]
        [BsonIgnoreIfNull]
        public double LicenseCost { get; set; }
        
    }

    public class WindowServices
    {

        public WindowServices()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach(var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("server_required")]
        [BsonIgnoreIfNullAttribute]
        public bool ServerRequired { get; set; }


        [DataMember]
        [BsonElement("service_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServiceName { get; set; }

        [DataMember]
        [BsonElement("display_name")]
        [BsonIgnoreIfNullAttribute]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("startup_mode")]
        [BsonIgnoreIfNullAttribute]
        public string StartupMode { get; set; }

        [DataMember]
        [BsonElement("monitored")]
        [BsonIgnoreIfNullAttribute]
        public bool Monitored { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNullAttribute]
        public string Status { get; set; }
    }

    public class CASTest
    {

        public CASTest()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("test_name")]
        public string TestName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("url")]
        public string URL { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("credentials_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CredentialsId { get; set; }
    }

    public class Tests
    {

        public Tests()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("name")]
        public string TestName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("threshold")]
        public int? Threshold { get; set; }

    }

    public class WebSphereNode
    {

        public WebSphereNode()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("node_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NodeId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("node_name")]
        public string NodeName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("host_name")]
        public string HostName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("websphere_servers")]
        public List<WebSphereServer> WebSphereServers { get; set; }

    }

    public class WebSphereServer
    {

        public WebSphereServer()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("server_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServerId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("server_name")]
        public string ServerName { get; set; }

    }

    public class NetworkLatencyServer
    {

        public NetworkLatencyServer()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("yellow_threshold")]
        public int? YellowThreshold { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("red_threshold")]
        public int? RedThreshold { get; set; }
    }

    public class DominoCustomStat
    {

        public DominoCustomStat()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("stat_name")]
        public String StatName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("yellow_threshold")]
        public String ThresholdValue { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("greater_than_or_less_than")]
        public String GreaterThanOrLessThan { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("times_in_a_row")]
        public int? TimesInARow { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("console_command")]
        public String ConsoleCommand { get; set; }
    }

    public class LogFileKeyword
    {

      

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("keyword")]
        public String Keyword { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("exclude")]
        public String Exclude { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("one_alert_per_day")]
        public Boolean? OneAlertPerDay { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("scan_log")]
        public Boolean? ScanLog { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("scan_agent_log")]
        public Boolean? ScanAgentLog { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("event_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EventId { get; set; }

        //[DataMember]
        //[BsonElement("key_id")]
        //[BsonIgnoreIfNullAttribute]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string TaskId { get; set; }

    }

    public class NameValuePair
    {
        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("name")]
        public string Name { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("value")]
        public string Value { get; set; }

    }

    public class DiskSetting
    {
        public DiskSetting()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }
        //added for disk data migration in server
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("disk_name")]
        [BsonIgnoreIfNullAttribute]
        public string DiskName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("threshold")]
        public double? Threshold { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("threshold_type")]
        public string ThresholdType { get; set; }
    }

}
