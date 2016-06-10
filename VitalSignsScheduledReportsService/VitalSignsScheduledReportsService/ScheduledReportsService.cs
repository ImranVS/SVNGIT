using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Configuration;

namespace VitalSignsScheduledReportsService
{
    public class SchedReportDefinition
    {
        private int _ID;
        private int _ReportID;
        private string _Frequency;
        private string _Days;
        private int _SpecificDay;
        private string _Subject;
        private string _Body;
        private string _SendTo;
        private string _CopyTo;
        private string _BlindCopyTo;
        private string _FileFormat;
        private bool _RunNow;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int ReportID
        {
            get { return _ReportID; }
            set { _ReportID = value; }
        }
        public string Frequency
        {
            get { return _Frequency; }
            set { _Frequency = value; }
        }
        public string Days
        {
            get { return _Days; }
            set { _Days = value; }
        }
        public int SpecificDay
        {
            get { return _SpecificDay; }
            set { _SpecificDay = value; }
        }
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        public string SendTo
        {
            get { return _SendTo; }
            set { _SendTo = value; }
        }
        public string CopyTo
        {
            get { return _CopyTo; }
            set { _CopyTo = value; }
        }
        public string BlindCopyTo
        {
            get { return _BlindCopyTo; }
            set { _BlindCopyTo = value; }
        }
        public string FileFormat
        {
            get { return _FileFormat; }
            set { _FileFormat = value; }
        }
        public bool RunNow
        {
            get { return _RunNow; }
            set { _RunNow = value; }
        }

        public SchedReportDefinition(int ID,
            int ReportID,
            string Frequency,
            string Days,
            int SpecificDay,
            string Subject,
            string Body,
            string SendTo,
            string CopyTo,
            string BlindCopyTo,
            string FileFormat,
            bool RunNow)
	    {
            this._ID = ID;
            this._ReportID = ReportID;
            this._Frequency = Frequency;
            this._Days = Days;
            this._SpecificDay = SpecificDay;
            this._Subject = Subject;
            this._Body = Body;
            this._SendTo = SendTo;
            this._CopyTo = CopyTo;
            this._BlindCopyTo = BlindCopyTo;
            this._FileFormat = FileFormat;
            this._RunNow = RunNow;
	    }

        public SchedReportDefinition() { }
    }

    public partial class ScheduledReportsService : ServiceBase
    {
        SqlConnection con = new SqlConnection();
        //2/4/2014 NS commented out while testing
        VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
        VSWebUI.ScheduledReportsSend.ScheduledReports sr = new VSWebUI.ScheduledReportsSend.ScheduledReports();
        bool Stopping = false;

        public ScheduledReportsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string strAppPath;

