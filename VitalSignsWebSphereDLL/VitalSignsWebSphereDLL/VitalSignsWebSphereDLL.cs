using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

namespace VitalSignsWebSphereDLL
{
	public class VitalSignsWebSphereDLL
	{

		private string ExecuteCommand(string cmd, string AppClientFolder, string ServicePath, int timeoutSec = 60)
		{

			System.Diagnostics.Process process = new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C " + cmd;
			startInfo.WorkingDirectory = ServicePath;
			// *** Redirect the output ***
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			process.StartInfo = startInfo;
			process.Start();

			if (!process.WaitForExit(300 * 1000))
				throw new Exception("Process did not complete in the specified time");

			//for debugging
			string s = process.StandardOutput.ReadToEnd();
			string p = process.StandardError.ReadToEnd();

            LogUtilities.LogUtils.WriteDeviceHistoryEntry("All", "WebSphereDLL", DateTime.Now.ToString() + " Output from console: " + s + "\n\nError from console: " + p);

            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(s, "<cells>.*<\\/cells>");
            return match.Groups[0].Value;
			//throw new Exception(cmd + "......" + AppClientFolder + "...." + ServicePath + "....." +s + "...." + p);
		}

		private object DecodeXMLFromPath(string pathToXML, Type type)
		{
			XmlSerializer serializer = new XmlSerializer(type);

			XmlDocument doc = new XmlDocument();
			doc.Load(pathToXML);

			XmlNodeReader reader = new XmlNodeReader(doc);

			object obj = serializer.Deserialize(reader);

			return obj;
		}

		private object DecodeXMLFromString(string xmlContents, Type type)
		{
			XmlSerializer serializer = new XmlSerializer(type);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlContents);

			XmlNodeReader reader = new XmlNodeReader(doc);

			object obj = serializer.Deserialize(reader);

			return obj;
		}

		private string ExecuteGetServerListCmd(CellProperties cellProperties, string AppClientFolder, string ServicePath)
		{

			//string AppClientFolder = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
			string pathToBatch = "GET_SERVER_LIST.bat";

			string arguments = "";
			arguments += " \"" + cellProperties.HostName + "\"";
			arguments += " \"" + cellProperties.Port + "\"";
			arguments += " \"" + cellProperties.ConnectionType + "\"";
			arguments += " \"" + cellProperties.UserName + "\"";
			arguments += " \"" + cellProperties.Password + "\"";
			arguments += " \"" + cellProperties.Realm + "\"";
			arguments += " \"" + AppClientFolder + "\"";

			return ExecuteCommand(pathToBatch + "" + arguments, AppClientFolder, ServicePath, 60);
		}

		private string ExecuteGetServerStatsCmd(MonitoredItems.WebSphere cellProperties, string AppClientFolder, string ServicePath)
		{

			//string AppClientFolder = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
			string pathToBatch = "GET_SERVER_STATS.bat";

			string arguments = "";
			arguments += " \"" + cellProperties.CellHostName + "\"";
			arguments += " \"" + cellProperties.Port + "\"";
			arguments += " \"" + cellProperties.ConnectionType + "\"";
			arguments += " \"" + cellProperties.UserName + "\"";
			arguments += " \"" + cellProperties.Password + "\"";
			arguments += " \"" + cellProperties.Realm + "\"";
			arguments += " \"" + AppClientFolder + "\"";
			arguments += " \"" + cellProperties.NodeName + "\"";
			arguments += " \"" + cellProperties.ServerName + "\"";

			//throw new Exception("Path: " + pathToBatch + "" + arguments + ". App:" + AppClientFolder + "Serv:" + ServicePath);
			return ExecuteCommand(pathToBatch + "" + arguments, AppClientFolder, ServicePath, 60);
		}

		public Cells getServerList(CellProperties cellProperties, string AppClientPath = "", string ServicePath = "")
		{

			try
			{

				VSFramework.RegistryHandler registry = new VSFramework.RegistryHandler();
                //string AppClientPath = "";
                //string ServicePath = "";

                if (AppClientPath == "")
                {
                    try
                    {
                        AppClientPath = registry.ReadFromRegistry("WebSphereAppClientPath").ToString();
                    }
                    catch
                    {
                        AppClientPath = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
                    }
                }
				if (!AppClientPath.EndsWith("\\"))
					AppClientPath += "\\";

                //WS switched to use registry value in case of different paths on HA installs
                if (ServicePath == "")
                {
                    try
                    {
                        ServicePath = registry.ReadFromVitalSignsComputerRegistry("InstallPath").ToString();
                    }
                    catch (Exception ex)
                    {
                        //ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
                    }
                }
				if (ServicePath == "")
				{
					try
					{
						ServicePath = registry.ReadFromRegistry("InstallLocation").ToString();
					}
					catch
					{
						//ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
					}
				}

				if (ServicePath == "")
					ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";

				if (!ServicePath.EndsWith("\\"))
					ServicePath += "\\";


				string result = ExecuteGetServerListCmd(cellProperties, AppClientPath, ServicePath);

				Cells cells = (Cells)DecodeXMLFromString(result, typeof(Cells));

				int id = cellProperties.ID;

				return cells;
			}

			catch (Exception ex)
			{
				throw ex;
			}


		}

