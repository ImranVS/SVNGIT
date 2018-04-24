using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using DevExpress.Web;
using DevExpress.XtraCharts;

namespace VSWebUI
{
    public class UI
    {
        private static UI _self = new UI();
        public static UI Ins
        {
            get
            {
                return _self;
            }

        }
		//1/27/2016 Durga modified for VSPLUS-2474
		private string _loginpagesessions = "showsummary,BlackBerryServers,MaintServers,BlackBerryDevicePrbegrid,DominoCluster,DominoCustom,NotesDB,DominoServer,sametime,MailServices,MaintServers,NetworkDevices,NotesDatabase,NotesMailProbe,URLs,ServerVisibleDataGrid,visible,ServerNotVisibleDataGrid,NotVisible,Servers,Users,Locations,Attempts,UserLogin,UserFullName,UserPassword,UserID,UserId,UserEmail,UserSecurityQuestion1,UserSecurityQuestion1Answer,UserSecurityQuestion2,UserSecurityQuestion2Answer,Isconfigurator,IsDashboard,Refreshtime,StartupURL,Isconsolecomm,CustomBackground,UserPreferences,ViewBy,FilterByValue,RestrictedServers,divControl";

		private string[] LogInPageSessions
		{
			get {
				return _loginpagesessions.Split(',');
			}
		}
        public void KillSessionsList(string strWorkingPage, string strSessionslist)
        {
           
            if (System.Web.HttpContext.Current.Session["KillSessions"] != null)
            {
                string strPrevSessionslist = System.Web.HttpContext.Current.Session["KillSessions"].ToString();
                string strPrevPage = System.Web.HttpContext.Current.Session["WorkingPage"].ToString();
				//1/21/2016 Durga modified for VSPLUS-2474
                if (strPrevPage != strWorkingPage && strPrevSessionslist != strSessionslist)
                {
                    string[] strSession = strPrevSessionslist.Split(',');
					
                    foreach (string str in strSession)
                    {
						//1/21/2016 Durga modified for VSPLUS-2474
						if (!LogInPageSessions.Contains(str))
							{						
							System.Web.HttpContext.Current.Session[str] = null;
							}				
						
                    }
                }
            }
            System.Web.HttpContext.Current.Session["WorkingPage"] = strWorkingPage;
            System.Web.HttpContext.Current.Session["KillSessions"] = strSessionslist;

        }

