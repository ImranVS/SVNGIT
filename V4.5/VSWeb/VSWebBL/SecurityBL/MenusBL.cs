using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.SecurityBL
{
    public class MenusBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static MenusBL _self = new MenusBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static MenusBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        /// <summary>
        /// Call to Insert Data into Locations
        ///  </summary>
        /// <param name="ServerObject">Locations object</param>
        /// <returns></returns>
        public object InsertData(Menus objMenu)
        {
			try
			{
				DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetDataByName(objMenu);
				if (dt.Rows.Count == 0)
				{

					return VSWebDAL.SecurityDAL.MenusDAL.Ins.InsertData(objMenu);

				}
				else return "Menu Already Exists.";
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
        public Object UpdateData(Menus objMenu)
        {
			try
			{
				DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.MenuExists(objMenu);
				if (dt.Rows.Count > 0)
				{
					return VSWebDAL.SecurityDAL.MenusDAL.Ins.UpdateData(objMenu);
				}
				else return "Menu Already Exists.";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="ServersObject"></param>
        /// <returns></returns>
        public Object DeleteData(Menus objMenu)
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.DeleteData(objMenu);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetParentMenu()
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetParentMenu();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public DataTable GetFeatures()
		{
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetFeatures();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}

        //public DataTable GetCommonFeatures()
        //{
        //    return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetCommonFeatures();
        //}
        public DataTable GetSelectedFeatures()
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetSelectedFeatures();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable MenuTree(string MenuArea)
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.MenuTree(MenuArea);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
        }
        public DataTable SelectedMenuTree(string FeatureName, string MenuArea)
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.SelectedMenuTree(FeatureName, MenuArea);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public bool InsertFeatureMenus(string FeatureName, DataTable MenuDt)
        {
			try
			{
				  return VSWebDAL.SecurityDAL.MenusDAL.Ins.InsertFeatureMenus(FeatureName, MenuDt);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        
        }
        public bool DeleteFeatureMenus(string FeatureName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.DeleteFeatureMenus(FeatureName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public bool InsertFeatures(DataTable MenuDt)
        {
			try
			{
				return VSWebDAL.SecurityDAL.MenusDAL.Ins.InsertFeatures(MenuDt);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        // 3/15/2016 Durga Addded for VSPLUS-2717
        public DataTable GetCommonFeature()

        {
            try
            {
                return VSWebDAL.SecurityDAL.MenusDAL.Ins.GetCommonFeature();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