            try
            {
                strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                File.Delete(strAppPath + "/log_files/ScheduledReports_Service.txt");
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " Error deleting log file " + ex.ToString());
            }
            //2/4/2014 NS commented out while testing
            con.ConnectionString = myAdapter.GetDBConnectionString("VitalSigns");
            WriteServiceHistoryEntry(DateTime.Now.ToString() + " The VitalSigns Scheduled Reports Service is starting up.");
            WriteServiceHistoryEntry(DateTime.Now.ToString() + " The VitalSigns Scheduled Reports Service is Copyright " + DateTime.Now.Year.ToString() + ", JNIT, Inc. dba RPR Wyatt and MZL Software Development, Inc.");
            ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceWorkerThread));
        }

        protected override void OnStop()
        {
            WriteServiceHistoryEntry(DateTime.Now.ToString() + " The VitalSigns Scheduled Reports Service is shutting down.");
            this.Stopping = true;
        }

        private void ServiceWorkerThread(Object stateInfo)
        {
            WriteServiceHistoryEntry(DateTime.Now.ToString() + " The Service Worker Thread is starting up.");
            //while (!this.Stopping)
            //{
                try
                {
                    //Retrieve records from the SQL table to see if anything needs to be sent
                    ProcessScheduledReports();
                    //Thread.Sleep(86400000);
                    //this.Stop();
                }
                catch (Exception ex)
                {
                    WriteServiceHistoryEntry(DateTime.Now.ToString() + " Error ServiceWorkerThread when calling ProcessScheduledReports(): " + ex.Message);
                    Thread.Sleep(5000);
                }
            //}
            WriteServiceHistoryEntry(DateTime.Now.ToString() + " The Service Worker Thread is shutting down... ");
        }

        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;
            string ServiceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/log_files/ScheduledReports_Service.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }

        private void ProcessScheduledReports()
        {
            string sqlStr = "";
            int retVal = 0;
            SqlDataAdapter da1 = new SqlDataAdapter();
            SqlParameter param = new SqlParameter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SchedReportDefinition srdef = new SchedReportDefinition();
            List<SchedReportDefinition> srdefArr = new List<SchedReportDefinition>();

            con.Open();
            sqlStr = "SELECT [ID],[ReportID],[Frequency],[Days],[SpecificDay],[SendTo],ISNULL(CopyTo,'') CopyTo, " +
                "ISNULL(BlindCopyTo,'') BlindCopyTo,[Title],[Body],[FileFormat] FROM ScheduledReports";
            try
            {
                da1 = new SqlDataAdapter(sqlStr, con);
                da1.Fill(ds1, "RunNow");
                dt1 = ds1.Tables[0];
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error getting a dataset from the table - " + ex.Message);
            }
            //WriteServiceHistoryEntry(DateTime.Now.ToString() + " rows: " + dt1.Rows.Count.ToString());
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    srdef = new SchedReportDefinition(Convert.ToInt32(dt1.Rows[i]["ID"].ToString()), 
                        Convert.ToInt32(dt1.Rows[i]["ReportID"].ToString()),
                        dt1.Rows[i]["Frequency"].ToString(), 
                        dt1.Rows[i]["Days"].ToString(), 
                        Convert.ToInt32(dt1.Rows[i]["SpecificDay"].ToString()),
                        dt1.Rows[i]["Title"].ToString(), 
                        dt1.Rows[i]["Body"].ToString(), 
                        dt1.Rows[i]["SendTo"].ToString(), 
                        dt1.Rows[i]["CopyTo"].ToString(),
                        dt1.Rows[i]["BlindCopyTo"].ToString(), 
                        dt1.Rows[i]["FileFormat"].ToString(), 
                        Convert.ToBoolean(retVal));
                    srdefArr.Add(srdef);
                }
            }
            bool notEmpty = srdefArr.Any();
            if (notEmpty)
            {
                try
                {
                    da1 = new SqlDataAdapter("ShouldScheduledReportsBeSent", con);
                    da1.SelectCommand.CommandType = CommandType.StoredProcedure;
                }
                catch (Exception ex)
                {
                    WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error getting a handle on the stored proc - " + ex.Message);
                }
                try
                {
                    da1.SelectCommand.Parameters.Add(new SqlParameter("@Frequency", SqlDbType.VarChar, 50));
                    da1.SelectCommand.Parameters.Add(new SqlParameter("@Days", SqlDbType.VarChar, 150));
                    da1.SelectCommand.Parameters.Add(new SqlParameter("@SpecificDay", SqlDbType.Int));
                    param.Direction = ParameterDirection.ReturnValue;
                    param.ParameterName = "returnValue";
                    da1.SelectCommand.Parameters.Add(param);
                    for (int i = 0; i < srdefArr.Count; i++)
                    {
                        srdef = srdefArr[i];
                        da1.SelectCommand.Parameters["@Frequency"].Value = srdef.Frequency;
                        da1.SelectCommand.Parameters["@Days"].Value = srdef.Days;
                        da1.SelectCommand.Parameters["@SpecificDay"].Value = srdef.SpecificDay;
                        da1.SelectCommand.ExecuteNonQuery();
                        retVal = Convert.ToInt32(da1.SelectCommand.Parameters["returnValue"].Value.ToString());
                        srdef.RunNow = Convert.ToBoolean(retVal);
                    }
                }
                catch (Exception ex)
                {
                    WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error executing ShouldScheduledReportsBeSent stored procedure - " + ex.Message);
                }
                con.Close();
                for (int i = 0; i < srdefArr.Count; i++)
                {
                    srdef = srdefArr[i];
                    if (srdef.RunNow)
                    {
                        WriteServiceHistoryEntry(DateTime.Now.ToString() + " Sending report " + srdef.Subject + " to " + srdef.SendTo);
                        try
                        {
                            sr.SendReport(srdef.ReportID, srdef.Subject, srdef.Body, srdef.SendTo, srdef.CopyTo, srdef.BlindCopyTo, srdef.FileFormat);
                        }
                        catch (Exception ex)
                        {
                            WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error executing SendReport - " + ex.Message);
                        }
                    }
                }               
            }
        }    
    }
}
