using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.StatusDAL
{
    public class StatusTDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static StatusTDAL _self = new StatusTDAL();


        public static StatusTDAL Ins
        {
            get { return _self; }
        }

        public bool InsertData(Status StatusObj)
        {
            bool Insert = false;

            try
            {
               //string SqlQuery = "Insert Into Status(Type,Location,Category,Name,Status,Details,LastUpdate,Description" +
               // ",PendingMail,DeadMail,MailDetails,Upcount,DownCount,UpPercent,ResponseTime,ResponseThreshold,PendingThreshold" +
               //  ",DeadThreshold,UserCount,MyPercent,"+"(StatusObj.NextScan == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? "" : "NextScan")"+",DominoServerTasks,TypeANDName,Icon,OperatingSystem,DominoVersion" +
               // ",UpMinutes,DownMinutes,UpPercentMinutes,PercentageChange,CPU,HeldMail,HeldMailThreshold,Severity,Memory)" +
               // "Values('"+StatusObj.Type+"','"+StatusObj.Location+"','"+StatusObj.Category+"','"+StatusObj.Name+"','"+StatusObj.sStatus+
               // "','"+StatusObj.Details+"','"+StatusObj.LastUpdate+"','"+StatusObj.Description+"','"+StatusObj.PendingMail+
               // "','"+StatusObj.DeadMail+"','"+StatusObj.MailDetails+"',"+StatusObj.Upcount+","+StatusObj.DownCount+","+StatusObj.UpPercent+
               // ","+StatusObj.ResponseTime+","+StatusObj.ResponseThreshold+","+StatusObj.PendingThreshold+","+StatusObj.DeadThreshold+
               //  ","+StatusObj.UserCount+","+StatusObj.MyPercent+",(StatusObj.NextScan == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? "" : "'" + StatusObj.NextScan + "',"),'"+StatusObj.DominoServerTasks+
               // "','"+StatusObj.TypeANDName+"',"+StatusObj.Icon+",'"+StatusObj.OperatingSystem+"','"+StatusObj.DominoVersion+
               // "',"+StatusObj.UpMinutes+","+StatusObj.DownMinutes+","+StatusObj.UpPercentMinutes+","+StatusObj.PercentageChange+
               // ","+StatusObj.CPU+","+StatusObj.HeldMail+","+StatusObj.HeldMailThreshold+","+StatusObj.Severity+","+StatusObj.Memory+")";

                string SqlQuery = "Insert Into Status(Type,Location,Category,Name,Status,Details,LastUpdate,Description" +
                ",PendingMail,DeadMail,MailDetails,Upcount,DownCount,UpPercent,ResponseTime,ResponseThreshold,PendingThreshold" +
                ",DeadThreshold,UserCount,MyPercent," +
                (StatusObj.NextScan == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? "" : "NextScan,") +
                "DominoServerTasks,TypeANDName,Icon,OperatingSystem,DominoVersion" +
                ",UpMinutes,DownMinutes,UpPercentMinutes,PercentageChange,CPU,HeldMail,HeldMailThreshold,Severity,Memory,StatusCode)" +
                "Values('" + StatusObj.Type+ "','" + StatusObj.Location + "','" + StatusObj.Category + "','" + StatusObj.Name + "','" + StatusObj.sStatus +
                "','" + StatusObj.Details + "','" + StatusObj.LastUpdate + "','" + StatusObj.Description + "','" + StatusObj.PendingMail +
                "','" + StatusObj.DeadMail + "','" + StatusObj.MailDetails + "'," + StatusObj.Upcount + "," + StatusObj.DownCount + "," + StatusObj.UpPercent +
                "," + StatusObj.ResponseTime + "," + StatusObj.ResponseThreshold + "," + StatusObj.PendingThreshold + "," + StatusObj.DeadThreshold +
                "," + StatusObj.UserCount + "," + StatusObj.MyPercent + "," +
                 (StatusObj.NextScan == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? "" : "'" + StatusObj.NextScan + "',") +
                "'" + StatusObj.DominoServerTasks + "','" + StatusObj.TypeANDName + "'," + StatusObj.Icon + ",'" + StatusObj.OperatingSystem + "','" + StatusObj.DominoVersion +
                "'," + StatusObj.UpMinutes + "," + StatusObj.DownMinutes + "," + StatusObj.UpPercentMinutes + "," + StatusObj.PercentageChange +
                "," + StatusObj.CPU + "," + StatusObj.HeldMail + "," + StatusObj.HeldMailThreshold + "," + StatusObj.Severity + "," + StatusObj.Memory + ",'"+StatusObj.sStatus+"')";
                

               Insert= objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch 
            {

                Insert = false;
            }
            return Insert;
        }

        public DataTable DistinctServerTypes()
        {
            DataTable serverTypes = new DataTable();
            try
            {
                string SqlQuery = "select Distinct [Type] from Status";
                serverTypes = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return serverTypes;
        }
        public DataTable DistinctLocations()
        {
            DataTable locations = new DataTable();
            try
            {
                string SqlQuery = "select Distinct Location from Status where Location<>null or Location<>''";
                locations = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return locations;
        }

        public bool UpdateforScan(Status StatusObj)
        {
            bool update = false;

            try
            {

                string SqlQuery = "update Status set Description='Queued for immediate scanning...', Details='Queued for immediate scanning...' where Type= '" + StatusObj.Type + "' and Name='" + StatusObj.Name + "'";


                update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {

                update = false;
            }
            return update;
        }

    }
}
