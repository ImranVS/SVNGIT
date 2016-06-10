using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;


namespace VSWebUI.Security
{
    public partial class AssignNavigator : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    FillUserNameComboBox();
                    Session["DataNotVisible"] = null;
                    //Navigator panel fields and buttons
                    //NavigatorRoundPanel.Enabled = false;

                }

                //Navigator
                if (Session["DataVisible"] == null)
                {
                    DataTable NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.NavigatorVisibleUpdateTree();
                    Session["DataVisible"] = NavigatorTree;
                }

                NavigatorVisibleTreeList.DataSource = (DataTable)Session["DataVisible"];
                NavigatorVisibleTreeList.DataBind();
                NavigatorNotVisibleTreeList.DataSource = (DataTable)Session["DataNotVisible"];
                NavigatorNotVisibleTreeList.DataBind();



            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }           
        }
        private void FillUserNameComboBox()
        {

            DataTable UsersDataTable = VSWebBL.SecurityBL.UsersBL.Ins.GetAllData();
            UserNameComboBox.DataSource = UsersDataTable;
            UserNameComboBox.TextField = "FullName";
            UserNameComboBox.ValueField = "FullName";
            UserNameComboBox.DataBind();
        }
        protected void UserNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selValue;
            selValue = "";

            for (int i = 0; i < UserNameComboBox.Items.Count; i++)
            {
                if (UserNameComboBox.Items[i].Selected)
                {
                    selValue = UserNameComboBox.Items[i].Value.ToString();
                    //NavigatorRoundPanel.Enabled = true;

                    LoadUserRestrictions(selValue);

                }
            }
        }
        private void LoadUserRestrictions(string UserName)
        {
            try
            {
                //Load allowed navigation into tree
                DataTable NavigatorTree = new DataTable();
                NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.NavigatorVisibleUpdateTree();
                NavigatorVisibleTreeList.ClearNodes();
                if (NavigatorTree.Rows.Count > 0)
                {
                    NavigatorVisibleTreeList.DataSource = NavigatorTree;
                    NavigatorVisibleTreeList.DataBind();
                }

                DataTable RestrictedNavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetRestrictedNavigatorByUserID(UserName, "<=2");
                NavigatorNotVisibleTreeList.ClearNodes();
                if (RestrictedNavigatorTree.Rows.Count >= 0)
                {
                    NavigatorNotVisibleTreeList.DataSource = RestrictedNavigatorTree;
                    NavigatorNotVisibleTreeList.KeyFieldName = "ID";
                    //NavigatorNotVisibleTreeList.ParentFieldName = "ParentID";
                    NavigatorNotVisibleTreeList.DataBind();
                    Session["DataNotVisible"] = RestrictedNavigatorTree;
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void NavigatorMoveNotVisibleButton_Click(object sender, EventArgs e)
        {
            NavigatorsRebuildTrees(NavigatorVisibleTreeList, NavigatorNotVisibleTreeList);

        }

        private DataTable NavigatorBuildDataTable(object selTreeList)
        {
            ASPxTreeList tree = (ASPxTreeList)selTreeList;
            DataTable dt = new DataTable();
            string[] fieldValues = new string[5];
            List<string> fieldValuesID = new List<string>();
            List<string> fieldValuesDisplayText = new List<string>();
            List<string> fieldValuesOrderNum = new List<string>();
            List<string> fieldValuesParentID = new List<string>();
            List<string> fieldValuesPageLink = new List<string>();
            dt.Columns.Add("ID");
            dt.Columns.Add("DisplayText");
            dt.Columns.Add("OrderNum");
            dt.Columns.Add("ParentID");
            dt.Columns.Add("PageLink");
            IEnumerable<TreeListNode> list = tree.GetAllNodes();
            foreach (TreeListNode node in list)
            {
                fieldValues[0] = node.GetValue("ID").ToString();
                fieldValues[1] = node.GetValue("DisplayText").ToString();
                fieldValues[2] = node.GetValue("OrderNum").ToString();
                fieldValues[3] = node.GetValue("ParentID").ToString();
                fieldValues[4] = node.GetValue("PageLink").ToString();
                dt.Rows.Add(fieldValues);
            }
            return dt;
        }

        protected void NavigatorMoveVisibleButton_Click(object sender, EventArgs e)
        {
            NavigatorsUpdateRemoveTrees(NavigatorNotVisibleTreeList, NavigatorVisibleTreeList);
        }
        private void NavigatorsRebuildTrees(object fromTreeVal, object toTreeVal)
        {
            ASPxTreeList fromTree = (ASPxTreeList)fromTreeVal;
            ASPxTreeList toTree = (ASPxTreeList)toTreeVal;
            DataTable dataTo = new DataTable();
            string[] fieldValues = new string[5];
            List<string> fieldValuesID = new List<string>();
            List<string> fieldValuesDisplayText = new List<string>();
            List<string> fieldValuesOrderNum = new List<string>();
            List<string> fieldValuesParentID = new List<string>();
            List<string> fieldValuesPageLink = new List<string>();
            List<TreeListNode> selectItemsNavID = fromTree.GetSelectedNodes();



            foreach (TreeListNode selectItemNavID in selectItemsNavID)
            {
                //Check if a selected node has a parent
                if (selectItemNavID.ParentNode != null && selectItemNavID.ParentNode.Key != "")
                {
                    //If the parent node is not already in the list, add it to the list
                    //if (fieldValuesID != null)
                    //{
                    //    if (!fieldValuesID.Contains(selectItemNavID.ParentNode.GetValue("ID").ToString()))
                    //    {
                    //        fieldValuesID.Add(selectItemNavID.ParentNode.GetValue("ID").ToString());
                    //        fieldValuesDisplayText.Add(selectItemNavID.ParentNode.GetValue("DisplayText").ToString());
                    //        fieldValuesOrderNum.Add(selectItemNavID.ParentNode.GetValue("OrderNum").ToString());
                    //        fieldValuesParentID.Add(selectItemNavID.ParentNode.GetValue("ParentID").ToString());
                    //        fieldValuesPageLink.Add(selectItemNavID.ParentNode.GetValue("PageLink").ToString());
                    //    }
                    //}
                }
                //If selected node is not already in the list, add it to the list
                if (fieldValuesID != null)
                {
                    if (!fieldValuesID.Contains(selectItemNavID.GetValue("ID").ToString()))
                    {
                        fieldValuesID.Add(selectItemNavID.GetValue("ID").ToString());
                        fieldValuesDisplayText.Add(selectItemNavID.GetValue("DisplayText").ToString());
                        fieldValuesOrderNum.Add(selectItemNavID.GetValue("OrderNum").ToString());
                        fieldValuesParentID.Add(selectItemNavID.GetValue("ParentID").ToString());
                        fieldValuesPageLink.Add(selectItemNavID.GetValue("PageLink").ToString());
                    }
                }
            }
            //Build data table based on the existing nodes in the tree
            dataTo = NavigatorBuildDataTable(toTree);
            dataTo.PrimaryKey = new DataColumn[] { dataTo.Columns["ID"] };
            //Add new values to the data table, then re-assign the data table to the navigator
            for (int i = 0; i < fieldValuesID.Count; i++)
            {
                fieldValues[0] = fieldValuesID[i];
                fieldValues[1] = fieldValuesDisplayText[i];
                fieldValues[2] = fieldValuesOrderNum[i]; ;
                fieldValues[3] = fieldValuesParentID[i];
                fieldValues[4] = fieldValuesPageLink[i];
                if (!dataTo.Rows.Contains(fieldValues[0]))
                {
                    dataTo.Rows.Add(fieldValues);
                }
            }
            Session["DataNotVisible"] = dataTo;
            /* 
            IEnumerable<TreeListNode> list = fromTree.GetAllNodes();
            foreach (TreeListNode node in list)
            {
                int ind = fieldValuesID.IndexOf(node.GetValue("ID").ToString());
                if (ind == -1)
                {
                    fieldValues[0] = node.GetValue("ID").ToString();
                    fieldValues[1] = node.GetValue("DisplayText").ToString();
                    fieldValues[2] = node.GetValue("OrderNum").ToString();
                    fieldValues[3] = node.GetValue("ParentID").ToString();
                    fieldValues[4] = node.GetValue("PageLink").ToString();
                    dataFrom.Rows.Add(fieldValues);
                }
            }
            fromTree.DataSource = dataFrom;
            fromTree.KeyFieldName = "ID";
            fromTree.ParentFieldName = "ParentID";
            fromTree.DataBind();
             */
            //fromTree.UnselectAll();
            NavigatorVisibleTreeList.UnselectAll();
            toTree.DataSource = dataTo;
            toTree.KeyFieldName = "ID";
            toTree.ParentFieldName = "ParentID";
            toTree.DataBind();
        }

        private void NavigatorsUpdateRemoveTrees(object fromTreeVal, object toTreeVal)
        {
            ASPxTreeList fromTree = (ASPxTreeList)fromTreeVal;
            ASPxTreeList toTree = (ASPxTreeList)toTreeVal;
            DataTable dataFrom = new DataTable();
            string[] fieldValues = new string[5];
            List<string> fieldValuesID = new List<string>();
            List<string> fieldValuesDisplayText = new List<string>();
            List<string> fieldValuesOrderNum = new List<string>();
            List<string> fieldValuesParentID = new List<string>();
            List<string> fieldValuesPageLink = new List<string>();
            List<TreeListNode> selectItemsNavID = fromTree.GetSelectedNodes();


            foreach (TreeListNode selectItemNavID in selectItemsNavID)
            {
                //If selected node is not already in the list, add it to the list
                if (fieldValuesID != null)
                {
                    if (!fieldValuesID.Contains(selectItemNavID.GetValue("ID").ToString()))
                    {
                        fieldValuesID.Add(selectItemNavID.GetValue("ID").ToString());
                        fieldValuesDisplayText.Add(selectItemNavID.GetValue("DisplayText").ToString());
                        fieldValuesOrderNum.Add(selectItemNavID.GetValue("OrderNum").ToString());
                        fieldValuesParentID.Add(selectItemNavID.GetValue("ParentID").ToString());
                        fieldValuesPageLink.Add(selectItemNavID.GetValue("PageLink").ToString());
                    }
                }
            }
            //Build data table based on the existing nodes in the tree
            dataFrom = NavigatorBuildDataTable(fromTree);
            dataFrom.PrimaryKey = new DataColumn[] { dataFrom.Columns["ID"] };
            DataRow dr;
            //Remove values from the data table, then re-assign the data table to the navigator
            for (int i = 0; i < fieldValuesID.Count; i++)
            {
                fieldValues[0] = fieldValuesID[i];
                fieldValues[1] = fieldValuesDisplayText[i];
                fieldValues[2] = fieldValuesOrderNum[i]; ;
                fieldValues[3] = fieldValuesParentID[i];
                fieldValues[4] = fieldValuesPageLink[i];
                if (dataFrom.Rows.Contains(fieldValues[0]))
                {
                    if (fieldValues[3] != "")
                    {
                        DataRow[] foundRows = dataFrom.Select("ParentID = '" + fieldValues[3].ToString() + "'", "ParentID ASC", DataViewRowState.Added);
                        if (foundRows.Length == 1)
                        {
                            //DataTable dtmenus = foundRows.CopyToDataTable();
                            //foreach (DataRow row in dtmenus.Rows)
                            //{
                            //    dr = dataFrom.Rows.Find(row[0].ToString());
                            //    dataFrom.Rows.Remove(dr);
                            //}
                            dr = dataFrom.Rows.Find(fieldValues[3]);
                            if (dr != null)
                            {
                                dataFrom.Rows.Remove(dr);
                            }
                        }
                    }
                    dr = dataFrom.Rows.Find(fieldValues[0]);
                    dataFrom.Rows.Remove(dr);
                }
            }
            Session["DataNotVisible"] = dataFrom;
            /* 
            IEnumerable<TreeListNode> list = fromTree.GetAllNodes();
            foreach (TreeListNode node in list)
            {
                int ind = fieldValuesID.IndexOf(node.GetValue("ID").ToString());
                if (ind == -1)
                {
                    fieldValues[0] = node.GetValue("ID").ToString();
                    fieldValues[1] = node.GetValue("DisplayText").ToString();
                    fieldValues[2] = node.GetValue("OrderNum").ToString();
                    fieldValues[3] = node.GetValue("ParentID").ToString();
                    fieldValues[4] = node.GetValue("PageLink").ToString();
                    dataFrom.Rows.Add(fieldValues);
                }
            }
            fromTree.DataSource = dataFrom;
            fromTree.KeyFieldName = "ID";
            fromTree.ParentFieldName = "ParentID";
            fromTree.DataBind();
             */
            fromTree.UnselectAll();

            fromTree.DataSource = dataFrom;
            fromTree.KeyFieldName = "ID";
            fromTree.ParentFieldName = "ParentID";
            fromTree.DataBind();
        }



        void SetNodeSelectionSettings()
        {
            TreeListNodeIterator iterator = NavigatorVisibleTreeList.CreateNodeIterator();
            TreeListNode node;
            while (true)
            {
                node = iterator.GetNext();
                if (node == null) break;

                node.AllowSelect = !node.HasChildren;

            }
        }

        protected void NavigatorVisibleTreeList_DataBound(object sender, EventArgs e)
        {
            SetNodeSelectionSettings();
        }


        protected void ResetServerAccessButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    FillUserNameComboBox();
                    UserNameComboBox.Text = "";

                    NavigatorNotVisibleTreeList.ClearNodes();
                    NavigatorVisibleTreeList.UnselectAll();


                }
            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void AssignServerAccessButton_Click(object sender, EventArgs e)
        {

            // AssignServerTypeRestrictions(SpecificServersNotVisibleGridView, SpecificServersVisibleGridView, ServerTypeNotVisibleListBox,UserNameComboBox);
            string Uname = UserNameComboBox.Text;


            if (NavigatorVisibleTreeList.SelectionCount > 0 || NavigatorNotVisibleTreeList.SelectionCount > 0)
            {

                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "Please select Move button and then Assign button to save.";
            }
            else
            {
                bool returnmenu;

                returnmenu = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Menus(Uname, GetRestrictedMenu());

                if (returnmenu == true)
                {

                    NavigatorPopupControl.ShowOnPageLoad = true;
                    MsgLabel.Text = "Menu updated successfully.";
                }
                else
                {
                    NavigatorPopupControl.ShowOnPageLoad = true;
                    MsgLabel.Text = "Menu not updated.";
                }


            }

            //bool returnmenu;

            //returnmenu = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Menus(Uname, GetRestrictedMenu());


            //if (returnmenu == true)
            //{


            //    NavigatorPopupControl.ShowOnPageLoad = true;
            //    MsgLabel.Text = "Menu updated successfully.";
            //}
            //else
            //{
            //    
            //    MsgLabel.Text = "Menu not updated.";
            //}

            if (UserNameComboBox.Text == Session["UserFullName"].ToString())
            {
                Session["GroupIndex"] = null;
                Session["ItemIndex"] = null;
                Session["Submenu"] = null;
            }
        }

        private string GetRestrictedMenu()
        {
            string selValues = "";
            TreeListNodeIterator iterator = NavigatorNotVisibleTreeList.CreateNodeIterator();
            TreeListNode node;
            while (true)
            {
                node = iterator.GetNext();
                if (node == null) break;
                if (selValues == "")
                {
                    selValues = "" + node.Key + "";
                }
                else
                {
                    selValues += "," + "" + node.Key + "";
                }

            }

            return selValues;
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }
		protected void CancelButton_Click(object sender, EventArgs e)
		{

			Response.Redirect("~/Configurator/Default.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}

        //5/2/2016 sowmya Added for VSPLUS 2573
        protected void CollapseButton_Click(object sender, EventArgs e)
        {

            ToggleButton();
        }
        public void ToggleButton()
        {

            try
            {

                if (CollapseButton.Text == "Collapse All Rows")
                {
                    NavigatorVisibleTreeList.CollapseAll();
                    CollapseButton.Image.Url = "~/images/icons/add.png";
                    CollapseButton.Text = "Expand All Rows";
                }
                else
                {
                    NavigatorVisibleTreeList.ExpandAll();
                    CollapseButton.Image.Url = "~/images/icons/forbidden.png";
                    CollapseButton.Text = "Collapse All Rows";

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

    }
}