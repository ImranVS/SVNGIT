using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI.Controls
{
    public partial class MailStatus : System.Web.UI.UserControl
    {
        private object _ThresholdValue;
        private object _ActualValue;
        protected void Page_Load(object sender, EventArgs e)
        {
            SetCircleStatus("Pending Mail", Convert.ToDouble((_ActualValue is System.DBNull ? 0 : _ActualValue)), Convert.ToDouble((_ThresholdValue is System.DBNull ? 0 : _ThresholdValue)));
        }
        public object ActualValue
        {
            get { return _ActualValue; }
            set
            {
                _ActualValue = value;

            }
        }
        public object ThresholdValue
        {
            get { return _ThresholdValue; }
            set
            {
                _ThresholdValue = value;

            }
        }
        public void SetCircleStatus(string Mail, double ActualValue, double ThresholdValue)
        {

            if (Mail == "Pending Mail")
            {
                if (ActualValue == 0)
                {
                    tbl.Rows[0].Cells[0].Visible = false;
                    lblPendingMail.Visible = false;
                }
                else
                {
                    tbl.Rows[0].Cells[0].Style["background-color"] = (ActualValue < 0.85 * ThresholdValue ? "green" : (ActualValue >= 0.85 * ThresholdValue && ActualValue < ThresholdValue ? "yellow" : "red"));
                    lblPendingMail.Text = Mail + ": " + ActualValue.ToString() + "<br>";
                    tbl.Rows[0].Cells[0].Visible = true;
                    lblPendingMail.Visible = true;
                }
            }
            if (Mail == "Dead Mail")
            {
                if (ActualValue == 0)
                {
                    tbl.Rows[0].Cells[1].Visible = false;
                    lblDeadMail.Visible = false;
                }
                else
                {
                    tbl.Rows[0].Cells[1].Style["background-color"] = (ActualValue < 0.85 * ThresholdValue ? "green" : (ActualValue >= 0.85 * ThresholdValue && ActualValue < ThresholdValue ? "yellow" : "red"));
                    lblDeadMail.Text = Mail + ": " + ActualValue.ToString() + "<br>";
                }
            }
            if (Mail == "Held Mail")
            {
                if (ActualValue == 0)
                {
                    tbl.Rows[0].Cells[2].Visible = false;
                    lblHeldMail.Visible = false;
                }
                else
                {
                    tbl.Rows[0].Cells[2].Style["background-color"] = (ActualValue < 0.85 * ThresholdValue ? "green" : (ActualValue >= 0.85 * ThresholdValue && ActualValue < ThresholdValue ? "yellow" : "red"));
                    lblHeldMail.Text = Mail + ": " + ActualValue.ToString();
                }
            }
        }
    }
}