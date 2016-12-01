using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class DominoSettingsModel
    {
        [JsonProperty("notes_program_directory")]
        public string NotesProgramDirectory { get; set; }

        [JsonProperty("notes_user_id")]
        public string NotesUserID { get; set; }

        [JsonProperty("notes_ini")]
        public string NotesIni { get; set; }

        [JsonProperty("notes_password")]
        public string NotesPassword { get; set; }

        [JsonProperty("enableex_journal")]
        public bool EnableExJournal { get; set; }

        [JsonProperty("enable_domino_console_commands")]
        public bool EnableDominoConsoleCommands { get; set; }

        [JsonProperty("exjournal_threshold")]
        public string ExJournalThreshold { get; set; }


        [JsonProperty("consecutive_telnet")]
        public string ConsecutiveTelnet { get; set; }

        [JsonProperty("is_modified")]
        public bool IsModified { get; set; }
    }
}
