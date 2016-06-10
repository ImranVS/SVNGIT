using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Web;
using System.Configuration;

using System.Data.SqlClient;

namespace LogUtilities
{
	public class LogUtils
	{

		static String logPath;
		static int maxFileSize;
		static int maxLogRotations;
		static Dictionary<string, string> fileNameDicnry = new Dictionary<string, string>();

		public enum LogLevel
        {
            Verbose = 0,
            Debug = 1,
            Normal = 2,
        }
        
		public LogUtils() 
		{
			DataSet DsSettings = new DataSet();

			string Svalue = "";

			string strSQL;
			string LogFilepath = "";
			try
			{
				LogFilepath = GetSettingValue("Log Files Path-New");
			}
			catch (Exception ex)
			{
                LogFilepath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\";
			}
			try
			{
				string level = GetSettingValue("Log Level");
				DefaultLogLevel = (LogUtils.LogLevel)Enum.Parse(typeof(LogUtils.LogLevel), level, true);

			}
			catch (Exception ex) 
			{
				DefaultLogLevel = LogUtils.LogLevel.Normal;
			}

			try
			{
				string level = GetSettingValue("Log Level VSAdapter");
				DefaultVSAdapterLogLevel = (LogUtils.LogLevel)Enum.Parse(typeof(LogUtils.LogLevel), level, true);

			}
			catch (Exception ex)
			{
				DefaultVSAdapterLogLevel = LogUtils.LogLevel.Normal;
			}

			try
			{
				string size = GetSettingValue("Log File Size");
				maxFileSize = Convert.ToInt32(size);

			}
			catch (Exception ex)
			{
				maxFileSize = 10;
			}

			try
			{
				string count = GetSettingValue("Log File Rotation");
				maxLogRotations = Convert.ToInt32(count);

			}
			catch (Exception ex)
			{
				maxLogRotations = 10;
			}



			if (LogFilepath.Trim() == "")
				LogFilepath = "C:\\Program Files (x86)\\VitalSignsPlus\\Log_Files\\";
			if (!LogFilepath.EndsWith("\\"))
				LogFilepath += "\\";

			logPath = LogFilepath;
			WriteAuditEntries(logPath);
		}

        private static LogLevel DefaultLogLevel = LogLevel.Normal;
		private static LogLevel DefaultVSAdapterLogLevel = LogLevel.Normal;

