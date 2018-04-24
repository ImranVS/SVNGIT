using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class DominoCustomStatisticsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static DominoCustomStatisticsDAL _self = new DominoCustomStatisticsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoCustomStatisticsDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoCustomStatValues 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {

            DataTable DominoCustomStatDataTable = new DataTable();
            DominoCustomStatValues ReturnDSObject = new DominoCustomStatValues();
            try
            {
                string SqlQuery = "SELECT  ID,ServerName,StatName,ThresholdValue,GreaterThanORLessThan,TimesInARow,ConsoleCommand"+
                " FROM DominoCustomStatValues";

                DominoCustomStatDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return DominoCustomStatDataTable;
        }
        /// <summary>
        /// Get Data from DominoCustomStatValues based on Key
        /// </summary>
        /// <param name="DCSObject">DominoCustomStatValues object</param>
        /// <returns></returns>
        public DominoCustomStatValues GetData(DominoCustomStatValues DCSObject)
        {
            DataTable DominoCustomStatDataTable = new DataTable();
            DominoCustomStatValues ReturnDCSObject = new DominoCustomStatValues();
            try
            {
                string SqlQuery = "Select * from DominoCustomStatValues where ID=" + DCSObject.ID.ToString();
                DominoCustomStatDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object

                ReturnDCSObject.ServerName = DominoCustomStatDataTable.Rows[0]["ServerName"].ToString();
                ReturnDCSObject.StatName = DominoCustomStatDataTable.Rows[0]["StatName"].ToString();
                if(DominoCustomStatDataTable.Rows[0]["ThresholdValue"].ToString()!="")
                ReturnDCSObject.ThresholdValue =float.Parse( DominoCustomStatDataTable.Rows[0]["ThresholdValue"].ToString());
                ReturnDCSObject.GreaterThanORLessThan = DominoCustomStatDataTable.Rows[0]["GreaterThanORLessThan"].ToString();
                ReturnDCSObject.ConsoleCommand = DominoCustomStatDataTable.Rows[0]["ConsoleCommand"].ToString();
                if(DominoCustomStatDataTable.Rows[0]["TimesInARow"].ToString()!="")
                ReturnDCSObject.TimesInARow =int.Parse(DominoCustomStatDataTable.Rows[0]["TimesInARow"].ToString());
                             
                               
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnDCSObject;
        }
        /// <summary>
        /// Insert data into DominoCustomStatValues table
        /// </summary>
        /// <param name="DCSObject">DominoCustomStatValues object</param>
        /// <returns></returns>
        public bool InsertData(DominoCustomStatValues DCSObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO DominoCustomStatValues(ServerName,StatName,ThresholdValue,GreaterThanORLessThan,TimesInARow,ConsoleCommand)" +
                  " VALUES ('"+DCSObject.ServerName+"','"+DCSObject.StatName+"',"+DCSObject.ThresholdValue+",'"+DCSObject.GreaterThanORLessThan+
                  "',"+DCSObject.TimesInARow+",'"+DCSObject.ConsoleCommand+"')";

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

        /// <summary>
        /// Insert data into DominoCustomStatValues table
        /// </summary>
        /// <param name="DCSObject">DominoCustomStatValues object</param>
        /// <returns></returns>
        public Object UpdateData(DominoCustomStatValues DCSObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE DominoCustomStatValues SET ServerName='" + DCSObject.ServerName + "',StatName='" + DCSObject.StatName+
                     "', ThresholdValue=" + DCSObject.ThresholdValue + ",GreaterThanORLessThan='"+DCSObject.GreaterThanORLessThan+
                    "' ,TimesInARow= " + DCSObject.TimesInARow + " ,ConsoleCommand='" + DCSObject.ConsoleCommand + "' WHERE ID = '" + DCSObject.ID + "'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        //delete Data from DominoCluster Table

        public Object DeleteData(DominoCustomStatValues DCSObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete DominoCustomStatValues Where ID=" + DCSObject.ID;

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }


    }
}
