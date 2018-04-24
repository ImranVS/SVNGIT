
   public class License

    {
       public License()
       {}

       public string LicenseKey
       {
           get { return _LicenseKey; }
           set { _LicenseKey = value; }
       
       }
       private string _LicenseKey;
	   public int Units
	   {
		   get { return _Units; }
		   set { _Units = value; }
	   }
	   private int _Units;
	    public string InstallType
       {
           get { return _InstallType; }
           set { _InstallType = value; }
       
       }
       private string _InstallType;

      public string CompanyName
       {
           get { return _CompanyName; }
           set { _CompanyName = value; }
       
       }
       private string _CompanyName;

	    public string LicenseType
       {
           get { return _LicenseType; }
           set { _LicenseType = value; }
       
       }
       private string _LicenseType;
	   public string ExpirationDate
	   {
		   get { return _ExpirationDate; }
		   set { _ExpirationDate = value; }
	   }
	   private string _ExpirationDate;
	   public string EncUnits
	   {
		   get { return _EncUnits; }
		   set { _EncUnits = value; }
	   }
	   private string _EncUnits;


	   public License(string LicenseKey, int Units, string InstallType, string CompanyName, string LicenseType, string ExpirationDate, string EncUnits)
       {
		   this._LicenseKey = LicenseKey;
		   	   this._Units = Units;
		    this._InstallType = InstallType;
	
		   this._CompanyName = CompanyName;
		   this._LicenseType = LicenseType;
		  
		   this._ExpirationDate = ExpirationDate;
		   this._EncUnits = EncUnits;
		  
       }
    }

