using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;
using VSWebDO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
namespace VSWebUI.Configurator
{
    public partial class NetworkLatencyTest : System.Web.UI.Page
    {
        bool isValid = true;
        int id;
        int yellowthershold;
        int latency;
        bool checkedvalue;
        protected DataTable nwDataTable = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //10/9/2013 NS modified in order to avoid an exception when Cluster grid is empty
            if (!IsPostBack)
            {

                Session["Submenu"] = "";
                FillServerTypeComboBox();
                //FillExchangeServerGrid(ServerTypeComboBox.SelectedItem.Text);
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MSServersGrid|MSServerGridView")
                        {
                            NetworkLatencyTestgrd.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillNwLtncyServerGridfromSession();

            }
            if (groupname.Text != null)
            {
                if (groupname.Text.ToString() != "")
                {
                    successDiv.InnerHtml = "Latency information for <b>" + groupname.Text.ToString() +
                        "</b> updated successfully.";
                    successDiv.Style.Value = "display: block";
                    groupname.Text = "";
                }
            }
        }



        private void FillServerTypeComboBox()
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetSpecificServerTypes();
            //DataTable ServerDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorChildsByRefName("ServersDevices");
            ServerTypeComboBox.DataSource = ServerDataTable;
            ServerTypeComboBox.TextField = "ServerType";
            ServerTypeComboBox.ValueField = "ServerType";
            ServerTypeComboBox.DataBind();
        }
        private void FillNwlatencyServerGrid(string servertype)
        {

            try
            {
                DataTable networklatencyDataTable = new DataTable();
                NetworkLatency NetworkLatencyObject = new NetworkLatency();
                networklatencyDataTable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.Getvalue();

                if ((scantext.Text == null) || (scantext.Text == ""))
                {
                    scantext.Text = "8";
                }
                else
                {
                    scantext.Text = networklatencyDataTable.Rows[0]["ScanInterval"].ToString();
                }
                if ((testduration.Text == null) || (testduration.Text == ""))
                {
                    testduration.Text = "5";
                }
                else
                {
                    testduration.Text = networklatencyDataTable.Rows[0]["TestDuration"].ToString();
                }




                groupname.Text = networklatencyDataTable.Rows[0]["TestDuration"].ToString();



                nwDataTable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetAllData1(ServerTypeComboBox.SelectedItem.Text);
               // for(int i=0;i>d)
                //{
                    //ID
                    //insert in to network table
                //}
                DataColumn[] columns = new DataColumn[1];
                columns[0] = nwDataTable.Columns["ServerID"];
                nwDataTable.PrimaryKey = columns;
                Session["NetworkLatency"] = nwDataTable;
                NetworkLatencyTestgrd.DataSource = nwDataTable;
                NetworkLatencyTestgrd.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillNwLtncyServerGridfromSession()
        {
            try
            {
                DataTable nwlatencyDataTable = new DataTable();
                if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
                    nwlatencyDataTable = (DataTable)Session["NetworkLatency"];//VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetAllData();
                if (nwlatencyDataTable.Rows.Count > 0)
                {
                    GridViewDataColumn column5 = NetworkLatencyTestgrd.Columns["LatencyYellowThreshold"] as GridViewDataColumn;
                    GridViewDataColumn column6 = NetworkLatencyTestgrd.Columns["LatencyRedThreshold"] as GridViewDataColumn;
                    int startIndex = NetworkLatencyTestgrd.PageIndex * NetworkLatencyTestgrd.SettingsPager.PageSize;
                    int endIndex = Math.Min(NetworkLatencyTestgrd.VisibleRowCount, startIndex + NetworkLatencyTestgrd.SettingsPager.PageSize);
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        ASPxTextBox txtValue = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column5, "txtyellowthreshValue");
                        ASPxTextBox txtValue2 = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column6, "txtredthreshValue");
                        nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = txtValue.Text;
                        nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = txtValue2.Text;
                        if (NetworkLatencyTestgrd.Selection.IsRowSelected(i))
                        {
                            checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "true");
                        }
                        else
                        {
                            nwlatencyDataTable.Rows[i]["Enabled"] = "false";
                            checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "false");
                        }
                        id = Convert.ToInt32(NetworkLatencyTestgrd.GetRowValues(i, "ID"));
                        yellowthershold = Convert.ToInt32(txtValue.Text);
                        latency = Convert.ToInt32(txtValue2.Text);
                        object dt = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.updateEnableLatencyTest(id, yellowthershold, latency, checkedvalue);
                    }
                }
                NetworkLatencyTestgrd.DataSource = nwlatencyDataTable;
                NetworkLatencyTestgrd.DataBind();

            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void FormSaveButton_Click(object sender, EventArgs e)
        {
            NetworkLatency nl = new NetworkLatency();
            // st.svalue = Enreportcheckbox.Checked.ToString();
            object blret = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.UpdateData(nl);
            //Object result1 = VSWebBL.ExchangeBAL.Ins.UpdateExchangeSettingsData(CollectDataforExchangeSettings());


            DataTable nwDataTable = (DataTable)Session["NetworkLatency"];
            FillNwlatencyServerGrid(ServerTypeComboBox.SelectedItem.Text);
            successDiv.Style.Value = "display: block";
        }
        protected void NetworkLatencyTestgrd_PreRender(object sender, EventArgs e)
        {
            try
            {

                if (isValid)
                {

                    ASPxGridView NetworkLatencyTestgrd = (ASPxGridView)sender;
                    for (int i = 0; i < NetworkLatencyTestgrd.VisibleRowCount; i++)
                    {
                       
                        if (NetworkLatencyTestgrd.GetRowValues(i, "Enabled").ToString() != "")
                        {
                            NetworkLatencyTestgrd.Selection.SetSelection(i, (bool)NetworkLatencyTestgrd.GetRowValues(i, "Enabled") == true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected void ServerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServerTypeComboBox.SelectedIndex != -1)
            {



                FillNwlatencyServerGrid(ServerTypeComboBox.SelectedItem.Text);
                // string value=//send to servertype






            }

        }

    }
}








