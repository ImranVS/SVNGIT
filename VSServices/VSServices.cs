using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Web;

namespace RPRWyatt.VitalSigns.Services
{
    public class VSServices
    {
        string logPath;
        Dictionary<string, string> fileNameDicnry = new Dictionary<string, string>();
        enum LogLevel
        {
            Verbose = 0,
            Debug = 1,
            Normal = 2,
        }
       
        
        private static LogLevel DefaultLogLevel = LogLevel.Normal;
       // LogLevel DefaultLogLevel = new LogLevel();

        public VSServices()
        {

            VSFramework.VSAdaptor vsobject = new VSFramework.VSAdaptor();
            DataSet DsSettings = new DataSet();

            string Svalue = "";

            string strSQL;
            string LogFilepath = "";
            try
            {
                strSQL = "Select svalue from Settings Where sname=\'Log Files Path\'";
                vsobject.FillDatasetAny("VitalSigns", "None", strSQL, ref DsSettings, "Settings");
                LogFilepath = DsSettings.Tables["Settings"].Rows[0]["svalue"].ToString();
            }
            catch (Exception ex)
            {
                Svalue = "0";
            }
            try
            {
                strSQL = "Select svalue from Settings Where sname='Log Level'";
                vsobject.FillDatasetAny("VitalSigns", "None", strSQL, ref DsSettings, "Settings");
                string level = DsSettings.Tables["Settings"].Rows[0]["svalue"].ToString();
               DefaultLogLevel= (LogLevel)Enum.Parse(typeof(LogLevel), level, true);

            }
            catch (Exception ex)
            {
                DefaultLogLevel = LogLevel.Normal;
            }

            logPath = LogFilepath;
        }

        //public OnStart(){
        //    //ServiceOnStart();
        //}

        //abstract fdsajfl ServiceOnStart()

        public void WriteLogToFiles(ref Dictionary<string, string> fileNameDicnry1)
        {
            Dictionary<string, string> fileNameDicnry = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> pair1 in fileNameDicnry1)
            {
                fileNameDicnry.Add(pair1.Key, fileNameDicnry1[pair1.Key]);
            }

            foreach (KeyValuePair<string, string> pair in fileNameDicnry)
            {
                StreamWriter sw = null;
                string strAppPath;

                try
                {
                    strAppPath = (logPath + "\\" + pair.Key);
                    sw = new StreamWriter(strAppPath, true, System.Text.Encoding.Unicode);
                    sw.Close();
                }
                catch
                {
                    strAppPath = (logPath + "\\" + pair.Key);

                }

                try
                {
                    if (File.Exists(strAppPath))
                    {
                        sw = new StreamWriter(strAppPath, true, System.Text.Encoding.Unicode);
                    }
                    else
                    {
                        sw = new StreamWriter(strAppPath, false, System.Text.Encoding.Unicode);

                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    if ((pair.Key != ""))
                    {
                        if (fileNameDicnry1.ContainsKey(pair.Key))
                        {
                            sw.Write(fileNameDicnry1[pair.Key]);
                            fileNameDicnry1[pair.Key] = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }

            }
        }

        public void WriteLogEntries(string fileName, string logText, ref Dictionary<string, string> fileNameDicnry)
        {
            //StreamWriter sa = new StreamWriter("");

            if (fileNameDicnry.ContainsKey(fileName))
            {
                fileNameDicnry[fileName] = fileNameDicnry[fileName] + logText + Environment.NewLine;
            }
            else
            {
                fileNameDicnry.Add(fileName, logText + Environment.NewLine);
            }
            

        }

        protected void WriteAuditEntries()
        {
            VSFramework.VSAdaptor vsobject = new VSFramework.VSAdaptor();
            DataSet DsSettings = new DataSet();
            string Svalue = "";
            Int32 threadsleep = 0;
            string strSQL;
            //  Dim LogFilepath As String = ""
            try
            {
                strSQL = "Select svalue from Settings Where sname=\'ThreadSleep\'";
                vsobject.FillDatasetAny("VitalSigns", "None", strSQL, ref  DsSettings, "Settings");
                threadsleep = Convert.ToInt32(DsSettings.Tables["Settings"].Rows[0]["svalue"].ToString());

            }
            catch (Exception ex)
            {
                Svalue = "0";
            }
           
            while (true)
            {
                WriteLogToFiles(ref fileNameDicnry);
                Thread.Sleep((threadsleep * 1000));
            }
          
        }
        
        public void WriteAuditEntry(string strMsg, string fileName)
        {
            // strAuditText += strMsg & vbCrLf
            string filename = fileName;
            WriteLogEntries(filename, strMsg, ref fileNameDicnry);
        }

        private void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg)
        {
            //string DeviceLogDestination;
            //bool appendMode = true;
            if ((DeviceName.IndexOf("/") + 1) > 0)
            {
                DeviceName = DeviceName.Replace("/", "_");
            }
            string filename = (DeviceType + ("_" + (DeviceName + "_Log.txt")));
            WriteLogEntries(filename, strMsg, ref fileNameDicnry);
        }

        //public void WriteAuditEntry(string strMsg)//(alertdll.vb)
        //{
        //    // strAuditText += strMsg & vbCrLf
        //    string filename = (DeviceType + ("_" + (DeviceName + "_Log.txt")));
        //    //string filename = "Master_Service_Log.txt";
        //    WriteLogEntries(filename, strMsg, ref fileNameDicnry);
        //}
        
        private void WriteHistoryEntry(string strMsg, string fileName, LogLevel logLevelInput = LogLevel.Verbose)
        {
           
            // Warning!!! Optional parameters not supported
            if ((logLevelInput < DefaultLogLevel))
            {
                return;
            }
            string filename = fileName;
            WriteLogEntries(filename, strMsg, ref fileNameDicnry);
        }

        private void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, LogLevel LogLevelInput = LogLevel.Normal)
        {
            //string DeviceLogDestination;
            // Warning!!! Optional parameters not supported
            bool appendMode = true;
            if ((LogLevelInput < DefaultLogLevel))
            {
                return;
            }
            if (((DeviceName.IndexOf("/") + 1) > 0))
            {
                DeviceName = DeviceName.Replace("/", "_");
            }
            string filename = (DeviceType + ("_"+ (DeviceName + "_Log.txt")));
            WriteLogEntries(filename, strMsg, ref fileNameDicnry);
        }
    }
}
