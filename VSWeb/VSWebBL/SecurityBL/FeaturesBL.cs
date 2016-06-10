using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
   public class FeaturesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static FeaturesBL _self = new FeaturesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static FeaturesBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.GetAllData();
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
        public Object ValidateUpdate(Features LocObject)
        {
            Object ReturnValue = "";
            try
            {
                if (LocObject.Name == null || LocObject.Name == "")
                {
                    return "ER#Please enter the Feature name";
                }
               
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion

        /// <summary>
        /// Call to Get Data from Features based on Primary key
        /// </summary>
        /// <param name="LocObject">Features object</param>
        /// <returns></returns>
        public Features GetData(Features LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.GetData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       // get data based on Name

        public Features GetDataForFeature(Features LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.GetDataForFeature(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }


        /// <summary>
        /// Call to Insert Data into Features
        ///  </summary>
        /// <param name="LocObject">Features object</param>
        /// <returns></returns>
        public object InsertData(Features LocObject)
        {
			
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.FeaturesDAL.Ins.GetDataForFeatureByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.InsertData(LocObject);
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


        public Object UpdateData(Features LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.FeaturesDAL.Ins.GetDataForFeatureByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.UpdateData(LocObject);
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
        public Object DeleteData(Features LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.FeaturesDAL.Ins.DeleteData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

    }
}
