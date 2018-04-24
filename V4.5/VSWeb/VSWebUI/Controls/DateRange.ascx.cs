using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.Controls
{
    public partial class DateRange : System.Web.UI.UserControl
    {
        public string FromDate
        {
            get
            {
                if (hfrange.Text != "")
                {
                    if (hfrange.Text.IndexOf("-") > 0)
                    {
                        return hfrange.Text.Substring(0, hfrange.Text.IndexOf("-") - 1) + " 00:00:00";
                    }
                }
                return DateTime.Now.AddDays(-7).ToShortDateString() + " 00:00:00";
            }
            set
            {
                hfrange.Text = value;

            }
        }

        public string ToDate
        {
            get
            {
                if (hfrange.Text != "")
                {
                    if (hfrange.Text.IndexOf("-") > 0)
                    {
                        return hfrange.Text.Substring(hfrange.Text.IndexOf("-") + 2) + " 23:59:59";
                    }
                }
                return  DateTime.Now.ToShortDateString() + " 00:00:00";
            }
            set
            {
                hfrange.Text = value;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (hfrange.Text == "")
                hfrange.Text = DateTime.Now.AddDays(-7).ToShortDateString() + " - " + DateTime.Now.ToShortDateString();
        }
    }
}