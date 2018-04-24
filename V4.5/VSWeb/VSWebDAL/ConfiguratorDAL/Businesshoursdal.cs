using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
	   public class Businesshoursdal
	{
		   private Adaptor objAdaptor = new Adaptor();
		   private static Businesshoursdal _self = new Businesshoursdal();
		   public static Businesshoursdal Ins
		   {
			   get { return _self; }

		   }
		   public bool InsertData(HoursIndicator BusinesshoursObject)

		   {
			   bool Insert = false;
			   try
			   {
				   DataTable NetworkDevicesDataTable = new DataTable();



                   //4/23/2015 NS modified
				   string SqlQuery = "INSERT INTO [HoursIndicator] (Type,Starttime,Duration,Issunday,IsMonday" +
					   ",IsTuesday,IsWednesday,IsThursday,IsFriday,Issaturday,UseType) " +
					   "VALUES('" + BusinesshoursObject.Type + "','" + BusinesshoursObject.Starttime + "','" + BusinesshoursObject.Duration + "','" + BusinesshoursObject.Issunday +
					   "','" + BusinesshoursObject.IsMonday + "','" + BusinesshoursObject.IsTuesday + "','" + BusinesshoursObject.IsWednesday +
					   "','" + BusinesshoursObject.IsThursday + "','" + BusinesshoursObject.IsFriday + "','" + BusinesshoursObject.Issaturday + 
                       "'," + BusinesshoursObject.UseType + ")";
					  Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			   }
			   catch
			   {
				   Insert = false;
			   }
			   finally
			   {
			   }
			   return Insert;
		   }
		   public DataTable GetBusinessHours()
		   {


			   // string logopath;
			   DataTable BusinessHours = new DataTable();
			   try
			   {
				   string sqlQuery = "Select * from HoursIndicator where id>=0 and  Type not in('Specific Hours','All Hours')";
				   BusinessHours = objAdaptor.FetchData(sqlQuery);

				   //5/23/2013 NS modified

			   }
			   catch (Exception)
			   {

				   throw;
			   }

			   return BusinessHours;
		   }
		  
 //          public DataTable GetDefaultGMT()
 //{


 //               string logopath;
 //              DataTable DefaultGMT = new DataTable();
 //              try
 //              {
 //                  string sqlQuery = "Select GMT from BusinessHours where name='Default'";
 //                  DefaultGMT = objAdaptor.FetchData(sqlQuery);

 //                  5/23/2013 NS modified

 //              }
 //              catch (Exception)
 //              {

 //                  throw;
 //              }

 //              return DefaultGMT;
 //          }
		   public HoursIndicator GetData(HoursIndicator BusinessObjects)

		   {
			   DataTable BusinesshoursDataTable = new DataTable();

			   HoursIndicator ReturnDSObject = new HoursIndicator();

			   try
			   {

                   //4/23/2015 NS modified
				   string SqlQuery = "select *,ISNULL(UseType,2) UseType2 from HoursIndicator where ID=" + BusinessObjects.ID + "";


				   BusinesshoursDataTable = objAdaptor.FetchData(SqlQuery);
				  

				   if (BusinesshoursDataTable.Rows.Count > 0)
				   {

					   ReturnDSObject.Type = BusinesshoursDataTable.Rows[0]["Type"].ToString();
					   if (BusinesshoursDataTable.Rows[0]["Duration"].ToString() != null && BusinesshoursDataTable.Rows[0]["Duration"].ToString() != "")
					   {
						   ReturnDSObject.Duration = Convert.ToInt32(BusinesshoursDataTable.Rows[0]["Duration"].ToString());
					   }
						   ReturnDSObject.Starttime = BusinesshoursDataTable.Rows[0]["Starttime"].ToString();
					  // ReturnDSObject.Issunday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["Issunday"].ToString());
						   if (BusinesshoursDataTable.Rows[0]["Issunday"].ToString() != null && BusinesshoursDataTable.Rows[0]["Issunday"].ToString() != "")
						   {
							   ReturnDSObject.Issunday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["Issunday"].ToString());
						   }
						   if (BusinesshoursDataTable.Rows[0]["IsMonday"].ToString() != null && BusinesshoursDataTable.Rows[0]["IsMonday"].ToString() != "")
					   {
							   ReturnDSObject.IsMonday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["IsMonday"].ToString());
					   }
						   if (BusinesshoursDataTable.Rows[0]["IsTuesday"].ToString() != null && BusinesshoursDataTable.Rows[0]["IsTuesday"].ToString() != "")
					   {
						   ReturnDSObject.IsTuesday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["IsTuesday"].ToString());
						}
						   if (BusinesshoursDataTable.Rows[0]["IsWednesday"].ToString() != null && BusinesshoursDataTable.Rows[0]["IsWednesday"].ToString() != "")
					   {
							ReturnDSObject.IsWednesday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["IsWednesday"].ToString());
					    
						}
						   if (BusinesshoursDataTable.Rows[0]["IsThursday"].ToString() != null && BusinesshoursDataTable.Rows[0]["IsThursday"].ToString() != "")
					   {
							ReturnDSObject.IsThursday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["IsThursday"].ToString());
						}
						   if (BusinesshoursDataTable.Rows[0]["IsFriday"].ToString() != null && BusinesshoursDataTable.Rows[0]["IsFriday"].ToString() != "")
					   {
							ReturnDSObject.IsFriday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["IsFriday"].ToString());
					    }
						   if (BusinesshoursDataTable.Rows[0]["Issaturday"].ToString() != null && BusinesshoursDataTable.Rows[0]["Issaturday"].ToString() != "")
					   {
							ReturnDSObject.Issaturday = Convert.ToBoolean(BusinesshoursDataTable.Rows[0]["Issaturday"].ToString());
						}
                       //4/23/2015 NS added
                           ReturnDSObject.UseType = Convert.ToInt32(BusinesshoursDataTable.Rows[0]["UseType2"].ToString());
                    }
			   }

			   catch
			   {
			   }
			   finally
			   {
			   }
			   return ReturnDSObject;
		   }
		   public Object DeleteBusinessHoursDetails(HoursIndicator Business)


		   {
			   Object Delete;
			   try
			   {
				   string SqlQuery = "Delete from HoursIndicator where ID=" + Business.ID +" and ID not in(0,1)" ;
				   Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
			   }
			   catch
			   {

				   Delete = false;
			   }
			   finally
			   {
			   }

			   return Delete;

		   }
		   public bool UpdateBusinesshours(HoursIndicator obj)

		   {
			   bool Update = false;
			   try
			   {
                   //4/23/2015 NS modified
				   string SqlQuery = "Update HoursIndicator set Type='" + obj.Type + "',Starttime='" + obj.Starttime + "',Duration='" + obj.Duration + "'," +
				  " Issunday='" + obj.Issunday + "',IsMonday='" + obj.IsMonday + "',IsTuesday='" + obj.IsTuesday + "',IsWednesday='" + obj.IsWednesday + 
                  "',IsThursday='" + obj.IsThursday + "',IsFriday='" + obj.IsFriday + "',Issaturday='" + obj.Issaturday + 
                  "',UseType=" + obj.UseType.ToString() + " where ID=" + obj.ID + "";
				   
				   Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			   }
			   catch (Exception)
			   {

				   Update = false;
			   }
			   return Update;
		   }
		   public bool UpdateAlertDetails(HoursIndicator obj)

		   {
			   bool Update = false;
			   try
			   {
                   //6/29/2015 NS modified for VSPLUS-1821
				   //string SqlQuery = "Update AlertDetails set StartTime='" + obj.Starttime + "',Duration=" + obj.Duration + " where HoursIndicator=" + obj.ID + "";
                   string SqlQuery = "UPDATE AlertDetails SET StartTime='" + obj.Starttime + "',Duration=" + obj.Duration + ",Day='" +
                                  obj.Days + "' WHERE HoursIndicator=" + obj.ID + "";

				   Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			   }
			   catch (Exception)
			   {

				   Update = false;
			   }
			   return Update;
		   }
		   public DataTable GetBusinesshoursNames()

		   {

			   DataTable BusinesshoursnameDataTable = new DataTable();
			   HoursIndicator ReturnLOCbject = new HoursIndicator();
			   try
			   {
				   //3/19/2014 NS modified for VSPLUS-484
                   //4/23/2015 NS modified
				   string SqlQuery = "SELECT * FROM HoursIndicator WHERE UseType!=0 ORDER BY TYPE";

				   BusinesshoursnameDataTable = objAdaptor.FetchData(SqlQuery);

			   }
			   catch
			   {
			   }
			   finally
			   {
			   }
			   return BusinesshoursnameDataTable;
		   }
		   public HoursIndicator GetDataForBUsinesshrs(HoursIndicator Busibject)
		   {
			   DataTable BusinessDataTable = new DataTable();
			   HoursIndicator ReturnBUsibject = new HoursIndicator();
			   object response = "";
			   try
			   {
				   string SqlQuery = "Select * from HoursIndicator where Type='" + Busibject.Type + "'";
				   BusinessDataTable = objAdaptor.FetchData(SqlQuery);
				   //populate & return data object
				   if (BusinessDataTable.Rows.Count > 0)
				   {
					   if (BusinessDataTable.Rows[0]["ID"].ToString() != "")
					   {
						   ReturnBUsibject.ID = int.Parse(BusinessDataTable.Rows[0]["ID"].ToString());
						   ReturnBUsibject.Starttime = BusinessDataTable.Rows[0]["Starttime"].ToString();
						   if (BusinessDataTable.Rows[0]["Duration"].ToString() != null && BusinessDataTable.Rows[0]["Duration"].ToString() != "")
						   {
							   ReturnBUsibject.Duration = int.Parse(BusinessDataTable.Rows[0]["Duration"].ToString());
						   }
					   }


				   }
				   else
				   {
					   response = 0;
				   }
			   }
			   catch
			   {
			   }
			   finally
			   {
			   }
			   return ReturnBUsibject;
		   }

		   public DataTable GetName(HoursIndicator Busibject)


		   {
			   //SametimeServers SametimeObj = new SametimeServers();
			   DataTable BsTable = new DataTable();
			   try
			   {
				   //if (Busibject.ID == 0)
				   //{
				   //    string sqlQuery = "Select * from HoursIndicator where Type='" + Busibject.Type + "' ";
				   //    BsTable = objAdaptor.FetchData(sqlQuery);
				   //}
				   //else
				   //{
				       string sqlQuery = "Select * from HoursIndicator where (Type='" + Busibject.Type + "')and ID<>" + Busibject.ID + " ";
					   BsTable = objAdaptor.FetchData(sqlQuery);
				   //}
			   }
			   catch (Exception)
			   {

				   throw;
			   }
			   return BsTable;

		   }
		   public DataTable GetBusiandOffhoursName(HoursIndicator Busibject)
              {
			  
			   DataTable BsTable = new DataTable();
			   try
			   {
				  
				   string sqlQuery = "Select * from HoursIndicator where ID=" + Busibject.ID ;
				   BsTable = objAdaptor.FetchData(sqlQuery);
				   
			   }
			   catch (Exception)
			   {

				   throw;
			   }
			   return BsTable;

		   }
		   public DataTable GetNamebydropdown(HoursIndicator Busibject)

		   {
			   
			   DataTable BsTable = new DataTable();
			   try
			   {
				   string sqlQuery = "Select * from HoursIndicator where Type='" + Busibject.Type + "' ";
			   }
			   catch (Exception)
			   {

				   throw;
			   }
			   return BsTable;

		   }
	}

   }	  
						  
			
