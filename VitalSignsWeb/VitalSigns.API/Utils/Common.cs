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
        public static ServerTypeModel GetServerTypeTabs(string serverTypeName)
        {
            List<ServerTypeModel> serverTypeList = (List<ServerTypeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(Startup.ServerTypeJsonPath), typeof(List<ServerTypeModel>));
            var serverType = serverTypeList.Where(x => x.ServerTypeName.ToUpper() == serverTypeName.ToUpper()).FirstOrDefault();
            return serverType;
        }
        public static APIResponse CreateResponse(object data, string status = "Success", string message = "Success")
        {
            return new APIResponse { Data = data, Status = status, Message = message };

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
            } catch (Exception ex)
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
            List<VSNext.Mongo.Entities.NameValue> result = new List<VSNext.Mongo.Entities.NameValue>();
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
                result = namevalueRepository.Collection.AsQueryable().FirstOrDefault(x => (x.Name == name));

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

                if (string.IsNullOrEmpty(hostName))
                {
                    throw new System.ArgumentException("You must configure an SMTP server in Alert settings before new user passwords can be mailed.");
                }


                if (list.Find(x => x.Name == "PrimaryUserId") != null)
                {
                    emailUserId = list.Where(x => x.Name == "PrimaryUserId").First().Value;
                }
                else
                {
                    emailUserId = "";
                }

                if (list.Find(x => x.Name == "Primarypwd") != null)
                {
                    emailPassword = list.Where(x => x.Name == "Primarypwd").First().Value;
                    if (!String.IsNullOrWhiteSpace(emailPassword))
                    {
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
                }
                else
                {
                    Password = "";
                }
                emailPassword = Password;

                bool sendSuceess = false;
                try

                {
                    sendSuceess = sendnewPasswordEmail(emailId, password, hostName, emailUserId, emailPassword, port, SSL);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (sendSuceess == true)
                {
                    return true;
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
                    if (String.IsNullOrWhiteSpace(emailPassword))
                    {
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
        public bool SendSmtpSevers(string emailId, string password, string emailHostName, string emailUserId, string emailPassword, int emailPort, Boolean emailSSL, string Body, string Subject)
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
            mailMessage.Body = Body;
            mailMessage.Subject = Subject;

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
    
    public static class MicrosoftConnections
    {
        public static System.Security.SecureString String2SecureString(string password)
        {
            System.Security.SecureString remotePassword = new System.Security.SecureString();
            for (int i = 0; i < password.Length; i++)
                remotePassword.AppendChar(password[i]);

            return remotePassword;
        }

        public static System.Management.Automation.PowerShell ConnectToExchange(string ServerName, string UserName, string Password, string IPAddress, string AuthenticationType)
        {
            try
            {
                var targetFw = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
                System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
                System.Security.SecureString securePassword = String2SecureString(Password);

                System.Management.Automation.PSCredential creds = new System.Management.Automation.PSCredential(UserName, securePassword);
                System.Management.Automation.Runspaces.PowerShellProcessInstance instance = new System.Management.Automation.Runspaces.PowerShellProcessInstance(new Version(4, 0), null, null, false);
                System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateOutOfProcessRunspace(new System.Management.Automation.Runspaces.TypeTable(new string[0]), instance);
                //System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();

                System.Management.Automation.PowerShell powershell = System.Management.Automation.PowerShell.Create();

                System.Management.Automation.PSCommand command = new System.Management.Automation.PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ConfigurationName", "Microsoft.Exchange");
                command.AddParameter("ConnectionUri", uri);
                command.AddParameter("Credential", creds);
                command.AddParameter("Authentication", AuthenticationType);
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> results = new System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject>();

                System.Management.Automation.Remoting.PSSessionOption sessionOption = new System.Management.Automation.Remoting.PSSessionOption();
                sessionOption.SkipCACheck = true;
                sessionOption.SkipCNCheck = true;
                sessionOption.SkipRevocationCheck = true;

                command.AddParameter("SessionOption", sessionOption);
                powershell.Commands = command;

                powershell.Runspace = runspace;
                powershell.Runspace.Open();

                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> result = powershell.Invoke();


                foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    //CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

                    powershell.Streams.Error.Clear();

                    string script = @"$computerName = '" + ServerName + @"'

									$ipconfigSet = [System.Net.Dns]::GetHostAddresses($computerName) | where {$_.IsIPv6LinkLocal -eq $false } |  select IPAddressToString
									#get-wmiObject Win32_NetworkAdapterConfiguration -ComputerName $computerName -Credential $cred | select IPAdd

									foreach($ipObj in $ipconfigSet )
									{

										$ip = $ipObj.IPAddressToString

										$tempSession = New-PSSession -ConfigurationName Microsoft.Exchange -Credential $cred -ConnectionUri $('https://' + $ip + '/powershell') -Authentication " + AuthenticationType + @" -SessionOption $so
										if($tempSession.State.toString() -eq 'Opened')
										{
											$tempSession
											Return
            
										}

									}";


                    command = new System.Management.Automation.PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke();

                    foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new System.Management.Automation.PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);


                command = new System.Management.Automation.PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                //WriteDeviceHistoryEntry("All", "Microsoft_", "EX: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

                // First import the cmdlets in the current runspace (using Import-PSSession)

                command = new System.Management.Automation.PSCommand();
                command.AddScript("Import-PSSession -Session $ra");
                powershell.Commands = command;
                powershell.Invoke();

                string searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    //PSObj.ErrorMessage = "The Exchange Module was not able to be located";
                }

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Imported the PSSession.", role, LogLevel.Normal);



                //PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                //PSObj.Connected = true;
                return powershell;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static System.Management.Automation.PowerShell ConnectToSharePoint(string ServerName, string UserName, string Password, string IPAddress, string[] Modules = null)
        {
            try
            {
                var targetFw = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
                System.Security.SecureString securePassword = String2SecureString(Password);

                System.Management.Automation.PSCredential creds = new System.Management.Automation.PSCredential(UserName, securePassword);
                System.Management.Automation.Runspaces.PowerShellProcessInstance instance = new System.Management.Automation.Runspaces.PowerShellProcessInstance(new Version(4, 0), null, null, false);
                System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateOutOfProcessRunspace(new System.Management.Automation.Runspaces.TypeTable(new string[0]), instance);
                //System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();

                System.Management.Automation.PowerShell powershell = System.Management.Automation.PowerShell.Create();

                System.Management.Automation.PSCommand command = new System.Management.Automation.PSCommand();
                command = new System.Management.Automation.PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ComputerName", IPAddress.Replace("https://", "").Replace("http://", ""));
                command.AddParameter("Credential", creds);
                command.AddParameter("Authentication", "Credssp");
                //command.AddParameter("Authentication", "Default");
                powershell.Commands.Clear();
                powershell.Streams.Error.Clear();
                powershell.Commands = command;
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> result = powershell.Invoke<System.Management.Automation.PSObject>();


                foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    //CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

                    powershell.Streams.Error.Clear();

                    string script = @"$computerName = '" + ServerName + @"'

									$ipconfigSet = [System.Net.Dns]::GetHostAddresses($computerName) | where {$_.IsIPv6LinkLocal -eq $false } |  select IPAddressToString
									#get-wmiObject Win32_NetworkAdapterConfiguration -ComputerName $computerName -Credential $cred | select IPAdd

									foreach($ipObj in $ipconfigSet )
									{

										$ip = $ipObj.IPAddressToString

										$tempSession = New-PSSession -Credential $cred -Computername $ip -Authentication Credssp
										if($tempSession.State.toString() -eq 'Opened')
										{
											$tempSession
											Return
            
										}

									}";


                    command = new System.Management.Automation.PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke();

                    foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new System.Management.Automation.PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                command = new System.Management.Automation.PSCommand();
                command.AddScript("Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell;}");
                powershell.Commands = command; ;
                powershell.Invoke();
                foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    //WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer  Importing the Snapin: " + strError, role, LogLevel.Normal);
                }
                //WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the Snapin.", role, LogLevel.Normal);

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);

                if (Modules != null)
                {
                    foreach (String mod in Modules)
                    {
                        try
                        {
                            command = new System.Management.Automation.PSCommand();
                            command.AddCommand("Import-Module");
                            command.AddParameter("Name", mod);
                            powershell.Commands = command;
                            powershell.Invoke();

                            //WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the module " + mod + ".", role, LogLevel.Normal);
                        }
                        catch (Exception ex)
                        {
                            //WriteDeviceHistoryEntry(ServerType, ServerName, "Exception importing module " + mod + ". Exception: " + ex.Message, role, LogLevel.Normal);
                        }
                    }
                }

                command = new System.Management.Automation.PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                result = powershell.Invoke();

                //WriteDeviceHistoryEntry("All", "Microsoft_", "EX: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

                // First import the cmdlets in the current runspace (using Import-PSSession)

                command = new System.Management.Automation.PSCommand();
                command.AddScript("Import-PSSession -Session $ra");
                powershell.Commands = command;
                powershell.Invoke();

                string searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    //PSObj.ErrorMessage = "The Exchange Module was not able to be located";
                }

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Imported the PSSession.", role, LogLevel.Normal);



                //PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                //PSObj.Connected = true;
                return powershell;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static System.Management.Automation.PowerShell ConnectToOffice365(string ServerName, string UserName, string Password, string IPAddress, string proxyType="")
        {
            try
            {
                string tenantName = "";
                if (UserName != "")
                    tenantName = UserName.Split('@')[1].ToString().Split('.')[0].ToString();
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before Import", Common.LogLevel.Normal);
                System.Management.Automation.Runspaces.InitialSessionState session = System.Management.Automation.Runspaces.InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "MSOnline" });
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after Import", Common.LogLevel.Normal);

                System.Uri uri = new Uri("https://outlook.office365.com" + "/powershell-liveid/");
                System.Security.SecureString securePassword = String2SecureString(Password);

                System.Management.Automation.PSCredential creds = new System.Management.Automation.PSCredential(UserName, securePassword);
                System.Management.Automation.Runspaces.PowerShellProcessInstance instance = new System.Management.Automation.Runspaces.PowerShellProcessInstance(new Version(4, 0), null, null, false);
                System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateOutOfProcessRunspace(new System.Management.Automation.Runspaces.TypeTable(new string[0]), instance);
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before open", Common.LogLevel.Normal);
                runspace.Open();
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after open", Common.LogLevel.Normal);
                System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create();
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets-2", Common.LogLevel.Normal);


                powerShell.Runspace = runspace;

                System.Management.Automation.PSCommand psSession = new System.Management.Automation.PSCommand();
                psSession.AddCommand("New-PSSession");
                psSession.AddParameter("ConfigurationName", "Microsoft.Exchange");
                psSession.AddParameter("ConnectionUri", uri);
                psSession.AddParameter("Credential", creds);
                psSession.AddParameter("Authentication", "Basic");
                psSession.AddParameter("AllowRedirection");
                System.Management.Automation.Remoting.PSSessionOption sessionOption = new System.Management.Automation.Remoting.PSSessionOption();
                switch (proxyType)
                {

                    case null:
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.None;
                        break;

                    case "":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.None;
                        break;

                    case "None":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.None;
                        break;

                    case "IEConfig":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.IEConfig;
                        break;

                    case "AutoDetect":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.AutoDetect;
                        break;

                    case "NoProxyServer":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.NoProxyServer;
                        break;

                    case "WinHttpConfig":
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.WinHttpConfig;
                        break;

                    default:
                        //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets using default proxy value. Value: " + Server.ProxyType, Common.LogLevel.Normal);
                        sessionOption.ProxyAccessType = System.Management.Automation.Remoting.ProxyAccessType.None;
                        break;
                }
                //Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets proxy value: " + sessionOption.ProxyAccessType.ToString(), Common.LogLevel.Verbose);
                psSession.AddParameter("SessionOption", sessionOption);

                powerShell.Commands = psSession;
                //var result = powerShell.Invoke();
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> result = powerShell.Invoke<System.Management.Automation.PSObject>();
                //Common.WriteDeviceHistoryEntry("All", ServerType, "In  PrereqForOffice365WithCmdlets result:" + result[0], Common.LogLevel.Normal);


                System.Management.Automation.PSCommand setVar = new System.Management.Automation.PSCommand();
                setVar.AddScript("$ra = $(Get-PSSession)[0]");

                powerShell.Commands = setVar;
                powerShell.Runspace = runspace;
                powerShell.Invoke();

                System.Management.Automation.PSCommand importSession = new System.Management.Automation.PSCommand();
                importSession.AddScript("Import-PSSession -Session $ra");
                powerShell.Commands = importSession;
                powerShell.Runspace = runspace;
                powerShell.Invoke();

                string searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powerShell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    //ErrorMessage = "The module was not able to be located";
                }


                System.Management.Automation.PSCommand command;

                foreach (String mod in new String[] { "MSOnline", "Microsoft.Online.SharePoint.PowerShell" })
                {
                    command = new System.Management.Automation.PSCommand();
                    command.AddCommand("Import-Module");
                    command.AddParameter("Name", mod);
                    powerShell.Commands = command;
                    try
                    {
                        powerShell.Invoke();
                    }
                    catch (Exception ex)
                    {
                        //WriteDeviceHistoryEntry(ServerType, ServerName, " Failed to import the module " + mod + "." + ex.Message.ToString(), role, LogLevel.Normal);
                    }

                    //WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the module " + mod + ".", LogLevel.Normal);
                }


                //WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before MSOL connection", Common.LogLevel.Normal);
                System.Management.Automation.PSCommand connect = new System.Management.Automation.PSCommand();
                connect.AddCommand("Connect-MsolService");
                connect.AddParameter("Credential", creds);
                powerShell.Commands = connect;
                try
                {
                    powerShell.Invoke();
                }
                catch (Exception ex)
                {
                    //WriteDeviceHistoryEntry(ServerType, ServerName, " Failed to connect to MsolService." + ex.Message.ToString(), role, LogLevel.Normal);
                }

                //WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after MSOL connection", Common.LogLevel.Normal);
                System.Management.Automation.PSCommand connectSPOL = new System.Management.Automation.PSCommand();
                connectSPOL.AddCommand("Connect-SPOService");
                //WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection", Common.LogLevel.Normal);
                string sharePointURL = "";
                if (tenantName != "")
                {
                    sharePointURL = "https://" + tenantName + "-admin.sharepoint.com";
                }
                //WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);
                connectSPOL.AddParameter("Url", sharePointURL);
                connectSPOL.AddParameter("Credential", creds);
                powerShell.Commands = connectSPOL;
                try
                {
                    powerShell.Invoke();
                }
                catch (Exception ex)
                {
                    //WriteDeviceHistoryEntry(ServerType, ServerName, " Failed to connect to SPOService." + ex.Message.ToString(), role, LogLevel.Normal);
                }
                //WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);

                //PSObj.PS = powerShell;
                //PSObj.runspace = runspace;
                //PSObj.Connected = true;

                command = new System.Management.Automation.PSCommand();
                command.AddScript("$PID");
                powerShell.Commands = command; ;
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> results = powerShell.Invoke();

                //WriteDeviceHistoryEntry(ServerType, ServerName, "O365: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                return powerShell;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            
            //    }
            //}
        }

        public static System.Management.Automation.PowerShell ConnectToActiveDirectory(string ServerName, string UserName, string Password, string IPAddress)
        {
            try
            {
                var targetFw = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
                System.Security.SecureString securePassword = String2SecureString(Password);

                System.Management.Automation.PSCredential creds = new System.Management.Automation.PSCredential(UserName, securePassword);
                System.Management.Automation.Runspaces.PowerShellProcessInstance instance = new System.Management.Automation.Runspaces.PowerShellProcessInstance(new Version(4, 0), null, null, false);
                System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateOutOfProcessRunspace(new System.Management.Automation.Runspaces.TypeTable(new string[0]), instance);
                //System.Management.Automation.Runspaces.Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();

                System.Management.Automation.PowerShell powershell = System.Management.Automation.PowerShell.Create();

                System.Management.Automation.PSCommand command = new System.Management.Automation.PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("computer", IPAddress);
                command.AddParameter("Credential", creds);
                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> results = new System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject>();
                
                powershell.Commands = command;

                powershell.Runspace = runspace;
                powershell.Runspace.Open();

                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> result = powershell.Invoke();


                foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    //CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    //WriteDeviceHistoryEntry("Exchange", ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

                    powershell.Streams.Error.Clear();

                    string script = @"$computerName = '" + ServerName + @"'

									$ipconfigSet = [System.Net.Dns]::GetHostAddresses($computerName) | where {$_.IsIPv6LinkLocal -eq $false } |  select IPAddressToString
									#get-wmiObject Win32_NetworkAdapterConfiguration -ComputerName $computerName -Credential $cred | select IPAdd

									foreach($ipObj in $ipconfigSet )
									{

										$ip = $ipObj.IPAddressToString

										$tempSession = New-PSSession -Credential $cred -ComputerName $ip
										if($tempSession.State.toString() -eq 'Opened')
										{
											$tempSession
											Return 
            
										}

									}";


                    command = new System.Management.Automation.PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke();

                    foreach (System.Management.Automation.ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        //WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new System.Management.Automation.PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                //WriteDeviceHistoryEntry("Exchange", ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);


                command = new System.Management.Automation.PSCommand();
                command.AddScript("Invoke-Command -session $ra -script { Import-Module ActiveDirectory }");
                powershell.Commands = command;
                powershell.Invoke();

                command = new System.Management.Automation.PSCommand();
                command.AddScript("Import-PSSession -session $ra -module ActiveDirectory ");
                powershell.Commands = command;
                powershell.Invoke();

                return powershell;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static void ClosePowerShell( ref System.Management.Automation.PowerShell ps)
        {
            try
            {
                ps.Commands.Clear();
                string script = "Get-PSSession | Remove-PSSession;  $PID";
                ps.Commands = new System.Management.Automation.PSCommand();
                ps.Commands.AddScript(script);
               var v = ps.Invoke();
            }
            catch (Exception ex)
            { }
            try
            {
                ps.Commands.Clear();
                string script = "Stop-process -Id $PID";
                ps.Commands = new System.Management.Automation.PSCommand();
                ps.Commands.AddScript(script);
                var v = ps.Invoke();
            }
            catch (Exception ex)
            { }
            if (ps != null && ps.Runspace != null)
            {
                if (ps.Runspace.RunspaceStateInfo.State == System.Management.Automation.Runspaces.RunspaceState.Opened)
                {
                    ps.Runspace.Close();
                }
                ps.Runspace.Close();
                ps.Runspace.Dispose();
                ps.Runspace = null;

            }
            if (ps != null)
            {
                ps.Dispose();
            }
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

