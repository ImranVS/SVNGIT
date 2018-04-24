using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
     public class Menus
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
         public Menus()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;

        public string DisplayText
        {
            get { return _DisplayText; }
            set {  _DisplayText=value; }
        }
        private string _DisplayText;

         public int OrderNum
        {
            get { return _OrderNum; }
            set { _OrderNum = value; }
        
        }
        private int _OrderNum;

        public string ParentMenu
        {
            get { return _ParentMenu; }
            set { _ParentMenu = value; }
        }
        private string _ParentMenu;

        public string PageLink
        {
            get { return _PageLink; }
            set { _PageLink = value; }
        }
        private string _PageLink;

        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
        
        }
        private int _Level;

        public string RefName
        {
            get { return _RefName; }
            set { _RefName = value; }
        }
        private string _RefName;

     
        public string ImageURL
        {

            get { return _ImageURL; }
            set { _ImageURL = value; }
        }
        private string _ImageURL;

        public string MenuArea
        {

            get { return _MenuArea; }
            set { _MenuArea = value; }
        }
        private string _MenuArea;

        /// <summary>
        /// User defined Contructor
        /// <summary>


        public Menus(int ID, string DisplayText,
      int OrderNum,
      string ParentMenu,
      string PageLink,
      int Level,
      string RefName,
      string ImageURL,
      string MenuArea)
        {
            this._ID = ID;
            this._DisplayText = DisplayText;
            this._OrderNum = OrderNum;
            this._ParentMenu = ParentMenu;
            this._PageLink = PageLink;
            this._Level = Level;
            this._RefName = RefName;
            this._ImageURL = ImageURL;
            this._MenuArea = MenuArea;

        }
     
     }

  }
