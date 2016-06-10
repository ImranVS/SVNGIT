using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
   public class FeaturesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static FeaturesDAL _self = new FeaturesDAL();

        public static FeaturesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from FeaturesDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable FeaturesDataTable = new DataTable();
            Features ReturnLOCbject = new Features();
            try
            {
                string SqlQuery = "SELECT * FROM [Features] ORDER BY Name";
                FeaturesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return FeaturesDataTable;
        }
 
       public Features GetData(Features LOCbject)
        {
            DataTable FeaturesDataTable = new DataTable();
            Features ReturnLOCbject = new Features();
            try
            {
                string SqlQuery = "Select * from Features where [ID]=" + LOCbject.ID.ToString();
                FeaturesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnLOCbject.Name = FeaturesDataTable.Rows[0]["Name"].ToString();
              
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }
        public Features GetDataForFeature(Features LOCbject)
        {
            DataTable FeaturesDataTable = new DataTable();
            Features ReturnLOCbject = new Features();
            object response = "";
            try
            {
                string SqlQuery = "Select * from Features where Name='" + LOCbject.Name + "'";
                FeaturesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (FeaturesDataTable.Rows.Count > 0)
                {
                    if (FeaturesDataTable.Rows[0]["ID"].ToString() != "")
                    {
                        ReturnLOCbject.ID = int.Parse(FeaturesDataTable.Rows[0]["ID"].ToString());
                        
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
            return ReturnLOCbject;
        }

        /// <summary>
        /// Insert data into Features table
        /// </summary>
        /// <param name="DSObject">Features object</param>
        /// <returns></returns>

        public bool InsertData(Features LOCbject)
        {

            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO Features (Name) VALUES('"+LOCbject.Name+"')";
                   

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
        /// Update data into Features table
        /// </summary>
        /// <param name="LOCbject">Features object</param>
        /// <returns></returns>
        public Object UpdateData(Features LOCbject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE Features SET Name='" + LOCbject.Name+"'WHERE ID = " + LOCbject.ID + "";
                    
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
        //delete Data from Features Table

        public Object DeleteData(Features LOCbject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete Features Where ID=" + LOCbject.ID;

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

        public DataTable GetDataForFeatureByname(Features LOCbject)
        {
            DataTable FeaturesDataTable = new DataTable();
            
            
            try
            {
                string SqlQuery = "Select * from Features where Name='" + LOCbject.Name + "'";
                FeaturesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
               
               
            }
            catch
            {
            }
            finally
            {
            }
            return FeaturesDataTable;
        }

    }
}
