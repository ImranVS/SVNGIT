using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;


namespace VSWebUI.Dashboard
{
	public class mytemplate 
	{
		#region ITemplate Members

    public void InstantiateIn(Control container) {
        LiteralControl lc = new LiteralControl();
        lc.ID = "label";
        GridViewEditItemTemplateContainer templateContainer = container as GridViewEditItemTemplateContainer;
        lc.Text = templateContainer.Text;
        container.Controls.Add(lc);
    }
    #endregion
}
	}