		public Cells_ServerStats getServerStats(MonitoredItems.WebSphere serverProperties)
		{
			try
			{
				VSFramework.RegistryHandler registry = new VSFramework.RegistryHandler();
				string AppClientPath = "";
				string ServicePath = "";

				try
				{
					AppClientPath = registry.ReadFromRegistry("WebSphereAppClientPath").ToString();
				}
				catch
				{
					AppClientPath = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
				}

				if (!AppClientPath.EndsWith("\\"))
					AppClientPath += "\\";

				//WS switched to use registry value in case of different paths on HA installs
				try
				{
					ServicePath = registry.ReadFromVitalSignsComputerRegistry("InstallPath").ToString();
				}
				catch
				{
					//ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
				}

				if (ServicePath == "")
				{
					try
					{
						ServicePath = registry.ReadFromRegistry("InstallLocation").ToString();
					}
					catch
					{
						//ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
					}
				}


				if (ServicePath == "")
					ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";

				if (!ServicePath.EndsWith("\\"))
					ServicePath += "\\";


				string result = ExecuteGetServerStatsCmd(serverProperties, AppClientPath, ServicePath);

                Cells_ServerStats cell = (Cells_ServerStats)DecodeXMLFromString(result, typeof(Cells_ServerStats));
                cell.rawXml = result;
				return cell;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/*
		public Cells_CellScan getCellStats(CellProperties cellProperties)
		{

			try
			{

				VSFramework.RegistryHandler registry = new VSFramework.RegistryHandler();
				string AppClientPath = "";
				try
				{
					AppClientPath = registry.ReadFromRegistry("WebSphereAppClientPath").ToString();
				}
				catch
				{
					AppClientPath = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
				}

				if (!AppClientPath.EndsWith("\\"))
					AppClientPath += "\\";

				//blahbalhblah(cellProperties, AppClientPath);

				string filePath = AppClientPath + "VitalSigns\\xml\\ServerStats.xml";

				Cells_CellScan cells = (Cells_CellScan)DecodeXMLFromPath(filePath, typeof(Cells_CellScan));

				return cells;
			}

			catch (Exception ex)
			{
				throw ex;
			}


		}
		*/

		#region SupportClasses

		public class CellProperties
		{
			public string HostName;
			public int Port;
			public string ConnectionType;
			public string UserName;
			public string Password;
			public string Realm;

			public string Name;
			public int ID;

		}

		public class NodeProperties
		{
			public string Name;
			public int ID;
		}

		public class ServerProperties : MonitoredItems.WebSphere
		{

		}




		#region Get_Server_List

		[XmlRoot(ElementName = "servers")]
		public class Servers
		{
			[XmlElement(ElementName = "server")]
			public List<string> Server { get; set; }
		}

		[XmlRoot(ElementName = "node")]
		public class Node
		{
			[XmlElement(ElementName = "servers")]
			public Servers Servers { get; set; }
			[XmlAttribute(AttributeName = "hostName")]
			public string HostName { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "nodes")]
		public class Nodes
		{
			[XmlElement(ElementName = "node")]
			public List<Node> Node { get; set; }
		}

		[XmlRoot(ElementName = "cell")]
		public class Cell
		{
			[XmlElement(ElementName = "nodes")]
			public Nodes Nodes { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "cells")]
		public class Cells
		{
			[XmlElement(ElementName = "cell")]
			public List<Cell> Cell { get; set; }
			[XmlElement(ElementName = "timestamp")]
			public string TimeStamp { get; set; }
			[XmlElement(ElementName = "connection-status")]
			public string Connection_Status { get; set; }
		}

		#endregion

		#region GET_SERVER_STATS

		[XmlRoot(ElementName = "status")]
		public class Status
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }

		}