		//Writes the contents of the dictionary to the files
        public static void FlushDataToFiles()
        {
			StreamWriter swTemp = null;

            foreach (KeyValuePair<string, string> pair in fileNameDicnry.ToList())
            {
				if (fileNameDicnry[pair.Key] == "")
					continue;
                StreamWriter sw = null;
                string strAppPath;

				try
				{
					string path = (logPath + pair.Key);
					path = path.Substring(0, path.LastIndexOf("\\"));
					Directory.CreateDirectory(path);
				}
				catch
				{

				}

                try
                {
                    strAppPath = (logPath + pair.Key);
                    sw = new StreamWriter(strAppPath, true, System.Text.Encoding.Unicode);
                    sw.Close();
                }
                catch
                {
                    strAppPath = (logPath + "\\" + pair.Key);

                }

                /*try
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
				*/

                try
                {
					
                    if ((pair.Key != ""))
                    {
						CheckForFileSize(strAppPath, ref sw);
                        if (fileNameDicnry.ContainsKey(pair.Key))
                        {
							sw.Write(fileNameDicnry[pair.Key].ToString());
                            fileNameDicnry[pair.Key] = "";
						}
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
					if (sw != null)
					{
						sw.Flush();
						sw.Close();
					}
                }

            }


			swTemp = null;

        }

		//Constructs and adds to the dictionary
		public static void WriteLogEntries(string fileName, string logText)
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

		//Function that infinitly loops and writes to log
		public static void WriteAuditEntries(string path)
        {
			logPath = path;
			
            DataSet DsSettings = new DataSet();
            string Svalue = "";
            Int32 threadsleep = 0;
            string strSQL;
            //  Dim LogFilepath As String = ""
            try
            {

                threadsleep = Convert.ToInt32(GetSettingValue("ThreadSleep"));

            }
            catch (Exception ex)
            {
				threadsleep = 5;
            }

			//threadsleep = 1;

			Thread thread = new Thread(new ThreadStart(() =>
			{
				while (true)
				{
					try
					{
						FlushDataToFiles();
						Thread.Sleep((threadsleep * 1000));
					}
					catch
					{ }
				}
			}));
			thread.Name = "LogUtils Thread";
			thread.Start();
          
        }
        
		//Formats file names before adding to dictionary
		public static void WriteAuditEntry(string strMsg, string fileName, LogLevel LogLevelInput = LogLevel.Normal)
        {
            if ((LogLevelInput < DefaultLogLevel))
            {
                return;
            }
            // strAuditText += strMsg & vbCrLf
            string filename = fileName;
            WriteLogEntries(filename, strMsg);
        }

		public static void WriteDeviceHistoryEntry(string DeviceType, string DeviceName, string strMsg, LogLevel LogLevelInput = LogLevel.Normal)
        {
            if ((LogLevelInput < DefaultLogLevel))
            {
                return;
            }
            //string DeviceLogDestination;
            //bool appendMode = true;
            if ((DeviceName.IndexOf("/") + 1) > 0)
            {
                DeviceName = DeviceName.Replace("/", "_");
            }
            string filename = (DeviceType + ("_" + (DeviceName + "_Log.txt")));
            WriteLogEntries(filename, strMsg);
        }

		public static void WriteHistoryEntry(string strMsg, string fileName, LogLevel logLevelInput = LogLevel.Verbose)
        {
           
            // Warning!!! Optional parameters not supported
            if ((logLevelInput < DefaultLogLevel))
            {
                return;
            }
            string filename = fileName;
            WriteLogEntries(filename, strMsg);
        }

		public static void WriteAuditEntryForVSAdapter(string strMsg, string fileName, LogLevel LogLevelInput = LogLevel.Normal)
		{
			if ((LogLevelInput < DefaultVSAdapterLogLevel))
			{
				return;
			}
			// strAuditText += strMsg & vbCrLf
			string filename = fileName;
			WriteLogEntries(filename, strMsg);
		}

		public static void WriteEntryForNotResponding(string TypeANDName)
		{
			WriteLogEntries("NotRespondingServers.txt", DateTime.Now.ToString() + "  " + TypeANDName + " is Not Responding");
		}

		private static void CheckForFileSize(string filePath, ref StreamWriter sw)
		{
			//length in bytes.  bytes -> kb -> mb
			if ((new FileInfo(filePath)).Length > (maxFileSize * 1024 * 1024))
			{
				//move the file up the line
				string dateFormat = "yyyyMMddHHmmss";
				string file = filePath.Substring(filePath.LastIndexOf("\\") + 1);
				string folder = filePath.Substring(0, filePath.LastIndexOf("\\"));
				string parentFolder = folder.Substring(0, folder.LastIndexOf("\\"));
				string logFilesRotationFolder = parentFolder + "\\Log_Files_Rotation";
				Directory.CreateDirectory(logFilesRotationFolder);

				string newFilePath = logFilesRotationFolder + "\\" + file.Replace(".txt", "");
				newFilePath += "#" + DateTime.Now.ToString(dateFormat) + ".txt";

				File.Move(filePath, newFilePath);

				List<String> files = Directory.GetFiles(logFilesRotationFolder, file.Replace(".txt", "") + "#*").ToList<String>();

				while (files.Count > maxLogRotations)
				{
					files.Sort();
					File.Delete(files[0].ToString());
					files.Remove(files[0]);
				}


				File.Delete(filePath);
				sw = new StreamWriter(filePath, false, System.Text.Encoding.Unicode);

			}
			else
			{
				sw = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);
			}
		}

		public static double GetLogRotationSizeMB()
		{
			double size = 0;
			try
			{
				string logFilesRotationFolder = (new DirectoryInfo(logPath).Parent.FullName) + "\\Log_Files_Rotation";
				if (!new DirectoryInfo(logFilesRotationFolder).Exists)
					return 0;
				foreach (String file in Directory.GetFiles(logFilesRotationFolder))
				{
					//b -> kb -> mb -> gb
					double fileGB = (new FileInfo(file).Length) / 1024.0 / 1024.0;
					size += fileGB;
				}
				return Math.Round(size, 1);
			}
			catch (Exception ex)
			{
				WriteLogEntries("LogUtilsLog.txt", DateTime.Now.ToString() + "  Exception getting log rotation size. Exception: " + ex.Message);
				return 0;
			}
		}

		public static void DeleteLogRotationFolder()
		{
			Directory.Delete((new DirectoryInfo(logPath).Parent.FullName) + "\\Log_Files_Rotation", true);
		}




		private static String GetSettingValue(string sname)
		{
			String sql = "SELECT Svalue FROM Settings WHERE Sname='" + sname + "'";

			string sqlcon = getSqlConnectionString("VitalSigns");
			SqlConnection con = new SqlConnection(sqlcon);

			DataTable dt = getData(con, sql);

			con.Dispose();

			try
			{
				return dt.Rows[0][0].ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
		}

		private static DataTable getData(SqlConnection con, String sql)
		{
			DataTable dt = new DataTable();
			try
			{
				if (con.State == ConnectionState.Closed) con.Open();
				DataSet ds = new DataSet();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = sql;
				myAdapter.SelectCommand = myCommand;
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myCommand.Dispose();
				con.Close();

			}
			catch (Exception ex)
			{

			}
			finally
			{

			}
			return dt;
		}

		private static string getSqlConnectionString(string DatabaseName)
		{
			String connectionStringName = DatabaseName + "ConnectionString";
			String sConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
			return sConnectionString;
		}

    }

}
