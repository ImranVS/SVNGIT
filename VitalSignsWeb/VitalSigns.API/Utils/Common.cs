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
using System.ComponentModel;
using VSNext.Mongo.Entities;
using System.Net.Mail;

namespace VitalSigns.API
{
    public class Common 
    {
        public enum ResponseStatus
        {
            [Description("Success")]
            Success,
            [Description("Error")]
            Error,
            [Description("Warning")]
            Warning,
            [Description("Info")]
            Info
        }
        private IRepository<NameValue> nameValueRepository;
        string success = ResponseStatus.Success.ToDescription();
        public static Dictionary<string, string> GetServerTypeIcons()
        {
            Dictionary<string, string> serverTypeIcons = new Dictionary<string, string>();
           
            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
            foreach (ServerTypeModel item in serverTypeList)
                serverTypeIcons[item.ServerTypeName] = item.Icon;
            return serverTypeIcons;
        }
        public static List<ComboBoxListItem> GetServerTypes()
        {
            List<ComboBoxListItem> serverTypes = new List<ComboBoxListItem>();

            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
             serverTypes = serverTypeList.Select(x => new ComboBoxListItem { DisplayText = x.ServerTypeName, Value = x.ServerTypeName }).ToList().ToList();
           
        
            return serverTypes;
        }
        public static ServerTypeModel GetServerTypeTabs(string serverTypeName )
        {   
            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
            var serverType = serverTypeList.Where(x => x.ServerTypeName.ToUpper() == serverTypeName.ToUpper()).FirstOrDefault();
            return serverType;
        }
        public static APIResponse CreateResponse(object data,string status= "Success", string message = "Success")
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

        public bool SendPasswordEmail(string emailId, string password)
        {
            string hostName = "";
            int port = 0;
            Boolean SSL = false;
            string emailUserId = "";
            string emailPassword = "";
            string strEncryptedPassword = "";
            string Password = "";
            byte[] myPass;
            string[] str1;
            byte[] bstr1;

            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();

            try
            {
                Repository<VSNext.Mongo.Entities.NameValue> nameValueRepository = new Repository<VSNext.Mongo.Entities.NameValue>(Startup.ConnectionString + @"/" + Startup.DataBaseName);
                var filterDef = nameValueRepository.Filter.In(x => x.Name, new string[] { "PrimaryHostName", "PrimaryUserId", "Primarypwd", "PrimaryPort", "PrimarySSL" });
                List<NameValue> list = nameValueRepository.Find(filterDef).ToList();

                hostName = list.Where(x => x.Name == "PrimaryHostName").First().Value;
                port = Convert.ToInt32(list.Where(x => x.Name == "PrimaryPort").First().Value);
                SSL = Convert.ToBoolean(list.Where(x => x.Name == "PrimarySSL").First().Value);
                emailUserId = list.Where(x => x.Name == "PrimaryUserId").First().Value;
                if (list.Find(x => x.Name == "Primarypwd") != null)
                {
                    emailPassword = list.Where(x => x.Name == "Primarypwd").First().Value;
                    //3/2/2017 NS modified for VSPLUS-3250
                    //Password decoded
                    strEncryptedPassword = emailPassword;
                    str1 = strEncryptedPassword.Split(',');
                    bstr1 = new byte[str1.Length];
                    for (int i = 0; i < str1.Length; i++)
                    {
                        bstr1[i] = Convert.ToByte(str1[i]);
                    }
                    myPass = bstr1;
                    if (strEncryptedPassword != null)
                    {
                        Password = mySecrets.Decrypt(myPass); //password in clear text, stored in memory now
                    }
                    else
                    {
                        Password = "";
                    }
                }
                else
                {
                    Password = "";
                }
                emailPassword = Password;

                try
                {
                    return sendnewPasswordEmail(emailId, password, hostName, emailUserId, emailPassword, port, SSL);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                filterDef = nameValueRepository.Filter.In(x => x.Name, new string[] { "SecondaryHostName", "SecondaryUserId", "SecondaryPwd", "SecondaryPort", "SecondarySSL" });
                list = nameValueRepository.Find(filterDef).ToList();

                hostName = list.Where(x => x.Name == "SecondaryHostName").First().Value;
                port = Convert.ToInt32(list.Where(x => x.Name == "SecondaryPort").First().Value);
                SSL = Convert.ToBoolean(list.Where(x => x.Name == "SecondarySSL").First().Value);
                emailUserId = list.Where(x => x.Name == "SecondaryUserId").First().Value;
                if (list.Find(x => x.Name == "SecondaryPwd") != null)
                {
                    emailPassword = list.Where(x => x.Name == "SecondaryPwd").First().Value;
                    //3/2/2017 NS modified for VSPLUS-3250
                    //Password decoded
                    strEncryptedPassword = emailPassword;
                    str1 = strEncryptedPassword.Split(',');
                    bstr1 = new byte[str1.Length - 1];
                    for (int i = 0; i < str1.Length; i++)
                    {
                        bstr1[i] = Convert.ToByte(str1[i]);
                    }
                    myPass = bstr1;
                    if (strEncryptedPassword != null)
                    {
                        Password = mySecrets.Decrypt(myPass); //password in clear text, stored in memory now
                    }
                    else
                    {
                        Password = "";
                    }
                }
                else
                {
                    Password = "";
                }
                emailPassword = Password;

                try
                {
                    return sendnewPasswordEmail(emailId, password, hostName, emailUserId, emailPassword, port, SSL);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool sendnewPasswordEmail(string emailId, string password, string emailHostName, string emailUserId, string emailPassword, int emailPort, Boolean emailSSL)
        {
            System.Net.Mail.MailMessage mailMessage = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(emailPort);
            client.EnableSsl = Convert.ToBoolean(emailSSL);
            client.Host = emailHostName;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(emailUserId, emailPassword);
            mailMessage.To.Add(emailId);
            mailMessage.From = new MailAddress(emailUserId);
            mailMessage.IsBodyHtml = false;
            mailMessage.Body = "Your VitalSigns account details are as follows: \n\rUser name: " + emailId.ToString() + "\nPassword: " + password + "";
            mailMessage.Subject = "Your VitalSigns Account Information Update";

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    public class ApiEnums
    {
        /// <summary>
        /// Server type enumerations
        /// </summary>
    

    }
    /// <summary>
    /// To implement the extension methods from Enum class
    /// </summary>
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Returns the base server type for  server type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetBaseServerType(this Enum value)
        {
            var attribute = value.GetAttribute<BaseServerTypeAttribute>();
            return attribute == null ? string.Empty : attribute.Name;
        }



    }



    /// <summary>
    /// Attribute used to set the base server type for the servertype enum. By default, when this attribute
    /// is not specified, the empty string will be set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class BaseServerTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the base server typr field attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the base server type.</param>
        public BaseServerTypeAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty Base server type is not allowed", "value");
            Name = value;
        }

        /// <summary>
        /// Gets the name of the base server type.
        /// </summary>
        /// <value>The name of the base server type.</value>
        public virtual string Name { get; private set; }
    }

}
