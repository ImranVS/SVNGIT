using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace DAL
{
	public class Adaptor
	{
		/// <summary>
		/// Start DB connection
		/// </summary>
		/// <returns></returns>
		private SqlConnection StartConnection()
		{
			SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LicenseConnectionString"].ToString());
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
				//3/8/2013 NS added
				int rowcount = 0;
				con = StartConnection();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				//3/8/2013 NS added
				rowcount = myCommand.ExecuteNonQuery();
				myCommand.Dispose();

				//3/8/2013 NS added
				if (rowcount > 0)
				{
					results = true;
				}

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

		public int ExecuteNonQueryRetRows(string SQLStatement)
		{
			int results = 0;
			SqlConnection con = new SqlConnection();
			try
			{
				con = StartConnection();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = SQLStatement;
				results = myCommand.ExecuteNonQuery();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				results = 0;
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
			Int32 intId = 0;
			SqlConnection con = new SqlConnection();
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
				//StopConnection(con);

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
		public DataTable FetchStatus(string storedprocedure, string parameter)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (storedprocedure == "StatusByType" || storedprocedure == "StatusByCategory")
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Location", parameter.ToString()));
				}
				else
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Type", parameter.ToString()));
				}
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ",parameter: " + parameter + ", Exception - " + ex);
				
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchStatusbyint(string storedprocedure, int parameter)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (storedprocedure == "StatusByType" || storedprocedure == "StatusByCategory")
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CellID", parameter));
				}
				else
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CellID", parameter));
				}
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ",parameter: " + parameter.ToString() + ", Exception - " + ex);
			
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchSametimeStatusbyint(string storedprocedure, int parameter)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (storedprocedure == "StatusByType" || storedprocedure == "StatusByCategory")
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CellID", parameter));
				}
				else
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CellID", parameter));
				}
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ",parameter: " + parameter.ToString() + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		public DataTable FetchSpecificservers(string storedprocedure, string Page,string Control)

		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();

				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (storedprocedure == "StatusByType" || storedprocedure == "StatusByCategory")
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Page", Page));
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Control", Control));
				}
				else
				{
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Page", Page));
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Control", Control));
				}
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ", Exception - " + ex);
			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}

		public DataTable GetNavigatorByUserID(string storedprocedure, int UserId, string Level,string MenuArea)
		{
			SqlConnection con = new SqlConnection();
			DataTable dt = new DataTable();
			try
			{
				con = StartConnection();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				DataSet ds = new DataSet();

				//myCommand.Connection = con;
				//myCommand.CommandText = SQLStatement;
				myAdapter.SelectCommand = new SqlCommand(storedprocedure, con);
				myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserId", UserId));
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@MenuArea ", MenuArea));
					myAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Level", Level = Level.Substring(2)));
				

				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myAdapter.SelectCommand.Parameters.Clear();
				myCommand.Dispose();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ",SP:sp_MenuSorting, frmdate: MenuArea: " + MenuArea + ",Level: " + Level + ",UserId: " + UserId.ToString() + ", Exception - " + ex);


			}
			finally
			{
				StopConnection(con);

			}
			return dt;
		}
		//7/30/2013 NS added
		public bool ExecuteQueryWithParams(string SQLStatement, int paramnum, string[] paramnames, string[] paramvalues)
		{
			bool results = false;
			SqlParameter param;
			SqlConnection con = new SqlConnection();
			try
			{
				int rowcount = 0;
				con = StartConnection();
				SqlCommand myCommand = new SqlCommand(SQLStatement);
				for (int i = 0; i < paramnum; i++)
				{
					param = new SqlParameter();
					param.ParameterName = paramnames[i];
					param.Value = paramvalues[i];
					myCommand.Parameters.Add(param);
				}
				myCommand.Connection = con;
				rowcount = myCommand.ExecuteNonQuery();
				if (rowcount > 0)
				{
					results = true;
				}
				myCommand.Dispose();


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



		public DataTable GetDataFromProcedure(string storedprocedure)
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
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", StoredProcedure: " + storedprocedure + ", Exception - " + ex);
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
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + ", strMsg: " + strMsg + ", Exception - " + ex);
			}
			finally
			{
				GC.Collect();
			}
		}

	}

}
