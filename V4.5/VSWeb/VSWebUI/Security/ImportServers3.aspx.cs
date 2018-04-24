using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class ImportServers3 : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        DominoServerTasks DSTaskObjectRet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ImportedServers"];
                SrvCheckBoxList.DataSource = dt;
                SrvCheckBoxList.TextField = "ServerName";
                SrvCheckBoxList.ValueField = "ServerName";
                SrvCheckBoxList.DataBind();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SrvLabel.Text += dt.Rows[i]["ServerName"].ToString() + ", ";
                }
                SrvLabel.Text = SrvLabel.Text.Substring(0, SrvLabel.Text.Length - 2);

                DataTable ServerTasksDT = new DataTable();
                ServerTasksDT = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetAllData();
                if (ServerTasksDT.Rows.Count > 0)
                {
                    SrvTaskCheckBoxList.DataSource = ServerTasksDT;
                    SrvTaskCheckBoxList.TextField = "TaskName";
                    SrvTaskCheckBoxList.ValueField = "TaskName";
                    SrvTaskCheckBoxList.DataBind();
                    SrvTaskIDCheckBoxList.DataSource = ServerTasksDT;
                    SrvTaskIDCheckBoxList.TextField = "TaskID";
                    SrvTaskIDCheckBoxList.ValueField = "TaskID";
                    SrvTaskIDCheckBoxList.DataBind();
                }
            }
        }

        protected void SelectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.SelectAll();
        }

        protected void DeselectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.UnselectAll();
        }

        protected void AssignButton_Click(object sender, EventArgs e)
        {
            //try
            //{
                Object ReturnValue;
                Servers ServersObject;
                DominoServerTasks DSTaskObject;
                ServerTaskSettings STSettings;
                DataTable dt;
                DataTable dt2;
                DataTable dtsrv = new DataTable();
                DataTable dtsrvtask = new DataTable();
                string serverid = "";
                string taskid = "";
                if (SrvTaskCheckBoxList.SelectedItems.Count > 0)
                {
                    dtsrvtask.Columns.Add("TaskID");
                    dtsrvtask.Columns.Add("TaskName");
                    dtsrvtask.Columns.Add("ServerID");
                    dtsrvtask.Columns.Add("Enabled");
                    dtsrvtask.Columns.Add("SendLoadCommand");
                    dtsrvtask.Columns.Add("SendRestartCommand");
                    dtsrvtask.Columns.Add("SendExitCommand");
                    dtsrvtask.Columns.Add("RestartOffHours");
                    dtsrv.Columns.Add("ID");
                    dtsrv.Columns.Add("ServerName");
                    dtsrv.Columns.Add("IPAddress");
                    dtsrv.Columns.Add("Description");
                    dtsrv.Columns.Add("ServerType");
                    dtsrv.Columns.Add("Location");
                    dtsrv.Columns.Add("LocationID");
                    for (int j = 0; j < SrvTaskCheckBoxList.Items.Count; j++)
                    {
                        if (SrvTaskCheckBoxList.Items[j].Selected)
                        {
                            SrvCheckBoxList.SelectAll();
                            if (SrvCheckBoxList.SelectedItems.Count > 0)
                            {
                                for (int i = 0; i < SrvCheckBoxList.SelectedItems.Count; i++)
                                {
                                    DataRow drtask = dtsrvtask.NewRow();
                                    drtask["TaskID"] = SrvTaskIDCheckBoxList.Items[j].Text;
                                    drtask["TaskName"] = SrvTaskCheckBoxList.Items[j].Text;
                                    drtask["ServerID"] = "";
                                    drtask["Enabled"] = "true";
                                    drtask["SendLoadCommand"] = "false";
                                    drtask["SendRestartCommand"] = "false";
                                    drtask["SendExitCommand"] = "false";
                                    drtask["RestartOffHours"] = "false";
                                    DataRow dr = dtsrv.NewRow();
                                    dr["ID"] = "";
                                    dr["ServerName"] = SrvCheckBoxList.SelectedItems[i].ToString();
                                    dr["IPAddress"] = "";
                                    dr["Description"] = "Production";
                                    dr["ServerType"] = "Domino";
                                    dr["Location"] = "";
                                    dr["LocationID"] = 0;
                                    DSTaskObject = new DominoServerTasks();
                                    DSTaskObject.TaskName = SrvTaskCheckBoxList.Items[j].Text;
                                    try
                                    {
                                        DSTaskObjectRet = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTaskObject);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    ServersObject = CollectDataForServers("Insert", dr);
                                    try
                                    {
                                        dt = VSWebBL.SecurityBL.ServersBL.Ins.GetDataByName(ServersObject);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        drtask["ServerID"] = dt.Rows[0]["ID"].ToString();
                                        serverid = dt.Rows[0]["ID"].ToString();
                                        taskid = SrvTaskIDCheckBoxList.Items[j].Text;
                                        try
                                        {

                                            dt2 = VSWebBL.ConfiguratorBL.ServerTaskSettingsBL.Ins.SelectData(serverid, taskid);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                        if (dt2.Rows.Count == 0)
                                        {
                                            STSettings = CollectDataForServerTaskSettings("Insert", drtask);
                                            try
                                            {
                                                ReturnValue = VSWebBL.ConfiguratorBL.ServerTaskSettingsBL.Ins.InsertData(STSettings);
                                            }
                                            catch (Exception ex)
                                            {
                                                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            //10/3/2013 NS moved the code below outside of the task selection check to allow to proceed to the next page without any selection
                SrvTaskCheckBoxList.UnselectAll();
                SrvCheckBoxList.UnselectAll();
                Response.Redirect("~/Security/ImportServers4.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private Servers CollectDataForServers(string Mode, DataRow ServersRow)
        {
            try
            {
                Servers ServersObject = new Servers();
                if (Mode == "Update")
                {
                    ServersObject.ID = int.Parse(ServersRow["ID"].ToString());
                }
                ServersObject.ServerName = ServersRow["ServerName"].ToString();
                ServersObject.IPAddress = ServersRow["IPAddress"].ToString();
                ServersObject.Description = ServersRow["Description"].ToString();
                ServerTypes STypeobject = new ServerTypes();
                STypeobject.ServerType = ServersRow["ServerType"].ToString();
                ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
                ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
                ServersObject.ServerTypeID = ReturnValue.ID;

                Locations LOCobject = new Locations();
                LOCobject.Location = ServersRow["Location"].ToString();

                Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
                ServersObject.LocationID = ReturnLocValue.ID;

                return ServersObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; }
            finally { }
        }

        private ServerTaskSettings CollectDataForServerTaskSettings(string Mode, DataRow STSettingsRow)
        {
            try
            {
                ServerTaskSettings STSettingsObject = new ServerTaskSettings();
                STSettingsObject.Enabled = Convert.ToBoolean(STSettingsRow["Enabled"]);
                DominoServerTasks DSTasksObject = new DominoServerTasks();

                DSTasksObject.TaskName = STSettingsRow["TaskName"].ToString();
                DominoServerTasks ReturnValue;
                try
                {
                    ReturnValue = VSWebBL.ConfiguratorBL.DominoServerTasksBL.Ins.GetDataForTaskName(DSTasksObject);
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                STSettingsObject.TaskID = ReturnValue.TaskID;

                STSettingsObject.ServerID = int.Parse(STSettingsRow["ServerID"].ToString());
                STSettingsObject.SendLoadCommand = Convert.ToBoolean(STSettingsRow["SendLoadCommand"]);
                STSettingsObject.SendRestartCommand = Convert.ToBoolean(STSettingsRow["SendRestartCommand"]);
                STSettingsObject.RestartOffHours = Convert.ToBoolean(STSettingsRow["RestartOffHours"]);
                STSettingsObject.SendExitCommand = Convert.ToBoolean(STSettingsRow["SendExitCommand"]);
                STSettingsObject.Modified_By = Convert.ToInt32(Session["UserId"].ToString());
                STSettingsObject.Modified_On = DateTime.Now.ToString();

                if (Mode == "Update")
                {

                } 

                return STSettingsObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex); 
                throw ex;
            }
            finally { }
        }

        protected void SelectAllTasksButton_Click(object sender, EventArgs e)
        {
            SrvTaskCheckBoxList.SelectAll();
        }

        protected void DeselectAllTasksButton_Click(object sender, EventArgs e)
        {
            SrvTaskCheckBoxList.UnselectAll();
        }
    }
}