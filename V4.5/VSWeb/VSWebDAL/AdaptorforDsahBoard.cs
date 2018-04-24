using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace VSWebDAL
{
	public class AdaptorforDsahBoard
	{
		/// <summary>
		/// Start DB connection
		/// </summary>
		/// <returns></returns>
		private SqlConnection StartConnection()
		{
			SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VSS_StatisticsConnectionString"].ToString());
			try
			{
				con.Open();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", Exception - " + ex);
			}
			return con;

		}

		/// <summary>
		/// Stop DB connection
		/// </summary>
		/// <param name="con"></param>
		private void StopConnection(SqlConnection con)
		{
			try
			{
				con.Close();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", Exception - " + ex);
			}
		}

		/// <summary>
		/// Execute Insert/Update/Delete
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <returns>true if success or false if failure</returns>
		public bool ExecuteNonQuery(string SQLStatement)
		{
			bool results = false;

			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				myCommand.ExecuteNonQuery();
				myCommand.Dispose();

				results = true;
			}
			catch (Exception ex)
			{
				results = false;
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + SQLStatement + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return results;
		}

		/// <summary>
		/// Execute Select query
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <returns>returns rows count</returns>
		public Int32 ExecuteScalar(string SQLStatement)
		{
			SqlConnection con = new SqlConnection();
			Int32 intId = 0;
			try
			{
				con = StartConnection();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				if (myCommand.ExecuteScalar() == null)
				{
					intId = 0;
				}
				else
				{
					intId = Convert.ToInt32(myCommand.ExecuteScalar());
				}
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				intId = 0;
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + SQLStatement + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return intId;
		}

		/// <summary>
		/// Fetches Data based on SQL query
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <returns>Datatable</returns>
		public DataTable FetchData(string SQLStatement)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = myCommand;

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];

				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + SQLStatement + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable GetDataFrompivotProcedure(string storedprocedure)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + storedprocedure + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataSet FetchData1(string SQLStatement)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			DataSet ds = new DataSet();

			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				//DataSet ds = new DataSet();

				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = myCommand;

				myAdapter.Fill(ds);
				dt = ds.Tables[0];

				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + SQLStatement + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return ds;
		}

		public DataTable FetchDeviceHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetDeviceHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetDeviceHourlyVals , frmdate: " + frmdate + ",statname: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchOffice365HourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetMicrosoftOffice365HourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetMicrosoftOffice365HourlyVals , frmdate: " + frmdate + ",statname: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchNetworkDeviceHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetNetworkDeviceHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetNetworkDeviceHourlyVals, frmdate: " + frmdate + ",statname: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable FetchLatencyHourlyVals(DateTime frmdate, string statname, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetMailLatencyDailyStats", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@sourceserver ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DestinationServer", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetMailLatencyDailyStats, frmdate: " + frmdate + ",statname: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		//2/5/2015 NS modified for VSPLUS-1370
		public double GetDominoDiskConsumption(string ServerType, string ServerName, string DiskName, int ismatch, bool isSummary)
		{
			double dcon = 0;

			SqlDataAdapter myAdapter = new SqlDataAdapter();
			SqlCommand cmd = new SqlCommand();
			DataTable dt = new DataTable();
			DataSet ds = new DataSet();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				if (!isSummary)
				{
					myAdapter.SelectCommand = new SqlCommand("CalcAvgDiskConsumtpion", con);
				}
				else
				{
					myAdapter.SelectCommand = new SqlCommand("CalcAvgDiskConsumptionTotal", con);
				}
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				cmd = myAdapter.SelectCommand;
				SqlParameter param0 = new SqlParameter("@ServerTypeIn", SqlDbType.VarChar);
				param0.Value = 50;
				param0.Direction = ParameterDirection.Input;
				cmd.Parameters.AddWithValue("@ServerTypeIn", ServerType);
				SqlParameter param = new SqlParameter("@ServerNameIn", SqlDbType.VarChar);
				param.Value = 50;
				param.Direction = ParameterDirection.Input;
				cmd.Parameters.AddWithValue("@ServerNameIn", ServerName);
				if (!isSummary)
				{
					SqlParameter param2 = new SqlParameter("@DiskNameIn", SqlDbType.VarChar);
					param2.Size = 10;
					param2.Direction = ParameterDirection.Input;
					cmd.Parameters.AddWithValue("@DiskNameIn", DiskName);
				}
				else
				{
					SqlParameter param4 = new SqlParameter("@ExactMatch", SqlDbType.Int);
					param4.Direction = ParameterDirection.Input;
					cmd.Parameters.AddWithValue("@ExactMatch", ismatch);
				}
				SqlParameter param3 = new SqlParameter("@DiskConsumption", SqlDbType.Float);
				param3.Direction = ParameterDirection.Output;
				cmd.Parameters.Add(param3);
				cmd.ExecuteScalar();
				dcon = Convert.ToDouble(cmd.Parameters["@DiskConsumption"].Value.ToString());

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:CalcAvgDiskConsumtpion,ServerType: " + ServerType + ",ServerName: " + ServerName + ",DiskName: " + DiskName + ", ismatch: " + ismatch.ToString() + ",isSummary: " + isSummary.ToString() + ",Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}

			return dcon;
		}


		//MD Exchange   
		public DataTable FetchMicrosoftHourlyVals(string statname, DateTime frmdate, string DeviceName, int ServerTypeId)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetMicrosoftHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ServerTypeId", ServerTypeId));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetMicrosoftHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",ServerTypeId: " + ServerTypeId.ToString() + ", Exception - " + ex);


			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		//2/4/2014 NS added for sched reports
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
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", strMsg: " + strMsg + ",, Exception - " + ex);

			}
			finally
			{
				GC.Collect();
			}
		}

		//20Feb14 Mukund added for Notes page in Dashboard
		public DataTable FetchNotesHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				myAdapter.SelectCommand = new SqlCommand("GetNotesHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetNotesHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",, Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		/*
        public DataTable FetchExchHourlyVals(string statname, DateTime frmdate, string DeviceName)
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            try
            {
                con = StartConnection();
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                SqlCommand myCommand = new SqlCommand();
                DataSet ds = new DataSet();

                myAdapter.SelectCommand = new SqlCommand("GetExchHourlyVals", con);
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

                myAdapter.Fill(ds, "dt");
                dt = ds.Tables[0];
                myAdapter.SelectCommand.Parameters.Clear();
                myCommand.Dispose();

            }
            catch
            {

            }
            finally
            {
                StopConnection(con);

            }
            return dt;
        }
        public DataTable FetchLyncHourlyVals(string statname, DateTime frmdate, string DeviceName)
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            try
            {


                con = StartConnection();
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                SqlCommand myCommand = new SqlCommand();
                DataSet ds = new DataSet();

                //myCommand.Connection = con;
                //myCommand.CommandText = SQLStatement;
                myAdapter.SelectCommand = new SqlCommand("GetLyncHourlyVals", con);
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
                myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

                myAdapter.Fill(ds, "dt");
                dt = ds.Tables[0];
                myAdapter.SelectCommand.Parameters.Clear();
                myCommand.Dispose();

            }
            catch
            {

            }
            finally
            {
                StopConnection(con);

            }
            return dt;
        }*/
		public DataTable FetchchatData(string SQLStatement)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = myCommand;

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];

				myCommand.Dispose();


			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SQL: " + SQLStatement + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		//Mukund, 15Sep14
		/*public DataTable FetchsharepointHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{


				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetSharepointHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch
			{

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		*/
		//Mukund, 15Sep14
		public DataTable FetchBESHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{


				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetBESHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetBESHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		/*public DataTable FetchGenericHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{


				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetWindowsHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch
			{

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}*/

		/*public DataTable FetchActiveDirectoryHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{


				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetActiveDirectoryHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch
			{

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}*/
		public DataTable FetchSNMPHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetDeviceHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SP:GetDeviceHourlyVals,frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchCloudHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("CloudHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SP:CloudHourlyVals,frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable FetchURLHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetDeviceHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", SP:GetDeviceHourlyVals,frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchNetworkHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetDeviceHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetDeviceHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}


		public DataTable FetchSametimeHourlyVals(string statname, DateTime frmdate, string DeviceName, int ServerTypeId)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetSametimeHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));
				//myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ServerTypeId", ServerTypeId));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetSametimeHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",DeviceName: " + DeviceName + ", ServerTypeId: " + ServerTypeId.ToString() + ",Exception - " + ex);

			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable FetchWebSphereHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetWebSphereHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));
				//myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ServerTypeId", ServerTypeId));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetWebSphereHourlyVals , frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}


			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable FetchWebSphereValsPerScan(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("GetWebSphereScanVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));
				//myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ServerTypeId", ServerTypeId));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:GetWebSphereHourlyVals , frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}


			finally
			{
				StopConnection(con);

			}
			return dt;
		}


		public DataTable FetchcloudHourlyVals(string statname, DateTime frmdate, string DeviceName)
		{

			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection();
			try
			{


				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand("CloudHourlyVals", con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@dtfrom", frmdate));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatName ", statname));
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DeviceName", DeviceName.ToString()));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:CloudHourlyVals, frmdate: " + frmdate + ",StatName: " + statname + ",DeviceName: " + DeviceName + ",DeviceName: " + DeviceName + ", Exception - " + ex);

			}

			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable GetDataFromProcedure(string storedprocedure, string parameterName, string parameterValue)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + parameterName, parameterValue));

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ",parameterName: " + parameterName + ",parameterValue: " + parameterValue + ", Exception - " + ex);
				//throw ex;
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}


	}
}
