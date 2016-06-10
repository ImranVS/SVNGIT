using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Data;

namespace VSWebUI.SchedReports
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

    public partial class ScheduleReport : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection();
        VSFramework.XMLOperation myAdapter = new VSFramework.XMLOperation();
        ScheduledReportsSend.ScheduledReportsSend sr = new ScheduledReportsSend.ScheduledReportsSend();
		private static string LogFileName = "~/LogFiles/VSPlusLog.txt";
        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
				WriteServiceHistoryEntry("Executing  Scheduled Reports.", false);
				con.ConnectionString = myAdapter.GetDBConnectionString("VitalSigns");
				string strAppPath;
				//strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				//File.Delete(strAppPath + "/log_files/ScheduledReports_Service.txt");
				strAppPath=Page.MapPath(LogFileName);
				File.Delete(strAppPath);
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " Error deleting log file " + ex.ToString(), true);
            }

            ProcessScheduledReports();

            SchedReportLabel.Text = SchedReportLabel.Text + "<br /><br /> End of logging.";
            
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
			try{
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
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error getting a dataset from the table - " + ex.Message, true);
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
                    WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error getting a handle on the stored proc - " + ex.Message, true);
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
                    WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error executing ShouldScheduledReportsBeSent stored procedure - " + ex.Message, true);
                }
				finally
				{
					con.Close();
				}
			
                for (int i = 0; i < srdefArr.Count; i++)
                {
                    srdef = srdefArr[i];
                    if (srdef.RunNow)
                    {
                        WriteServiceHistoryEntry(DateTime.Now.ToString() + " Sending report " + srdef.Subject + " to " + srdef.SendTo, false);
                        try
                        {
                            sr.SendReport(srdef.ReportID, srdef.Subject, srdef.Body, srdef.SendTo, srdef.CopyTo, srdef.BlindCopyTo, srdef.FileFormat);
                        }
                        catch (Exception ex)
                        {
                            WriteServiceHistoryEntry(DateTime.Now.ToString() + " In ProcessScheduledReports: Error executing SendReport - " + ex.Message, true);
                        }
                    }
                }
			
            
        }
			}
			catch { }
			
		}
        protected void WriteServiceHistoryEntry(string strMsg, bool isError)
        {
            if (isError)
                SchedReportLabel.ForeColor = System.Drawing.Color.Red;
            SchedReportLabel.Text = SchedReportLabel.Text +"<br />"+ strMsg;

            bool appendMode = true;

			string ServiceLogDestination =Page.MapPath("~/LogFiles/ScheduledReports_Service.txt");
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
    }
}