using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
   public class LocationsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static LocationsBL _self = new LocationsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static LocationsBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(Locations LocObject)
        {
            Object ReturnValue = "";
            try
            {
                if (LocObject.Location == null || LocObject.Location == "")
                {
                    return "ER#Please enter the Location name";
                }
               
            }
            catch (Exception ex )
            { throw ex ; }
            finally
            { }
            return "";
        }

        #endregion

        /// <summary>
        /// Call to Get Data from Locations based on Primary key
        /// </summary>
        /// <param name="LocObject">Locations object</param>
        /// <returns></returns>
        public Locations GetData(Locations LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       // get data based on location

        public Locations GetDataForLocation(Locations LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetDataForLocation(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }


        /// <summary>
        /// Call to Insert Data into Locations
        ///  </summary>
        /// <param name="LocObject">Locations object</param>
        /// <returns></returns>
        public object InsertData(Locations LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetDataForLocationByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.LocationsDAL.Ins.InsertData(LocObject);
					}
					else return ReturnValue;
				}
				else return "";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
            
            
        }
        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="LocObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(Locations LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetDataForLocationByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.LocationsDAL.Ins.UpdateData(LocObject);
					}
					else return ReturnValue;
				}
				else return "";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public string DeleteData(Locations LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.DeleteData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

		public DataTable GetAllCountries()
		{
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetAllCountries();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetStatesFromCountry(string Country)
		{
			try
			{
				return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetStatesFromCountry(Country);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		////public DataTable GetCitiesFromStateAndCountry(string State, string Country)
		////{
		////    try
		////    {
		////        return VSWebDAL.SecurityDAL.LocationsDAL.Ins.GetCitiesFromStateAndCountry(State, Country);
		////    }
		////    catch (Exception ex)
		////    {
				
		////        throw ex;
		////    }
			
		////}

    }
}
 