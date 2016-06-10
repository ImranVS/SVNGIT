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

namespace VSWebUI.Configurator
{
	public partial class AlertEventTemplate : System.Web.UI.Page
	{
		int Eventid;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillAlertEventTEmplategrid();
			}
			else
			{
				//fillTemplateGridfromSession();
			}
			if (Session["eventtemplatename"] != null)//MS-raju-VSPLUS:2260-for event template code
			{
				if (Session["eventtemplatename"].ToString() != "")
				{
					if (Request.QueryString["Mode"] != null)
					{
						string Mode = Request.QueryString["Mode"].ToString();
						if (Mode == "insert")
						{
							//10/9/2014 NS modified for VSPLUS-990
							successDiv.InnerHtml = "The event(s) for  template <b>" + Session["eventtemplatename"].ToString() +
								"</b> saved successfully." +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							successDiv.Style.Value = "display: block";
							Session["eventtemplatename"] = "";
						}
						else
						{
							successDiv.InnerHtml = "The event(s) for  template <b>" + Session["eventtemplatename"].ToString() +
								"</b> updated successfully." +
								"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							successDiv.Style.Value = "display: block";
							Session["eventtemplatename"] = "";
						}

					}
				}
			}
		}
		public void fillTemplateGridfromSession()
		{
			DataTable Templatetable = new DataTable();
			try
			{
				if (Session["Eventtemplate"] != "" && Session["Eventtemplate"] != null)
					Templatetable = Session["Eventtemplate"] as DataTable;
				if (Templatetable.Rows.Count > 0)
				{
					Templatetable.PrimaryKey = new DataColumn[] { Templatetable.Columns["ID"] };
				}
				AlertEventTEmplategrid.DataSource = Templatetable;
				AlertEventTEmplategrid.DataBind();
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "Myaccountdetails")
			{
				Response.Redirect("~/Configurator/AlertDefinitions_Grid.aspx?dboard=" + true);
				Context.ApplicationInstance.CompleteRequest();
			}
		}
		protected void AlertEventTEmplategrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

			e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
			
			if (e.RowType == GridViewRowType.EditForm)
			{
				string mode;
				try
				{
					if (e.GetValue("ID") != "" && e.GetValue("ID") != null)
					{

						mode="update";

						Response.Redirect("AlertEventTemplate_Edit.aspx?Key=" + e.GetValue("ID")+"&Mode="+ mode +"&Name="+e.GetValue("Name"), false);
						Context.ApplicationInstance.CompleteRequest();
					}
					else
					{
						 mode = "insert";
						Response.Redirect("AlertEventTemplate_Edit.aspx?Mode=" + mode, false);
						Context.ApplicationInstance.CompleteRequest();

					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}

			}
		}

		protected void AlertEventTEmplategrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			DataTable Htable = Session["Eventtemplate"] as DataTable;
			DataRow[] dr = Htable.Select("ID=" + Convert.ToInt32(e.Keys[0]));
			foreach (DataRow r in dr)
			{
				r.Delete();
				r.AcceptChanges();
			}
			//7/3/2013 NS added detailes deletion
			EventsTemplate tempobj = new EventsTemplate();
			tempobj.ID = Convert.ToInt32(e.Keys[0]);
			//VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteHoursData(Alertobj);
			VSWebBL.ConfiguratorBL.AlertsBL.Ins.Deletetemplatedata(tempobj);
			Session["Eventtemplate"] = Htable;
			ASPxGridView gridview = (ASPxGridView)sender;
			gridview.CancelEdit();
			e.Cancel = true;
			FillAlertEventTEmplategrid();
			//fillTemplateGridfromSession();

		}
		private void FillAlertEventTEmplategrid()
		{
			try
			{

				DataTable dt = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlerteventnames();
				Session["Eventtemplate"] = dt;

				AlertEventTEmplategrid.DataSource = dt;
				AlertEventTEmplategrid.DataBind();


			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //9/16/2015 NS added for VSPLUS-1953
			Response.Redirect("~/Configurator/AlertEventTemplate_Edit.aspx?Mode=insert", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
		protected void btn_Click(object sender, EventArgs e)
		{
			ImageButton btn = (ImageButton)sender;
			// btn.Attributes["onClick"]=
			Servers ServerObject = new Servers();
			ServerObject.ID = Convert.ToInt32(btn.CommandArgument);
			Eventid = Convert.ToInt32(btn.CommandArgument);
			string name = btn.AlternateText;
			pnlAreaDtls.Style.Add("visibility", "visible");
			divmsg.InnerHtml = "Are you sure you want to delete the Event Template " + name + "?";
			
		}
		protected void btn_CancelClick(object sender, EventArgs e)
		{

			FillAlertEventTEmplategrid();
			
		}
		protected void OKButton_Click(object sender, EventArgs e)
		{
			NavigatorPopupControl.ShowOnPageLoad = false;
		}
		protected void btn_OkClick(object sender, EventArgs e)
		{
			EventsTemplate tempobj = new EventsTemplate();
			tempobj.ID = Eventid;
			VSWebBL.ConfiguratorBL.AlertsBL.Ins.Deletetemplatedata(tempobj);
			
				FillAlertEventTEmplategrid();
			
			
			Site1 currMaster = (Site1)this.Master;
			currMaster.refreshStatusBoxes();

		}
	}
}