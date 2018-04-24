using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using VSWebBL;

namespace VSWebUI.Configurator
{
    public partial class ReadVSLogs : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            //4/28/2016 Sowjanya modified for VSPLUS-2676
		
            try
            {
				if (!IsPostBack)
				{
					filldropdown_ReadLogsComboBox();
					//filldropdown_HistoryCombobox()
				}
				else
				{
					filldropdown_ReadLogsComboBoxfromsession();
				}
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

		
		protected void filldropdown_ReadLogsComboBox()
		{
			DataTable dt = new DataTable();

			dt = getLogFilesDataTable();
			
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                string newrow = dt.Rows[i]["Path"].ToString();
                if (!newrow.EndsWith(".txt"))
                    dr.Delete();
            }
			ReadLogsComboBox.DataSource = dt;
			Session["LogFiles"] = dt;
		    ReadLogsComboBox.TextField = "Path";
            ReadLogsComboBox.ValueField = "FullPath";
			ReadLogsComboBox.DataBind();
		}
		protected DataTable getLogFilesDataTable()
		{
			string logPath = "";

			logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
			if (!logPath.EndsWith("\\"))
				logPath += "\\";
			if (logPath == "")
			{
				logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("Path", typeof(String));
			dt.Columns.Add("FullPath", typeof(String));
			dt.Columns.Add("ParentID", typeof(System.Int32));
			dt.Columns.Add("isFolder", typeof(Boolean));
			dt.Columns.Add("ID", typeof(System.Int32));
			dt.Columns["ID"].AutoIncrement = true;
			dt.Columns["ID"].AutoIncrementSeed = 0;
			dt.Columns["ID"].AutoIncrementStep = 1;


			GetAllLogFiles(logPath, ref dt, -1);

			return dt;
		}

        //public static void getfilePath(string sDir, string fileName)

        //   {
               
        //    try
        //    {
               
        //        foreach (string d in Directory.GetDirectories(sDir))
        //        {
        //            foreach (string f in Directory.GetFiles(d))
        //            {
                       
        //                if (f.Contains(fileName))
        //                {
                           
        //                    fullFilePath = f;
        //                }
        //            }
        //            if (!fileFound)
        //            {
        //                getfilePath(d, fileName);
        //            }
        //        }
        //    }
        //    catch (Exception excpt)
        //    {
        //        throw excpt;
        //    }
        //}

		protected void GetAllLogFiles(string path, ref DataTable dt, int i)
		{
			string[] filePaths = System.IO.Directory.GetFiles(path);
			string[] folderPaths = System.IO.Directory.GetDirectories(path);

			foreach (string file in filePaths)
			{
				if (file.Contains("LogFiles.z"))
					continue;
				DataRow row = dt.NewRow();
				row["FullPath"] = file;
				row["Path"] = file.Substring(file.LastIndexOf("\\") + 1);
				row["isFolder"] = false;
				if (i != -1)
				{
					row["ParentID"] = i;
				}
				dt.Rows.Add(row);
			}

			foreach (string dir in folderPaths)
			{
				DataRow row = dt.NewRow();
				row["FullPath"] = dir;
				row["Path"] = dir.Substring(dir.LastIndexOf("\\") + 1);
				row["isFolder"] = true;
				if (i != -1)
				{
					row["ParentID"] = i;
				}
				dt.Rows.Add(row);
				int k = int.Parse(row["ID"].ToString());
                //string contents = Directory.ReadAllText(dir);
                //File.WriteAllText(dir, contents);
				GetAllLogFiles(dir, ref dt, k);

			}
		}

		protected void ReadLogsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadHistory1();

		}
		public void LoadHistory1()
		{
            string logFilePath = string.Empty;
			try
			{
                //4/28/2016 Sowjanya modified for VSPLUS-2676
               if (ReadLogsComboBox.SelectedIndex >= 0)
				{
					
                  logFilePath = ReadLogsComboBox.SelectedItem.Value.ToString();
                    if (logFilePath != "")
					{
						//2/4/2015 NS added for VSPLUS-1411
						double maxfileLength = 5;
						try
						{
							//2/4/2015 NS added for VSPLUS-1411
                            long length = new System.IO.FileInfo(logFilePath).Length;
							double lengthd = length / 1024 / 1024;
							if (lengthd >= maxfileLength)
							{
                                HistoryMemo.Text = "The log file you selected is too large to be displayed in the browser. Please view the file " + logFilePath + " directly on the server.";
							}
							else
							{
								
                                using (StreamReader streamReader = new StreamReader(logFilePath))
								{
									HistoryMemo.Text = streamReader.ReadToEnd();
									streamReader.Close();
								}
							}
						}
						catch (Exception ex)
						{
							//8/13/2014 NS modified
                            logFilePath = ReadLogsComboBox.SelectedItem.Value.ToString();
							try
							{
								//2/4/2015 NS added for VSPLUS-1411
                                long length = new System.IO.FileInfo(logFilePath).Length;
								double lengthd = length / 1024 / 1024;
								if (lengthd >= maxfileLength)
								{
                                    HistoryMemo.Text = "The log file you selected is too large to be displayed in the browser. Please view the file " + logFilePath + " directly on the server.";
								}
								else
								{
                                    using (StreamReader streamReader = new StreamReader(logFilePath))
									{
										HistoryMemo.Text = streamReader.ReadToEnd();
										streamReader.Close();
									}
								}
							}
							catch
							{
								HistoryMemo.Text = ex.Message;
								//6/27/2014 NS added for VSPLUS-634
								Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		

		private void filldropdown_ReadLogsComboBoxfromsession()
		{
			DataTable dt = new DataTable();

			if (Session["LogFiles"] != "" && Session["LogFiles"] != null)
			{
				dt = Session["LogFiles"] as DataTable;
                ReadLogsComboBox.TextField = "Path";
                ReadLogsComboBox.ValueField = "FullPath";
				ReadLogsComboBox.DataBind();
			}
		}
			
    }
}