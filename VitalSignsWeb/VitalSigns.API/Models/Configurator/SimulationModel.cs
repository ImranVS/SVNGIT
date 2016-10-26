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

    }
}