		[XmlRoot(ElementName = "process")]
		public class Process
		{
			[XmlAttribute(AttributeName = "id")]
			public string ID { get; set; }
		}

        [XmlRoot(ElementName = "HeapSize")]
        public class HeapSize
        {
            [XmlAttribute(AttributeName = "unit")]
            public string Unit { get; set; }
            [XmlAttribute(AttributeName = "current")]
            public string Current { get; set; }
            [XmlAttribute(AttributeName = "initial")]
            public string initial { get; set; }
            [XmlAttribute(AttributeName = "maximum")]
            public string maximum { get; set; }
        }

     

		[XmlRoot(ElementName = "FreeMemory")]
		public class FreeMemory
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "UsedMemory")]
		public class UsedMemory
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "UpTime")]
		public class UpTime
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "ProcessCpuUsage")]
		public class ProcessCpuUsage
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "ActiveCount")]
		public class ActiveCount
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
		}

		[XmlRoot(ElementName = "PoolSize")]
		public class PoolSize
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
		}

		[XmlRoot(ElementName = "DeclaredThreadHungCount")]
		public class DeclaredThreadHungCount
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "ClearedThreadHangCount")]
		public class ClearedThreadHangCount
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "count")]
			public string Count { get; set; }
		}

		[XmlRoot(ElementName = "ConcurrentHungThreadCount")]
		public class ConcurrentHungThreadCount
		{
			[XmlAttribute(AttributeName = "unit")]
			public string Unit { get; set; }
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
		}

		[XmlRoot(ElementName = "ResponseTime")]
		public class ResponseTime
		{
			[XmlAttribute(AttributeName = "Value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "stats")]
		public class stats
		{
			[XmlElement(ElementName = "status")]
			public Status Status { get; set; }
			[XmlElement(ElementName = "process")]
			public Process Process { get; set; }
            [XmlElement(ElementName = "HeapSize")]
            public HeapSize HeapSize { get; set; }
			[XmlElement(ElementName = "FreeMemory")]
			public FreeMemory FreeMemory { get; set; }
			[XmlElement(ElementName = "UsedMemory")]
			public UsedMemory UsedMemory { get; set; }
			[XmlElement(ElementName = "UpTime")]
			public UpTime UpTime { get; set; }
			[XmlElement(ElementName = "ProcessCpuUsage")]
			public ProcessCpuUsage ProcessCpuUsage { get; set; }
			[XmlElement(ElementName = "ActiveCount")]
			public ActiveCount ActiveCount { get; set; }
            [XmlElement(ElementName = "PoolSize")]
            public PoolSize PoolSize { get; set; }
			[XmlElement(ElementName = "DeclaredThreadHungCount")]
			public DeclaredThreadHungCount DeclaredThreadHungCount { get; set; }
			[XmlElement(ElementName = "ClearedThreadHangCount")]
			public ClearedThreadHangCount ClearedThreadHangCount { get; set; }
			[XmlElement(ElementName = "ConcurrentHungThreadCount")]
			public ConcurrentHungThreadCount ConcurrentHungThreadCount { get; set; }
			[XmlElement(ElementName = "ResponseTime")]
			public ResponseTime ResponseTime { get; set; }
		}

		[XmlRoot(ElementName = "server")]
		public class Server_ServerStats
		{
			[XmlElement(ElementName = "stats")]
			public stats Stats { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "servers")]
		public class Servers_ServerStats
		{
			[XmlElement(ElementName = "server")]
			public Server_ServerStats Server { get; set; }
		}

		[XmlRoot(ElementName = "node")]
		public class Node_ServerStats
		{
			[XmlElement(ElementName = "servers")]
			public Servers_ServerStats Servers { get; set; }
			[XmlAttribute(AttributeName = "hostName")]
			public string HostName { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "nodes")]
		public class Nodes_ServerStats
		{
			[XmlElement(ElementName = "node")]
			public Node_ServerStats Node { get; set; }
		}

		[XmlRoot(ElementName = "cell")]
		public class Cell_ServerStats
		{
			[XmlElement(ElementName = "nodes")]
			public Nodes_ServerStats Nodes { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "cells")]
		public class Cells_ServerStats
		{
			[XmlElement(ElementName = "timestamp")]
			public string Timestamp { get; set; }
			[XmlElement(ElementName = "connection-status")]
			public string Connectionstatus { get; set; }
			[XmlElement(ElementName = "cell")]
			public Cell_ServerStats Cell { get; set; }

            public string rawXml;
        }

		#endregion


		#endregion

	}




}
