namespace VSWebDO
{
    public class AlertDefinitions
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public AlertDefinitions()
        { }


        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private bool _Enabled;


        public string AlertType
        {
            get { return _AlertType; }
            set { _AlertType = value; }
        }
        private string _AlertType;


        public bool DefaultAlert
        {
            get { return _DefaultAlert; }
            set { _DefaultAlert = value; }
        }
        private bool _DefaultAlert;


        public bool AppliesToDomino
        {
            get { return _AppliesToDomino; }
            set { _AppliesToDomino = value; }
        }
        private bool _AppliesToDomino;


        public bool AppliesToBlackBerry
        {
            get { return _AppliesToBlackBerry; }
            set { _AppliesToBlackBerry = value; }
        }
        private bool _AppliesToBlackBerry;


        public bool AppliesToURLs
        {
            get { return _AppliesToURLs; }
            set { _AppliesToURLs = value; }
        }
        private bool _AppliesToURLs;


        public bool AppliesToNetworkDevices
        {
            get { return _AppliesToNetworkDevices; }
            set { _AppliesToNetworkDevices = value; }
        }
        private bool _AppliesToNetworkDevices;


        public bool AppliesToLDAP
        {
            get { return _AppliesToLDAP; }
            set { _AppliesToLDAP = value; }
        }
        private bool _AppliesToLDAP;


        public bool AppliesToMailServices
        {
            get { return _AppliesToMailServices; }
            set { _AppliesToMailServices = value; }
        }
        private bool _AppliesToMailServices;


        public bool AppliesToNotesMailProbes
        {
            get { return _AppliesToNotesMailProbes; }
            set { _AppliesToNotesMailProbes = value; }
        }
        private bool _AppliesToNotesMailProbes;


        public bool AppliestoNotesDatabases
        {
            get { return _AppliestoNotesDatabases; }
            set { _AppliestoNotesDatabases = value; }
        }
        private bool _AppliestoNotesDatabases;


        public string EMailAddress
        {
            get { return _EMailAddress; }
            set { _EMailAddress = value; }
        }
        private string _EMailAddress;


        public string PagerID
        {
            get { return _PagerID; }
            set { _PagerID = value; }
        }
        private string _PagerID;


        public bool AppliestoSametime
        {
            get { return _AppliestoSametime; }
            set { _AppliestoSametime = value; }
        }
        private bool _AppliestoSametime;


        public bool AppliestoPendingMail
        {
            get { return _AppliestoPendingMail; }
            set { _AppliestoPendingMail = value; }
        }
        private bool _AppliestoPendingMail;


        public bool AppliestoOffHours
        {
            get { return _AppliestoOffHours; }
            set { _AppliestoOffHours = value; }
        }
        private bool _AppliestoOffHours;


        public bool AppliestoBusinessHours
        {
            get { return _AppliestoBusinessHours; }
            set { _AppliestoBusinessHours = value; }
        }
        private bool _AppliestoBusinessHours;


        public bool DownServers
        {
            get { return _DownServers; }
            set { _DownServers = value; }
        }
        private bool _DownServers;


        public bool NetworkIssue
        {
            get { return _NetworkIssue; }
            set { _NetworkIssue = value; }
        }
        private bool _NetworkIssue;


        public string SSMA_TimeStamp
        {
            get { return _SSMA_TimeStamp; }
            set { _SSMA_TimeStamp = value; }
        }
        private string _SSMA_TimeStamp;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public AlertDefinitions(bool Enabled,
            string AlertType,
            bool DefaultAlert,
            bool AppliesToDomino,
            bool AppliesToBlackBerry,
            bool AppliesToURLs,
            bool AppliesToNetworkDevices,
            bool AppliesToLDAP,
            bool AppliesToMailServices,
            bool AppliesToNotesMailProbes,
            bool AppliestoNotesDatabases,
            string EMailAddress,
            string PagerID,
            bool AppliestoSametime,
            bool AppliestoPendingMail,
            bool AppliestoOffHours,
            bool AppliestoBusinessHours,
            bool DownServers,
            bool NetworkIssue,
            string SSMA_TimeStamp)
        {
            this._Enabled = Enabled;
            this._AlertType = AlertType;
            this._DefaultAlert = DefaultAlert;
            this._AppliesToDomino = AppliesToDomino;
            this._AppliesToBlackBerry = AppliesToBlackBerry;
            this._AppliesToURLs = AppliesToURLs;
            this._AppliesToNetworkDevices = AppliesToNetworkDevices;
            this._AppliesToLDAP = AppliesToLDAP;
            this._AppliesToMailServices = AppliesToMailServices;
            this._AppliesToNotesMailProbes = AppliesToNotesMailProbes;
            this._AppliestoNotesDatabases = AppliestoNotesDatabases;
            this._EMailAddress = EMailAddress;
            this._PagerID = PagerID;
            this._AppliestoSametime = AppliestoSametime;
            this._AppliestoPendingMail = AppliestoPendingMail;
            this._AppliestoOffHours = AppliestoOffHours;
            this._AppliestoBusinessHours = AppliestoBusinessHours;
            this._DownServers = DownServers;
            this._NetworkIssue = NetworkIssue;
            this._SSMA_TimeStamp = SSMA_TimeStamp;
        }
    }
}