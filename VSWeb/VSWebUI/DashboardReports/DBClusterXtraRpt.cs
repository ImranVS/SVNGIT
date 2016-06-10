using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;

namespace VSWebUI.DashboardReports
{
    public partial class DBClusterXtraRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public int threshold = 0;
        public DBClusterXtraRpt()
        {
            InitializeComponent();
        }

        public DataTable GetThresholdList(DataTable dt, string paramA1, string paramB1, string paramC1, string paramA2, string paramB2, string paramC2)
        {
            double doccountA = 0;
            double doccountB = 0;
            double doccountC = 0;
            double dbsizeA = 0;
            double dbsizeB = 0;
            double dbsizeC = 0;
            double diffcount = 0;
            double diffsize = 0;
            double maxdoccount = 0;
            double mindoccount = 0;
            double maxdbsize = 0;
            double mindbsize = 0;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                doccountA = double.Parse(dt.Rows[i][paramA1].ToString());
                doccountB = double.Parse(dt.Rows[i][paramB1].ToString());
                doccountC = double.Parse(dt.Rows[i][paramC1].ToString());
                dbsizeA = double.Parse(dt.Rows[i][paramA2].ToString());
                dbsizeB = double.Parse(dt.Rows[i][paramB2].ToString());
                dbsizeC = double.Parse(dt.Rows[i][paramC2].ToString());
                if (dt.Rows[i]["ServerNameC"].ToString() != "")
                {
                    if (doccountA != 0 && doccountB != 0 && doccountC != 0)
                    {
                        //Find the max value of the three values
                        maxdoccount = Math.Max(doccountC, Math.Max(doccountA, doccountB));
                        //Find the min value of the three values
                        mindoccount = Math.Min(doccountC, Math.Min(doccountA, doccountB));
                    }
                    if (dbsizeA != 0 && dbsizeB != 0 && dbsizeC != 0)
                    {
                        maxdbsize = Math.Max(dbsizeC, Math.Max(dbsizeA, dbsizeB));
                        mindbsize = Math.Min(dbsizeC, Math.Min(dbsizeA, dbsizeB));
                    }
                }
                else
                {
                    if (doccountA != 0 && doccountB != 0)
                    {
                        //Find the max value of the two values
                        maxdoccount = Math.Max(doccountA, doccountB);
                        //Find the min value of the two values
                        mindoccount = Math.Min(doccountA, doccountB);
                    }
                    if (dbsizeA != 0 && dbsizeB != 0)
                    {
                        maxdbsize = Math.Max(dbsizeA, dbsizeB);
                        mindbsize = Math.Min(dbsizeA, dbsizeB);
                    }
                }
                diffcount = Convert.ToDouble((maxdoccount - mindoccount) / maxdoccount * 100);
                diffsize = Convert.ToDouble((maxdbsize - mindbsize) / maxdbsize * 100);
                if ((diffcount <= threshold || diffsize <= threshold) && threshold != 0)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        public void GetDBClusterInfo()
        {
            DataTable dt1 = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterServers(this.ClusterName.Value.ToString());
            if (dt1.Rows.Count > 0)
            {
                threshold = int.Parse(dt1.Rows[0]["Threshold"].ToString());
                ClusterNameLabel.Text += " " + this.ClusterName.Value.ToString() + "     " + "Failure Threshold: " + threshold.ToString() + "%";
            }
            DataTable dtraw = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterInfo(this.ClusterName.Value.ToString());
            DataTable dt = GetThresholdList(dtraw, "DocCountA", "DocCountB", "DocCountC", "DBSizeA", "DBSizeB", "DBSizeC");
            if (dt.Rows.Count > 0)
            {
                xrPivotGrid1.DataSource = dt;
                DataTable dt2 = VSWebBL.ReportsBL.ReportsBL.Ins.GetDBClusterServers(this.ClusterName.Value.ToString());
                if (dt2.Rows.Count > 0){
                    string serverA = dt2.Rows[0]["ServerNameA"].ToString();
                    string serverB = dt2.Rows[0]["ServerNameB"].ToString();
                    string serverC = dt2.Rows[0]["ServerNameC"].ToString();
                    xrPivotGrid1.Fields["DocCountA"].Caption = serverA.Substring(0, serverA.IndexOf("/")) + "\r\n" + "Document Count";
                    xrPivotGrid1.Fields["DocCountB"].Caption = serverB.Substring(0, serverB.IndexOf("/")) + "\r\n" + "Document Count";
                    if (serverC != "")
                    {
                        xrPivotGrid1.Fields["DocCountC"].Caption = serverC.Substring(0, serverC.IndexOf("/")) + "\r\n" + "Document Count";
                    }
                    else
                    {
                        xrPivotGrid1.Fields["DocCountC"].Caption = "";
                    }
                    xrPivotGrid1.Fields["DBSizeA"].Caption = serverA.Substring(0, serverA.IndexOf("/")) + "\r\n" + "DB Size";
                    xrPivotGrid1.Fields["DBSizeB"].Caption = serverB.Substring(0, serverB.IndexOf("/")) + "\r\n" + "DB Size";
                    if (serverC != "")
                    {
                        xrPivotGrid1.Fields["DBSizeC"].Caption = serverC.Substring(0, serverC.IndexOf("/")) + "\r\n" + "DB Size";
                    }
                    else
                    {
                        xrPivotGrid1.Fields["DBSizeC"].Caption = "";
                    }
                    xrPivotGrid1.BestFit();
                }
            } 
        }

        private void DBClusterXtraRpt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            GetDBClusterInfo();
        }
    }
}
