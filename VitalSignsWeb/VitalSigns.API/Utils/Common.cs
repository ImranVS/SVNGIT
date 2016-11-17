using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using VitalSigns.API.Models;
using System.Linq;
using VSNext.Mongo.Repository;
using MongoDB.Driver;
using System;
using System.Reflection;


namespace VitalSigns.API
{
    public class Common
    {

        public static Dictionary<string, string> GetServerTypeIcons()
        {
            Dictionary<string, string> serverTypeIcons = new Dictionary<string, string>();
           
            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
            foreach (ServerTypeModel item in serverTypeList)
                serverTypeIcons[item.ServerTypeName] = item.Icon;
            return serverTypeIcons;
        }

        public static ServerTypeModel GetServerTypeTabs(string serverTypeName )
        {   
            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
            var serverType = serverTypeList.Where(x => x.ServerTypeName.ToUpper() == serverTypeName.ToUpper()).FirstOrDefault();
            return serverType;
        }
        public static APIResponse CreateResponse(object data,string status="OK",string message = "Success")
        {
            return new APIResponse {Data=data,Status=status,Message=message };
            
        }

        public static bool SaveNameValues(List<VSNext.Mongo.Entities.NameValue> nameValues)
        {
            bool result = true;
            try
            {
                Repository<VSNext.Mongo.Entities.NameValue> namevalueRepository = new Repository<VSNext.Mongo.Entities.NameValue>(Startup.ConnectionString + @"/" + Startup.DataBaseName);
                foreach (var setting in nameValues)
                {
                    if (namevalueRepository.Collection.AsQueryable().Where(x => x.Name.Equals(setting.Name)).Count() > 0)
                    {
                        var filterDefination = Builders<VSNext.Mongo.Entities.NameValue>.Filter.Where(p => p.Name == setting.Name);
                        var updateDefinitaion = namevalueRepository.Updater.Set(p => p.Value, setting.Value);
                        var results = namevalueRepository.Update(filterDefination, updateDefinitaion);
                    }
                    else
                    {
                        namevalueRepository.Insert(setting);
                    }
                }
            }catch(Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static bool SaveNameValue(VSNext.Mongo.Entities.NameValue setting)
        {
            bool result = true;
            try
            {
                Repository<VSNext.Mongo.Entities.NameValue> namevalueRepository = new Repository<VSNext.Mongo.Entities.NameValue>(Startup.ConnectionString + @"/" + Startup.DataBaseName);


                if (namevalueRepository.Collection.AsQueryable().Where(x => x.Name.Equals(setting.Name)).Count() > 0)
                {
                    var filterDefination = Builders<VSNext.Mongo.Entities.NameValue>.Filter.Where(p => p.Name == setting.Name);
                    var updateDefinitaion = namevalueRepository.Updater.Set(p => p.Value, setting.Value);
                    var results = namevalueRepository.Update(filterDefination, updateDefinitaion);
                }
                else
                {
                    namevalueRepository.Insert(setting);
                }


            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static List<VSNext.Mongo.Entities.NameValue> GetNameValues(List<string> nameValues)
        {
            List<VSNext.Mongo.Entities.NameValue> result =new  List<VSNext.Mongo.Entities.NameValue>();
            try
            {
                Repository<VSNext.Mongo.Entities.NameValue> namevalueRepository = new Repository<VSNext.Mongo.Entities.NameValue>(Startup.ConnectionString + @"/" + Startup.DataBaseName);
                 result = namevalueRepository.Collection.AsQueryable().Where(x => nameValues.Contains(x.Name)).ToList();
                    
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public static VSNext.Mongo.Entities.NameValue GetNameValue(string name)
        {
            VSNext.Mongo.Entities.NameValue result = new VSNext.Mongo.Entities.NameValue();
            try
            {
                Repository<VSNext.Mongo.Entities.NameValue> namevalueRepository = new Repository<VSNext.Mongo.Entities.NameValue>(Startup.ConnectionString + @"/" + Startup.DataBaseName);
                result = namevalueRepository.Collection.AsQueryable().FirstOrDefault(x =>(x.Name== name));

            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
        public void refreshNodeAssignment()
        {
            VitalSignsLicensing.Licensing l = new VitalSignsLicensing.Licensing();
            l.refreshServerCollectionWrapper();
            
        }

        public VSNext.Mongo.Entities.License getLicenseInfo(string key)
        {
            VitalSignsLicensing.Licensing l = new VitalSignsLicensing.Licensing();
            return l.getLicenseInfo(key);


        }

       public static void SetObjectProperty(object theObject, string propertyName, object value)
        {
            //Type type = theObject.GetType();
            //var property = type.GetProperty(propertyName);
            //var setter = property.SetMethod();
            //setter.Invoke(theObject, new ojbject[] { value });

            PropertyInfo propertyInfo = theObject.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(propertyInfo, value, null);
        }

    }
}
