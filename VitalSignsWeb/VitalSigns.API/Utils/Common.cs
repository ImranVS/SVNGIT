using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using VitalSigns.API.Models;
using System.Linq;

namespace VitalSigns.API
{
    public class Common
    {

        public static Dictionary<string, string> GetServerTypeIcons()
        {
            Dictionary<string, string> serverTypeIcons = new Dictionary<string, string>();
           
            List<ServerType> serverTypeList = (List<ServerType>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerType>));
            foreach (ServerType item in serverTypeList)
                serverTypeIcons[item.ServerTypeName] = item.Icon;
            return serverTypeIcons;
        }

        public static ServerType GetServerTypeTabs(string serverTypeName )
        {   
            List<ServerType> serverTypeList = (List<ServerType>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerType>));
            var serverType = serverTypeList.Where(x => x.ServerTypeName.ToUpper() == serverTypeName.ToUpper()).FirstOrDefault();
            return serverType;
        }
        public static APIResponse CreateResponse(object data,string status="OK",string message = "Success")
        {
            return new APIResponse {Data=data,Status=status,Message=message };
            
        }
    }
}
