using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using VSWebDO;
using System.Configuration;

namespace VSWebDAL.SecurityDAL
{
    public class MenusDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MenusDAL _self = new MenusDAL();

        public static MenusDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from MenusDAL
        /// </summary>


        public DataTable GetAllData()
        {

            DataTable MenusDataTable = new DataTable();
           try
            {
                //string SqlQuery = "  select m1.ID, m1.DisplayText,m1.OrderNum,'' Parentmenu,m1.PageLink,m1.Level,m1.RefName,m1.ImageURL,m1.MenuArea  from MenuItems m1 where m1.parentid is null union select m1.ID, m1.DisplayText,m1.OrderNum,m2.DisplayText Parentmenu,m1.PageLink,m1.Level,m1.RefName,m1.ImageURL,m1.MenuArea from MenuItems m1,MenuItems m2 where m1.parentid=m2.id  ";
                string SqlQuery = "  select m1.ID, m1.DisplayText,m1.OrderNum,'' Parentmenu,m1.PageLink,m1.Level,m1.RefName,m1.ImageURL,m1.MenuArea  from MenuItems m1 where m1.parentid is null union select m1.ID, m1.DisplayText,m1.OrderNum,m2.DisplayText +' | ' + m2.MenuArea as Parentmenu,m1.PageLink,m1.Level,m1.RefName,m1.ImageURL,m1.MenuArea from MenuItems m1,MenuItems m2 where m1.parentid=m2.id  ";
                MenusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return MenusDataTable;
        }

        public DataTable GetParentMenu()
        {

            DataTable MenusDataTable = new DataTable();
            try
            {
                string SqlQuery = " select ID,DisplayText +' | ' + MenuArea as Parentmenu from MenuItems ";
                MenusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return MenusDataTable;
        }

        /// <summary>
        /// Insert data into Locations table
        /// </summary>
        /// <param name="DSObject">Locations object</param>
        /// <returns></returns>

        public bool InsertData(Menus objMenu)
        {
            bool Insert = false;
            try
            {
                string ParentID = "";
                DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetParentMenuId(objMenu);
                if (dt.Rows.Count == 0)
                {
                    ParentID = "NULL";
                }
                else
                {
                    ParentID = dt.Rows[0]["ID"].ToString();
                }
                string SqlQuery = "INSERT INTO [MenuItems] ([DisplayText],[OrderNum],[ParentID],[PageLink],[Level],[RefName],[ImageURL],[MenuArea]) " +
                       "VALUES('" + objMenu.DisplayText + "', " + objMenu.OrderNum + "," + ParentID + ",'" + objMenu.PageLink +
                "'," + objMenu.Level + ",'" + objMenu.RefName + "','" + objMenu.ImageURL + "','" + objMenu.MenuArea + "')";
              
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
        /// Update data into Locations table
        /// </summary>
        /// <param name="ServerObject">Locations object</param>
        /// <returns></returns>
        public Object UpdateData(Menus objMenu)
        {
            Object Update;
            try
            {
                string ParentID = "";
                DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetParentMenuId(objMenu);
                if (dt.Rows.Count == 0)
                {
                    ParentID = "NULL";
                }
                else
                {
                    ParentID = dt.Rows[0]["ID"].ToString();
                }
                string SqlQuery = "UPDATE MenuItems SET [DisplayText]='" + objMenu.DisplayText + "',[OrderNum]=" + objMenu.OrderNum + ",[ParentID]=" + ParentID + ",[PageLink]='" +
                    objMenu.PageLink + "',[Level]=" + objMenu.Level + ",[RefName]='" + objMenu.RefName + "',[ImageURL]='" + objMenu.ImageURL + "',[MenuArea]='" + objMenu.MenuArea + 
                    "' where ID=" + objMenu.ID ;
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
        //delete Data from Locations Table

        public Object DeleteData(Menus objMenu)
        {
            Object Update;
            try
            {

                string SqlQuery = "DELETE FROM [MenuItems] WHERE [ID]=" + objMenu.ID + "";

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

        public DataTable GetDataByName(Menus objMenu)
        {

            DataTable MenusDataTable = new DataTable();
            try
            {
                if (objMenu.ID == 0)
                {

                    string SqlQuery = "SELECT * from dbo.MenuItems  where DisplayText='" + objMenu.DisplayText + "' " +
                            " and ID='" + objMenu.ID + "' ";

                    MenusDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {

                    string SqlQuery = "SELECT * from MenuItems where DisplayText='" + objMenu.DisplayText + "'  and ID<>'" + objMenu.ID + "'";
                    MenusDataTable = objAdaptor.FetchData(SqlQuery);


                }
            }
            catch
            {
            }
            finally
            {
            }
            return MenusDataTable;
        }

        public DataTable MenuExists(Menus objMenu)
        {

            DataTable MenusDataTable = new DataTable();
            try
            {
                

                    string SqlQuery = "SELECT * from MenuItems where  ID=" + objMenu.ID ;
                    MenusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return MenusDataTable;
        }
        public DataTable GetParentMenuId(Menus objMenu)
        {

            DataTable MenusDataTable = new DataTable();
            try
            {


                string SqlQuery = "SELECT * from MenuItems where  DisplayText +' | ' + MenuArea='" + objMenu.ParentMenu + "'";
                MenusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return MenusDataTable;
        }
		public DataTable GetFeatures()
		{

			DataTable FeaturesDataTable = new DataTable();
			try
			{

                // 3/15/2016 Durga Addded for VSPLUS-2717
                string SqlQuery = "SELECT * from Features  where ID!=11 order by Name";
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

        //public DataTable GetCommonFeatures()
        //{

        //    DataTable FeaturesDataTable = new DataTable();
        //    try
        //    {


        //        string SqlQuery = "SELECT * from Features where Name!='Common Features'";
        //        FeaturesDataTable = objAdaptor.FetchData(SqlQuery);

        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return FeaturesDataTable;
        //}

        public DataTable GetSelectedFeatures()
        {

            DataTable FeaturesDataTable = new DataTable();
            try
            {


               string SqlQuery = "SELECT f.Name from Features f, SelectedFeatures sf where sf.FeatureID=f.ID";
                //string SqlQuery = "SELECT f.Name from Features f";
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
       
        public DataTable MenuTree(string MenuArea)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, DisplayText, OrderNum, ParentID, PageLink FROM Menuitems t1 where [Level]<3 and MenuArea='" + MenuArea + "'";
                SqlQuery += "ORDER BY Level,OrderNum";
                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return NavigatorDataTable;
        }

        public DataTable SelectedMenuTree(string FeatureName,string MenuArea)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, DisplayText, OrderNum, ParentID, PageLink FROM Menuitems t1 ,FeatureMenus t2,Features t3 where [Level]<3  and t1.ID=t2.MenuID and t2.FeatureID=t3.ID and t3.Name='"+FeatureName+"' and t1.MenuArea='" + MenuArea + "'";
                 NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return NavigatorDataTable;
        }

        public DataTable GetFeatureID(string FeatureName)
        {

            DataTable FeaturesDataTable = new DataTable();
            try
            {


                string SqlQuery = "SELECT * from Features where Name='" + FeatureName + "'";
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
        // 3/15/2016 Durga Addded for VSPLUS-2717
        public DataTable GetCommonFeature()
        {

            DataTable FeaturesDataTable = new DataTable();
            try
            {


                string SqlQuery = "SELECT * from Features where ID=11";
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
        public bool DeleteFeatureMenus(string FeatureName)
        {
            bool Insert = false;
            try
            {
                string FeatureID = "";
                DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetFeatureID(FeatureName);
                if (dt.Rows.Count > 0)
                {
                    FeatureID = dt.Rows[0]["ID"].ToString();
                    string SqlQuery = "Delete [FeatureMenus] where [FeatureID]=" + FeatureID;

                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                }
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

        public bool InsertFeatureMenus(string FeatureName, DataTable MenuDt)
        {
            bool Insert = false;
            string SqlQuery = "";
            try
            {
                string FeatureID = "";
                DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetFeatureID(FeatureName);
                if (dt.Rows.Count > 0)
                {
                    FeatureID = dt.Rows[0]["ID"].ToString();

                    for (int i = 0; i < MenuDt.Rows.Count; i++)
                    {
                         SqlQuery = "INSERT INTO [FeatureMenus] ([FeatureID],[MenuID]) " +
                               "VALUES(" + FeatureID + "," + MenuDt.Rows[i]["ID"].ToString() + ")";

                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                }
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

        public bool InsertFeatures(DataTable MenuDt)
        {
            bool Insert = false;
            string SqlQuery = "";
            try
            {
                 SqlQuery = "Delete [SelectedFeatures]";

                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                for (int i = 0; i < MenuDt.Rows.Count; i++)
                {
                    DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetFeatureID(MenuDt.Rows[i]["Name"].ToString());
                    string FeatureID = dt.Rows[0]["ID"].ToString();
                    SqlQuery = "INSERT INTO [SelectedFeatures] ([FeatureID]) " +
                          "VALUES(" + FeatureID + ")";

					Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                }

			
                    //SqlQuery = "INSERT INTO [SelectedFeatures] ([FeatureID]) " +
                    //  "VALUES(11)";

                    //Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				
   
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


    
    }
}