		public void ChangeUserPreference(string PreferenceName, int PageSize)
		{
			try
			{
				DataTable dt = HttpContext.Current.Session["UserPreferences"] as DataTable;
				int max = Convert.ToInt32(dt.AsEnumerable().Max(row => row["ID"]));
						
				DataRow[] UserPreferencesRow = dt.Select("PreferenceName = '" + PreferenceName + "'");
				if (UserPreferencesRow.Length > 0)
				{
					UserPreferencesRow[0]["PreferenceValue"] = PageSize.ToString();
				}
				else
				{
					dt.Rows.Add(max+1, PreferenceName, PageSize.ToString(), HttpContext.Current.Session["UserID"].ToString());
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public void GetUserPreferenceSession(string PreferenceName, ASPxGridView grid)
		{
			try
			{
				if (HttpContext.Current.Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)HttpContext.Current.Session["UserPreferences"];
					//foreach (DataRow dr in UserPreferences.Rows)
					DataRow[] UserPreferencesRow = UserPreferences.Select("PreferenceName =  '" + PreferenceName + "' ");
					//{
					//if (dr[1].ToString() == "LotusDominoServers|DominoServerGridView")
					//{
					//    DominoServerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
					//}
					if (UserPreferencesRow.Length > 0)
					{
						grid.SettingsPager.PageSize = Convert.ToInt32(UserPreferencesRow[0].ItemArray[2]);
					}
					//}
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        //7/9/2015 NS added for VSPLUS-1973
        public void RecalibrateChartAxes(object xrchart, string axisname="Y", string xintdbl="double", string yintdbl="double")
        {
            
            if ((XYDiagram)xrchart != null)
            {
                XYDiagram seriesXY = (XYDiagram)xrchart;
                int gs = 0;
                if (axisname != "X")
                {
                    if (yintdbl == "double")
                    {
                        double min = Convert.ToDouble(((XYDiagram)xrchart).AxisY.Range.MinValue);
                        double max = Convert.ToDouble(((XYDiagram)xrchart).AxisY.Range.MaxValue);
                        gs = (int)((max - min) / 5);
                        //12/12/2015 NS modified for VSPLUS-2298
                        //if (gs == 0 && max - min < 5 && max != min)
                        //3/4/2016 NS modified for VSPLUS-2488
                        if ((gs == 0 && max - min < 5 && max != min) || (gs == 1 && max <= 10 && min <= 10 && max != min))
                        {
                            if (max - min < 3)
                            {
                                gs = 1;
                            }
                            else
                            {
                                gs = 2;
                            }
                            seriesXY.AxisY.GridSpacingAuto = false;
                            seriesXY.AxisY.GridSpacing = gs;
                        }
                        else
                        {
                            seriesXY.AxisY.GridSpacingAuto = true;
                        }
                    }
                    else
                    {
                        int min = Convert.ToInt32(((XYDiagram)xrchart).AxisY.Range.MinValue);
                        int max = Convert.ToInt32(((XYDiagram)xrchart).AxisY.Range.MaxValue);
                        gs = (int)((max - min) / 5);
                        //12/12/2015 NS modified for VSPLUS-2298
                        //if (gs == 0 && max - min < 5 && max != min)
                        //3/4/2016 NS modified for VSPLUS-2488
                        if ((gs == 0 && max - min < 5 && max != min) || (gs == 1 && max <= 10 && min <= 10 && max != min))
                        {
                            if (max - min < 3)
                            {
                                gs = 1;
                            }
                            else
                            {
                                gs = 2;
                            }
                            seriesXY.AxisY.GridSpacingAuto = false;
                            seriesXY.AxisY.GridSpacing = gs;
                        }
                        else
                        {
                            seriesXY.AxisY.GridSpacingAuto = true;
                        }
                    }    
                }
                else
                {
                    if (xintdbl == "double")
                    {
                        double min = Convert.ToDouble(((XYDiagram)xrchart).AxisX.Range.MinValue);
                        double max = Convert.ToDouble(((XYDiagram)xrchart).AxisX.Range.MaxValue);

                        gs = (int)((max - min) / 5);
                        //12/12/2015 NS modified for VSPLUS-2298
                        //3/4/2016 NS modified for VSPLUS-2488
                        if ((gs == 0 && max - min < 5 && max != min) || (gs == 1 && max <= 10 && min <= 10 && max != min))
                        {
                            if (max - min < 3)
                            {
                                gs = 1;
                            }
                            else
                            {
                                gs = 2;
                            }
                            seriesXY.AxisX.GridSpacingAuto = false;
                            seriesXY.AxisX.GridSpacing = gs;
                        }
                        else
                        {
                            seriesXY.AxisX.GridSpacingAuto = true;
                        }
                    }
                    else
                    {
                        int min = Convert.ToInt32(((XYDiagram)xrchart).AxisX.Range.MinValue);
                        int max = Convert.ToInt32(((XYDiagram)xrchart).AxisX.Range.MaxValue);

                        gs = (int)((max - min) / 5);
                        //12/12/2015 NS modified for VSPLUS-2298
                        //3/4/2016 NS modified for VSPLUS-2488
                        if ((gs == 0 && max - min < 5 && max != min) || (gs == 1 && max <= 10 && min <= 10 && max != min))
                        {
                            if (max - min < 3)
                            {
                                gs = 1;
                            }
                            else
                            {
                                gs = 2;
                            }
                            seriesXY.AxisX.GridSpacingAuto = false;
                            seriesXY.AxisX.GridSpacing = gs;
                        }
                        else
                        {
                            seriesXY.AxisX.GridSpacingAuto = true;
                        }
                    }
                }
            }
        }
    }
}