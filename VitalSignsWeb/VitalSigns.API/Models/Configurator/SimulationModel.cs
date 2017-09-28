using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class SimulationModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("create_activity_threshold")]
        public int CreateActivityThreshold { get; set; }

        [JsonProperty ("create_blog_threshold")]
        public int CreateBlogThreshold { get; set; }

        [JsonProperty("create_bookmark_threshold")]
        public int CreateBookmarkThreshold { get; set; }

        [JsonProperty("create_community_threshold")]
        public int CreateCommunityThreshold { get; set; }

        [JsonProperty("create_file_threshold")]
        public int CreateFileThreshold { get; set; }

        [JsonProperty("create_wiki_threshold")]
        public int CreateWikiThreshold { get; set; }

        [JsonProperty("search_profile_threshold")]
        public int SearchProfileThreshold { get; set; }


        [JsonProperty("create_activity")]
        public bool CreateActivity { get; set; }

        [JsonProperty("create_blog")]
        public bool CreateBlog { get; set; }

        [JsonProperty("create_bookmark")]
        public bool CreateBookmark { get; set; }

        [JsonProperty ("create_community")]
        public bool CreateCommunity { get; set; }

        [JsonProperty("create_file")]
        public bool CreateFile { get; set; }

        [JsonProperty("create_wiki")]
        public bool CreateWiki { get; set; }

        [JsonProperty ("search_profile")]
        public bool SearchProfile { get; set; }

        [JsonProperty("community_uuid")]
        public string CommunityUUID { get; set; }

        [JsonProperty("test_url")]
        public string TestUrl { get; set; }
    }

    public class TestsModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mail_flow_threshold")]
        public int MailFlowThreshold { get; set; }

        [JsonProperty("create_folder_threshold")]
        public int CreateFolderThreshold { get; set; }

        [JsonProperty("create_site_threshold")]
        public int CreateSiteThreshold { get; set; }

        [JsonProperty("onedrive_upload_treshold")]
        public int OneDriveUploadThreshold { get; set; }

        [JsonProperty("onedrive_download_threshold")]
        public int OneDriveDownloadThreshold { get; set; }

        [JsonProperty("mail_flow")]
        public bool MailFlow { get; set; }

        [JsonProperty("create_folder")]
        public bool CreateFolder { get; set; }

        [JsonProperty("create_site")]
        public bool CreateSite { get; set; }

        [JsonProperty("onedrive_upload")]
        public bool OneDriveUpload { get; set; }

        [JsonProperty("onedrive_download")]
        public bool OneDriveDownload { get; set; }

        [JsonProperty("smtp")]
        public bool SMTP { get; set; }

        [JsonProperty("auto_discovery")]
        public bool AutoDiscovery { get; set; }

        [JsonProperty("create_calendar")]
        public bool CreateCalendar { get; set; }

        [JsonProperty("imap")]
        public bool IMAP { get; set; }

        [JsonProperty("pop3")]
        public bool POP3 { get; set; }

        [JsonProperty("mapi_connectivity")]
        public bool MAPIConnectivity { get; set; }

        [JsonProperty("inbox")]
        public bool Inbox { get; set; }

        [JsonProperty("owa")]
        public bool OWA { get; set; }
    }
}
