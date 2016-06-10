using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;

using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.Web.ASPxGauges.Gauges.Linear;
using DevExpress.Web.ASPxGauges.Base;
using DevExpress.Web.ASPxGauges;
using DevExpress.Web.ASPxGauges.Gauges;
using DevExpress.XtraGauges.Base;
using System.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;

using DevExpress.XtraGauges.Core.Base;
using System.Text;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using System.ComponentModel;



//using DevExpress.XtraGauges.Core.Model;


namespace VSWebUI
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		string status = "";
		string typeloc = "";
		string filterval = "";
		string MapLoc = "";
		string str9 = "";
		DataRow myRow = null;
		string id = ""; string str34; string Disknames;

		protected void Page_PreInit(object sender, EventArgs e)
		{


            if (Session["showsummary"] != null)
            {
                if (Session["showsummary"].ToString() == "False")
                {
                    if (Session["UserLogin"] == null || Session["UserLogin"] == "")
                    {
                        Response.Redirect("~/login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                }
            }
          
			//Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event
			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}
		private void Master_ButtonClick(object sender, EventArgs e)
		{
			//Mukund 05Nov13, This Method will be Called from Timer Click from Master page
			// FillGrid();
			// ExpandGrid();
		}
		protected void Page_Load(object sender, EventArgs e)
		{

			if (Session["ViewBy"] == null) Session["ViewBy"] = "ServerType";
			if (Session["FilterByValue"] == null) Session["FilterByValue"] = "null";

			if (!IsPostBack)
			{

				StatusListPopupMenu.Items.Clear();
				if (Session["Isconfigurator"] != null)
				{
					if (Session["Isconfigurator"].ToString() == "True")
					{
						DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
						item.Text = "Scan Now";
						item.Name = "ScanNow";
						StatusListPopupMenu.Items.Add(item);
						DevExpress.Web.MenuItem item1 = new DevExpress.Web.MenuItem();
						item1.Text = "Edit in Configurator";
						item1.Name = "EditConfigurator";
						StatusListPopupMenu.Items.Add(item1);
						DevExpress.Web.MenuItem item2 = new DevExpress.Web.MenuItem();
						item2.Text = "Suspend Temporarily";
						item2.Name = "Suspend";
						StatusListPopupMenu.Items.Add(item2);
					}
					else
					{
						DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
						item.Text = "Scan Now";
						item.Name = "ScanNow";
						StatusListPopupMenu.Items.Add(item);
					}
				}
				else
				{
					DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
					item.Text = "Scan Now";
					item.Name = "ScanNow";
					StatusListPopupMenu.Items.Add(item);
				}

				//10/1/2014 NS added for VSPLUS-683
				if (Session["Isconsolecomm"] != null)
				{
					if (Session["Isconsolecomm"].ToString() == "True")
					{
						DevExpress.Web.MenuItem item3 = new DevExpress.Web.MenuItem();
						item3.Text = "Send Console Command";
						item3.Name = "SendConsoleCommand";


						//item3.Enabled = false;
						StatusListPopupMenu.Items.Add(item3);
					}
				}

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "DeviceTypeList|DeviceGridView")
						{
							DeviceGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			try
			{
				status = Request.QueryString["status"];
				typeloc = Request.QueryString["typeloc"];
				if (Session["FilterByValue"] != "" && Session["FilterByValue"] != null)
				{
					filterval = Session["FilterByValue"].ToString();
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			//9/5/2013 NS commented out the !IsPostBack condition since otherwise page refresh did not take place
			//when async postback was sent by the timer
			//if (!IsPostBack)
			//{
            //3/31/2016 NS commented out per AF's request
            //if (Session["ViewBy"] != "" && Session["ViewBy"] != null)
            //    viewbyLabel.Text = "View By " + Session["ViewBy"].ToString();

            //if (Session["FilterByValue"] != "" && Session["FilterByValue"] != null)
            //{
            //    if (Session["FilterByValue"].ToString() != "null")
            //        FilterbyLabel.Text = "Filter By " + Session["FilterByValue"].ToString();
            //}
            //else
            //{
            //    FilterbyLabel.Text = "";
            //}


			//SetGraphForDiskSpace2("'" + servernamelbl.Text + "'");
			//DiskWebChart.ClientVisible = true;
			//2/11/2013 NS added
			string servername = Request.QueryString["server"];
			if (servername != "" && servername != null)
			{
				string filterexp = "[Name] Like '%" + servername + "%'";
				DeviceGridView.FilterExpression = filterexp;
			}

			if (Request.QueryString["MapLoc"] != "" && Request.QueryString["MapLoc"] != null)
			{
				MapLoc = Request.QueryString["MapLoc"];
			}

			if (!IsPostBack)
			{
				FillGrid();
				Session["myRow"] = null;
			}
			else
			{
				//Mukund 05Nov13, Handle grid refresh Status List, when user expands a group. Timer refresh calls  Master_ButtonClick for DB data & expands all groups
				FillgridfromSession();

				//if (CollapseButton.Text == "Collapse All Rows")
				//{
				//    DeviceGridView.CollapseAll();
				//    CollapseButton.Image.Url = "~/images/icons/add.png";
				//    CollapseButton.Text = "Expand All Rows";
				//}
				//	CollapseButton.Text = "Collapse All Rows";
				//ToggleButton()

			}
			expandButton();
			//9/4/2013 NS added timer refresh interval; set in milliseconds; refreshtime comes from the Users table and should be set in seconds
			if (Session["Refreshtime"] != "" && Session["Refreshtime"] != null)
			{
				//timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;
			}
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			Session["BackURL"] = url;// "DeviceTypeList.aspx";
			//7/29/2013 NS added the lines below in order to clear the values if a status box is selected from the Configurator
			Session["GroupIndex"] = null;
			Session["ItemIndex"] = null;
			Session["Submenu"] = null;
			Session["SubmenuItem"] = null;
			//	CollapseButton.Text = "Collapse All Rows";
		}
		public void FillGrid()
		{

			try
			{

				DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatus(status, typeloc, filterval, Session["ViewBy"].ToString(), MapLoc);
				//DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatus2();
				//if (StatusTable.Rows.Count > 0)
				//{
				//    StatusTable.PrimaryKey = new DataColumn[] { StatusTable.Columns["TypeandName"] };
				//}
				//Session["StatusTable"] = StatusTable;
				if (StatusTable.Rows.Count > 0)
				{
					if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
					{
						List<int> ServerID = new List<int>();
						List<int> LocationID = new List<int>();
						DataTable resServers = (DataTable)Session["RestrictedServers"];
						foreach (DataRow resser in resServers.Rows)
						{
							foreach (DataRow dominorow in StatusTable.Rows)
							{

								if (resser["serverid"].ToString() == dominorow["ID"].ToString())
								{
									ServerID.Add(StatusTable.Rows.IndexOf(dominorow));
								}
							}

						}
						foreach (int Id in ServerID)
						{
							StatusTable.Rows[Id].Delete();
						}
						StatusTable.AcceptChanges();

					

						////foreach (int Lid in LocationID)
						////{
						////    DSTaskSettingsDataTable.Rows[Lid].Delete();
						////}
						//foreach (int lid in LocationID)
						//{
						//    DataRow[] row = DSTaskSettingsDataTable.Select("locationid=" + lid + "");
						//    for (int i = 0; i < ro = w.Count(); i++)
						//    {
						//        DSTaskSettingsDataTable.Rows.Remove(row[i]);
						//        DSTaskSettingsDataTable.AcceptChanges();
						//    }
						//}
						//DSTaskSettingsDataTable.AcceptChanges();
					}
					//Session["DominoServer"] = DSTaskSettingsDataTable;
					//DominoServerGridView.DataSource = dtTypes;
					//DominoServerGridView.DataBind();
				}
				//2/9/16 Sowjanya added for VSPLUS-2527
				StatusTable.Columns.Add("IssueCount", typeof(int));

				for (int i = 0; i < StatusTable.Rows.Count; i++)
				{
					DataTable count = new DataTable();
					count = VSWebBL.DashboardBL.DashboardBL.Ins.GetDevicecount(StatusTable.Rows[i]["Name"].ToString());
					
					if (count.Rows.Count > 0)
					{
						StatusTable.Rows[i]["IssueCount"] = count.Rows[0]["IssueCount"].ToString();
					}
					else
					{
						StatusTable.Rows[i]["IssueCount"] = "0";
					}
					//}
				}

				//Session["StatusTable"] = StatusTable;

                //4/19/16 Sowjanya added for VSPLUS-2863
                StatusTable.Columns.Add("MonitoredURL", typeof(string));

                for (int i = 0; i < StatusTable.Rows.Count; i++)
                {
                    DataTable count = new DataTable();
                    count = VSWebBL.DashboardBL.DashboardBL.Ins.GetMonitoredURL(StatusTable.Rows[i]["Name"].ToString());
                    if (count.Rows.Count > 0)
                    {
                        StatusTable.Rows[i]["MonitoredURL"] = "<b>Monitored URL: <b>" + count.Rows[0]["MonitoredURL"].ToString();
                    }
                   
                }

                Session["StatusTable"] = StatusTable;

				for (int i = 0; i < StatusTable.Rows.Count; i++)
				{
					string type = StatusTable.Rows[i]["Type"].ToString();
					string Name = StatusTable.Rows[i]["Name"].ToString();
					if (type == "WebSphere")
					{
						DataRow row = VSWebBL.DashboardBL.webSphereServerDetailsBL.Ins.GetWebSphereServerDetails(Name).Rows[0];

						// StatusTable.Rows[i]["OperatingSystem"] =" PID: "+processid;

						string Node = row["NodeName"].ToString();
						string Cell = row["CellName"].ToString();
						string Host = row["HostName"].ToString();
						string PID = row["ProcessId"].ToString();
						//string TotalText = "Node: " + Node + Environment.NewLine + "Cell: " + Cell + Environment.NewLine + "Host: " + Host + Environment.NewLine + "PID: " + PID;
						string TotalText = "Node: " + Node + "@" + "Cell: " + Cell + "@" + "Host: " + Host + "@" + "PID: " + PID;

						StatusTable.Rows[i]["OperatingSystem"] = TotalText.Replace("@", Environment.NewLine + "<br />");
						//StatusTable.Rows[i]["OperatingSystem"] = TotalText.Replace("@", ""+System.Environment.NewLine);



					}
				}
				DeviceGridView.DataSource = StatusTable;
				DeviceGridView.DataBind();
				((GridViewDataColumn)DeviceGridView.Columns["Type"]).GroupBy();
				//if (Session["ViewBy"] == "ServerType")
				//{
				//    ((GridViewDataColumn)DeviceGridView.Columns["Type"]).GroupBy();
				//}
				if (Session["ViewBy"] == "Location")
				{
					((GridViewDataColumn)DeviceGridView.Columns["Location"]).GroupBy();
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally
			{
			}
		}
		public void FillgridfromSession()
		{

			if (Session["StatusTable"] == null || Session["StatusTable"] == "")
			{
				try
				{
					DataTable StatusTable = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatus(status, typeloc, filterval, Session["ViewBy"].ToString(), MapLoc);
					//2/9/16 Sowjanya added for VSPLUS-2527
					StatusTable.Columns.Add("IssueCount", typeof(int));

					for (int i = 0; i < StatusTable.Rows.Count; i++)
					{
						DataTable count = new DataTable();
						count = VSWebBL.DashboardBL.DashboardBL.Ins.GetDevicecount(StatusTable.Rows[i]["Name"].ToString());
						if (count.Rows.Count > 0)
						{
							StatusTable.Rows[i]["IssueCount"] = count.Rows[0]["IssueCount"].ToString();
						}
						else
						{
							StatusTable.Rows[i]["IssueCount"] = "0";
						}
					}
                    //4/19/16 Sowjanya added for VSPLUS-2863
                    StatusTable.Columns.Add("MonitoredURL", typeof(string));

                    for (int i = 0; i < StatusTable.Rows.Count; i++)
                    {
                        DataTable count = new DataTable();
                         count = VSWebBL.DashboardBL.DashboardBL.Ins.GetMonitoredURL(StatusTable.Rows[i]["Name"].ToString());
                        if (count.Rows.Count > 0)
                        {
                            StatusTable.Rows[i]["MonitoredURL"] = count.Rows[0]["MonitoredURL"].ToString();
                        }
                       
                    }

                    
					Session["StatusTable"] = StatusTable;


				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
			}

			if (Session["StatusTable"] != "" && Session["StatusTable"] != null)
			{
				DataTable StatusTable = Session["StatusTable"] as DataTable;
				try
				{
					DeviceGridView.DataSource = StatusTable;
					DeviceGridView.DataBind();
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally
				{
				}
			}
		}
		protected string GetUrl(GridViewDataItemTemplateContainer Container)
		{
			object[] values = (object[])Container.Grid.GetRowValues(Container.VisibleIndex, new string[] { "Name", "Type" });
			return "DeviceChart.aspx?Name=" + values[0].ToString() + "&Type=" + values[1].ToString();
		}
		public string SetCircleStatus(GridViewDataItemTemplateContainer Container)
		{

			//ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
			//ASPxGridView grd = cph.FindControl("DeviceGridView") as ASPxGridView;
			HtmlTable tbl = (HtmlTable)Container.FindControl("tbl");
			Label pav = (Label)Container.FindControl("lblPendingActual");
			Label pth = (Label)Container.FindControl("lblPendingthresholdval");
			Label dav = (Label)Container.FindControl("lblDeadactual");
			Label dth = (Label)Container.FindControl("lblDeadthresholdval");
			Label hav = (Label)Container.FindControl("lblHeldactual");
			Label hth = (Label)Container.FindControl("lblHeldthresholdval");
			Label stype = (Label)Container.FindControl("lblservertype");
			Label sec = (Label)Container.FindControl("lblSecondaryRole");
			//Label CPU = (Label)Container.FindControl("lblCPU");
			if (stype.Text == "Exchange" || stype.Text == "BES" || sec.Text == "BES" || stype.Text == "Domino" || stype.Text == "Quickr" || sec.Text == "Quickr" || sec.Text == "Domino" || sec.Text == "Traveler")
			{
				string a = pth.Text;
				string b = pav.Text;
				double PendingActualValue = Convert.ToDouble(b == "" ? "0" : pav.Text);
				double PendingThresholdValue = Convert.ToDouble(a == "" ? "0" : pth.Text);
				double Deadactual = Convert.ToDouble(dav.Text == "" ? "0" : dav.Text);
				double Deadthresholdval = Convert.ToDouble(dth.Text == "" ? "0" : dth.Text);
				double Heldactual = Convert.ToDouble(hav.Text == "" ? "0" : hav.Text);
				double Heldthresholdval = Convert.ToDouble(hth.Text == "" ? "0" : hth.Text);

				// double CPUTH = Convert.ToDouble(CPU.Text == "" ? "0" : CPU.Text);
				tbl.Attributes.Add("OnMouseOver", "tblmouseover('" + stype.Text + "'," + PendingActualValue + "," + PendingThresholdValue + "," + Deadactual + "," + Deadthresholdval + "," + Heldactual + "," + Heldthresholdval + ")");
				tbl.Attributes.Add("OnMouseOut", "tblmouseout()");

				tbl.Rows[0].Cells[0].Style["background-color"] = (PendingActualValue < 0.85 * PendingThresholdValue ? "green" : (PendingActualValue >= 0.85 * PendingThresholdValue && PendingActualValue < PendingThresholdValue ? "yellow" : "red"));
				//lblPendingMail.Text = Mail + ": " + ActualValue.ToString() + "<br>";
				tbl.Rows[0].Cells[0].Visible = true;
				if (PendingActualValue == 0 && PendingThresholdValue == 0)
					tbl.Rows[0].Cells[0].Style["background-color"] = "green";



				tbl.Rows[0].Cells[1].Style["background-color"] = (Deadactual < 0.85 * Deadthresholdval ? "green" : (Deadactual >= 0.85 * Deadthresholdval && Deadactual < Deadthresholdval ? "yellow" : "red"));

				if (Deadactual == 0 && Deadthresholdval == 0)

					tbl.Rows[0].Cells[1].Style["background-color"] = "green";



				tbl.Rows[0].Cells[2].Style["background-color"] = (Heldactual < 0.85 * Heldthresholdval ? "green" : (Heldactual >= 0.85 * Heldthresholdval && Heldactual < Heldthresholdval ? "yellow" : "red"));
				if (Heldactual == 0 && Heldthresholdval == 0)
					tbl.Rows[0].Cells[2].Style["background-color"] = "green";

			}

			else
			{
				if (stype.Text == "Sametime")
				{
					tbl.Attributes.Add("OnMouseOver", "tblmouseover('" + stype.Text + "'");
				}

			}

			return "";
		}
		LinearScaleRangeBarComponent GetGaugeRangeBar(ASPxGaugeControl gaugeControl, int gaugeIndex, int rangeBarIndex)
		{
			return ((ILinearGauge)gaugeControl.Gauges[gaugeIndex]).RangeBars[rangeBarIndex];
		}
		private void CreateLinearGauge(ASPxGaugeControl gaugeControl, string CPUvalue, string CPUTHvalue)
		{
			// Creates a new instance of the ASPxGaugeControl class
			// with default settings.
			//ASPxGaugeControl gaugeControl = new ASPxGaugeControl();
			// Creates a new instance of the LinearGauge class and
			// adds it to the gauge control's Gauges collection.
			LinearGauge linearGauge = (LinearGauge)gaugeControl.AddGauge(GaugeType.Linear);
			linearGauge.AddDefaultElements();
			DevExpress.XtraGauges.Core.Model.LinearScaleBackgroundLayer bg = linearGauge.BackgroundLayers[0];
			linearGauge.Orientation = DevExpress.XtraGauges.Core.Model.ScaleOrientation.Horizontal;
			// Change the background layer's paint style.
			//bg.ShapeType = DevExpress.XtraGauges.Core.Model.BackgroundLayerShapeType.Linear_Style2;

			LinearScaleComponent scale = linearGauge.Scales[0]; //new LinearScaleComponent();//linearGauge.Scales[0];
			//linearGauge.Scales.Add(
			// Customize the scale's settings.

			scale.MinValue = 0;
			scale.MaxValue = 100;
			scale.MajorTickCount = 6;
			scale.MajorTickmark.FormatString = "{0:F0}";
			scale.MajorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Linear_Style6_3;
			scale.MinorTickCount = 3;
			scale.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Linear_Style5_2;
			// Shift tick marks to the right.
			scale.MajorTickmark.ShapeOffset = 6;
			scale.MinorTickmark.ShapeOffset = 6;
			scale.MinorTickmark.ShowTick = false;
			//SolidBrushObject sb=new SolidBrushObject();
			// sb

			scale.Value = float.Parse(CPUvalue);
			scale.AppearanceTickmarkText.TextBrush = new SolidBrushObject(Color.Transparent);
			//scale.v
			//<dx:LinearScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:Green&quot;/&gt;" 
			//                                                EndThickness="-15" EndValue="40" Name="Range0" ShapeOffset="-10" 
			//                                                StartThickness="-8" />

			//                                            <dx:LinearScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:Yellow&quot;/&gt;" 
			//                                                EndThickness="-20" EndValue="72.5" Name="Range1" ShapeOffset="-10" 
			//                                                StartThickness="-15" StartValue="40" />
			//                                            <dx:LinearScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:Red&quot;/&gt;" 
			//                                                EndThickness="-25" EndValue="100" Name="Range2" ShapeOffset="-10" 
			//                                                StartThickness="-20" StartValue="72.5" />
			if (float.Parse(CPUTHvalue) != 0)
			{
				LinearScaleRangeWeb range1 = new LinearScaleRangeWeb();
				range1.AppearanceRange.ContentBrush = new SolidBrushObject(Color.Green);
				//range1.EndThickness = -15; //15;//-15;
				range1.ShapeOffset = -10;//-10;
				range1.StartThickness = -8;
				range1.StartValue = 0;
				if (CPUTHvalue != "")
				{
					range1.EndValue = (float.Parse(CPUTHvalue) >= 10 ? float.Parse(CPUTHvalue) - 10 : 0);
					range1.EndThickness = float.Parse("-" + ((range1.EndValue / 100) * 40).ToString()); //15;//-15;
				}
				else
				{
					range1.EndValue = 0;
				}
				LinearScaleRangeWeb range2 = new LinearScaleRangeWeb();
				range2.AppearanceRange.ContentBrush = new SolidBrushObject(Color.Yellow);
				//range2.EndThickness = -25;//30;//-20;
				range2.ShapeOffset = -10;//-10;
				range2.StartThickness = range1.EndThickness; //15;//-15;
				if (CPUTHvalue != "")
				{
					range2.StartValue = (float.Parse(CPUTHvalue) >= 10 ? float.Parse(CPUTHvalue) - 10 : 0);
				}
				else
				{
					range2.StartValue = 0;
				}
				if (CPUTHvalue != "")
				{
					range2.EndValue = float.Parse(CPUTHvalue);
					range2.EndThickness = float.Parse("-" + (((range1.EndValue + 10) / 100) * 40).ToString());
				}
				else
				{
					range2.EndValue = 0;
				}


				LinearScaleRangeWeb range3 = new LinearScaleRangeWeb();
				range3.AppearanceRange.ContentBrush = new SolidBrushObject(Color.Red);
				range3.EndThickness = -40;//-50;
				range3.ShapeOffset = -10;//-10;
				range3.StartThickness = range2.EndThickness;//-25;//30;//-20;
				if (CPUTHvalue != "")
				{
					range3.StartValue = float.Parse(CPUTHvalue);
				}
				else
				{
					range3.StartValue = 0;
				}
				range3.EndValue = 100;
				//LinearScaleLevelComponentBuilder c = new LinearScaleLevelComponentBuilder();
				//c.
				//linearGauge.Markers=
				//linearGauge.Markers
				//LinearScaleLevelComponent level = linearGauge.Levels[0];
				//level.Update();

				//float.Parse(CPUvalue);

				scale.Ranges.AddRange(new DevExpress.XtraGauges.Core.Model.IRange[] { range3, range2, range1 });
			}
			else
			{
				LinearScaleRangeWeb range1 = new LinearScaleRangeWeb();
				range1.AppearanceRange.ContentBrush = new SolidBrushObject(Color.Green);
				range1.EndThickness = -40; //15;//-15;
				range1.ShapeOffset = -10;//-10;
				range1.StartThickness = -8;
				range1.StartValue = 0;
				//if (CPUTHvalue != "")
				//{
				//    range1.EndValue = 100;//(float.Parse(CPUTHvalue) >= 10 ? float.Parse(CPUTHvalue) - 10 : 0);
				//}
				//else
				//{
				range1.EndValue = 100;
				// }
				scale.Ranges.AddRange(new DevExpress.XtraGauges.Core.Model.IRange[] { range1 });
			}
			//linearGauge.Levels[0].ShapeType
			LinearScaleLevelComponent levelBar = linearGauge.Levels[0];


			levelBar.ShapeType = DevExpress.XtraGauges.Core.Model.LevelShapeSetType.Style3;


			// levelBar.Shapes.Add(
			//linearGauge.Levels.
			//linearGauge.Scales[0].com
			//linearGauge.Scales.Add(scale);

			// linearGauge.AutoSize = DevExpress.Utils.DefaultBoolean.True;

			// Change the levelBar's paint style.
			//LinearScaleLevelComponent levelBar = linearGauge.Levels[0];
			//levelBar.ShapeType = DevExpress.XtraGauges.Core.Model.LevelShapeSetType.Style4;
			// Shift the background layer up and to the left.

			//        bg.ScaleStartPos = new PointF2D(bg.ScaleStartPos.X - 0.001f,
			//bg.ScaleStartPos.Y - 0.005f);
			//        bg.ScaleEndPos = new PointF2D(bg.ScaleEndPos.X - 0.005f,
			//            bg.ScaleEndPos.Y);  
			bg.ScaleStartPos = new PointF2D(100, 300);//(bg.ScaleStartPos.X - 0.005f,                bg.ScaleStartPos.Y - 0.015f);
			bg.ScaleEndPos = new PointF2D(100, 0);//(bg.ScaleEndPos.X - 0.005f, bg.ScaleEndPos.Y);

			// Add the gauge control to the form.

			//gaugeControl.Width = 250;
			gaugeControl.Height = 50;
			//gaugeControl.Parent = this;
			//Page.Form.Controls.Add(gaugeControl);
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

            //4/8/2016 NS modified for VSPLUS-2750
            //6/52016 Durga Modified for VSPLUS-2894
			string[] serverTypes = { "Exchange", "Domino", "Quickr", "SharePoint", "Active Directory", "Windows" };

			/*if (stype.Text == "Exchange" || stype.Text == "Sametime" || sec.Text == "Sametime" || 
				stype.Text == "Domino" || stype.Text == "Quickr" || stype.Text == "SharePoint" || 
				sec.Text == "Quickr" || sec.Text == "Domino" || sec.Text == "Traveler" || 
				sec.Text == "SharePoint")*/
			if (serverTypes.Where(s => s == stype.Text).Count() > 0)
			{
				double CPUval = Convert.ToDouble(CPU.Text == "" ? "0" : CPU.Text);

				double CPUTHval = (Convert.ToDouble(CPUTH.Text == "" ? "0" : CPUTH.Text)) * 100;
				string cputhreshhold = CPUTHval.ToString();
				//12/12/2012 NS added - the page throws an error if the CPU.Text value is an empty string/NULL
				string cputxt = (CPU.Text == "" ? "0" : CPU.Text);
				//CreateLinearGauge(gaugecntrl, CPU.Text, cputhreshhold);
				//Below line commented by Mukund- VSPLUS-374. 12Feb14
				//CreateLinearGauge(gaugecntrl, cputxt, cputhreshhold);

				display.Text = CPUval + "/" + CPUTHval;
				//http://help.devexpress.com/#AspNet/CustomDocument5242
				lblCPUDetails.Text = "<br><br><b>" +
									"CPU Utilization: " + CPUval.ToString() + " %<br>" +
									(CPUTHval == 0 ? "No Threshold" : "Threshold:       " + CPUTHval.ToString() + " %") + "</b>";
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

		public string SetDisk(GridViewDataItemTemplateContainer Container)//VSPLUS-2284-MSRaj
		{
			DevExpress.Web.ASPxGridView grdview = (DevExpress.Web.ASPxGridView)Container.FindControl("DiskHealthGrid");
			//WebChartControl wc = (WebChartControl)Container.FindControl("DiskWebChart");

			HtmlTable tbl2 = (HtmlTable)Container.FindControl("tbl2");
			Label lblstatus = (Label)Container.FindControl("lblstatus2");
			Label stype = (Label)Container.FindControl("Type");
			Label srvrname = (Label)Container.FindControl("srvrname1");
			Label lblCPUDetails = (Label)Container.FindControl("lblCPUDetails");
            //4/8/2016 NS modified for VSPLUS-2750
			string[] serverTypes = { "Exchange", "Sametime", "Domino", "Quickr", "SharePoint", "Active Directory", "WebSphere", "Windows" };
			string servername = Convert.ToString(srvrname.Text == "" ? "null" : srvrname.Text);
			srvrname.Visible = false;
			Session["srvrname"] = servername;
			string Diskstatusvalue = Convert.ToString(lblstatus.Text == "" ? "null" : lblstatus.Text);
			DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetDeviceStatusdiskdetails(servername);
            //3/10/2016 Durga Modified for VSPLUS-2683
           
				grdview.DataSource = dt;
				grdview.DataBind();
			
			if (serverTypes.Where(s => s == stype.Text).Count() > 0)
			{
				if (Diskstatusvalue == "1")
				{
					tbl2.Visible = true;
					tbl2.Rows[0].Cells[0].Style["background-color"] = "Yellow";
					tbl2.Rows[0].Cells[0].InnerText = "Caution";
					tbl2.Rows[0].Cells[0].Style["color"] = "Blue";
				}
				else if (Diskstatusvalue == "2")
				{
					tbl2.Rows[0].Cells[0].Style["background-color"] = "LightGreen";
					tbl2.Rows[0].Cells[0].InnerText = "OK";
					tbl2.Rows[0].Cells[0].Style["color"] = "Blue";
				}
				else if (Diskstatusvalue == "0")
				{
                    //4/13/2016 NS modified for VSPLUS-2750
                    //The circle color will be Yellow for cases when the disk status is either Caution or Issue
                    //for consistency with other VS statuses
					//tbl2.Rows[0].Cells[0].Style["background-color"] = "Red";
                    tbl2.Rows[0].Cells[0].Style["background-color"] = "Yellow";
                    tbl2.Rows[0].Cells[0].Style["color"] = "Blue";
					tbl2.Rows[0].Cells[0].InnerText = "Issue";
				}
				else
				{
					tbl2.Rows[0].Cells[0].Style["background-color"] = "Gray";
					tbl2.Rows[0].Cells[0].Style["color"] = "White";
					tbl2.Rows[0].Cells[0].InnerText = "NM";
				}
				//http://help.devexpress.com/#AspNet/CustomDocument5242

				if (tbl2.Rows[0].Cells[0].InnerText == "NM")
				{
					lblCPUDetails.Visible = true;
					lblCPUDetails.Text = "<br><br>" +
									"<b>NM :</b> Not Monitored." + "<br>" +
									"<b> " + srvrname.Text + "<br>" +
									"</b>";
				}
				else
				{
					lblCPUDetails.Visible = true;
					lblCPUDetails.Text = "<br><br>" +
										"<b> " + srvrname.Text + "<br>" +
										"</b>";
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
        //3/10/2016 Durga Modified for VSPLUS-2683
        protected void DiskWebChart_Load(object sender, EventArgs e)//VSPLUS-2284-Somaraj
        {
            DataTable dt = new DataTable();
            WebChartControl chartControl = (WebChartControl)sender;
            DevExpress.Web.GridViewDataItemTemplateContainer gridc = (DevExpress.Web.GridViewDataItemTemplateContainer)chartControl.Parent;
            string srvname = DataBinder.Eval(gridc.DataItem, "ServerName").ToString();
            string diskname = DataBinder.Eval(gridc.DataItem, "DiskName").ToString();
            dt = VSWebBL.DashboardBL.DiskHealthBLL.Ins.SetGraph(srvname, diskname);
            chartControl.DataSource = dt;
            chartControl.Series[0].DataSource = dt;
            chartControl.Series[0].ArgumentDataMember = "DiskName";
            chartControl.Series[0].ValueDataMembers.AddRange("DiskUsed");
            chartControl.Series[0].Visible = true;
            chartControl.Series[1].DataSource = dt;
            chartControl.Series[1].ArgumentDataMember = "DiskName";
            chartControl.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            chartControl.Series[1].Visible = true;
            chartControl.DataBind();
        }
		public string GetDiskInfo(GridViewDataItemTemplateContainer Container)
		{

			Label diskname = (Label)Container.FindControl("hfNameLabel22");

			//Label comments = (Label)Container.FindControl("lblReasons");
			Label detailsspan = (Label)Container.FindControl("detailsspan");
			//System.Web.UI.WebControls.Image Imgforinfo = (System.Web.UI.WebControls.Image)Container.FindControl("Imgforinfo");
			//	System.Drawing.Image Imgforinfo = (System.Drawing.Image)Container.FindControl("Imgforinfo");
			//Image Imgforinfo = (Image)Container.FindControl("");
			System.Web.UI.HtmlControls.HtmlImage Imgforinfo = (System.Web.UI.HtmlControls.HtmlImage)Container.FindControl("Imgforinfo");

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

            //4/8/2016 NS modified for VSPLUS-2750
            //6/52016 Durga Modified for VSPLUS-2894
			string[] serverTypes = { "Exchange","Domino", "Quickr", "SharePoint", "Active Directory", "Windows" };

			/*if (stype.Text == "Exchange" || stype.Text == "Sametime" || sec.Text == "Sametime" || 
				stype.Text == "Domino" || stype.Text == "Quickr" || stype.Text == "SharePoint" || 
				sec.Text == "Quickr" || sec.Text == "Domino" || sec.Text == "Traveler" || 
				sec.Text == "SharePoint")*/
			if (serverTypes.Where(s => s == stype.Text).Count() > 0)
			{
				double Memval = Convert.ToDouble(Mem.Text == "" ? "0" : Mem.Text);

				double MemTHval = (Convert.ToDouble(MemTH.Text == "" ? "0" : MemTH.Text)) * 100;
				string Memthreshhold = MemTHval.ToString();
				//12/12/2012 NS added - the page throws an error if the Mem.Text value is an empty string/NULL
				string Memtxt = (Mem.Text == "" ? "0" : Mem.Text);
				//CreateLinearGauge(gaugecntrl, Mem.Text, Memthreshhold);
				//Below line commented by Mukund- VSPLUS-374. 12Feb14
				//CreateLinearGauge(gaugecntrl, Memtxt, Memthreshhold);

				display.Text = Memval + "/" + MemTHval;
				//http://help.devexpress.com/#AspNet/CustomDocument5242
				lblMemDetails.Text = "<br><br><b>" +
									"Memory Utilization: " + Memval.ToString() + " %<br>" +
									(MemTHval == 0 ? "No Threshold" : "Threshold:       " + MemTHval.ToString() + " %") + "</b>";
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
		public string SetStatus(GridViewDataItemTemplateContainer Container)
		{
			DevExpress.Web.ASPxGauges.ASPxGaugeControl gaugecntrl = (DevExpress.Web.ASPxGauges.ASPxGaugeControl)Container.FindControl("gControl_Page3");
			//Label stype = (Label)Container.FindControl("lblservertype");
         
			Label NameLabel = (Label)Container.FindControl("NameLabel");
			Label stype = (Label)Container.FindControl("LabelType");
			Label lblResponseTime = (Label)Container.FindControl("lblResponseTime");
			lblResponseTime.Text = "New Response Time: " + lblResponseTime.Text + "<br>";
			Label lblUserCount = (Label)Container.FindControl("lblUserCount");
			lblUserCount.Text = "Users: " + lblUserCount.Text + "<br><br>";
			Label lblDownMinutes = (Label)Container.FindControl("lblDownMinutes");
			string strDownMinutes = lblDownMinutes.Text;
			lblDownMinutes.Text = "Elapsed DownTime: " + lblDownMinutes.Text + " mins<br><br>";
			Label lblPendingMail = (Label)Container.FindControl("lblPendingMail");
			Label lblPendingPercent = (Label)Container.FindControl("lblPendingPercent");
			Label lblPendingThreshold = (Label)Container.FindControl("lblPendingThreshold");
            Label lblDeadMail = (Label)Container.FindControl("lblDeadMail");
            Label lblDeadPercent = (Label)Container.FindControl("lblDeadPercent");
            Label lblDeadThreshold = (Label)Container.FindControl("lblDeadThreshold");
            Label lblHeldMail = (Label)Container.FindControl("lblHeldMail");
            Label lblHeldPercent = (Label)Container.FindControl("lblHeldPercent");
            Label lblHeldMailThreshold = (Label)Container.FindControl("lblHeldMailThreshold");
            //2/5/2016 Sowjanya modified forVSPLUS-2906
            if (stype.Text == "Domino" || stype.Text == "Office365")
            {
                lblPendingMail.Text = "Pending Mail: " + lblPendingMail.Text + " = " + lblPendingPercent.Text + "% of Threshold " + lblPendingThreshold.Text + "<br>";
                lblDeadMail.Text = "Dead Mail: " + lblDeadMail.Text + " = " + lblDeadPercent.Text + "% of Threshold " + lblDeadThreshold.Text + "<br>";
                lblHeldMail.Text = "Held Mail: " + lblHeldMail.Text + " = " + lblHeldPercent.Text + "% of Threshold " + lblHeldMailThreshold.Text + "<br><br>";
            }
            else
            {

                lblPendingMail.Text = "Submission Queues: " + "<br>" + lblPendingMail.Text + " = " + lblPendingPercent.Text + "% of Threshold " + lblPendingThreshold.Text + "<br>";
                lblDeadMail.Text = "Unreachable Queues: " + "<br>" + lblDeadMail.Text + " = " + lblDeadPercent.Text + "% of Threshold " + lblDeadThreshold.Text + "<br>";
                lblHeldMail.Text = "Shadow Queues: " + "<br>" + lblHeldMail.Text + " = " + lblHeldPercent.Text + "% of Threshold " + lblHeldMailThreshold.Text + "<br><br>";
            }
			//2/9/16 Sowjanya added for VSPLUS-2527
			Label lblIssueCount = (Label)Container.FindControl("lblIssueCount");
			lblIssueCount.Text = lblIssueCount.Text;
            //4/19/16 Sowjanya added for VSPLUS-2863
            Label lblMonitoredURL = (Label)Container.FindControl("lblMonitoredURL");
            lblMonitoredURL.Text = "<b>Monitored URL: <b>" + lblMonitoredURL.Text;
			// if (lblDownMinutes.Text == "Elapsed DownTime: 0 mins<br><br>")
			if (strDownMinutes == "" || strDownMinutes == "0")
			{
				
				lblResponseTime.Visible = true;
				lblUserCount.Visible = true;
				lblPendingMail.Visible = true;
				lblDeadMail.Visible = true;
				lblHeldMail.Visible = true;
              
			}
			else
			{
				lblDownMinutes.Visible = true;
			}
            //4/19/2016 NS modified for VSPLUS-2750
          
            if (stype.Text != "Domino" && stype.Text != "Exchange" && stype.Text != "Office365")
			{
				lblPendingMail.Visible = false;
				lblDeadMail.Visible = false;
				lblHeldMail.Visible = false;
			}
			//2/18/16 Sowjanya added for VSPLUS-2619
			if (stype.Text == "Cloud")
			{
      
				lblUserCount.Visible = false;
                lblMonitoredURL.Visible = true;
              
			}
			return "";
		}
		protected void DeviceGridView_SelectionChanged(object sender, EventArgs e)
		{
			if (IsPostBack)
			{


				if (DeviceGridView.Selection.Count > 0)
				{
					System.Collections.Generic.List<object> Type = this.DeviceGridView.GetSelectedFieldValues("Name");
					System.Collections.Generic.List<object> ServerType = this.DeviceGridView.GetSelectedFieldValues("Type");
					System.Collections.Generic.List<object> LastDate = this.DeviceGridView.GetSelectedFieldValues("LastUpdate");
					//9/1/2015 NS added for VSPLUS-2096
					System.Collections.Generic.List<object> SecRole = this.DeviceGridView.GetSelectedFieldValues("SecondaryRole");

					System.Collections.Generic.List<object> Location = this.DeviceGridView.GetSelectedFieldValues("Location");
					//3/22/2014 NS added
					//System.Collections.Generic.List<object> ID = DeviceGridView.GetSelectedFieldValues("ID");
					//9/19/2014 NS added
					System.Collections.Generic.List<object> Status = this.DeviceGridView.GetSelectedFieldValues("Status");
					if (Type.Count > 0)
					{
						string Name = Type[0].ToString();
						string SType = ServerType[0].ToString();
						string LastUpdate = LastDate[0].ToString();
						//9/19/2014 NS added
						string SStatus = Status[0].ToString();
						string location = Location[0].ToString();
						//9/1/2015 NS added for VSPLUS-2096
						string secrole = SecRole[0].ToString();
						// Session["Type"] = Type[0];
						//MD Notes 20Feb14
						if (!(Session["UserFullName"] != null && Session["UserFullName"].ToString() == "Anonymous"))
						{
							//Mukund: VSPLUS-844, Page redirect on callback
							try
							{
                                if (SType == "Office365")
								{
									DevExpress.Web.ASPxWebControl.RedirectOnCallback("Office365Health.aspx?Type=" + SType + "&Name=" + Name);
									Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								}
                                //3/16/2016 NS added for VSPLUS-2716
                                //4/11/2016 Sowjanya modified for VSPLUS-2813
                                else if (SType == "IBM Connections")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("ConnectionsDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "NotesMail Probe")
                                {
                                    //11/7/2014 NS modified for VSPLUS-1153
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("NotesMailProbeDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "Exchange") //MD exchange Dec13
                                {
                                    //9/30/2014 NS modified
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("ExchangeServerDetailsPage3.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "Skype for Business")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("Lyncdetailspage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                //3/22/2014 NS added
                                //else if (SType == "Exchange DAG")
                                //else if (SType == "Database Availability Group")
                                // {
                                // DevExpress.Web.ASPxWebControl.RedirectOnCallback("DAGDetail.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&ID=" + ID + "");
                                //}
                                //19/06/2014
                                else if (SType == "BES")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("BlackBerryServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "Exchange Mail Flow")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("ExchangeMailProbeDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "SharePoint")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("Sharepointdetailspage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                //1/30/2015 NS modified - DAGDetail page is outdated
                                else if (SType == "Database Availability Group")
                                {
                                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DAGDetail.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&ID=" + ID + "");
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("DAGHealth.aspx?Name=" + Name + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }
                                else if (SType == "Windows")
                                {

                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("WindowsServerDetails.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }
                                else if (SType == "Active Directory")
                                {
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("ActiveDirectoryServerDetailsPage3.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }
                                else if (SType == "Network Device")
                                {

                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("NetworkServerDetails.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }

                                else if (SType == "SNMP Device")
                                {

                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("SNMPDeviceDetails.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }
                                else if (SType == "URL")
                                {

                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("URLDetails.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }
                                else if (SType == "WebSphere")
                                {
                                    string details = DeviceGridView.GetSelectedFieldValues("Details")[0].ToString();
                                    if (details == "WebSphere Node" || details == "WebSphere Cell")
                                    {
                                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("WebsphereServerHealth.aspx");
                                        Context.ApplicationInstance.CompleteRequest();
                                    }
                                    else
                                    {
                                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("WebSphereServerDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&LastDate=" + LastUpdate + "&Status=" + SStatus + "");
                                        Context.ApplicationInstance.CompleteRequest();
                                    }
                                }

                                else if (SType == "Sametime")
                                {

                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("SametimeServerDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");

                                    Context.ApplicationInstance.CompleteRequest();
                                }

                                else if (SType == "Domino")
                                {
                                    //9/1/2015 NS added for VSPLUS-2096
                                    if (secrole == "Traveler")
                                    {
                                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "&Type=" + secrole + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    }
                                    else
                                    {
                                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    }
                                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DefaultDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    Context.ApplicationInstance.CompleteRequest();
                                }
                                else
                                {
                                    //9/19/2014 NS modified
                                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("DefaultDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                                    Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                                }



								if (SType == "Office365")
								{
									DevExpress.Web.ASPxWebControl.RedirectOnCallback("Office365Health.aspx?Type=" + SType + "&Name=" + Name);
									Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								}

							}
							catch (Exception ex)
							{
								Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
								//throw ex;
							}
						}
					}


				}
			}

		}
		protected void DiskHealthGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)//VSPLUS-2284-MSRaj
		{
			ASPxGridView gv = sender as ASPxGridView;
			GridViewDataColumn col = (GridViewDataColumn)DeviceGridView.Columns["Free"];
			GridViewDataColumn col1 = (GridViewDataColumn)DeviceGridView.Columns["Size"];
			GridViewDataColumn col2 = (GridViewDataColumn)DeviceGridView.Columns["Threshold(GB)"];
			Label redthreshold = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hdlblredthrvalue") as Label;
			Label diskfree = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hdlbldiskfree") as Label;
			Label hdlbldsksize = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hdlbldsksize") as Label;
			Label diskname = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hdlbldskname") as Label;
			Label yellowthreshold = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hdlblyellowthrvalue") as Label;
			//WebChartControl wc = sender as WebChartControl;

			if (diskfree != null && redthreshold != null && yellowthreshold != null)
			{
				double dikfree2 = Convert.ToDouble(diskfree.Text);
				double redthreshold2 = Convert.ToDouble(redthreshold.Text);
				double yellowthreshold2 = Convert.ToDouble(yellowthreshold.Text);
				string diskname2 = Convert.ToString(diskname.Text);
				if (dikfree2 > yellowthreshold2)
				{
					e.Cell.BackColor = System.Drawing.Color.Green;
					e.Cell.ForeColor = System.Drawing.Color.White;
					diskname.ForeColor = System.Drawing.Color.White;
				}
				else if ((redthreshold2 < dikfree2) && (dikfree2 < yellowthreshold2))
				{
					e.Cell.BackColor = System.Drawing.Color.Yellow;
					e.Cell.ForeColor = System.Drawing.Color.Blue;
					diskname.ForeColor = System.Drawing.Color.Blue;
				}
				else if (dikfree2 <= redthreshold2)
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ForeColor = System.Drawing.Color.White;
					diskname.ForeColor = System.Drawing.Color.White;
				}
			}
		}
		protected void DeviceGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			ASPxGridView gv = sender as ASPxGridView;

			Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfNameLabel2") as Label;
			Label dsksts = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "detailsLabel") as Label;

			if (e.DataColumn.FieldName == "Diskstatus" && dsksts.Text.ToString() == "1")
			{
				//e.DataColumn.CellStyle.CssClass = "GridCss3";
				dsksts.Text = "Yellow";
				e.Cell.Text = "Caution";
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				e.Cell.CssClass = "circle4";
				e.Cell.ForeColor = System.Drawing.Color.Blue;
			}
			else if (e.DataColumn.FieldName == "Diskstatus" && dsksts.Text.ToString() == "2")
			{
				dsksts.Text = "Green";
				e.Cell.Text = "OK";
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Blue;
				e.Cell.CssClass = "circle4";
			}
			else if ((e.DataColumn.FieldName == "Diskstatus" && dsksts.Text.ToString() == "0"))
			{
				dsksts.Text = "Red";
				e.Cell.Text = "Issue";
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
				e.Cell.CssClass = "circle4";
			}
			else if ((e.DataColumn.FieldName == "Diskstatus" && dsksts.Text.ToString() == ""))
			{
				dsksts.Text = "";
				e.Cell.Text = "";
				//e.Cell.BackColor = System.Drawing.Color.White;

				e.Cell.CssClass = "";
			}
			if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Responding")
			{
				//e.DataColumn.CellStyle.CssClass = "GridCss3";
				e.Cell.CssClass = "GridCss3";
			}

			//6/10/2013 NS modified - added Telnet status
			if (e.DataColumn.FieldName == "Status" && (hfStatus.Text.ToString() == "OK" || hfStatus.Text.ToString() == "Scanning"))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}

			else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Responding")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
				//e.DataColumn.CellStyle.CssClass = "GridCss3";
			}
			else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Scanned")
			{

				e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			//3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
			else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Disabled")
			{
				e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
				// e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Maintenance")
			{
				e.Cell.BackColor = System.Drawing.Color.LightBlue;

			}
			else if (e.DataColumn.FieldName == "Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				// e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

			}

		}
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
					DeviceGridView.CollapseAll();
					CollapseButton.Image.Url = "~/images/icons/add.png";
					CollapseButton.Text = "Expand All Rows";
				}
				else
				{
					DeviceGridView.ExpandAll();
					CollapseButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseButton.Text = "Collapse All Rows";

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public void expandButton()
		{

			try
			{

				if (CollapseButton.Text == "Collapse All Rows")
				{
					DeviceGridView.ExpandAll();
					//CollapseButton.Image.Url = "~/images/icons/add.png";
					//CollapseButton.Text = "Expand All Rows";
				}
				else
				{
					DeviceGridView.CollapseAll();
					//CollapseButton.Image.Url = "~/images/icons/forbidden.png";
					//CollapseButton.Text = "Collapse All Rows";

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		private void ExpandGrid()
		{
			//if (CollapseButton.Text == "Collapse All Rows")
			//{
			//    DeviceGridView.CollapseAll();
			//    CollapseButton.Image.Url = "~/images/icons/add.png";
			//    CollapseButton.Text = "Expand All Rows";
			//}
			//else
			//{
			DeviceGridView.ExpandAll();
			CollapseButton.Image.Url = "~/images/icons/forbidden.png";
			CollapseButton.Text = "Collapse All Rows";

			//}
		}
		protected void ExpandButton_Click(object sender, EventArgs e)
		{
			DeviceGridView.ExpandAll();
		}
		protected void DeviceGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			if (e.RowType == GridViewRowType.Data)
			{
				ASPxGridView gv = (ASPxGridView)sender;
				GridViewDataColumn col = (GridViewDataColumn)DeviceGridView.Columns["Status"];
				if (col != null)
				{
					for (int i = 0; i < gv.DetailRows.VisibleCount; i++)
					{

						if (gv.GetDataRow(i).ItemArray[1] == "Not Responding")
						{
							e.Row.Cells[i].CssClass = "GridCss3";
						}
						else
						{
							e.Row.Cells[i].CssClass = "GridCss1";
						}
						//e.Row.Cells[i].Style.Remove("background-color"); //.Add( "background-color", " ");
					}
				}
				// Remove your cell style here
				//GridViewRow s = (GridViewRow)e.Row;
				//12/12/2012 NS modified - commented out the lines below e.Row.Cells[3] due to errors in IE - 'oDiv is undefined'

				//e.Row.Cells[3].Attributes.Add("onmouseover", "oDiv.style.removeProperty('background-color');");
				//Add your cell style here
				//e.Row.Cells[3].Attributes.Add("onmouseout", "oDiv.style.background-color=#FFFF");

				//GridViewDataColumn col = (GridViewDataColumn)DeviceGridView.Columns["Status"];
				//if (col != null)
				//{
				//    for (int i = 0; i < e.Row.Cells.Count; i++)
				//    {

				//        e.Row.Cells[i].Style.Remove("background-color"); //.Add( "background-color", " ");
				//    }

				//    //e.Row.Cells[3].Attributes.Add("onmouseover", "oDiv.style.removeProperty('background-color');");
				//    ////Add your cell style here
				//    //e.Row.Cells[3].Attributes.Add("onmouseout", "oDiv.style.background-color=#FFFF");
				//}
				e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

				e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

			}
		}
		protected void DeviceGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{

			//if (e.Row.RowType == DataControlRowType.DataRow)
			//{
			//    // Remove your cell style here
			//    //GridViewRow s = (GridViewRow)e.Row;
			//    e.Row.Cells[3].Attributes.Add("onmouseover", "oDiv.style.removeProperty('background-color');");
			//    //Add your cell style here
			//    e.Row.Cells[3].Attributes.Add("onmouseout", "oDiv.style.background-color=#FFFF");

			//    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //this.style.backgroundColor='#C0C0C0';");
			//    //if(e.Row.BackColor!= System.Drawing.Color.White)
			//    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
			//}
		}
		protected void StatusListMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

		}
		protected void StatusListPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			//VSPLUS-766, Mukund, 15Sep14 
			ErrorMsg.InnerHtml = "";
			ErrorMsg.Style.Value = "display: none";
			string name;
			ScriptManager.RegisterStartupScript(this, GetType(), "DeviceGridView_ContextMenu", "DeviceGridView_ContextMenu(s,e);", true);
			string sScript = "";
			sScript = sScript + "function DeviceGridView_ContextMenu(s,e) {";
			sScript = sScript + "if (e.objectType == 'row') {" +
				"s.GetRowValues(e.index, 'Type');}";
			sScript = sScript + "}";
			//if (Request.QueryString["server"] != "" && Request.QueryString["server"] != null)
			//{
			//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();

			if (e.Item.Name == "ScanNow")
			{
				if (DeviceGridView.FocusedRowIndex > -1)
				{
					myRow = DeviceGridView.GetDataRow(DeviceGridView.FocusedRowIndex);
					Session["myRow"] = myRow;
					try
					{
						Status StatusObj = new Status();
						StatusObj.Name = myRow["Name"].ToString();
						StatusObj.Type = myRow["Type"].ToString();

						bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

						if (StatusObj.Type == "Mail")
						{
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
						}
						else if (StatusObj.Type == "Sametime Server")
						{
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
						}
						else if (StatusObj.Type == "BES")
						{
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
						}
						else if (StatusObj.Type == "Database Availability Group")
						{
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDAGASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
						}
						else
						{
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("Scan" + StatusObj.Type.Replace(" ", "") + "ASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
						}


					}
					catch (Exception ex)
					{
						//myUserName = "";
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
				}
				else
				{
					msgPopupControl.HeaderText = "Scan Now";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "EditConfigurator")
			{
				if (Session["Isconfigurator"] == null)
				{
					msgPopupControl.HeaderText = "Edit Configurator";
					msgLabel.Text = "Access Denied.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
				else if (Session["Isconfigurator"].ToString() == "False")
				{
					msgPopupControl.HeaderText = "Edit Configurator";
					msgLabel.Text = "Access Denied.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
				else if (DeviceGridView.FocusedRowIndex > -1)
				{
					myRow = DeviceGridView.GetDataRow(DeviceGridView.FocusedRowIndex);
					Session["myRow"] = myRow;
					try
					{

						//1/2/2014 NS added

						if (myRow["Name"].ToString() != "" && myRow["Name"].ToString() != null)
						{
							name = myRow["Name"].ToString();
                            string Loc = "", Cat = "", IPaddr = "", NodeName = string.Empty, CellName = string.Empty;
                            DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);

                            //5/3/2016 Sowjanya added for VSPLUS-2896
                            DataTable dt1 = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByWSName(name);
                            if (dt1.Rows.Count > 0)
                            {
                                NodeName = dt1.Rows[0]["NodeName"].ToString();
                                CellName = dt1.Rows[0]["CellName"].ToString();
                            }
                            //Mukund 10Sep14 VE-70
							if (dt.Rows.Count > 0)
							{
								id = dt.Rows[0]["ID"].ToString();
								Loc = dt.Rows[0]["Location"].ToString();
								Cat = dt.Rows[0]["ServerType"].ToString();
								IPaddr = dt.Rows[0]["ipaddress"].ToString();

							}
							Session["Submenu"] = "";
							if (myRow["Type"].ToString() == "BES")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "BlackBerry";
								Response.Redirect("~/Configurator/BlackBerryEntertpriseServer.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Sametime")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/SametimeServer.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Exchange")
							{
								//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/ExchangeServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + Cat + "&Loc=" + Loc + "&ipaddr=" + IPaddr, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Skype for Business")
							{
								//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/LyncServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + Cat + "&Loc=" + Loc, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "SharePoint")
							{
								//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/SharepointServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + Cat + "&Loc=" + Loc, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Mail")
							{
								id = (VSWebBL.ConfiguratorBL.MailServicesBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "Mail";
								Response.Redirect("~/Configurator/MailService.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "URL")
							{
								id = (VSWebBL.ConfiguratorBL.URLsBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/URLProperties.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Network Device")
							{
								id = (VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Response.Redirect("~/Configurator/NetworkDeviceProperties.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Notes Database")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "LotusDomino";
								Response.Redirect("~/Configurator/EditNotes.aspx?ID=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "NotesMail Probe")
							{
								//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "Mail";
								Response.Redirect("~/Configurator/EditNotesMailProbe.aspx?Name=" + myRow["Name"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Exchange Mail Flow")
							{
								//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "Mail";
								Response.Redirect("~/Configurator/EditExchangeMailProbe.aspx?Name=" + myRow["Name"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Windows")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "Windows";
								Response.Redirect("~/Configurator/WindowsProperties.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							else if (myRow["Type"].ToString() == "Office365")
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetO365ServerIDbyServerName(myRow["Name"].ToString())).ToString();
								Session["Submenu"] = "Office365";
								string mode = "Update";
								Response.Redirect("~/Configurator/O365ServerProperties.aspx?ID=" + id + "&Name=" + name + "&Mode="+mode, false);
								Context.ApplicationInstance.CompleteRequest();
							}
                            //5/3/2016 Sowjanya added for VSPLUS-2896
                            else if (myRow["Type"].ToString() == "IBM Connections")
                            {
                                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
                                Response.Redirect("~/Configurator/IBMConnections.aspx?ID=" + id, false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else if (myRow["Type"].ToString() == "Active Directory")
                            {
                                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
                                Response.Redirect("~/Configurator/ActiveDirectoryProperties.aspx?ID=" + id + "&Name=" + name + "&Loc=" + dt.Rows[0]["Location"].ToString(), false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else if (myRow["Type"].ToString() == "WebSphere")
                            {
                                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
                                Response.Redirect("~/Configurator/WebSphereProperties.aspx?ID=" + id + "&Name=" + name + "&Loc=" + dt.Rows[0]["Location"].ToString() + "&NodeName=" + dt1.Rows[0]["NodeName"].ToString() + "&CellName=" + dt1.Rows[0]["CellName"].ToString(), false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
						//if (myRow["Type"].ToString() == "Domino")
							else
							{
								id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
								
								Response.Redirect("~/Configurator/DominoProperties.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
								Context.ApplicationInstance.CompleteRequest();
							}
							//else 
						}
					}
					catch (Exception ex)
					{
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
				}
				else
				{
					msgPopupControl.HeaderText = "Edit Configurator";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "Suspend")
			{
				if (DeviceGridView.FocusedRowIndex > -1)
				{
					myRow = DeviceGridView.GetDataRow(DeviceGridView.FocusedRowIndex);
					Session["myRow"] = myRow;

					//VSPLUS-766, Mukund, 15Sep14 , Stopping click for NotesMail Probe
					if (myRow["Type"].ToString() != "NotesMail Probe")
					{
						SuspendPopupControl.HeaderText = "Suspend Temporarily";
						//ASPxLabel9.Text = myRow["Name"].ToString();
						SuspendPopupControl.ShowOnPageLoad = true;

					}
					else
					{
						//10/3/2014 NS modified for VSPLUS-990
						ErrorMsg.InnerHtml = "The option 'Suspend Temporarily' does not apply to NotesMail Probes." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						ErrorMsg.Style.Value = "display: block";
					}
				}
				else
				{
					msgPopupControl.HeaderText = "Suspend Temporarily";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}

			//10/1/2014 NS added for VSPLUS-683
			if (e.Item.Name == "SendConsoleCommand")
			{

				if (IsPostBack)
				{
					//id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(myRow["Name"].ToString())).ToString();
					if (DeviceGridView.FocusedRowIndex > -1)
					{
						myRow = DeviceGridView.GetDataRow(DeviceGridView.FocusedRowIndex);
						//myRow = DeviceGridView.GetDataRow(this.DeviceGridView.ScrollToVisibleIndexOnClient);
						Session["myRow"] = myRow;
						if (myRow["Type"].ToString() == "Domino")
						{
							ConsoleCmdPopupControl.HeaderText = "Send Console Command";
							ConsoleCmdPopupControl.ShowOnPageLoad = true;
						}
						else
						{
							ErrorMsg.InnerHtml = "The action 'Send Console Command' is applicable only to Domino servers." +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							ErrorMsg.Style.Value = "display: block";
						}
					}
				}
				else
				{
					msgPopupControl.HeaderText = "Send Console Command";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
		}
		protected void BtnApply_Click(object sender, EventArgs e)
		{
			try
			{
				DataRow row = (DataRow)Session["myRow"];
				if (row != null)
				{
					List<string> serverIDValues = new List<string>();
					List<string> servertypeIDValues = new List<string>();
					Random random = new Random((int)DateTime.Now.Ticks);
					//RandomString(5, random);
					//string Name = row["Name"].ToString() + "-Temp-" + DateTime.Now.ToLongDateString();
					string Name = row["Name"].ToString() + "-Temp-" + DateTime.Now.ToString();//.ToLongDateString();

					string StartDate = DateTime.Now.ToShortDateString();
					string StartTime = DateTime.Now.ToShortTimeString();
					DateTime sdt = Convert.ToDateTime(StartDate);
					string Duration = TbDuration.Text;
					string Durationtype = "";
					string EndDate = DateTime.Now.ToShortDateString();
					DateTime edt = Convert.ToDateTime(EndDate);
					string MaintType = "1";
					string MaintDaysList = "";
					string altime = DateTime.Now.ToShortTimeString();
					DateTime al = Convert.ToDateTime(altime);
					ASPxScheduler sh = new ASPxScheduler();
					Appointment apt = sh.Storage.CreateAppointment(AppointmentType.Pattern);
					Reminder r = apt.CreateNewReminder();

					//int min = Convert.ToInt32(MaintDurationTextBox.Text);
					int min = Convert.ToInt32(TbDuration.Text);
					r.AlertTime = al.AddMinutes(min);
					//3/24/2015 NS modified for DevExpress upgrade 14.2
					//ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r, DateSavingType.LocalTime);
					ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r);
					string rem = reminderHelper.ToXml().ToString();

					RecurrenceInfo reci = new RecurrenceInfo();
					reci.BeginUpdate();
					reci.AllDay = false;
					reci.Periodicity = 10;
					reci.Range = RecurrenceRange.EndByDate;
					reci.Start = sdt;
					reci.End = edt;
					reci.Duration = edt - sdt;
					reci.Type = RecurrenceType.Yearly;


					OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(reci);
					TimeInterval ttc = new TimeInterval(reci.Start, reci.End + new TimeSpan(1, 0, 0));


					var bcoll = calc.CalcOccurrences(ttc, apt);
					if (bcoll.Count != 0)
					{
						reci.OccurrenceCount = bcoll.Count;
					}
					else
					{
						reci.OccurrenceCount = 1;
					}
					reci.Range = RecurrenceRange.OccurrenceCount;
					reci.EndUpdate();
					string s = reci.ToXml();

					string EndDateIndicator = "";
					DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(row["Name"].ToString());
					if (dt != null && dt.Rows.Count > 0)
					{
						//VSPLUS-833:Suspend temporarily is not working correctly 
						//19Jul14, Mukund, The below two were reversely assigned so suspend wasnt working. 
						servertypeIDValues.Add(dt.Rows[0][2].ToString());
						serverIDValues.Add(dt.Rows[0][0].ToString());
					}
					bool update = false;
					if (servertypeIDValues != null && servertypeIDValues.Count > 0)
					{
						update = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.UpdateMaintenanceWindows(null, Name, StartDate, StartTime, Duration,
							  EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true", "1");
					}
					if (update == true)
					{
						//10/3/2014 NS modified for VSPLUS-990
						SuccessMsg.InnerHtml = "Monitoring for " + row["Name"].ToString() + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						SuccessMsg.Style.Value = "display: block";

					}
					else
					{
						//10/3/2014 NS modified for VSPLUS-990
						ErrorMsg.InnerHtml = "The Settings were NOT updated." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						ErrorMsg.Style.Value = "display: block";
					}
					SuspendPopupControl.ShowOnPageLoad = false;
				}
				else
				{
					msgPopupControl.HeaderText = "Suspend Temporarily";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		protected void DeviceGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DeviceTypeList|DeviceGridView", DeviceGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		//10/1/2014 NS added for VSPLUS-683
		protected void ConsoleCmdButton_Click(object sender, EventArgs e)
		{
			try
			{
				DataRow row = (DataRow)Session["myRow"];
				if (row != null)
				{
					bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(row["Name"].ToString(), ConsoleCmdTextBox.Text, Session["UserFullName"].ToString());
					if (returnval == true)
					{
						SuccessMsg.InnerHtml = "Console command for " + row["Name"].ToString() + " has been submitted." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						SuccessMsg.Style.Value = "display: block";
					}
					else
					{
						ErrorMsg.InnerHtml = "Console command for " + row["Name"].ToString() + " has NOT been submitted. Make sure you have sufficient rights to perform this operation." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						ErrorMsg.Style.Value = "display: block";
					}
				}
				ConsoleCmdPopupControl.ShowOnPageLoad = false;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		protected void Rrport_Click(object sender, EventArgs e)
		{
			try
			{
				//Response.Redirect("ScanNowItems.aspx");

			}
			catch (Exception ex)
			{

			}
			finally { }

		}
		protected void DataBound(object sender, EventArgs e)
		{
			SetItemCount();
		}
		protected void PreRender(object sender, EventArgs e)
		{
			SetItemCount();
		}
		protected void BeforeGetCallbackResult(object sender, EventArgs e)
		{
			SetItemCount();
		}
		void SetItemCount()
		{
			int itemCount = (int)DeviceGridView.GetTotalSummaryValue(DeviceGridView.TotalSummary["Name"]);
			DeviceGridView.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount.ToString() + " items)";
		}
		object GetDataSource(int count)
		{
			List<object> result = new List<object>();
			for (int i = 0; i < count; i++)
				result.Add(new { TypeandName = i, Name = "Name_" + i });
			return result;
		}

        //2/11/2016 NS added for VSPLUS-2568
        protected void DeviceGridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {
                if (e.Value.ToString() == "Office365")
                {
                    e.DisplayText = "Office 365";
                }
            }
        }

	}


}