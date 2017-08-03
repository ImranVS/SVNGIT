using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Data;
using VSFramework;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace VitalSignsMicrosoftClasses
{
    public static class Common
    {

        //Determines the verbosity of the log file
        public enum LogLevel
        {
            Verbose = LogUtilities.LogUtils.LogLevel.Verbose,
            Debug = LogUtilities.LogUtils.LogLevel.Debug,
            Normal = LogUtilities.LogUtils.LogLevel.Normal
        }

        public static LogLevel currLogLevel = LogLevel.Normal;

        public static void setLogLevel()
        {
            try
            {
                VSFramework.RegistryHandler registry = new VSFramework.RegistryHandler();
                string level = registry.ReadFromRegistry("Log Level").ToString();
                currLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), level, true);
            }
            catch (Exception ex)
            {
                currLogLevel = LogLevel.Normal;
            }
        }

        public static void WriteTestResults(String ServerName, String Category, String TestName, String Result, String Details)
        {
            // Modify to try an update first, if that fails then do an insert
            string strSQL;
            strSQL = "Insert INTO StatusDetail(TypeAndName, Category, TestName, Result, Details, LastUpdate) VALUES ( 'Exchange-" + ServerName + "', '" + Category + "', '" + TestName + "', '" + Result + "', '" + Details + "', '" + DateTime.Now.ToString() + "')";
            CommonDB DB = new CommonDB();
            DB.Execute(strSQL);
        }

        public static void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, LogLevel Loglvl = LogLevel.Normal)
        {
            WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, commonEnums.ServerRoles.Empty, Loglvl);
        }

        public static void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, commonEnums.ServerRoles Role, LogLevel Loglvl = LogLevel.Verbose)
        {
            try
            {
                DeviceType = DeviceType[0].ToString().ToUpper() + DeviceType.Substring(1).ToLower();
                string DeviceLogDestination;
                string path;
                bool appendMode = true;
                string LogUtilDest = "";

                string[] FolderExceptions = { "All", "DailyTask", "HourlyTask", "DAG" };
                FolderExceptions = FolderExceptions.Select(s => s.ToLowerInvariant()).ToArray();

                //Depending on the log level, either write this out to the log or not
                if (Loglvl < currLogLevel)
                    return;

                if (DeviceName.IndexOf("/") > 0 || DeviceName.IndexOf("\\") > 0)
                {
                    DeviceName = DeviceName.Replace("/", "_");
                    DeviceName = DeviceName.Replace("\\", "_");
                }

                //if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\Microsoft\\"))
                //	System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\Microsoft\\");

                try
                {
                    if (FolderExceptions.Contains(DeviceName.ToLowerInvariant()) || DeviceType == "All" || DeviceType == "ALL")// || DeviceName == "DailyTask" || DeviceName == "HourlyTask")
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\Microsoft\\";
                        LogUtilDest = "Microsoft\\";
                    }
                    else
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\Microsoft\\" + DeviceType + "\\";
                        //System.IO.Directory.CreateDirectory(path);

                        path += DeviceType + "_" + DeviceName + "\\";
                        //System.IO.Directory.CreateDirectory(path);

                        LogUtilDest = "Microsoft\\" + DeviceType + "\\" + DeviceType + "_" + DeviceName + "\\";

                    }
                }
                catch (Exception)
                {
                    throw;
                }


                if (Role == commonEnums.ServerRoles.Empty)
                {
                    try
                    {
                        DeviceLogDestination = path + DeviceType + "_" + DeviceName + "_Log.txt";
                        LogUtilDest += DeviceType + "_" + DeviceName + "_Log.txt";
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        DeviceLogDestination = path + DeviceType + "_" + DeviceName + "_" + Role.ToString() + "_Log.txt";
                        LogUtilDest += DeviceType + "_" + DeviceName + "_" + Role.ToString() + "_Log.txt";
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                try
                {
                    if (DeviceName.IndexOf("stme") > 0)
                    {
                        if (DeviceLogDestination.IndexOf("http://") > 0)
                        {
                            DeviceLogDestination = DeviceLogDestination.Replace("http://", "");
                        }
                        if (DeviceLogDestination.IndexOf(":8088") > 0)
                        {
                            DeviceLogDestination = DeviceLogDestination.Replace(":8088", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //   WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString() + " Exception Computing log file name as  " + ex.ToString());
                }

                try
                {
                    if (DeviceName.IndexOf("stme") > 0)
                    {
                        if (LogUtilDest.IndexOf("http://") > 0)
                        {
                            LogUtilDest = LogUtilDest.Replace("http://", "");
                        }
                        if (LogUtilDest.IndexOf(":8088") > 0)
                        {
                            LogUtilDest = LogUtilDest.Replace(":8088", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //   WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString() + " Exception Computing log file name as  " + ex.ToString());
                }


                LogUtilities.LogUtils.WriteLogEntries(LogUtilDest, DateTime.Now.ToString() + "  " + strMsg);
                return;

                try
                {
                    StreamWriter sw = new StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode);
                    sw.WriteLine(DateTime.Now.ToString() + "  " + strMsg);
                    sw.Close();
                    sw = null;
                }
                catch (Exception ex)
                {
                }
                GC.Collect();
            }
            catch
            {
            }
            //}
        }

        //public static void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, commonEnums.ServerRoles Role, LogLevel Loglvl = LogLevel.Normal)
        //{
        //    WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, Role, "Exchange", Loglvl);
        //}


        //public static string getRoleClass(string roleType)
        //{

        //    if (roleType == "HUB")
        //        return "VitalSignsMicrosoftClasses.ExchangeHUB";
        //    if (roleType == "CAS")
        //        return "VitalSignsMicrosoftClasses.ExchangeCAS";
        //    return "";
        //}

        public static ArrayList getRoleClasses(string[] Roles, string Version)
        {
            ArrayList ArClasses = new ArrayList();

            bool HubEdgeDone = false;

            foreach (string s in Roles)
            {

                if (s.ToUpper() == "CAS")
                {
                    if (Version == "2013")
                    {
                        //ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeHUB");
                        //ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeEDGE");
                        ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeHubEdge");
                    }
                    ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeCAS");
                }
                if (!HubEdgeDone && (s.ToUpper() == "HUB" || s.ToUpper() == "EDGE"))
                {
                    ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeHubEdge");
                    HubEdgeDone = true;
                }
                //if (s.ToUpper() == "EDGE")
                //	ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeHubEdge");
                if (s.ToUpper() == "MAILBOX" || s.ToUpper() == "MB")
                {
                    ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeMB");
                }


                //if (s.ToUpper() == "UNIFIED MESSAGING" || s.ToUpper() == "UM")
                //    ArClasses.Add("VitalSignsMicrosoftClasses.ExchangeUM");
            }


            return ArClasses;
        }
        public static SecureString String2SecureString(string password)
        {
            SecureString remotePassword = new SecureString();
            for (int i = 0; i < password.Length; i++)
                remotePassword.AppendChar(password[i]);

            return remotePassword;
        }

        public static ReturnPowerShellObjects PrereqForExchangeWithCmdlets(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role, string cmdlets, string AuthenticationType)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {
                //WriteDeviceHistoryEntry("Exchange", ServerName, DateTime.Now.ToString() + " : In PrereqForExchange.", LogLevel.Normal);

                System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);

                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));

                PowerShell powershell = PowerShell.Create();

                PSCommand command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ConfigurationName", "Microsoft.Exchange");
                command.AddParameter("ConnectionUri", uri);
                command.AddParameter("Credential", creds);
                command.AddParameter("Authentication", AuthenticationType);
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                PSSessionOption sessionOption = new PSSessionOption();
                sessionOption.SkipCACheck = true;
                sessionOption.SkipCNCheck = true;
                sessionOption.SkipRevocationCheck = true;

                command.AddParameter("SessionOption", sessionOption);
                powershell.AddScript(@"set-executionpolicy unrestricted");
                powershell.Commands = command;


                // open the remote runspace
                //runspace.Open();
                // associate the runspace with powershell
                powershell.Runspace = runspace;
                powershell.Runspace.Open();
                PSObj.PS = powershell;
                // invoke the powershell to obtain the results

                Collection<PSObject> result = powershell.Invoke();


                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry("Exchange", ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

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


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry("Exchange", ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }

                WriteDeviceHistoryEntry("Exchange", ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                WriteDeviceHistoryEntry("Exchange", ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);


                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "EX: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

                // First import the cmdlets in the current runspace (using Import-PSSession)

                command = new PSCommand();
                command.AddScript("Import-PSSession -Session $ra " + cmdlets + " -FormatTypeName *");
                powershell.Commands = command;
                powershell.Invoke();

                string searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    PSObj.ErrorMessage = "The Exchange Module was not able to be located";
                }

                WriteDeviceHistoryEntry("Exchange", ServerName, "Imported the PSSession.", role, LogLevel.Normal);



                PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                PSObj.Connected = true;
            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "EX Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "EX Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;

                WriteDeviceHistoryEntry("Exchange", ServerName, "Error in PrereqForExchange: " + ex.Message, role, LogLevel.Normal);
            }


            return PSObj;

        }
        public static ReturnPowerShellObjects PrereqForSharepointWithCmdlets(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role, string cmdlets, string[] Modules = null)
        {
            Modules = Modules ?? new string[0];

            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {
                //WriteDeviceHistoryEntry("Exchange", ServerName, DateTime.Now.ToString() + " : In PrereqForExchange.", LogLevel.Normal);

                //System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);
                //WSManConnectionInfo wsman = new WSManConnectionInfo(true, ServerName,443,@"/wsman",@"http://schemas.microsoft.com/powershell/Microsoft.PowerShell",creds);


                //Runspace runspace = RunspaceFactory.CreateRunspace(wsman);

                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));


                PowerShell powershell = PowerShell.Create();
                PSObj.PS = powershell;
                PSCommand command = new PSCommand();
                //command.AddScript("enable-wsmancredssp -role client -delegatecomputer * -force");
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                //powershell.Commands = command;
                //powershell.AddScript("enable-wsmancredssp -role client -delegatecomputer * -force");


                // open the remote runspace
                runspace.Open();
                // associate the runspace with powershell
                powershell.Runspace = runspace;
                //Collection<PSSession> temp = powershell.Invoke<PSSession>();
                // invoke the powershell to obtain the results
                Collection<PSObject> result;// = powershell.Invoke<PSSession>();

                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                }
                command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ComputerName", IPAddress.Replace("https://", "").Replace("http://", ""));
                command.AddParameter("Credential", creds);
                command.AddParameter("Authentication", "Credssp");
                //command.AddParameter("Authentication", "Default");
                powershell.Commands.Clear();
                powershell.Streams.Error.Clear();
                powershell.Commands = command;
                result = powershell.Invoke<PSObject>();
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

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


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke<PSObject>();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;


                WriteDeviceHistoryEntry(ServerType, ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                }
                WriteDeviceHistoryEntry(ServerType, ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);

                command = new PSCommand();
                command.AddScript("Invoke-Command -Session $ra -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell;}");
                powershell.Commands = command; ;
                powershell.Invoke();
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer  Importing the Snapin: " + strError, role, LogLevel.Normal);
                }
                WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the Snapin.", role, LogLevel.Normal);

                foreach (String mod in Modules)
                {
                    try
                    {
                        command = new PSCommand();
                        command.AddCommand("Import-Module");
                        command.AddParameter("Name", mod);
                        powershell.Commands = command;
                        powershell.Invoke();

                        WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the module " + mod + ".", role, LogLevel.Normal);
                    }
                    catch (Exception ex)
                    {
                        WriteDeviceHistoryEntry(ServerType, ServerName, "Exception importing module " + mod + ". Exception: " + ex.Message, role, LogLevel.Normal);
                    }
                }

                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "SP: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

                PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                PSObj.Connected = true;

            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "SP Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "SP Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry(ServerType, ServerName, "Error in PrereqForSharePoint: " + ex.Message, role, LogLevel.Normal);
            }


            return PSObj;
        }

        public static ReturnPowerShellObjects PrereqForSharepointDBWithCmdlets(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role, string cmdlets)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {

                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);
                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
                PowerShell powershell = PowerShell.Create();
                PSObj.PS = powershell;
                PSCommand command = new PSCommand();
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();



                runspace.Open();
                powershell.Runspace = runspace;
                Collection<PSObject> result;

                WriteDeviceHistoryEntry(ServerType, ServerName, "After Invoke Pipeline", role, LogLevel.Normal);
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }
                command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ComputerName", IPAddress.Replace("https://", "").Replace("http://", ""));
                command.AddParameter("Credential", creds);

                powershell.Commands.Clear();
                powershell.Streams.Error.Clear();
                powershell.Commands = command;
                result = powershell.Invoke<PSObject>();
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

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


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke<PSObject>();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;


                WriteDeviceHistoryEntry(ServerType, ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();
                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                }
                WriteDeviceHistoryEntry(ServerType, ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);

                // First import the cmdlets in the current runspace (using Import-PSSession)
                command = new PSCommand();
                string scrpt = "Invoke-Command $ra -ScriptBlock { Get-WMIObject Win32_Service | ?{$_.Name -eq 'MSSQLSERVER'} | Select-Object Name, Caption, State }";
                command.AddScript(scrpt);
                powershell.Commands = command;

                results = powershell.Invoke();

                if (results.Count == 0)
                {
                    //Not a db server
                    PSObj.Connected = false;
                    return null;
                }
                PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                PSObj.Connected = true;

                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "SPDB: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "SPDB Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "SPDB Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry(ServerType, ServerName, "Error in PrereqForSharePoint: " + ex.Message, role, LogLevel.Normal);
            }

            return PSObj;

        }

        public static ReturnPowerShellObjects PrereqForWindows(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            PSObj.ServerType = "Windows";
            try
            {

                //System.Uri uri = new Uri(IPAddress + "/powershell?serializationLevel=Full");
                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);

                //Runspace runspace = RunspaceFactory.CreateRunspace();
                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
                //Runspace runspace = RunspaceFactory.CreateRunspacePool();

                PowerShell powershell = PowerShell.Create();
                PSObj.PS = powershell;
                PSCommand command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ComputerName", IPAddress.Replace("http://", "").Replace("https://", ""));
                command.AddParameter("Credential", creds);
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                PSSessionOption sessionOption = new PSSessionOption();
                sessionOption.SkipCACheck = true;
                sessionOption.SkipCNCheck = true;
                sessionOption.SkipRevocationCheck = true;

                command.AddParameter("SessionOption", sessionOption);
                powershell.AddScript(@"set-executionpolicy unrestricted");
                powershell.Commands = command;


                runspace.Open();
                powershell.Runspace = runspace;
                Collection<PSObject> result = powershell.Invoke();

                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

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


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception from IPAddress: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;


                WriteDeviceHistoryEntry("Windows", ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "Win: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

                WriteDeviceHistoryEntry("Windows", ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);

                //PSObj.runspace = runspace;
                PSObj.Connected = true;


            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "Win Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "Win Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }

                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry("Windows", ServerName, "Error in prefeqForWindows: " + ex.Message, role, LogLevel.Normal);
            }


            return PSObj;

        }


        public static ReturnPowerShellObjects PrereqForActiveDirectoryWithCmdlets(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role, string cmdlets)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {
                //WriteDeviceHistoryEntry("Exchange", ServerName, DateTime.Now.ToString() + " : In PrereqForExchange.", LogLevel.Normal);

                System.Security.SecureString securePassword = String2SecureString(Password);
                PSCredential creds = new PSCredential(UserName, securePassword);
                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
                PowerShell powershell = PowerShell.Create();
                PSObj.PS = powershell;
                PSCommand command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("computer", IPAddress);
                command.AddParameter("Credential", creds);

                powershell.Commands = command;


                // open the remote runspace
                runspace.Open();
                // associate the runspace with powershell
                powershell.Runspace = runspace;
                // invoke the powershell to obtain the results
                Collection<PSObject> result = powershell.Invoke<PSObject>();

                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", role, LogLevel.Normal);

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


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke<PSObject>();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry(ServerType, ServerName, "Error in StartProcessForServer: " + strError, role, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;

                WriteDeviceHistoryEntry(ServerType, ServerName, "Connection established.", role, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                WriteDeviceHistoryEntry(ServerType, ServerName, "Set the local variable for the runspace.", role, LogLevel.Normal);

                // First import the cmdlets in the current runspace (using Import-PSSession)
                command = new PSCommand();
                command.AddScript("Invoke-Command -session $ra -script { Import-Module ActiveDirectory }");
                powershell.Commands = command;
                powershell.Invoke();

                string searchMsg = "was not loaded because no valid module file was found in any module directory.";
                if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    PSObj.ErrorMessage = "The Active Directory Module was not able to be located";
                }


                command = new PSCommand();
                command.AddScript("Import-PSSession -Session $ra -module ActiveDirectory");
                powershell.Commands = command;
                powershell.Invoke();

                searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powershell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    PSObj.ErrorMessage = "The Active Directory Module was not able to be located";
                }

                WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the PSSession.", role, LogLevel.Normal);

                PSObj.PS = powershell;
                //PSObj.runspace = runspace;
                PSObj.Connected = true;

                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                Collection<PSObject> results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "AD: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "AD Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "AD Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry(ServerType, ServerName, "Error in PrereqForActiveDirectory: " + ex.Message, role, LogLevel.Normal);
            }


            return PSObj;

        }
        public static ReturnPowerShellObjects PrereqForLync(string ServerName, string UserName, string Password, string IPAddress)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {
                //WriteDeviceHistoryEntry("Exchange", ServerName, DateTime.Now.ToString() + " : In PrereqForExchange.", LogLevel.Normal);

                System.Uri uri = new Uri(IPAddress + "/OcsPowershell");
                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);

                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
                //Runspace runspace = RunspaceFactory.CreateRunspacePool();

                PowerShell powershell = PowerShell.Create();
                PSObj.PS = powershell;
                PSCommand command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ConnectionUri", uri);
                command.AddParameter("Credential", creds);
                command.AddParameter("Authentication", "Default");
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                PSSessionOption sessionOption = new PSSessionOption();
                sessionOption.SkipCACheck = true;
                sessionOption.SkipCNCheck = true;
                sessionOption.SkipRevocationCheck = true;

                command.AddParameter("SessionOption", sessionOption);
                powershell.AddScript(@"set-executionpolicy unrestricted");
                powershell.Commands = command;


                // open the remote runspace
                runspace.Open();
                // associate the runspace with powershell
                powershell.Runspace = runspace;
                // invoke the powershell to obtain the results
                Collection<PSObject> result = powershell.Invoke<PSObject>();

                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                    WriteDeviceHistoryEntry("Skype for Business", ServerName, "Error in StartProcessForServer: " + strError, commonEnums.ServerRoles.Empty, LogLevel.Verbose);

                    CheckForPowerShellConnectionErrors(PSObj, powershell.Streams.Error);
                }

                if (result.Count != 1)
                {
                    WriteDeviceHistoryEntry("Skype for Business", ServerName, "Could not connect via the FQDN.  Will try to get IPAddress and use that.", commonEnums.ServerRoles.Empty, LogLevel.Normal);

                    powershell.Streams.Error.Clear();

                    string script = @"$computerName = '" + ServerName + @"'

									$ipconfigSet = [System.Net.Dns]::GetHostAddresses($computerName) | where {$_.IsIPv6LinkLocal -eq $false } |  select IPAddressToString
									#get-wmiObject Win32_NetworkAdapterConfiguration -ComputerName $computerName -Credential $cred | select IPAdd

									foreach($ipObj in $ipconfigSet )
									{

										$ip = $ipObj.IPAddressToString + '/OcsPowershell'

										$tempSession = New-PSSession -Credential $cred -ComputerName $ip
										if($tempSession.State.toString() -eq 'Opened')
										{
											$tempSession
											Return
            
										}

									}";


                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "cred");
                    command.AddParameter("Value", creds);

                    command.AddScript(script);
                    powershell.Commands = command;

                    result = powershell.Invoke<PSObject>();

                    foreach (ErrorRecord current in powershell.Streams.Error)
                    {
                        string strError = "Exception: " + current.Exception.ToString() + ",\r\nInner Exception: " + current.Exception.InnerException;
                        WriteDeviceHistoryEntry("Skype for Business", ServerName, "Error in StartProcessForServer: " + strError, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                    }


                    if (result.Count != 1)
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;


                WriteDeviceHistoryEntry("Skype for Business", ServerName, "Connection established.", commonEnums.ServerRoles.Empty, LogLevel.Normal);

                // Set the runspace as a local variable on the runspace
                command = new PSCommand();
                command.AddScript("$ra = $(Get-PSSession)[0]");
                powershell.Commands = command; ;
                powershell.Invoke();

                // First import the cmdlets in the current runspace (using Import-PSSession)

                command = new PSCommand();
                command.AddScript("Import-PSSession -Session $ra");
                powershell.Commands = command;
                powershell.Invoke();

                PSObj.PS = powershell;
                //PSObj.runspace = runspace;

                PSObj.Connected = true;

                command = new PSCommand();
                command.AddScript("$PID");
                powershell.Commands = command; ;
                results = powershell.Invoke();

                WriteDeviceHistoryEntry("All", "Microsoft_", "Lync: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry("All", "Microsoft_", "Lync Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry("All", "Microsoft_", "Lync Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry("Skype for Business", ServerName, "Error in PrereqForLync: " + ex.Message, LogLevel.Normal);
            }


            return PSObj;

        }
        public static ReturnPowerShellObjects PrereqForOffice365WithCmdlets(string ServerName, string UserName, string Password, string ServerType, string IPAddress, commonEnums.ServerRoles role, string cmdlets, MonitoredItems.Office365Server Server)
        {
            ReturnPowerShellObjects PSObj = new ReturnPowerShellObjects();
            try
            {
                string tenantName = "";
                if (UserName != "")
                    tenantName = UserName.Split('@')[1].ToString().Split('.')[0].ToString();
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets", Common.LogLevel.Normal);
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before Import", Common.LogLevel.Normal);
                InitialSessionState session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "MSOnline" });
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after Import", Common.LogLevel.Normal);

                System.Uri uri = new Uri(IPAddress + "/powershell-liveid/");
                System.Security.SecureString securePassword = String2SecureString(Password);

                PSCredential creds = new PSCredential(UserName, securePassword);
                Runspace runspace = RunspaceFactory.CreateOutOfProcessRunspace(new TypeTable(new string[0]));
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: before open", Common.LogLevel.Normal);
                runspace.Open();
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets: after open", Common.LogLevel.Normal);
                PowerShell powerShell = PowerShell.Create();
                PSObj.PS = powerShell;
                Common.WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets-2", Common.LogLevel.Normal);


                powerShell.Runspace = runspace;

                PSCommand psSession = new PSCommand();
                psSession.AddCommand("New-PSSession");
                psSession.AddParameter("ConfigurationName", "Microsoft.Exchange");
                psSession.AddParameter("ConnectionUri", uri);
                psSession.AddParameter("Credential", creds);
                psSession.AddParameter("Authentication", "Basic");
                psSession.AddParameter("AllowRedirection");

                powerShell.Commands = psSession;
                //var result = powerShell.Invoke();
                Collection<PSObject> result = powerShell.Invoke<PSObject>();
                //Common.WriteDeviceHistoryEntry("All", ServerType, "In  PrereqForOffice365WithCmdlets result:" + result[0], Common.LogLevel.Normal);
                Server.AuthenticationTest = true;
                foreach (ErrorRecord err in powerShell.Streams.Error)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets error" + err.Exception, Common.LogLevel.Normal);
                    if ((err.Exception.Message.ToLower().Contains("access denied")) || ((err.Exception.Message.ToLower().Contains("failed"))))
                        Server.AuthenticationTest = false;
                }

                //PSSession pssession = (PSSession)result[0];
                //PSObj.Session = pssession;



                PSCommand setVar = new PSCommand();
                setVar.AddScript("$ra = $(Get-PSSession)[0]");

                powerShell.Commands = setVar;
                powerShell.Runspace = runspace;
                powerShell.Invoke();

                PSCommand importSession = new PSCommand();
                importSession.AddScript("Import-PSSession -AllowClobber -Session $ra " + cmdlets + " -FormatTypeName *");
                powerShell.Commands = importSession;
                powerShell.Runspace = runspace;
                powerShell.Invoke();

                string searchMsg = "Running the Get-Command command in a remote session returned no results";
                if (powerShell.Streams.Error.Where(record => record.Exception.ToString().Contains(searchMsg)).ToArray().Length > 0)
                {
                    PSObj.ErrorMessage = "The module was not able to be located";
                }


                PSCommand command;

                foreach (String mod in new String[] { "MSOnlne", "Microsoft.Online.SharePoint.PowerShell" })
                {
                    command = new PSCommand();
                    command.AddCommand("Import-Module");
                    command.AddParameter("Name", mod);
                    powerShell.Commands = command;
                    powerShell.Invoke();

                    WriteDeviceHistoryEntry(ServerType, ServerName, "Imported the module " + mod + ".", LogLevel.Normal);
                }


                WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before MSOL connection", Common.LogLevel.Normal);
                PSCommand connect = new PSCommand();
                connect.AddCommand("Connect-MsolService");
                connect.AddParameter("Credential", creds);
                powerShell.Commands = connect;
                powerShell.Invoke();
                WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after MSOL connection", Common.LogLevel.Normal);
                PSCommand connectSPOL = new PSCommand();
                connectSPOL.AddCommand("Connect-SPOService");
                WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection", Common.LogLevel.Normal);
                string sharePointURL = "";
                if (tenantName != "")
                {
                    Server.tenantName = tenantName;
                    sharePointURL = "https://" + tenantName + "-admin.sharepoint.com";
                }
                WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets before SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);
                connectSPOL.AddParameter("Url", sharePointURL);
                connectSPOL.AddParameter("Credential", creds);
                powerShell.Commands = connectSPOL;
                powerShell.Invoke();
                WriteDeviceHistoryEntry(ServerType, ServerName, "In  PrereqForOffice365WithCmdlets after SPO connection-URL:" + sharePointURL, Common.LogLevel.Normal);

                PSObj.PS = powerShell;
                //PSObj.runspace = runspace;
                PSObj.Connected = true;

                command = new PSCommand();
                command.AddScript("$PID");
                powerShell.Commands = command; ;
                Collection<PSObject> results = powerShell.Invoke();

                WriteDeviceHistoryEntry(ServerType, ServerName, "O365: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);

            }
            catch (Exception ex)
            {
                try
                {
                    PSCommand command = new PSCommand();
                    command.AddScript("$PID");
                    PSObj.PS.Commands = command; ;
                    Collection<PSObject> results = PSObj.PS.Invoke();

                    WriteDeviceHistoryEntry(ServerType, ServerName, "O365 Failed: $PID = " + results[0].BaseObject, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                catch (Exception ex2)
                {
                    WriteDeviceHistoryEntry(ServerType, ServerName, "O365 Failed: " + ex2.Message, commonEnums.ServerRoles.Empty, LogLevel.Normal);
                }
                PSObj.Connected = false;
                //if (PSObj.PS == null)
                //PSObj.PS = PowerShell.Create();
                //if (PSObj.runspace == null)
                //	PSObj.runspace = RunspaceFactory.CreateRunspace();
                //if (PSObj.PS.Runspace == null)
                //	PSObj.PS.Runspace = PSObj.runspace;
                WriteDeviceHistoryEntry(ServerType, ServerName, " Failed Imported the PSSession." + ex.Message.ToString(), role, LogLevel.Normal);
            }


            return PSObj;
            //    }
            //}
        }
        public static void SetupServer(MonitoredItems.MicrosoftServer myServer, string ServerType, TestResults AllTestsResults, string cmdList = "", string AuthenticationType = "")
        {
            if (myServer.IsPrereqsDone)
                return;
            ReturnPowerShellObjects results = null;
            System.Collections.ObjectModel.Collection<PSObject> result = new System.Collections.ObjectModel.Collection<PSObject>();
            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Starting SetupServer.", Common.LogLevel.Normal);
            try
            {
                switch (myServer.ServerType)
                {
                    case "Exchange":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In PrereqForExchange in SetupServer ", Common.LogLevel.Normal);
                        using (results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, "Exchange", myServer.IPAddress, commonEnums.ServerRoles.Windows, cmdList, AuthenticationType))
                        {

                            PowerShell powershell = results.PS;
                            System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\PrereqForExchange.ps1");
                            String str = sr.ReadToEnd();

                            powershell.AddScript(str);
                            result = powershell.Invoke();
                            foreach (ErrorRecord ex in powershell.Streams.Error)
                            {
                                Common.WriteDeviceHistoryEntry("All", "Exchange", "error  went." + ex.Exception.ToString(), Common.LogLevel.Normal);
                            }
                            if (results.Connected == true && result.Count > 0)
                            {
                                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending for PrereqForExchange in SetupServer  ", Common.LogLevel.Normal);
                                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In PrereqForExchangeWin in SetupServer  ", Common.LogLevel.Normal);

                                using (results = Common.PrereqForWindows(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty))
                                {


                                    PowerShell powershells = results.PS;
                                    System.IO.StreamReader srt = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\PrereqForExchangeWin.ps1");
                                    String strn = srt.ReadToEnd();

                                    powershells.AddScript(strn);
                                    result = powershells.Invoke();
                                    foreach (ErrorRecord ex in powershells.Streams.Error)
                                    {
                                        Common.WriteDeviceHistoryEntry("All", "Exchange", "error  went." + ex.Exception.ToString(), Common.LogLevel.Normal);
                                    }


                                }
                                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending for PrereqForExchangeWin in SetupServer ", Common.LogLevel.Normal);

                            }

                        }

                        break;
                    case "SharePoint":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In for PrereqForSharePoint in SetupServer ", Common.LogLevel.Normal);

                        using (results = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList))
                        {


                            PowerShell powershell = results.PS;
                            System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\PrereqForSharePoint.ps1");
                            String str = sr.ReadToEnd();

                            powershell.AddScript(str);
                            result = powershell.Invoke();
                            foreach (ErrorRecord ex in powershell.Streams.Error)
                            {
                                Common.WriteDeviceHistoryEntry("All", "Exchange", "error  went." + ex.Exception.ToString(), Common.LogLevel.Normal);
                            }

                        }
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending for PrereqForSharePoint in SetupServer ", Common.LogLevel.Normal);

                        break;
                    case "Active Directory":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In  PrereqForActiveDirectory in SetupServer ", Common.LogLevel.Normal);

                        using (results = Common.PrereqForActiveDirectoryWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList))
                        {


                            PowerShell powershell = results.PS;
                            System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\PrereqForActiveDirectory.ps1");
                            String str = sr.ReadToEnd();

                            powershell.AddScript(str);
                            result = powershell.Invoke();
                            foreach (ErrorRecord ex in powershell.Streams.Error)
                            {
                                Common.WriteDeviceHistoryEntry("All", "Exchange", "error  went." + ex.Exception.ToString(), Common.LogLevel.Normal);
                            }

                        }
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending for PrereqForActiveDirectory in SetupServer ", Common.LogLevel.Normal);

                        break;
                    case "Windows":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In for PrereqForWindows in SetupServer ", Common.LogLevel.Normal);

                        using (results = Common.PrereqForWindows(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty))
                        {


                            PowerShell powershell = results.PS;
                            System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\PrereqForWindows.ps1");
                            String str = sr.ReadToEnd();

                            powershell.AddScript(str);
                            result = powershell.Invoke();
                            foreach (ErrorRecord ex in powershell.Streams.Error)
                            {
                                Common.WriteDeviceHistoryEntry("All", "Exchange", "error  went." + ex.Exception.ToString(), Common.LogLevel.Normal);
                            }

                        }
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ending for PrereqForWindows in SetupServer ", Common.LogLevel.Normal);

                        break;


                }

                bool completed = false;
                if (results.Connected == true && result.Count > 0)
                {
                    completed = true;
                }
                else
                {
                    completed = false;
                }
                myServer.IsPrereqsDone = completed;
                MongoStatementsUpdate<VSNext.Mongo.Entities.Server> mongoUpdateStatement = new MongoStatementsUpdate<Server>();
                mongoUpdateStatement.filterDef = mongoUpdateStatement.repo.Filter.Eq(x => x.Id, myServer.ServerObjectID);
                mongoUpdateStatement.updateDef = mongoUpdateStatement.repo.Updater.Set(x => x.ArePrerequisitesDone, completed);
                AllTestsResults.MongoEntity.Add(mongoUpdateStatement);
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", myServer.ServerType, "Error getting in SetupServer " + ex);
            }
        }

        public static string ServerStatusCode(string Status)
        {
            string StatusCode = "OK";
            try
            {
                switch (Status)
                {
                    case "OK":
                        StatusCode = "OK";
                        break;
                    case "Scanning":
                        StatusCode = "OK";
                        break;
                    case "Maintenance":
                        StatusCode = "Maintenance";
                        break;
                    case "Not Responding":
                        StatusCode = "Not Responding";
                        break;
                    case "Not Scanned":
                        StatusCode = "Not Scanned";
                        break;
                    case "Disabled":
                        StatusCode = "";
                        break;
                    case "Insufficient Licenses":
                        StatusCode = "Issue";
                        break;
                    default:
                        StatusCode = "Issue";
                        break;
                }
            }
            catch (Exception)
            {

                StatusCode = "";
            }
            return StatusCode;
        }

        public static void makeAlert(double Actual, double threshold, MonitoredItems.MicrosoftServer server, commonEnums.AlertType AlertType, ref TestResults AllTestsList, string category)
        {
            try
            {
                if (threshold == 0)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_").Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " has a threshold set of 0, so will not send alerts.", Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else if (threshold < 0)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " threshold is not set.  Please set a value in the configurator if you would like to receive alerts.", Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else if (threshold > Actual)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " is normal", Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " Failed. The threshold was set at " + threshold.ToString() + " , but the actual was:" + Actual.ToString(), Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.No, Category = category });
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(server.ServerType, server.Name, "Error in make alert: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }

        }

        public static void makeAlert(double Actual, double threshold, MonitoredItems.MicrosoftServer server, commonEnums.AlertType AlertType, ref TestResults AllTestsList, string AlertDetails, string category)
        {
            if (AlertDetails == "")
                makeAlert(Actual, threshold, server, AlertType, ref AllTestsList, category);

            try
            {
                if (threshold == 0)
                {

                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else if (threshold < 0)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else if (threshold > Actual)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.No, Category = category });
                }

            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(server.ServerType, server.Name, "Error in make alert: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }

        }

        public static void makeAlert(bool ResetAlert, MonitoredItems.MicrosoftServer server, commonEnums.AlertType AlertType, ref TestResults AllTestsList, string category)
        {
            try
            {

                if (ResetAlert)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " is normal", Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = "The " + AlertType.ToString() + " has an issue, detected ", Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.No, Category = category });
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(server.ServerType, server.Name, "Error in make alert: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }
        }

        public static void makeAlert(bool ResetAlert, MonitoredItems.MicrosoftServer server, commonEnums.AlertType AlertType, ref TestResults AllTestsList, string AlertDetails, string category)
        {
            try
            {
                if (ResetAlert)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertType = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.No, Category = category });
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(server.ServerType, server.Name, "Error in make alert: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }
        }
        public static void makeAlert(bool ResetAlert, MonitoredItems.MicrosoftServer server, string AlertType, ref TestResults AllTestsList, string AlertDetails, string category)
        {

            try

            {

                if (ResetAlert)
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertTypeString = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.Yes, Category = category });
                }
                else
                {
                    AllTestsList.AlertDetails.Add(new Alerting() { DeviceType = (commonEnums.AlertDevice)Enum.Parse(typeof(commonEnums.AlertDevice), server.ServerType.Replace(" ", "_"), true), DeviceName = server.Name, AlertTypeString = AlertType, Details = AlertDetails, Location = server.Location, ResetAlertQueue = commonEnums.ResetAlert.No, Category = category });
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(server.ServerType, server.Name, "Error in make alert: " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
            }
        }

        #region LicenseHandling

        public static string decodePasswordFromEncodedString(string s, string serverName)
        {

            TripleDES tripledes = new TripleDES();
            try
            {

                string[] str1 = s.Replace(" ", "").Split(',');

                byte[] bytes = str1.Select(t => Convert.ToByte(t)).ToArray();

                return tripledes.Decrypt(bytes);
            }
            catch (Exception ex)
            {
                //Common.WriteDeviceHistoryEntry("All", serverType, "The password for " + serverName + " does not seem to be encrypted and will use the raw string.  Error: " + ex.Message);
                return s;
            }
        }

        public static Boolean OffHours(string ServerName)
        {

            MaintenanceDLL.MaintenanceDll maint = new MaintenanceDLL.MaintenanceDll();

            try
            {
                if (maint.OffHours(ServerName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public static ReturnPowerShellObjects TestRepsonding(MonitoredItems.MicrosoftServer myServer, ref bool notResponding, ref TestResults AllTestsList, string cmdList = "", string AuthenticationType = "")
        {
            ReturnPowerShellObjects results = null;
            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Starting thread for Test Responding.", Common.LogLevel.Normal);
            //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance",""+ myServer.ServerType+ myServer.Name+ "Starting thread for Test Responding at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);

            try
            {
                switch (myServer.ServerType)
                {
                    case "Exchange":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Exchange at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, "Exchange", myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList, AuthenticationType);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Ended prereq for Exchange at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                    case "SharePoint":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Sharepoint at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForSharepointWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList);
                        if (results.Connected == false)
                            results = Common.PrereqForSharepointDBWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Ended prereq for Sharepoint at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                    case "Active Directory":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Active Directory at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForActiveDirectoryWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Ended prereq for Active Directory at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                    case "Windows":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Windows at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForWindows(myServer.Name, myServer.UserName, myServer.Password, myServer.ServerType, myServer.IPAddress, commonEnums.ServerRoles.Empty);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Windows at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                    case "Skype_for_Business":
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Skype_for_Business at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForLync(myServer.Name, myServer.UserName, myServer.Password, myServer.IPAddress);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Skype_for_Business at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                    default:
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Starting prereq for Exchange default at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        results = Common.PrereqForExchangeWithCmdlets(myServer.Name, myServer.UserName, myServer.Password, "Exchange", myServer.IPAddress, commonEnums.ServerRoles.Empty, cmdList, AuthenticationType);
                        //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "" + myServer.ServerType + myServer.Name + "Ended prereq for Exchange default at" + DateTime.Now.ToString() + " ", Common.LogLevel.Normal);
                        break;
                }
                if (results == null || results.Connected == false || results.ErrorMessage != "")
                {

                    if (results.PreferedStatus == "")
                    {

                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Was unable to connect to the server.  Will be marked as Not Responding.", Common.LogLevel.Normal);
                        notResponding = true;
                        myServer.IncrementDownCount();
                        myServer.AlertCondition = true;
                        if (myServer.FailureCount >= myServer.FailureThreshold)
                        {
                            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Was unable to connect to the server beyond the threshold limit.  Will now send an alert", Common.LogLevel.Normal);
                            Common.makeAlert(false, myServer, commonEnums.AlertType.Not_Responding, ref AllTestsList, "The server is not responding", myServer.ServerType);
                            //LogUtilities.LogUtils.WriteEntryForNotResponding(myServer.Name, myServer.ServerType);
                        }

                        CommonDB DB = new CommonDB();

                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Running Not Responding Quries", Common.LogLevel.Verbose);
                        DB.UpdateSQLStatements(AllTestsList, myServer);
                        string ErrorMessage = results == null ? "" : results.ErrorMessage.ToString();
                        DB.NotRespondingQueries(myServer, myServer.ServerType, ErrorMessage);

                    }
                    else
                    {

                        Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Was unable to connect due to server configuration. Will be marked as issue.", Common.LogLevel.Normal);
                        CommonDB DB = new CommonDB();

                        notResponding = true;
                        myServer.AlertCondition = false;
                        myServer.IncrementDownCount();

                        string ErrorMessage = results == null ? "" : results.ErrorMessage.ToString();
                        DB.SetServerStatus(myServer, myServer.ServerType, results.PreferedStatus, ErrorMessage);

                    }

                }
                else
                {
                    notResponding = false;
                    myServer.AlertCondition = false;
                    Common.makeAlert(true, myServer, commonEnums.AlertType.Not_Responding, ref AllTestsList, " The server is responding", myServer.ServerType);
                    myServer.IncrementUpCount();
                }

            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error testing if responding: Error " + ex.Message, Common.LogLevel.Normal);
            }
            GC.Collect();

            Thread.Sleep(5000);

            return results;
        }


        public static void RecordUpAndDownTimes(MonitoredItems.MicrosoftServer myServer, ref TestResults AllTestsList)
        {
            Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Starting Recording of Up and Down Times.", Common.LogLevel.Normal);

            try
            {
                AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "HourlyUpTimePercent", (myServer.UpPercentMinutes * 100).ToString()));
                AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "HourlyDownTimeMinutes", (myServer.DownMinutes).ToString()));
                myServer.ResetUpandDownCounts();
                if (myServer.ServerType == VSNext.Mongo.Entities.Enums.ServerType.Office365.ToDescription())
                {
                    MonitoredItems.Office365Server server = myServer as MonitoredItems.Office365Server;
                    foreach (MonitoredItems.Office365Server.ServiceTests test in server.ServiceResults)
                    {
                        //AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Services.HourlyUpTimePercent." + test.ServiceType + (String.IsNullOrWhiteSpace(test.TestName) ? "" : "." + test.TestName), (server.ServiceUpPercentMinutes(test.ServiceType, test.TestName) * 100).ToString()));
                        //AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Services.HourlyDownTimeMinutes." + test.ServiceType + (String.IsNullOrWhiteSpace(test.TestName) ? "" : "." + test.TestName), (server.ServiceDownMinutes(test.ServiceType, test.TestName)).ToString()));
                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Services.HourlyUpTimePercent." + test.ServiceType + "@" + myServer.Category, (server.ServiceUpPercentMinutes(test.ServiceType, test.TestName) * 100).ToString()));
                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoDailyStats(myServer, "Services.HourlyDownTimeMinutes." + test.ServiceType + "@" + myServer.Category, (server.ServiceDownMinutes(test.ServiceType, test.TestName)).ToString()));
                        test.ResetUpandDownCounts();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "Error recording up and down times: Error " + ex.Message, Common.LogLevel.Normal);
            }
            GC.Collect();

            Thread.Sleep(1000);
        }

        public static int GetWeekNumber(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

        }

        public static string GetInsertIntoDailyStats(string ServerName, string ServerTypeID, string StatName, string StatValue, string Details = "")
        {

            DateTime dtNow = DateTime.Now;

            string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
                        + " values('" + ServerName + "','" + ServerTypeID + "','" + dtNow + "','" + StatName + "','" + StatValue +
                        "'," + GetWeekNumber(dtNow) + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ",'')";

            return sqlQuery;

        }

        public static MongoStatementsInsert<DailyStatistics> GetInsertIntoDailyStats(MonitoredItems.MicrosoftServer server, string StatName, string StatValue, string Details = "")
        {

            DailyStatistics dailyStatistics = new DailyStatistics();
            dailyStatistics.DeviceId = server.ServerObjectID;
            dailyStatistics.StatName = StatName;
            dailyStatistics.StatValue = double.Parse(StatValue);
            dailyStatistics.DeviceName = server.Name;
            dailyStatistics.DeviceType = server.ServerType;

            MongoStatementsInsert<DailyStatistics> msi = new MongoStatementsInsert<DailyStatistics>();
            msi.listOfEntities.Add(dailyStatistics);
            return msi;

        }

        public static MongoStatementsInsert<SummaryStatistics> GetInsertIntoSummaryStats(MonitoredItems.MicrosoftServer server, string StatName, string StatValue, string Details = "")
        {

            SummaryStatistics dailyStatistics = new SummaryStatistics();
            dailyStatistics.DeviceId = server.ServerObjectID;
            dailyStatistics.StatName = StatName;
            dailyStatistics.StatValue = double.Parse(StatValue);
            dailyStatistics.DeviceType = server.ServerType;
            dailyStatistics.StatDate = DateTime.Now;
            dailyStatistics.DeviceName = server.Name;

            MongoStatementsInsert<SummaryStatistics> msi = new MongoStatementsInsert<SummaryStatistics>();
            msi.listOfEntities.Add(dailyStatistics);
            return msi;

        }

        public static void InsertInsufficentLicenses(MonitoredItems.MicrosoftServersCollection servers)
        {
            if (servers.Count > 0)
            {
                CheckForInsufficentLicenses(servers, servers.get_Item(0).ServerType, servers.get_Item(0).ServerType);
            }
            else
            {
                CheckForInsufficentLicenses(servers, servers.GetType().ToString(), servers.GetType().ToString());
            }

        }

        public static void InitStatusTable(MonitoredItems.MicrosoftServersCollection collection)
        {
           // return;
            try
            {
                TestResults testsList = new TestResults();
                String type = "";
                CommonDB db = new CommonDB();
                if (collection.Count > 0)
                    type = collection.get_Item(0).ServerType;

                if (type != "")
                {
                    VSNext.Mongo.Repository.Repository<Status> repo = new VSNext.Mongo.Repository.Repository<Status>(db.GetMongoConnectionString());
                    List<Status> list = repo.Find(i => i.DeviceType == type).ToList();

                    MongoStatementsInsert<Status> insertStatement = new MongoStatementsInsert<Status>();


                    foreach (MonitoredItems.MicrosoftServer server in collection)
                    {
                        String sql = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + server.Name + "-" + type + "') BEGIN " +
                            "INSERT INTO Status ( Type, Location, Category, Name, Status, Details, Description, TypeANDName, StatusCode ) VALUES " +
                            " ('" + type + "', '" + server.Location + "', '" + server.Category + "', '" + server.Name + "', '" + server.Status + "', 'This server has not yet been scanned.', " +
                            "'Microsoft " + type + " Server', '" + server.Name + "-" + type + "', '" + server.StatusCode + "') END";

                       // db.Execute(sql);

                        if (list.Where(i => i.DeviceName == server.Name).Count() == 0)
                        {
                            insertStatement.listOfEntities.Add(new Status()
                            {
                                DeviceType = type,
                                Location = server.Location,
                                Category = server.Category,
                                DeviceName = server.Name,
                                CurrentStatus = server.Status,
                                Details = "This server has not yet been scanned.",
                                TypeAndName = server.TypeANDName,
                                StatusCode = server.StatusCode
                            });

                        }



                    }

                    insertStatement.Execute();

                    Common.WriteDeviceHistoryEntry("All", type, type + " Servers are marked as Not Scanned", Common.LogLevel.Normal);

                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", "Microsoft", "Error in init status.  Error: " + ex.Message, Common.LogLevel.Normal);
            }

        }

        public static void CommonDailyTasks(MonitoredItems.MicrosoftServer myServer, ref TestResults AllTestsList, string ServerType)
        {

            CultureInfo culture = CultureInfo.CurrentCulture;

            try
            {
                CommonDB db = new CommonDB();
                VSNext.Mongo.Repository.Repository<VSNext.Mongo.Entities.Mailbox> repo = new VSNext.Mongo.Repository.Repository<Mailbox>(db.GetMongoConnectionString());
                FilterDefinition<VSNext.Mongo.Entities.Mailbox> filterDef = repo.Filter.Eq(x => x.DeviceId, myServer.ServerObjectID);
                List<VSNext.Mongo.Entities.Mailbox> listOfMailboxes = repo.Find(filterDef).ToList();

               // //string SqlQuery = "select DisplayName , ItemCount, TotalItemSizeInMB from ExchangeMailFiles Where Server='" + myServer.Name + "'";
               // CommonDB DB = new CommonDB("VSS_STATISTICS");
                //DataTable mailfiles = DB.GetData(SqlQuery.ToString());
                //mailfiles = DB.GetData(SqlQuery.ToString());
                Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSummaryStats: Count of Mailboxes." + listOfMailboxes.Count.ToString(), Common.LogLevel.Normal);
                if (listOfMailboxes.Count > 0)
                {
                    for (int i = 0; i < listOfMailboxes.Count; i++)
                    {
                        VSNext.Mongo.Entities.Mailbox mailbox = listOfMailboxes[i];
                        string DisplayName = mailbox.DisplayName;
                        string ItemCount = mailbox.ItemCount.HasValue ? mailbox.ItemCount.Value.ToString() : "0";
                        string TotalItemSizeInMB = mailbox.TotalItemSizeMb.HasValue ? mailbox.TotalItemSizeMb.Value.ToString() : "0";

                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoSummaryStats(myServer, "Mailbox." + DisplayName + ".TotalItems.SizeMb", TotalItemSizeInMB));
                        AllTestsList.MongoEntity.Add(Common.GetInsertIntoSummaryStats(myServer, "Mailbox." + DisplayName + ".TotalItems.Count", ItemCount));
                    }
                }
                //SqlQuery = "select COUNT(DisplayName) NoOfMailBoxes,SUM(itemcount) TotalItemCount,SUM(totalitemsizeinmb) TotalItemSizeInMB,round(AVG(totalitemsizeinmb),2) AvgSizeOfMailBox,round(AVG(itemcount),2) AvgCountOfItems from ExchangeMailFiles Where Server='" + myServer.Name + "'";
                //DataTable dtServers = DB.GetData(SqlQuery.ToString());
               // dtServers = DB.GetData(SqlQuery.ToString());
                //Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "doSummaryStats: Count of Mailboxes." + dtServers.Rows.Count.ToString(), Common.LogLevel.Normal);

                if (listOfMailboxes.Count > 0)
                {

                    string NoOfMailBoxes = listOfMailboxes.Count.ToString();// DR["NoOfMailBoxes"].ToString();
                    string TotalItemCount = listOfMailboxes.Where(x => x.ItemCount.HasValue).Sum(x => x.ItemCount).ToString(); // DR["TotalItemCount"].ToString();
                    string TotalItemSizeInMB = listOfMailboxes.Where(x => x.TotalItemSizeMb.HasValue).Sum(x => x.TotalItemSizeMb).ToString();// DR["TotalItemSizeInMB"].ToString();
                    string AvgSizeOfMailBox = Math.Round(listOfMailboxes.Where(x => x.TotalItemSizeMb.HasValue).Average(x => x.TotalItemSizeMb.Value), 2).ToString();// DR["AvgSizeOfMailBox"].ToString();
                    string AvgCountOfItems = Math.Round(listOfMailboxes.Where(x => x.ItemCount.HasValue).Average(x => x.ItemCount.Value), 2).ToString();//DR["AvgCountOfItems"].ToString();

                    AllTestsList.MongoEntity.Add(GetInsertIntoSummaryStats(myServer, "NoOfMailBoxes", NoOfMailBoxes));
                    AllTestsList.MongoEntity.Add(GetInsertIntoSummaryStats(myServer, "SizeOfMailBoxes", TotalItemCount));
                    AllTestsList.MongoEntity.Add(GetInsertIntoSummaryStats(myServer, "TotalNoOfItems", TotalItemSizeInMB));
                    AllTestsList.MongoEntity.Add(GetInsertIntoSummaryStats(myServer, "AvgSizeOfMailBoxes", AvgSizeOfMailBox));
                    AllTestsList.MongoEntity.Add(GetInsertIntoSummaryStats(myServer, "AvgCountOfItems", AvgCountOfItems));
                }


            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry("All", "Exchange", "Error getting in CommonDailyTasks .");
            }
        }


        public static int getThreadCount(string ServerType)
        {
            CommonDB db = new CommonDB();
            int numOfThreads = 35;

            try
            {
                RegistryHandler registry = new RegistryHandler();
                numOfThreads = Convert.ToInt32(registry.ReadFromRegistry("ThreadLimit" + ServerType));

            }
            catch (Exception ex)
            {
            }

            return numOfThreads;
        }

        public static void SetHourlyAlertsToObject(TestResults AllTestResults, MonitoredItems.MicrosoftServersCollection coll)
        {



            foreach (MonitoredItems.MicrosoftServer Server in coll)
            {
                List<MonitoredItems.MicrosoftServer.HourlyAlert> list = new List<MonitoredItems.MicrosoftServer.HourlyAlert>();

                foreach (Alerting alert in AllTestResults.AlertDetails.Where(s => s.DeviceName == Server.Name))
                {
                    list.Add(new MonitoredItems.MicrosoftServer.HourlyAlert() { AlertType = alert.AlertType.ToString(), AlertRaised = alert.ResetAlertQueue == commonEnums.ResetAlert.Yes ? false : true, AlertDetails = alert.Details });
                }

                if (AllTestResults.AlertDetails.Where(s => s.DeviceName == Server.Name).Count() > 0)
                    Server.HourlyAlerts = list;
            }



        }

        public static void CheckForPowerShellConnectionErrors(ReturnPowerShellObjects PSObj, PSDataCollection<ErrorRecord> errorStream)
        {

            foreach (ErrorRecord error in errorStream)
            {

                if (error.Exception.ToString().Contains("The maximum number of concurrent shells allowed for this plugin has been exceeded"))
                {
                    PSObj.ErrorMessage = "The maximum number of shells for PowerShell has been exceeded so cannot connect to the server at " + DateTime.Now.ToString("t") + ".";
                    PSObj.PreferedStatus = "Issue";
                }

            }

        }

        public static string GetMongoBsonEntityElementName(string entityName, string attributeName)
        {
            //Call like GetMongoBsonEntityElementName("Status", "FileWitnessServerName").  This will return file_witness_server_name.  
            //You pass it the C# Object class and attribute and it returned the bson equivelent

            System.ComponentModel.PropertyDescriptorCollection property_descriptor = System.ComponentModel.TypeDescriptor.GetProperties(Type.GetType("VSNext.Mongo.Entities." + entityName + ", VSNext.Mongo.Entities"));
            System.ComponentModel.AttributeCollection attributes = property_descriptor[attributeName].Attributes;
            MongoDB.Bson.Serialization.Attributes.BsonElementAttribute element = attributes[Type.GetType("MongoDB.Bson.Serialization.Attributes.BsonElementAttribute, MongoDB.Bson")] as MongoDB.Bson.Serialization.Attributes.BsonElementAttribute;
            return element.ElementName;
        }

        public static void ServerInMaintenance(MonitoredItems.MicrosoftServer myServer)
        {
            CommonDB db = new CommonDB();
            /*
            SQLBuild objSQL = new SQLBuild();
            objSQL.ifExistsSQLSelect = "SELECT * FROM Status WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";
            objSQL.onFalseDML = "INSERT INTO STATUS (NAME, STATUS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, SecondaryRole,ResponseThreshold, " +
                        "DominoVersion, OperatingSystem, NextScan, Details, CPU, Memory) VALUES ('" + myServer.Name + "', 'Maintenance', 'Maintenance', '" + DateTime.Now.ToString() + "','" + ServerType + "','" +
                        myServer.Location + "','" + myServer.Category + "','" + myServer.Name + "-" + ServerType + "', 'Microsoft " + ServerType + " Server', 0, 0, '', " +
                        "'" + myServer.ResponseThreshold + "', '" + serverType + "', '" + myServer.OperatingSystem + "', '" + myServer.NextScan + "', " +
                        "'This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.', 0, 0 )";

            objSQL.onTrueDML = "UPDATE Status set Status='Maintenance', StatusCode='Maintenance', LastUpdate='" + DateTime.Now + "', Details='This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.'," +
                " UserCount=0, CPU=0, Memory=0 WHERE TypeANDName='" + myServer.Name + "-" + ServerType + "'";

            string sqlQuery = objSQL.GetSQL(objSQL);
            db.Execute(sqlQuery);
            */

            MongoStatementsUpsert<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Status>();
            mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == myServer.TypeANDName);
            mongoStatement.updateDef = mongoStatement.repo.Updater
                .Set(i => i.DeviceName, myServer.Name)
                .Set(i => i.CurrentStatus, "Maintenance")
                .Set(i => i.StatusCode, "Maintenance")
                .Set(i => i.LastUpdated, DateTime.Now)
                .Set(i => i.NextScan, myServer.NextScan)
                .Set(i => i.DeviceType, myServer.ServerType)
                .Set(i => i.Location, myServer.Location)
                .Set(i => i.Category, myServer.Category)
                .Set(i => i.TypeAndName, myServer.TypeANDName)
                .Set(i => i.Description, "Microsoft")
                .Set(i => i.UserCount, 0)
                .Set(i => i.ResponseTime, 0)
                .Set(i => i.ResponseThreshold, int.Parse(myServer.ResponseThreshold.ToString()))
                .Set(i => i.SoftwareVersion, myServer.VersionNo)
                .Set(i => i.OperatingSystem, myServer.OperatingSystem)
                .Set(i => i.Details, "This server is in a scheduled maintenance period.  Monitoring is temporarily disabled.")
                .Set(i => i.CPU, 0)
                .Set(i => i.Memory, 0);

            mongoStatement.Execute();
        }

        


        #region HelperClasses
        //This region is responsible for making calls to the VitalSignsMicrosoft functions

        public static dynamic objFromParent;

        public static void initHelperClasses(dynamic obj)
        {
            objFromParent = obj;
        }


        public static void CheckForInsufficentLicenses(Object objServers, string ServerType, string ServerTypeForTypeAndName)
        {
            objFromParent.CheckForInsufficentLicenses(objServers, ServerType, ServerTypeForTypeAndName);
        }

        public static Boolean UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType ServerType, string NodeName)
        {
            return objFromParent.UpdateServiceCollection(ServerType, NodeName);
        }

        public static MonitoredItems.MonitoredDevice SelectServerToMonitor(MonitoredItems.MonitoredDevicesCollection collection)
        {
            return objFromParent.SelectServerToMonitor(collection);
        }


        #endregion

    }
    
    public class ActiveSyncDevice
    {
        public string DeviceID;  //SEC3F41705A7F5BC
        public string DeviceUserAgent;  // SAMSUNG-SPH-L710/101.403
        public string User;  //Dhanraj Seri
        public string DeviceFriendlyName;
        public string PrimarySMTPAddress;
        public string DevicePolicyApplied;
        public string Status;  //DeviceOk
        public string DeviceModel;  //SPH-L710
        public string DeviceOS;  //Android
        public string DeviceOSMin;
        public string DeviceType;  //iPhone
        public string DeviceOSLanguage; //English
        public string DeviceMobileOperator;  //Sprint
        public string IsRemoteWipeSupported;  //True
        public string DeviceAccessState; //Allowed
        public string DeviceActiveSyncVersion;
        public int NumberOfFoldersSynced;
        public string LastSuccessSync;
        public string Identity; //jnittech.com/Users/Dhanraj Seri/ExchangeActiveSyncDevices/SAMSUNGSPHL710§SEC3F41705A7F5BC
    }

    public class TestResults
    {
        public List<TestList> StatusDetails = new List<TestList>();
        public List<Alerting> AlertDetails = new List<Alerting>();
        public List<SQLstatements> SQLStatements = new List<SQLstatements>();
        public List<MongoStatements> MongoEntity = new List<MongoStatements>();

    }

    public class TestList
    {
        // Need to add additional attributes, such as category (enum of roles), Details, Result enum Pass Fail
        public commonEnums.ServerRoles Category;
        public string TestName;
        public commonEnums.ServerResult Result;
        public string Details;
    }

    public class SQLstatements
    {
        public string DatabaseName = "VitalSigns";
        public string SQL;
    }
    public class Alerting
    {
        public commonEnums.AlertDevice DeviceType;
        public string DeviceName;
        public commonEnums.AlertType AlertType = commonEnums.AlertType.None;
        public string AlertTypeString = string.Empty;
        public string Location;
        public string Details;
        public commonEnums.ResetAlert ResetAlertQueue;
        public string Category;

    }

    public class MongoStatements
    {

        public virtual bool Execute() { return false; }
        public override string ToString() { return "NOT IMPLEMENTED"; }


    }

    public class MongoStatementsWrapper<T> : MongoStatements where T : IEntity
    {
        CommonDB db = new CommonDB();
        string connString;

        public VSNext.Mongo.Repository.Repository<T> repo;// = new Repository<T>();

        public MongoStatementsWrapper()
        {
            CommonDB db = new CommonDB();
            connString = db.GetMongoConnectionString();
            repo = new VSNext.Mongo.Repository.Repository<T>(connString);

        }

        public virtual BsonDocument RenderToBsonDocument<T>(FilterDefinition<T> filter)
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();
            return filter.Render(documentSerializer, serializerRegistry);
        }

        public BsonDocument RenderToBsonDocument<T>(UpdateDefinition<T> filter)
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();
            return filter.Render(documentSerializer, serializerRegistry);
        }

        //            public abstract bool Execute();

    }

    public class MongoStatementsInsert<T> : MongoStatementsWrapper<T> where T : IEntity
    {
        public List<T> listOfEntities = new List<T>();

        public override bool Execute()
        {
            try
            {
                repo.Insert(listOfEntities);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public override string ToString()
        {
            return "Insert. Documents: " + String.Join("\n", listOfEntities.Select(x => x.ToBsonDocument().ToString()));
        }
    }

    public class MongoStatementsDelete<T> : MongoStatementsWrapper<T> where T : IEntity
    {

        public FilterDefinition<T> filterDef { get; set; }

        public override bool Execute()
        {
            try
            {
                
                repo.Delete(filterDef);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public override string ToString()
        {
            return "Delete. FilterDef " + base.RenderToBsonDocument(filterDef);
        }

    }

    public class MongoStatementsUpdate<T> : MongoStatementsWrapper<T> where T : IEntity
    {
        public FilterDefinition<T> filterDef;
        public UpdateDefinition<T> updateDef;

        /// <summary>
        /// Set to true if updating values inside an embedded document
        /// </summary>
        public bool embeddedDocument = false;

        public override bool Execute()
        {
            try
            {
                if (embeddedDocument)
                {
                    while (repo.Update(filterDef, updateDef))
                    { }
                    return true;
                }
                return repo.Update(filterDef, updateDef);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public override string ToString()
        {
            return "Update. FilterDef " + base.RenderToBsonDocument(filterDef) + "\n. UpdateDef " + base.RenderToBsonDocument(updateDef) + "\n . Embeded Doc " + embeddedDocument.ToString();
        }
    }

    public class MongoStatementsReplace<T> : MongoStatementsWrapper<T> where T : IEntity
    {
        public FilterDefinition<T> filterDef;
        public UpdateDefinition<T> updateDef;

        public override bool Execute()
        {
            try
            {
                //return repo.Replace(filterDef, T);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public override string ToString()
        {
            return "NOT IMPLEMENTED";
        }
    }

    public class MongoStatementsUpsert<T> : MongoStatementsWrapper<T> where T : IEntity
    {
        public FilterDefinition<T> filterDef;
        public UpdateDefinition<T> updateDef;

        public override bool Execute()
        {
            try
            {
                repo.Upsert(filterDef, updateDef);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public override string ToString()
        {
            return "Upsert. FilterDef " + base.RenderToBsonDocument(filterDef) + "\n. UpdateDef " + base.RenderToBsonDocument(updateDef);
        }
    }

    public class MongoStatementsArrayElements<T> : MongoStatementsWrapper<T> where T : IEntity
    {
        //This will mostly be used for arrays
        public FilterDefinition<T> searchFilterDef;

        public FilterDefinition<T> updateFilterDef;
        public UpdateDefinition<T> updateUpdaterDef;

        public UpdateDefinition<T> insertUpdaterDef;
        public FilterDefinition<T> insertFilterDef;

        public override bool Execute()
        {
            try
            {
                if (repo.Find(searchFilterDef).Count() > 0)
                {

                    repo.Update(updateFilterDef, updateUpdaterDef);
                }
                else
                {
                    repo.Update(insertFilterDef, insertUpdaterDef);
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override string ToString()
        {
            if (repo.Find(searchFilterDef).Count() > 0)
            {
                //return "Update. FilterDef " + base.RenderToBsonDocument(filterDef) + "\n. UpdateDef " + base.RenderToBsonDocument(updateDef) + "\n . Embeded Doc " + embeddedDocument.ToString();
            }
            else
            {
                //return "Update. FilterDef " + base.RenderToBsonDocument(filterDef) + "\n. UpdateDef " + base.RenderToBsonDocument(updateDef) + "\n . Embeded Doc " + embeddedDocument.ToString();
            }
            return "NOT IMPLEEMNTED";
            
        }

    }



    



    public class ReturnPowerShellObjects : IDisposable
    {
        public string[] ExchangeServersCollection;
        public PowerShell PS;
        //public Runspace runspace;
        //public PSSession Session;
        public String ErrorMessage = "";
        private Boolean disposed;
        public String ServerType = "";
        public Boolean Connected = false;
        public String PreferedStatus = "";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ReturnPowerShellObjects()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                try
                {
                    //if (!(this.Session == null && this.runspace == null && this.PS == null))
                    if (!(this.PS == null))
                    {


                        //Removes any and all modules improted
                        try
                        {
                            //String script = "Get-Module tmp* | Remove-Module";
                            String script = @"
$ModPath = ''
if((Get-Module | ?{ $_.Name -like 'tmp*' }).Count -gt 0)
{
    $ModPath = (Get-Item ((Get-Module | ?{ $_.Name -like 'tmp*' } ).Path)).Directory.FullName
    $ModPath
}
Get-PSSession | Remove-PSSession
Get-Module | ? { $_.Name -like 'tmp*' } | Remove-Module
if ($ModPath -ne '')
{
    Remove-Item $ModPath
}

"
                        ;

                            this.PS.Commands.Clear();
                            this.PS.Streams.ClearStreams();
                            this.PS.AddScript(script);
                            Collection<PSObject> results = this.PS.Invoke();
                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing all imported modules and Sessions. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                        }

                        string PID = "";
                        //Stops the process from running
                        try
                        {
                            PSCommand command = new PSCommand();
                            command.AddScript("$PID");
                            this.PS.Commands = command; ;
                            Collection<PSObject> results = this.PS.Invoke();
                            PID = results[0].BaseObject.ToString();
                            Common.WriteDeviceHistoryEntry("All", "Microsoft", "Trying to dispose PID " + PID.ToString(), Common.LogLevel.Normal);
                        }

                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing all imported modules. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                        }

                        //Stops the process from running
                        try
                        {
                            String script = "Stop-Process $PID";
                            this.PS.Commands.Clear();
                            this.PS.AddScript(script);
                            this.PS.Invoke();
                        }
                        catch (PSRemotingTransportException ex)
                        {
                            //This error is expected.  The above cmd will stop the process from running due to large code rework otherwise
                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception stopping process. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                        }


                        //closes the runspaces.  Might be repeats in there but better to be safe
                        try
                        {
                            //if (this.Session.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                            // this.Session.Runspace.Close();
                            if (this.PS.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                                this.PS.Runspace.Close();
                        }
                        catch (Exception ex)
                        {
                            Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception removing PSSession with ID. Exception: " + ex.Message.ToString(), Common.LogLevel.Normal);
                        }


                        //disposes of other resources and nulls them

                        this.PS.Dispose();
                        this.PS.Commands.Clear();
                        this.PS.Streams.ClearStreams();

                        this.ErrorMessage = null;
                        this.ExchangeServersCollection = null;
                        this.PS = null;
                        //this.Session = null;
                    }

                }
                catch (Exception ex)
                {
                    Common.WriteDeviceHistoryEntry("All", "Microsoft", "Exception cleaning up ReturnPowerShellObjects. Exception: " + ex.Message.ToString());
                }
            }


            disposed = true;
        }
    }

    public class commonEnums
    {
        public enum ServerRoles
        {
            CAS = 0,
            HUB = 1,
            MailBox = 2,
            Edge = 3,
            Windows = 4,
            MailFlow = 5,
            Services = 6,
            Lync = 7,
            Empty = 8,
            DAG = 9,
            DailyService = 10,
            SharePoint = 11

        }
        public enum ServerResult
        {
            Pass = 0,
            Fail = 1
        }
        public enum ResetAlert
        {
            Yes = 1,
            No = 0
        }
        public enum AlertType
        {
            None,

            // "_" becomes a " " in output
            SMTP,
            POP3,
            IMAP,
            RPC,
            Active_Sync,
            Active_Sync_Devices,
            Auto_Discovery,
            Memory,
            CPU,
            Disk_Space,
            Submission,
            Poison,
            Unreachable,
            Shadow,
            Reboot_Overdue,
            Not_Responding,
            Services,
            Database_Corruption,

            //  AlertType = AlertType + ":" +Services,
            OWA_Login,
            Health_Check,
            Response_Time,

            //DAG
            DAG_Member_Health,
            DAG_Database_Health,
            DAG_Activation_Preference,
            DAG_Replay_Queue,
            DAG_Copy_Queue,

            //MB
            Mailbox_Database_Size,
            Mailbox_Send_Prohibited,
            Mailbox_Receive_Prohibited,
            Mailbox_Replication_Service_Test,
            MailBox_Assistants_Service_Test,

            //AD
            Availability,
            Query_Latency,
            Logon_Test,
            Port_Test,
            Replication_Summary,
            Advertising_Test,
            FRS_System_Volume_Test,
            Replications_Test,
            Services_Test,
            DNS_Test,
            FSMO_Check_Test,
            DNS_Authentication_Test,
            DNS_Basic_Test,
            DNS_Fowarders_Test,
            DNS_Delegation_Test,
            DNS_Dynamic_Update_Test,
            DNS_Record_Registration_Test,
            DNS_External_Name_Test,


            //SP
            Web_Count,
            Database_Site_Warning_Count,
            Database_Site_Max_Count,
            Site_Health_Check,
            Network_Latency_Test,
            Search_Crawl,

            //Exch_Mailflow
            Mail_flow,
            Compose_Email,
            OWA,
            Create_Calendar_Entry,
            Delete_Calendar_Entry,
            OneDrive_Upload_Document,
            OneDrive_Download_Document,
            Create_Site,
            Delete_Site,
            Inbox,
            Create_Mail_Folder,
            Delete_Mail_Folder,
            ADFS,
            DirSync_Import,
            DirSync_Export,
            URL,
            Authentication,
            MAPI_Connectivity

        }


        public enum AlertDevice
        {
            Exchange,
            Skype_for_Business,
            SharePoint,
            Active_Directory,
            Database_Availability_Group,
            Exchange_Mail_Flow,
            Office365,
            Windows
        }
    }

    public class SQLBuild
    {
        public string ifExistsSQLSelect;
        public string onTrueDML;
        public string onFalseDML;
        public string GetSQL(SQLBuild objSQL)
        {

            string strSQL = "";
            if (onTrueDML == null && onFalseDML == null)
            {
                strSQL = "";
            }
            else if (onTrueDML != null && onFalseDML == null)
            {
                strSQL = "IF  EXISTS (" + objSQL.ifExistsSQLSelect + ") " +
                " BEGIN " +
                onTrueDML +
                " END ";
            }
            else if (onTrueDML != null && onFalseDML != null)
            {
                strSQL = "IF  EXISTS (" + objSQL.ifExistsSQLSelect + ") " +
                " BEGIN " +
                onTrueDML +
                " END " +
                " ELSE " +
                " BEGIN " +
                onFalseDML +
                " END ";
            }
            return strSQL;
        }

    }


    public class MonitorTables
    {
        static ActiveDirectoryMAIN adMain;
        static ExchangeMAIN exMain;
        static SharepointMAIN spMain;
        static WindowsMAIN wnMain;
        static Office365MAIN ofMain;
        public MonitorTables(ref ActiveDirectoryMAIN _adMain, ref ExchangeMAIN _exMain, ref SharepointMAIN _spMain, ref WindowsMAIN _wnMain, ref Office365MAIN _ofMain)
        {
            adMain = _adMain;
            exMain = _exMain;
            spMain = _spMain;
            wnMain = _wnMain;
            ofMain = _ofMain;
        }

        public void CheckForTableChanges()
        {
            string NodeName = "";
            while (true)
            {

                if (NodeName == "")
                {
                    try
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["VSNodeName"] != null)
                            NodeName = System.Configuration.ConfigurationManager.AppSettings["VSNodeName"].ToString();

                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry("All", "Exchange", "Error getting the node name.  Will not update services until node name is received.");
                    }
                }
                if (NodeName != "")
                {
                    try
                    {
                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.ActiveDirectory, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing AD Collection on demand");
                            if(adMain != null)
                            adMain.RefreshActiveDirectoryCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.DatabaseAvailabilityGroup, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing DAG Collection on demand");
                            if (exMain != null)
                                exMain.RefreshDAGCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Exchange, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing Exchange Collection on demand");
                            if (exMain != null)
                                exMain.RefreshExchangeCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.SkypeForBusiness, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing Skype for Business Collection on demand");
                            if (exMain != null)
                                exMain.RefreshLyncCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.SharePoint, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing SP Collection on demand");
                            if (spMain != null)
                                spMain.RefreshSharePointCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Windows, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing Windows Collection on demand");
                            if (wnMain != null)
                                wnMain.RefreshWindowsCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.NetworkLatency, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing Network Latency Collection on demand");
                            if (wnMain != null)
                                wnMain.RefreshLatencyCollection();
                        }

                        if (Common.UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Office365, NodeName))
                        {
                            Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing O365 Collection on demand");
                            if (ofMain != null)
                                ofMain.RefreshOffice365Collction();
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.WriteDeviceHistoryEntry("All", "Exchange", "Error refreshing collections on demand.  Error: " + ex.Message);
                    }

                }
                else
                {
                    Common.WriteDeviceHistoryEntry("All", "Exchange", "No Node Name.  Will not do automatic updates for the time being");
                }

                Thread.Sleep(5000);
            }

        }

        //public void CheckForTableChanges()
        //{
        //    CommonDB db = new CommonDB();
        //    System.Data.DataTable dt = null;
        //    // Refresh Collection of Devices and Monitor settings
        //    while (true)
        //    {
        //        try
        //        {
        //            //Sets the log level
        //            Common.setLogLevel();

        //            //All
        //            if (db.GetData("select svalue from settings where sname='ConfigurationsChangedServerAttributes'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedServerAttributes'").Rows[0][0].ToString() == "1")
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Microsoft", "Refreshing configuration of all exchange related Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Exchange'").Length > 0)
        //                    {
        //                        exMain.RefreshExchangeCollection();
        //                        exMain.RefreshLyncCollection();
        //                        exMain.RefreshExchangeMailFlowCollection();
        //                        exMain.RefreshDAGCollection();
        //                    }
        //                    if (dt.Select("Name='SharePoint'").Length > 0)
        //                        spMain.RefreshSharePointCollection();
        //                    if (dt.Select("Name='Active Directory'").Length > 0)
        //                        adMain.RefreshActiveDirectoryCollection();
        //                    if (dt.Select("Name='Windows'").Length > 0)
        //                        wnMain.RefreshWindowsCollection();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Microsoft", "Error refreshing configuration of  all exchange related Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedServerAttributes'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Microsoft", "Error resetting ServerAttributes registry values.  Error: " + ex.Message);
        //                }

        //            }


        //            //Exchange
        //            if (db.GetData("select svalue from settings where sname='ConfigurationsChangedExchangeSettings'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedExchangeSettings'").Rows[0][0].ToString() == "1")
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing configuration of Exchange Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Exchange'").Length > 0)
        //                        exMain.RefreshExchangeCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error refreshing configuration of Exchange Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedExchangeSettings'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error resetting Exchange Server registry values.  Error: " + ex.Message);
        //                }
        //            }


        //            //Lync
        //            if (db.GetData("select svalue from settings where sname='ConfigurationsChangedLyncServers'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedLyncServers'").Rows[0][0].ToString() == "1")
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Lync", "Refreshing configuration of Lync Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Exchange'").Length > 0)
        //                        exMain.RefreshLyncCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Lync", "Error refreshing configuration of Lync Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedLyncServers'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Lync", "Error resetting Lync Server registry values.  Error: " + ex.Message);
        //                }
        //            }

        //            //DAG
        //            if (db.GetData("select svalue from settings where sname='ConfigurationsChangedDagSettings'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedDagSettings'").Rows[0][0].ToString() == "1")
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing configuration of Dag Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Exchange'").Length > 0)
        //                        exMain.RefreshDAGCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error refreshing configuration of Dag Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedDagSettings'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error resetting Dag Server registry values.  Error: " + ex.Message);
        //                }
        //            }

        //            //MailFlow
        //            if (db.GetData("select svalue from settings where sname='ConfigurationsChangedExchangeMailProbe'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedExchangeMailProbe'").Rows[0][0].ToString() == "1")
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Refreshing configuration of MailFlow Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Exchange'").Length > 0)
        //                        exMain.RefreshExchangeMailFlowCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error refreshing configuration of MailFlow Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedExchangeMailProbe'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Exchange", "Error resetting MailFlow Server registry values.  Error: " + ex.Message);
        //                }
        //            }

        //            //SharePoint
        //            if ((db.GetData("select svalue from settings where sname='ConfigurationsChangedSharePointSettings'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedSharePointSettings'").Rows[0][0].ToString() == "1"))
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "SharePoint", "Refreshing configuration of Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='SharePoint'").Length > 0)
        //                        spMain.RefreshSharePointCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "SharePoint", "Error refreshing configuration of Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedSharePointSettings'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "SharePoint", "Error resetting Server registry values.  Error: " + ex.Message);
        //                }
        //            }

        //            //Active Directory
        //            if ((db.GetData("select svalue from settings where sname='ConfigurationsChangedActiveDirectorySettings'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedActiveDirectorySettings'").Rows[0][0].ToString() == "1"))
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Active Directory", "Refreshing configuration of Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    if (dt.Select("Name='Active Directory'").Length > 0)
        //                        adMain.RefreshActiveDirectoryCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Active Directory", "Error refreshing configuration of Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedActiveDirectorySettings'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "Active Directory", "Error resetting Server registry values.  Error: " + ex.Message);
        //                }
        //            }

        //            //Latency Test
        //            if ((db.GetData("select svalue from settings where sname='ConfigurationsChangedNetworkLatencyServers'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedNetworkLatencyServers'").Rows[0][0].ToString() == "1") || 
        //               (db.GetData("select svalue from settings where sname='ConfigurationsChangedNetworkLatency'").Rows.Count > 0 && db.GetData("select svalue from settings where sname='ConfigurationsChangedNetworkLatency'").Rows[0][0].ToString() == "1"))
        //            {
        //                try
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "LatencyTest", "Refreshing configuration of Servers");
        //                    dt = db.GetData("select Name from selectedfeatures sf left join features f on sf.FeatureID = f.ID");

        //                    wnMain.RefreshLatencyCollection();

        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "LatencyTest", "Error refreshing configuration of Servers.  Error: " + ex.Message);
        //                }

        //                try
        //                {
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedNetworkLatencyServers'");
        //                    db.Execute("update settings set svalue='0' where sname='ConfigurationsChangedNetworkLatency'");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Common.WriteDeviceHistoryEntry("All", "LatencyTest", "Error resetting Server registry values.  Error: " + ex.Message);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Common.WriteDeviceHistoryEntry("All", "Exchange", "Error refreshign collections on demand.  Error: " + ex.Message);
        //        }

        //        Thread.Sleep(5000);
        //    }
        //}

    }
}
