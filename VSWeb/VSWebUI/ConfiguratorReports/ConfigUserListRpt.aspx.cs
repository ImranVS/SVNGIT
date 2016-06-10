using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VSWebUI.ConfiguratorReports
{
    public partial class ConfigUserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string seluser = "";
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = VSWebBL.ReportsBL.ReportsBL.Ins.GetUserNameList();
                if (dt.Rows.Count > 0)
                {
                    //11/4/2015 NS modified for VSPLUS-2023
                    UserListComboBox.DataSource = dt;
					UserListComboBox.ValueField = "FullName";
                    UserListComboBox.TextField = "FullName";
                    UserListComboBox.DataBind();
					seluser = Session["UserFullName"].ToString();
					UserListComboBox.Items.FindByText(seluser).Selected = true;
					
                }
            }
			if (UserListComboBox.SelectedIndex != -1)
			{
				seluser = UserListComboBox.Items[UserListComboBox.SelectedIndex].Text;
			}
			ConfiguratorReports.ConfigUserListXtraRpt report = new ConfiguratorReports.ConfigUserListXtraRpt();
			report.Parameters["UserName"].Value = seluser;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }

        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void UserListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seluser = "";
            if (UserListComboBox.SelectedIndex != -1)
            {
                seluser = UserListComboBox.Items[UserListComboBox.SelectedIndex].Text;
            }
            ConfiguratorReports.ConfigUserListXtraRpt report = new ConfiguratorReports.ConfigUserListXtraRpt();
            report.Parameters["UserName"].Value = seluser;
            report.CreateDocument();
            ASPxDocumentViewer1.Report = report;
            ASPxDocumentViewer1.DataBind();
        }
    }
}