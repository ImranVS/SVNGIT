using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class ExchangeHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        DataTable srvroles = new DataTable();
        //10/15/2014 NS added for VE-133
        DataTable oslist = new DataTable();
        DataTable statuslist = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ExchangeServerList"] = null;
                Session["ExchangeDAGStatusList"] = null;
                Session["HubEdgeGrid"] = null;
                Session["CASStatusGrid"] = null;
                Session["MailBoxGridList"] = null;
                Session["CASArrayGrid"] = null;
                //7/18/2014 NS added
                srvroles.Columns.Add("Role");
                DataColumn dc = new DataColumn("RoleCount", typeof(int));
                srvroles.Columns.Add(dc);
                //10/15/2014 NS added for VE-133
                DataColumn oscol = new DataColumn("OS", typeof(string));
                oslist.Columns.Add(oscol);
                oscol = new DataColumn("OSCount", typeof(int));
                oslist.Columns.Add(oscol);
                DataColumn statuscol = new DataColumn("Status", typeof(string));
                statuslist.Columns.Add(statuscol);
                statuscol = new DataColumn("StatusCount", typeof(int));
                statuslist.Columns.Add(statuscol);
                FillServersListGrid();
                FillDAGStatusGrid();
                FillHubEdgeGrid();
                FillCASGrid();
                FillMailBoxGrid();
                FillCASArrayGrid();
                //7/18/2014 NS added
                SrvRolesWebChart.DataSource = srvroles;
                SrvRolesWebChart.Series[0].DataSource = srvroles;
                SrvRolesWebChart.Series[0].ArgumentDataMember = srvroles.Columns[0].ToString();
                SrvRolesWebChart.Series[0].ValueDataMembers.AddRange(srvroles.Columns[1].ToString());
                SrvRolesWebChart.Series[0].Visible = true;
                SrvRolesWebChart.DataBind();
                //10/15/2014 NS added for VE-133
                OSWebChart.DataSource = oslist;
                OSWebChart.Series[0].DataSource = oslist;
                OSWebChart.Series[0].ArgumentDataMember = oslist.Columns[0].ToString();
                OSWebChart.Series[0].ValueDataMembers.AddRange(oslist.Columns[1].ToString());
                OSWebChart.Series[0].Visible = true;
                OSWebChart.DataBind();
                StatusWebChart.DataSource = statuslist;
                StatusWebChart.Series[0].DataSource = statuslist;
                StatusWebChart.Series[0].ArgumentDataMember = statuslist.Columns[0].ToString();
                StatusWebChart.Series[0].ValueDataMembers.AddRange(statuslist.Columns[1].ToString());
                StatusWebChart.Series[0].Visible = true;
                StatusWebChart.DataBind();
            }
            else
            {
                FillServersListGridfromSession();
                FillDAGStatusGridfromSession();
                FillHubEdgeGridfromSession();
                FillCASGridfromSession();
                FillMailBoxGridfromSession();
                FillCASArrayGridfromSession();
            }
            //7/23/2014 NS added
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Session["BackURL"] = url;
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "ExchangeHealth|EXGHealthGridView")
                    {
                        EXGHealthGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "ExchangeHealth|DAGGridView")
                    {
                        DAGGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "ExchangeHealth|CASGridView")
                    {
                        CASGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "ExchangeHealth|HubEdgeGridView")
                    {
                        HubEdgeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "ExchangeHealth|MailBoxGridView")
                    {
                        MailBoxGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
           
        }

        public void FillServersListGrid()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ExchangeBAL.Ins.GetExchangeServerDetails();
            Session["ExchangeServerList"] = dt;
            //ServerList.Text = "0";
            //10/15/2014 NS added for VE-133
            DataRow osrow;
            DataRow[] foundRows;
            DataRow statusrow;
            osrow = oslist.NewRow();
            statusrow = statuslist.NewRow();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i=0; i < dt.Rows.Count; i++)
                {
                    if (oslist.Rows.Count > 0)
                    {
                        foundRows = oslist.Select("OS = '" + dt.Rows[i]["OperatingSystem"].ToString() + "'");
                        if (foundRows.Length == 0)
                        {
                            osrow = oslist.Rows.Add();
                            osrow["OS"] = dt.Rows[i]["OperatingSystem"].ToString();
                            osrow["OSCount"] = 1;
                            osrow = oslist.NewRow();
                        }
                        else
                        {
                            osrow = foundRows[0];
                            osrow["OSCount"] = Convert.ToInt32(osrow["OSCount"].ToString()) + 1;
                        }
                    }
                    else
                    {
                        osrow = oslist.Rows.Add();
                        osrow["OS"] = dt.Rows[i]["OperatingSystem"].ToString();
                        osrow["OSCount"] = 1;
                        osrow = oslist.NewRow();
                    }
                    if (statuslist.Rows.Count > 0)
                    {
                        foundRows = statuslist.Select("Status = '" + dt.Rows[i]["Status"].ToString() + "'");
                        if (foundRows.Length == 0)
                        {
                            statusrow = statuslist.Rows.Add();
                            statusrow["Status"] = dt.Rows[i]["Status"].ToString();
                            statusrow["StatusCount"] = 1;
                            statusrow = statuslist.NewRow();
                        }
                        else
                        {
                            statusrow = foundRows[0];
                            statusrow["StatusCount"] = Convert.ToInt32(statusrow["StatusCount"].ToString()) + 1;
                        }
                    }
                    else
                    {
                        statusrow = statuslist.Rows.Add();
                        statusrow["Status"] = dt.Rows[i]["Status"].ToString();
                        statusrow["StatusCount"] = 1;
                        statusrow = statuslist.NewRow();
                    }
                }
            }
            EXGHealthGridView.DataSource = dt;
            EXGHealthGridView.DataBind();
        }

        public void FillServersListGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["ExchangeServerList"] != null && Session["ExchangeServerList"] != "")
                {
                    DataServers = Session["ExchangeServerList"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
                    //ServerList.Text = DataServers.Rows.Count.ToString();
                }

                EXGHealthGridView.DataSource = DataServers;
                EXGHealthGridView.DataBind();
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void FillDAGStatusGrid()
        {
            string DAGList = "";
            DataTable dt = new DataTable();
            dt = VSWebBL.ExchangeBAL.Ins.GetDAGStatus("");
            Session["ExchangeDAGStatusList"] = dt;
            //DAGList.Text = "0";
            DAGList = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                //DAGList.Text = dt.Rows.Count.ToString();
                DAGList = dt.Rows.Count.ToString();
            }
            DAGGridView.DataSource = dt;
            DAGGridView.DataBind();
            //7/18/2014 NS added
            DataRow rolerow = srvroles.Rows.Add();
            rolerow["Role"] = "DAG";
            rolerow["RoleCount"] = Convert.ToInt32(DAGList);
        }

        public void FillDAGStatusGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["ExchangeDAGStatusList"] != null && Session["ExchangeDAGStatusList"] != "")
                {
                    DataServers = Session["ExchangeDAGStatusList"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["DAGName"] };
                    //DAGList.Text = DataServers.Rows.Count.ToString();
                }
                DAGGridView.DataSource = DataServers;
                DAGGridView.DataBind();
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void FillCASArrayGrid()
        {
            //DataTable dt = new DataTable();
            //dt = VSWebBL.ExchangeBAL.Ins.GetHubEdgeStatus();
            //Session["CASArrayGrid"] = dt;
            //CASArrayList.Text = "0";
            //var result = from row in dt.AsEnumerable()
            //             group row by row.Field<string>("RoleName") into grp
            //             select new
            //             {
            //                 RoleName = grp.Key,
            //                 MemberCount = grp.Count()
            //             };
            //foreach (var t in result)
            //{
            //    if (t.RoleName.ToString().ToLower() == "hub")
            //    {
            //        HUBList.Text = t.MemberCount.ToString();
            //    }
            //    if (t.RoleName.ToString().ToLower() == "edge")
            //    {
            //        EDGEList.Text = t.MemberCount.ToString();
            //    }
            //}
            //HubEdgeGridView.DataSource = dt;
            //HubEdgeGridView.DataBind();
        }

        public void FillCASArrayGridfromSession()
        {
        }
        public void FillHubEdgeGrid()
        {
            string HUBList = "";
            string EDGEList = "";
            DataTable dt = new DataTable();
            dt = VSWebBL.ExchangeBAL.Ins.GetHubEdgeStatus();
            Session["HubEdgeGrid"] = dt;
            //HUBList.Text = "0";
            //EDGEList.Text = "0";
            HUBList = "0";
            EDGEList = "0";
            var result = from row in dt.AsEnumerable()
              group row by row.Field<string>("RoleName") into grp
               select new
                 {
                  RoleName = grp.Key,
                  MemberCount = grp.Count()
                  };
            foreach (var t in result)
            {
                if(t.RoleName.ToString().ToLower() == "hub")
                {
                    //HUBList.Text = t.MemberCount.ToString();
                    HUBList = t.MemberCount.ToString();
                }
                if(t.RoleName.ToString().ToLower() == "edge")
                {
                    //EDGEList.Text = t.MemberCount.ToString();
                    EDGEList = t.MemberCount.ToString();
                }
            }
            HubEdgeGridView.DataSource = dt;
            HubEdgeGridView.DataBind();
            //7/18/2014 NS added
            DataRow rolerow = srvroles.Rows.Add();
            rolerow["Role"] = "HUB";
            rolerow["RoleCount"] = Convert.ToInt32(HUBList);
            rolerow = srvroles.Rows.Add();
            rolerow["Role"] = "EDGE";
            rolerow["RoleCount"] = Convert.ToInt32(EDGEList);
        }

        public void FillHubEdgeGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["HubEdgeGrid"] != null && Session["HubEdgeGrid"] != "")
                {
                    DataServers = Session["HubEdgeGrid"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    //7/21/2014 NS modified
                    //DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["CID"] };
                    DataView dv = new DataView((DataTable)Session["HubEdgeGrid"]);
                    //HUBList.Text = "0";
                    //EDGEList.Text = "0";
                    var result = from row in DataServers.AsEnumerable()
                                 group row by row.Field<string>("RoleName") into grp
                                 select new
                                 {
                                     RoleName = grp.Key,
                                     MemberCount = grp.Count()
                                 };
                    foreach (var t in result)
                    {
                        if (t.RoleName.ToString().ToLower() == "hub")
                        {
                            //HUBList.Text = t.MemberCount.ToString();
                        }
                        if (t.RoleName.ToString().ToLower() == "edge")
                        {
                            //EDGEList.Text = t.MemberCount.ToString();
                        }
                    }
                }
                HubEdgeGridView.DataSource = DataServers;
                HubEdgeGridView.DataBind();
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void FillCASGrid()
        {
            string CASList = "";
            DataTable dt = new DataTable();
            dt = VSWebBL.ExchangeBAL.Ins.GetCASStatus();
            Session["CASStatusGrid"] = dt;
            //CASList.Text = "0";
            CASList = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                //CASList.Text = dt.Rows.Count.ToString();
                CASList = dt.Rows.Count.ToString();
            }
            CASGridView.DataSource = dt;
            CASGridView.DataBind();
            //7/18/2014 NS added
            DataRow rolerow = srvroles.Rows.Add();
            rolerow["Role"] = "CAS";
            rolerow["RoleCount"] = Convert.ToInt32(CASList);
        }

        public void FillCASGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["CASStatusGrid"] != null && Session["CASStatusGrid"] != "")
                {
                    DataServers = Session["CASStatusGrid"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
                    //CASList.Text = DataServers.Rows.Count.ToString();
                }
                CASGridView.DataSource = DataServers;
                CASGridView.DataBind();
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void FillMailBoxGrid()
        {
            string MBXList = "";
            DataTable dt = new DataTable();
            dt = VSWebBL.ExchangeBAL.Ins.GetMailBoxStatus();
            Session["MailBoxGridList"] = dt;
            //MBXList.Text = "0";
            MBXList = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                //MBXList.Text = dt.Rows.Count.ToString();
                MBXList = dt.Rows.Count.ToString(); 
            }
            MailBoxGridView.DataSource = dt;
            MailBoxGridView.DataBind();
            //7/18/2014 NS added
			//9/15/14 WS modified due to not all servers marked as Mailbox have a database on them, which is what the above query searches for
            DataRow rolerow = srvroles.Rows.Add();
            rolerow["Role"] = "MBX";
            rolerow["RoleCount"] = VSWebBL.ExchangeBAL.Ins.GetMailboxRoleCount();
        }
        public void FillMailBoxGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["MailBoxGridList"] != null && Session["MailBoxGridList"] != "")
                {
                    DataServers = Session["MailBoxGridList"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ServerName"] };
                    //MBXList.Text = DataServers.Rows.Count.ToString();
                }
                MailBoxGridView.DataSource = DataServers;
                MailBoxGridView.DataBind();
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public string SetMemory(GridViewDataItemTemplateContainer Container)
        {
            DevExpress.Web.ASPxGauges.ASPxGaugeControl gaugecntrl = (DevExpress.Web.ASPxGauges.ASPxGaugeControl)Container.FindControl("gControl_Page3");

            Label Mem = (Label)Container.FindControl("lblMem");
            Label MemTH = (Label)Container.FindControl("lblMemTH");
            Label display = (Label)Container.FindControl("msgLabel");
            Label stype = (Label)Container.FindControl("Type");
            Label sec = (Label)Container.FindControl("Sec");
            Label Meminfo = (Label)Container.FindControl("poplbl");
            Label lblMemDetails = (Label)Container.FindControl("lblMemDetails");

            if (stype.Text == "Exchange" || sec.Text == "Exchange")
            {
                double Memval = Convert.ToDouble(Mem.Text == "" ? "0" : Mem.Text);

                double MemTHval = (Convert.ToDouble(MemTH.Text == "" ? "0" : MemTH.Text));
                string Memthreshhold = MemTHval.ToString();
                //12/12/2012 NS added - the page throws an error if the Mem.Text value is an empty string/NULL
                string Memtxt = (Mem.Text == "" ? "0" : Mem.Text);
                //CreateLinearGauge(gaugecntrl, Mem.Text, Memthreshhold);
                //Below line commented by Mukund- VSPLUS-374. 12Feb14
                //CreateLinearGauge(gaugecntrl, Memtxt, Memthreshhold);

                display.Text = Memval + "/" + MemTHval;
                //http://help.devexpress.com/#AspNet/CustomDocument5242
                //7/22/2014 NS commented out
                //lblMemDetails.Text = "<br><br><b>" +
                //                    "Memory Utilization: " + Memval.ToString() + " %<br>" +
                //                    (MemTHval == 0 ? "No Threshold" : "Threshold:       " + MemTHval.ToString() + " %") + "</b>";
                if (MemTHval != 0)
                {
                    display.ForeColor = (Memval >= 0 && Memval < (MemTHval * 0.4) ? Color.Green : (Memval >= (MemTHval * 0.4) && Memval < MemTHval ? Color.Orange : Color.Red));
                }

            }
            else
            {
                HtmlAnchor ahover = (HtmlAnchor)Container.FindControl("ahover");
                //ahover.Attributes.Add("visibility", "hidden"); 
                ahover.Attributes.Add("class", "noclass");
                //HtmlGenericControl parentdiv = (HtmlGenericControl)Container.FindControl("parentdiv");
                //parentdiv.Attributes.Add("visibility", "hidden");

                //gaugecntrl.Visible = false;
            }

            return "";
        }

        public string SetCPU(GridViewDataItemTemplateContainer Container)
        {
            DevExpress.Web.ASPxGauges.ASPxGaugeControl gaugecntrl = (DevExpress.Web.ASPxGauges.ASPxGaugeControl)Container.FindControl("gControl_Page3");

            Label CPU = (Label)Container.FindControl("lblCPU");
            Label CPUTH = (Label)Container.FindControl("lblCPUTH");
            Label display = (Label)Container.FindControl("msgLabel");
            Label stype = (Label)Container.FindControl("Type");
            Label sec = (Label)Container.FindControl("Sec");
            Label CPUinfo = (Label)Container.FindControl("poplbl");
            Label lblCPUDetails = (Label)Container.FindControl("lblCPUDetails");

            if (stype.Text == "Exchange" || sec.Text == "Exchange" )
            {
                double CPUval = Convert.ToDouble(CPU.Text == "" ? "0" : CPU.Text);

                double CPUTHval = (Convert.ToDouble(CPUTH.Text == "" ? "0" : CPUTH.Text));
                string cputhreshhold = CPUTHval.ToString();
                //12/12/2012 NS added - the page throws an error if the CPU.Text value is an empty string/NULL
                string cputxt = (CPU.Text == "" ? "0" : CPU.Text);
                //CreateLinearGauge(gaugecntrl, CPU.Text, cputhreshhold);
                //Below line commented by Mukund- VSPLUS-374. 12Feb14
                //CreateLinearGauge(gaugecntrl, cputxt, cputhreshhold);

                display.Text = CPUval + "/" + CPUTHval;
                //http://help.devexpress.com/#AspNet/CustomDocument5242
                //7/22/2014 NS commented out
                //lblCPUDetails.Text = "<br><br><b>" +
                //                    "CPU Utilization: " + CPUval.ToString() + " %<br>" +
                //                    (CPUTHval == 0 ? "No Threshold" : "Threshold:       " + CPUTHval.ToString() + " %") + "</b>";
                if (CPUTHval != 0)
                {
                    display.ForeColor = (CPUval >= 0 && CPUval < (CPUTHval * 0.4) ? Color.Green : (CPUval >= (CPUTHval * 0.4) && CPUval < CPUTHval ? Color.Orange : Color.Red));
                }

            }
            else
            {
                HtmlAnchor ahover = (HtmlAnchor)Container.FindControl("ahover");
                //ahover.Attributes.Add("visibility", "hidden"); 
                ahover.Attributes.Add("class", "noclass");
                //HtmlGenericControl parentdiv = (HtmlGenericControl)Container.FindControl("parentdiv");
                //parentdiv.Attributes.Add("visibility", "hidden");

                gaugecntrl.Visible = false;
            }

            return "";
        }

        public string SetResponse(GridViewDataItemTemplateContainer Container)
        {
            DevExpress.Web.ASPxGauges.ASPxGaugeControl gaugecntrl = (DevExpress.Web.ASPxGauges.ASPxGaugeControl)Container.FindControl("gControl_Page3");

            Label Response = (Label)Container.FindControl("lblResponse");
            Label ResponseTH = (Label)Container.FindControl("lblResponseTH");
            Label display = (Label)Container.FindControl("msgLabel");
            Label stype = (Label)Container.FindControl("Type");
            Label sec = (Label)Container.FindControl("Sec");
            Label Responseinfo = (Label)Container.FindControl("poplbl");
            Label lblResponseDetails = (Label)Container.FindControl("lblResponseDetails");

            if (stype.Text == "Exchange" || sec.Text == "Exchange")
            {
                double Responseval = Convert.ToDouble(Response.Text == "" ? "0" : Response.Text);

                double ResponseTHval = (Convert.ToDouble(ResponseTH.Text == "" ? "0" : ResponseTH.Text));
                //12/12/2012 NS added - the page throws an error if the CPU.Text value is an empty string/NULL
                string cputxt = (Response.Text == "" ? "0" : Response.Text);
                //CreateLinearGauge(gaugecntrl, CPU.Text, cputhreshhold);
                //Below line commented by Mukund- VSPLUS-374. 12Feb14
                //CreateLinearGauge(gaugecntrl, cputxt, cputhreshhold);

                display.Text = Responseval + "/" + ResponseTHval;
                //http://help.devexpress.com/#AspNet/CustomDocument5242
                //7/22/2014 NS commented out
                //lblResponseDetails.Text = "<br><br><b>" +
                //                    "Response Time: " + Responseval.ToString() + " %<br>" +
                //                    (ResponseTHval == 0 ? "No Threshold" : "Threshold:       " + ResponseTHval.ToString() + " %") + "</b>";
                if (ResponseTHval != 0)
                {
                    display.ForeColor = (Responseval >= 0 && Responseval < (ResponseTHval * 0.4) ? Color.Green : (Responseval >= (ResponseTHval * 0.4) && Responseval < ResponseTHval ? Color.Orange : Color.Red));
                }

            }
            else
            {
                HtmlAnchor ahover = (HtmlAnchor)Container.FindControl("ahover");
                //ahover.Attributes.Add("visibility", "hidden"); 
                ahover.Attributes.Add("class", "noclass");
                //HtmlGenericControl parentdiv = (HtmlGenericControl)Container.FindControl("parentdiv");
                //parentdiv.Attributes.Add("visibility", "hidden");

                gaugecntrl.Visible = false;
            }

            return "";
        }

        public string SetSubQ(GridViewDataItemTemplateContainer Container)
        {
            DevExpress.Web.ASPxGauges.ASPxGaugeControl gaugecntrl = (DevExpress.Web.ASPxGauges.ASPxGaugeControl)Container.FindControl("gControl_Page3");

            Label SubQ = (Label)Container.FindControl("lblSubQ");
            Label SubQTH = (Label)Container.FindControl("lblSubQTH");
            Label display = (Label)Container.FindControl("msgLabel");            
            Label lblSubQDetails = (Label)Container.FindControl("lblSubQDetails");

                double SubQval = Convert.ToDouble(SubQ.Text == "" ? "0" : SubQ.Text);

                double SubQTHval = (Convert.ToDouble(SubQTH.Text == "" ? "0" : SubQTH.Text));

                display.Text = SubQval + "/" + SubQTHval;
                //http://help.devexpress.com/#AspNet/CustomDocument5242
                lblSubQDetails.Text = "<br><br><b>" +
                                    "Submission Queue: " + SubQval.ToString() + " <br>" +
                                    (SubQTHval == 0 ? "No Threshold" : "Threshold:       " + SubQTHval.ToString() + " ") + "</b>";
                if (SubQTHval != 0)
                {
                    display.ForeColor = (SubQval >= 0 && SubQval < (SubQTHval * 0.4) ? Color.Green : (SubQval >= (SubQTHval * 0.4) && SubQval < SubQTHval ? Color.Orange : Color.Red));
                }

            

            return "";
        }

        protected void CASGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (CASGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {
                    List<object> fieldValues = CASGridView.GetSelectedFieldValues(new string[] {"redirectto"});
                   // System.Windows.Forms.MessageBox.Show(fieldValues.Count.ToString());
                    //System.Windows.Forms.MessageBox.Show();
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
                    Response.Redirect(fieldValues[0].ToString() + "&TabType=CAS");
                }
            }
        }
        protected void MailBoxGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (MailBoxGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {
                    List<object> fieldValues = MailBoxGridView.GetSelectedFieldValues(new string[] { "redirectto" });
                    Response.Redirect(fieldValues[0].ToString() + "&TabType=MailBox");
                }
            }
        }
        protected void EXGHealthGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (EXGHealthGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {
                    
                    List<object> fieldValues = EXGHealthGridView.GetSelectedFieldValues(new string[] { "redirectto" });
                    //7/22/2014 NS commented the line below, uncommented the following line - the page would auto redirect on each refresh
                    //Response.Redirect(fieldValues[0].ToString());
                 
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                }
            }
        }
        protected void DAGGridView_SelectionChanged(object sender, EventArgs e)
        {

            if (DAGGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> DAGName = DAGGridView.GetSelectedFieldValues("DAGName");

				if (DAGName.Count > 0)
                {
                    string sDAGName = DAGName[0].ToString();
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Dashboard/DAGDetail.aspx?id=" + sID + "&Name=" + sDAGName);
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Dashboard/DAGHealth.aspx?Name=" + sDAGName);
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                }
            }
        }

        protected void CASArrayGridView_SelectionChanged(object sender, EventArgs e)
        {
            //if (EXGHealthGridView.Selection.Count > 0)
            //{
            //    if (Session["UserFullName"] != null)
            //    {

            //        List<object> fieldValues = EXGHealthGridView.GetSelectedFieldValues(new string[] { "redirectto" });
            //        Response.Redirect(fieldValues[0].ToString());
            //        // DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
            //    }
            //}
        }

        protected void HubEdgeGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (HubEdgeGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {

                    List<object> fieldValues = HubEdgeGridView.GetSelectedFieldValues(new string[] { "redirectto" });
                    List<object> RoleType = HubEdgeGridView.GetSelectedFieldValues(new string[] {  "rolename" });
                    Response.Redirect(fieldValues[0].ToString() + "&TabType=" + RoleType[0].ToString());
                    // DevExpress.Web.ASPxWebControl.RedirectOnCallback(fieldValues[0].ToString());
                }
            }
        }

        protected void CASGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string RPC = "";
            string IMAP = "";
            string OWA = "";
            string POP3 = "";
            string ActiveSync = "";
            string SMTP = "";
            string EWS = "";
            string AutoDiscovery = "";
            string Services = "";
            RPC = e.GetValue("RPC").ToString();
            IMAP = e.GetValue("IMAP").ToString();
            OWA = e.GetValue("OWA").ToString();
            POP3 = e.GetValue("POP3").ToString();
            ActiveSync = e.GetValue("Active Sync").ToString();
            SMTP = e.GetValue("SMTP").ToString();
            EWS = e.GetValue("EWS").ToString();
            AutoDiscovery = e.GetValue("Auto Discovery").ToString();
            Services = e.GetValue("Services").ToString();
            switch (e.DataColumn.FieldName)
            {
                case "RPC":
                    if (RPC.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (RPC.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (RPC.ToUpper() == "OK" || RPC.ToUpper() == "PASS" || RPC.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "IMAP":
                    if (IMAP.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (IMAP.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (IMAP.ToUpper() == "OK" || IMAP.ToUpper() == "PASS" || IMAP.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "OWA":
                    if (OWA.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (OWA.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (OWA.ToUpper() == "OK" || OWA.ToUpper() == "PASS" || OWA.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "POP3":
                    if (POP3.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (POP3.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (POP3.ToUpper() == "OK" || POP3.ToUpper() == "PASS" || POP3.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "Active Sync":
                    if (ActiveSync.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (ActiveSync.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (ActiveSync.ToUpper() == "OK" || ActiveSync.ToUpper() == "PASS" || ActiveSync.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "SMTP":
                    if (SMTP.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (SMTP.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (SMTP.ToUpper() == "OK" || SMTP.ToUpper() == "PASS" || SMTP.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "EWS":
                    if (EWS.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (EWS.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (EWS.ToUpper() == "OK" || EWS.ToUpper() == "PASS" || EWS.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "Auto Discovery":
                    if (AutoDiscovery.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (AutoDiscovery.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (AutoDiscovery.ToUpper() == "OK" || AutoDiscovery.ToUpper() == "PASS" || AutoDiscovery.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case "Services":
                    if (Services.ToUpper() == "FAIL")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (Services.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    //3/27/2014 NS modified
                    else if (Services.ToUpper() == "OK" || Services.ToUpper() == "PASS" || Services.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
            }
        }

        protected void DAGGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string status = "";
            status = e.GetValue("Status").ToString();
            switch (e.DataColumn.FieldName)
            {
                case "Status":
                    if ((status.ToUpper()).Contains("FAIL"))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (status.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (status.ToUpper() == "OK" || status.ToUpper() == "PASS" || status.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
            }
        }

        protected void StatusWebChart_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e)
        {
            Pie3DDrawOptions drawOptions = e.SeriesDrawOptions as Pie3DDrawOptions;
            Pie3DDrawOptions legendOptions = e.LegendDrawOptions as Pie3DDrawOptions;
            drawOptions.FillStyle.FillMode = FillMode3D.Solid;
            legendOptions.FillStyle.FillMode = FillMode3D.Solid;
            if (e.SeriesPoint.Argument == "OK")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
            }
            else if (e.SeriesPoint.Argument == "Issue")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
            }
            else if (e.SeriesPoint.Argument == "Not Responding")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
                legendOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
            }
            else if (e.SeriesPoint.Argument == "Maintenance")
            {
                drawOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
                legendOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
            }
        }
        protected void EXGHealthGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeHealth|EXGHealthGridView", EXGHealthGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void CASGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeHealth|CASGridView", CASGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void HubEdgeGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeHealth|HubEdgeGridView", HubEdgeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void MailBoxGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeHealth|MailBoxGridView", MailBoxGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DAGGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeHealth|DAGGridView", DAGGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void EXGHealthGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status")
            {
                if (e.GetValue("Status").ToString() == "Not Responding")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else if (e.GetValue("Status").ToString() == "Issue")
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                }
            }
        }
    }
}