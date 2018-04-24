

public class HoursIndicator
    {


		public HoursIndicator()
	{ }

       
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _Type;

        public HoursIndicator(int ID,
            string Type)
            
        {
            this._ID = ID;
            this._Type = Type;
        
        }
		public bool Issunday
		{
			get { return _Issunday; }
			set { _Issunday = value; }
		}
		private bool _Issunday;

		public bool IsMonday
		{
			get { return _IsMonday; }
			set { _IsMonday = value; }
		}
		private bool _IsMonday;

		public bool IsTuesday
		{
			get { return _IsTuesday; }
			set { _IsTuesday = value; }
		}
		private bool _IsTuesday;

		public bool IsWednesday
		{
			get { return _IsWednesday; }
			set { _IsWednesday = value; }
		}
		private bool _IsWednesday;
		public bool IsThursday
		{
			get { return _IsThursday; }
			set { _IsThursday = value; }
		}
		private bool _IsThursday;

		public bool IsFriday
		{
			get { return _IsFriday; }
			set { _IsFriday = value; }
		}
		private bool _IsFriday;

		public bool Issaturday
		{
			get { return _Issaturday; }
			set { _Issaturday = value; }
		}
		private bool _Issaturday;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}
		private string _Name;

		public int? Duration
		{
			get { return _Duration; }
			set { _Duration = value; }
		}
		private int? _Duration;


		public string  Starttime
		{
			get { return _Starttime; }
			set { _Starttime = value; }
		}
		private string _Starttime;

        //4/23/2015 NS added
        public int UseType
        {
            get { return _UseType; }
            set { _UseType = value; }
        }
        private int _UseType;

        //6/29/2015 NS added for VSPLUS-1821
        public string Days
        {
            get { return _Days; }
            set { _Days = value; }
        }
        private string _Days;

        //4/23/2015 NS modified
		public HoursIndicator(string Type, string Starttime, int Duration, bool Issunday, bool IsMonday, bool IsTuesday, bool IsWednesday, bool IsThursday, bool IsFriday,
			  bool Issaturday, int UseType, string Days,
			int ID)
		{
			this._Name = Name;
			this._Type = Type;
			this._Starttime = Starttime;
			this._Duration = Duration;
			
			this._Issunday = Issunday;
			this._IsMonday = IsMonday;
			this._IsTuesday = IsTuesday;
			this._IsWednesday = IsWednesday;
			this._IsThursday = IsThursday;
			this._IsFriday = IsFriday;
			this._Issaturday = Issaturday;
            //4/23/2015 NS added
            this._UseType = UseType;
            //6/29/2015 NS added for VSPLUS-1821
            this._Days = Days;
			this._ID = ID;
            
		}
		}
		

		
		 

		  

		  
			
			  
			 
			
			 
			
			 
		   

	

  

