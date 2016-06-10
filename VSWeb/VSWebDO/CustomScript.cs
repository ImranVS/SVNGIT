using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class CustomScript
    {
        public CustomScript()
        {}

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }

        }
        private int _ID;

        public string ScriptName
        {
            get { return _ScriptName; }
            set { _ScriptName = value; }
        }
        private string _ScriptName;

        public string ScriptCommand
        {
            get { return _ScriptCommand; }
            set { _ScriptCommand = value; }
        }
        private string _ScriptCommand;

        public string ScriptLocation
        {
            get { return _ScriptLocation; }
            set { _ScriptLocation = value; }
        }
        private string _ScriptLocation;
    }
}
