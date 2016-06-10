using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class SharepointServerHealth : System.Web.UI.Page
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
       // DataTable oslist = new DataTable();
        DataTable Spacelist = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //srvroles.Columns.Add("Role");
                //DataColumn dc = new DataColumn("RoleCount", typeof(int));
                //srvroles.Columns.Add(dc);
                //10/15/2014 NS added for VE-133
                DataColumn rolecol = new DataColumn("Role", typeof(string));
                srvroles.Columns.Add(rolecol);
                rolecol = new DataColumn("RoleCount", typeof(int));
                srvroles.Columns.Add(rolecol);
				DataColumn statuscol = new DataColumn("AppName", typeof(string));
                Spacelist.Columns.Add(statuscol);
                statuscol = new DataColumn("SiteCount", typeof(int));
                Spacelist.Columns.Add(statuscol);
                FillServersListGrid();
				FillDatabaseServers();
				FillFarmGrid();
				FillFarmCombobox();
                   //18-04-2016 Durga Modified for VSPLUS-2851
                FillSharePointTimerJobsGridView();
				//GRAPHS
                SrvRolesWebChart.DataSource = srvroles;
                SrvRolesWebChart.Series[0].DataSource = srvroles;
                SrvRolesWebChart.Series[0].ArgumentDataMember = srvroles.Columns[0].ToString();
                SrvRolesWebChart.Series[0].ValueDataMembers.AddRange(srvroles.Columns[1].ToString());
                SrvRolesWebChart.Series[0].Visible = true;
                SrvRolesWebChart.DataBind();

                // Detect overlapping of series labels.
                ((PieSeriesLabel)SpaceWebChart.Series[0].Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                SpaceWebChart.DataSource = Spacelist;
                SpaceWebChart.Series[0].DataSource = Spacelist;
                SpaceWebChart.Series[0].ArgumentDataMember = Spacelist.Columns[0].ToString();
                SpaceWebChart.Series[0].ValueDataMembers.AddRange(Spacelist.Columns[1].ToString());
                SpaceWebChart.Series[0].Visible = true;
                SpaceWebChart.DataBind();
               
            }
              else
            {
                //18-04-2016 Durga Modified for VSPLUS-2851
                FillSharePointTimerJobsGridViewfromSession();
                FillServersListGridfromSession();
				FillDatabaseServersFromSession();
            }
            //7/23/2014 NS added
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Session["BackURL"] = url;
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "SharepointServerHealth|SharePointHealthGridView")
                    {
                        SharePointHealthGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "SharepointServerHealth|FarmGridView")
                    {
                        FarmGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "SharepointServerHealth|DBServersGridView")
                    {
                        DBServersGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "SharepointServerHealth|SiteCollectionSizeGridView")
                    {
                        SiteCollectionSizeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    //18-04-2016 Durga Modified for VSPLUS-2851
                    if (dr[1].ToString() == "SharepointServerHealth|SharePointTimerJobsGridView")
                    {
                        SharePointTimerJobsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
        

        }
        public void FillServersListGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["SharePointServerList"] != null && Session["SharePointServerList"] != "")
                {
                    DataServers = Session["SharePointServerList"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
                    //ServerList.Text = DataServers.Rows.Count.ToString();
                }

                SharePointHealthGridView.DataSource = DataServers;
                SharePointHealthGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        protected void SharePointHealthGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SharePointHealthGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {

                    List<object> fieldValues = SharePointHealthGridView.GetSelectedFieldValues(new string[] { "redirectto" });
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
        public void FillServersListGrid()
        {
            DataTable memorydt = new DataTable();
            DataTable dt2 = new DataTable();
            memorydt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetSharePointServerDetails();
            dt2 = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetSharePointServerHealthRoles();
            Session["SharePointServerList"] = memorydt;
            

			string Farmscount = memorydt.AsEnumerable().Select(s => s.Field<string>("Farm")).Distinct().Where(s => s.Contains(',') == false).ToList().Count.ToString();

			string serverscount = memorydt.Rows.Count.ToString();

            ASPxLabel143.Text = serverscount + " "+ "Servers in " + Farmscount + " "+ "Farms";
            //ServerList.Text = "0";
            //10/15/2014 NS added for VE-133
            DataRow rolerow;
            DataRow[] foundRows;
            DataRow Memoryrow;
            rolerow = srvroles.NewRow();
            Memoryrow = Spacelist.NewRow();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (srvroles.Rows.Count > 0)
                    {
                        foundRows = srvroles.Select("Role = '" + dt2.Rows[i]["RoleName"].ToString() + "'");
                        if (foundRows.Length == 0)
                        {
                            rolerow = srvroles.Rows.Add();
                            rolerow["Role"] = dt2.Rows[i]["RoleName"].ToString();
                            rolerow["RoleCount"] = 1;
                            rolerow = srvroles.NewRow();
                        }
                        else
                        {
                            rolerow = foundRows[0];
                            rolerow["RoleCount"] = Convert.ToInt32(rolerow["RoleCount"].ToString()) + 1;
                        }
                    }
                    else
                    {
                        rolerow = srvroles.Rows.Add();
                        rolerow["Role"] = dt2.Rows[i]["RoleName"].ToString();
                        rolerow["RoleCount"] = 1;
                        rolerow = srvroles.NewRow();
                    }
                }
            }
			//if (memorydt != null && memorydt.Rows.Count > 0)
			//{
			//    for (int i = 0; i < memorydt.Rows.Count; i++)
			//    {
			//        if (Spacelist.Rows.Count > 0)
			//        {
			//            foundRows = Spacelist.Select("Memory = '" + memorydt.Rows[i]["ContentMemory"].ToString() + "'");
			//            if (foundRows.Length == 0)
			//            {
			//                Memoryrow = Spacelist.Rows.Add();
			//                Memoryrow["Memory"] = memorydt.Rows[i]["ContentMemory"].ToString() + "MB";
			//                Memoryrow["MemoryCount"] = 1;
			//                Memoryrow = Spacelist.NewRow();
			//            }
			//            else
			//            {
			//                Memoryrow = foundRows[0];
			//                Memoryrow["MemoryCount"] = Convert.ToInt32(Memoryrow["MemoryCount"].ToString() + "MB") + 1;
			//            }
			//        }
			//        else
			//        {
			//            Memoryrow = Spacelist.Rows.Add();
			//            Memoryrow["Memory"] = memorydt.Rows[i]["ContentMemory"].ToString() + "MB";
			//            Memoryrow["MemoryCount"] = 1;
			//            Memoryrow = Spacelist.NewRow();
			//        }
			//    }
			//}

            SharePointHealthGridView.DataSource = memorydt;
            
         
            SharePointHealthGridView.DataBind();

           
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

            if (stype.Text == "SharePoint" || sec.Text == "SharePoint")
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

            if (stype.Text == "SharePoint" || sec.Text == "SharePoint")
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

            if (stype.Text == "SharePoint" || sec.Text == "SharePoint")
            {
                double Responseval = Convert.ToDouble(Response.Text == "" ? "0" : Response.Text);

                double ResponseTHval = (Convert.ToDouble(ResponseTH.Text == "" ? "0" : ResponseTH.Text));
                //12/12/2012 NS added - the page throws an error if the CPU.Text value is an empty string/NULL
                string cputxt = (Response.Text == "" ? "0" : Response.Text);
         

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
              
                ahover.Attributes.Add("class", "noclass");
               

                gaugecntrl.Visible = false;
            }

            return "";
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

		public void FillDatabaseServers()
		{
			DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetDatabaseServerDetails();

			Session["SharePointDatabaseList"] = dt;

			DBServersGridView.DataSource = dt;
			DBServersGridView.DataBind();

			object sumObject;
			sumObject = dt.Compute("Sum(DatabaseSiteCount)", "");
			UsersLabel.Text = sumObject.ToString() + " Sites";

			//DataRow rolerow;
			DataRow[] foundRows;
			DataRow SiteRow;



			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Spacelist.Rows.Count > 0)
					{
						foundRows = Spacelist.Select("AppName = '" + dt.Rows[i]["WebAppName"].ToString() + "'");
						if (foundRows.Length == 0)
						{
							SiteRow = Spacelist.Rows.Add();
							SiteRow["AppName"] = dt.Rows[i]["WebAppName"].ToString();
							SiteRow["SiteCount"] = Convert.ToInt32(dt.Rows[i]["DatabaseSiteCount"].ToString());
							SiteRow = Spacelist.NewRow();
						}
						else
						{
							//SiteRow = foundRows[0];
							//SiteRow["SiteCount"] = Convert.ToInt32(SiteRow["SiteCount"].ToString());
						}
					}
					else
					{
						SiteRow = Spacelist.Rows.Add();
						SiteRow["AppName"] = dt.Rows[i]["WebAppName"].ToString();
						SiteRow["SiteCount"] = Convert.ToInt32(dt.Rows[i]["DatabaseSiteCount"].ToString());
						SiteRow = Spacelist.NewRow();
					}
				}
			}


			/*
			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Spacelist.Rows.Count > 0)
					{
						foundRows = Spacelist.Select("Memory = '" + memorydt.Rows[i]["ContentMemory"].ToString() + "'");
						if (foundRows.Length == 0)
						{
							Memoryrow = Spacelist.Rows.Add();
							Memoryrow["Memory"] = memorydt.Rows[i]["ContentMemory"].ToString() + "MB";
							Memoryrow["MemoryCount"] = 1;
							Memoryrow = Spacelist.NewRow();
						}
						else
						{
							Memoryrow = foundRows[0];
							Memoryrow["MemoryCount"] = Convert.ToInt32(Memoryrow["MemoryCount"].ToString() + "MB") + 1;
						}
					}
					else
					{
						Memoryrow = Spacelist.Rows.Add();
						Memoryrow["Memory"] = memorydt.Rows[i]["ContentMemory"].ToString() + "MB";
						Memoryrow["MemoryCount"] = 1;
						Memoryrow = Spacelist.NewRow();
					}
				}
			}
			*/
		}

		public void FillDatabaseServersFromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["SharePointDatabaseList"] != null && Session["SharePointDatabaseList"] != "")
				{
					DataServers = Session["SharePointDatabaseList"] as DataTable;
				}

				DBServersGridView.DataSource = DataServers;
				DBServersGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}


		protected void DBServersGridView_SelectionChanged(object sender, EventArgs e)
        {
			if (DBServersGridView.Selection.Count > 0)
            {
                if (Session["UserFullName"] != null)
                {

					List<object> fieldValues = DBServersGridView.GetSelectedFieldValues(new string[] { "redirectto" });
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

		public void FillFarmGrid()
		{
			DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetFarmDetails();

			Session["SharePointFarmGrid"] = dt;

			FarmGridView.DataSource = dt;
			FarmGridView.DataBind();


		}

		public void FillFarmGridFromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["SharePointFarmGrid"] != null && Session["SharePointFarmGrid"] != "")
				{
					DataServers = Session["SharePointFarmGrid"] as DataTable;
				}

				FarmGridView.DataSource = DataServers;
				FarmGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}


		}

		protected void FarmGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			string col = e.DataColumn.FieldName;
			if (!(col == "LogOnTest" || col == "UploadTest" || col == "SiteCollectionTest"))
				return;
			string status = e.GetValue(col).ToString();
			if ((status.ToUpper()).Contains("FAIL"))
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (status.ToUpper() == "PASS")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
		}

        protected void SharePointHealthGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
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

		protected void FillFarmCombobox()
		{

			try
			{
				DataTable dt = new DataTable();
				dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetFarmNames();
				FarmForSiteCollComboBox.DataSource = dt;
				FarmForSiteCollComboBox.TextField = "Farm";
				FarmForSiteCollComboBox.ValueField = "Farm";
				FarmForSiteCollComboBox.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			//FarmForSiteCollComboBox
		}
        protected void SharePointHealthGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServerHealth|SharePointHealthGridView", SharePointHealthGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void FarmGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServerHealth|FarmGridView", FarmGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DBServersGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServerHealth|DBServersGridView", DBServersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void SiteCollectionSizeGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServerHealth|SiteCollectionSizeGridView", SiteCollectionSizeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

		protected void FarmForSiteCollComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{

				DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetSiteCollectionSize(FarmForSiteCollComboBox.Text.ToString());

				SiteCollectionSizeGridView.DataSource = dt;
				SiteCollectionSizeGridView.DataBind();
				
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }


		}

        //18-04-2016 Durga Modified for VSPLUS-2851
        protected void SharePointTimerJobsGridView_OnPageSizeChanged(object sender, EventArgs e)
        {

            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServerHealth|SharePointTimerJobsGridView", SharePointTimerJobsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FillSharePointTimerJobsGridView()
        {
            DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.GetSharePointTimerJobsDetails();

            Session["SharePointTimerJobsGridView"] = dt;

            SharePointTimerJobsGridView.DataSource = dt;
            SharePointTimerJobsGridView.DataBind();


        }

        public void FillSharePointTimerJobsGridViewfromSession()
        {
            DataTable SharePointTimerJobs = new DataTable();
            try
            {
                if (Session["SharePointTimerJobsGridView"] != null && Session["SharePointTimerJobsGridView"] != "")
                {
                    SharePointTimerJobs = Session["SharePointTimerJobsGridView"] as DataTable;
                }

                SharePointTimerJobsGridView.DataSource = SharePointTimerJobs;
                SharePointTimerJobsGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void SharePointTimerJobsGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status")
            {
                if (e.GetValue("Status").ToString() == "Failed")
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