using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VSWebUI
{
    public partial class StatusBox : System.Web.UI.UserControl
    {

        public string TitleLink
        {
            get { return aTitle.HRef; }
            set
            {
                aTitle.HRef = value;

            }
        }
        public string Label11Text
        {
            get { return Label11.Text; }
            set
            {
                Label11.Text = value;
                a1.Attributes.Add("class", (Label11.Text == "0" ? "statusbutton10" : "statusbutton11"));
                

            }
        }
        public string Label11CssClass
        {
            get { return Label11.CssClass; }
            set
            {
                Label11.CssClass = value;
                
            }
        }
        public string Label12Text
        {
            get { return Label12.Text; }
            set
            {
                Label12.Text = value;

            }
        }
        public string Label12CssClass
        {
            get { return Label12.CssClass; }
            set
            {
                Label12.CssClass = value;

            }
        }

        public string Button1Link
        {
            get { return a1.HRef; }
            set
            {
                a1.HRef = value;

            }
        }

        //

        public string Label21Text
        {
            get { return Label21.Text; }
            set
            {
                Label21.Text = value;
                a2.Attributes.Add("class", (Label21.Text == "0" ? "statusbutton20" : "statusbutton21"));

            }
        }
        public string Label21CssClass
        {
            get { return Label21.CssClass; }
            set
            {
                Label21.CssClass = value;

            }
        }
        public string Label22Text
        {
            get { return Label22.Text; }
            set
            {
                Label22.Text = value;

            }
        }
        public string Label22CssClass
        {
            get { return Label22.CssClass; }
            set
            {
                Label22.CssClass = value;

            }
        }

        public string Button2Link
        {
            get { return a2.HRef; }
            set
            {
                a2.HRef = value;

            }
        }

        //

        public string Label31Text
        {
            get { return Label31.Text; }
            set
            {
                Label31.Text = value;

                a3.Attributes.Add("class", (Label31.Text == "0" ? "statusbutton30" : "statusbutton31"));
            }
        }
        public string Label31CssClass
        {
            get { return Label31.CssClass; }
            set
            {
                Label31.CssClass = value;

            }
        }
        public string Label32Text
        {
            get { return Label32.Text; }
            set
            {
                Label32.Text = value;

            }
        }
        public string Label32CssClass
        {
            get { return Label32.CssClass; }
            set
            {
                Label32.CssClass = value;

            }
        }

        public string Button3Link
        {
            get { return a3.HRef; }
            set
            {
                a3.HRef = value;

            }
        }
        //
        public string Label41Text
        {
            get { return Label41.Text; }
            set
            {
                Label41.Text = value;

                a4.Attributes.Add("class", (Label41.Text == "0" ? "statusbutton40" : "statusbutton41"));
            }
        }
        public string Label41CssClass
        {
            get { return Label41.CssClass; }
            set
            {
                Label41.CssClass = value;

            }
        }
        public string Label42Text
        {
            get { return Label42.Text; }
            set
            {
                Label42.Text = value;

            }
        }
        public string Label42CssClass
        {
            get { return Label42.CssClass; }
            set
            {
                Label42.CssClass = value;

            }
        }

        public string Button4Link
        {
            get { return a4.HRef; }
            set
            {
                a4.HRef = value;

            }
        }
        //


        public string Button1CssClass
        {
            get { return ""; }
            set
            {
               Button1.Attributes.Add("class", value);
            }
        }
        public string Button2CssClass
        {
            get { return ""; }
            set
            {
                Button2.Attributes.Add("class", value);
            }
        }
        public string Button3CssClass
        {
            get { return ""; }
            set
            {
                Button3.Attributes.Add("class", value);
            }
        }
        public string Button4CssClass
        {
            get { return ""; }
            set
            {
                Button4.Attributes.Add("class", value);
            }
        }
        public string TitleTableCssClass
        {
            get { return TitleTable.CssClass; }
            set
            {
                TitleTable.CssClass = value;
                
            }
        }
        public string TitleCssClass
        {
            get { return TitleLabel.CssClass; }
            set
            {
                TitleLabel.CssClass = value;
            }
        }
        public Unit Width
        {
            get { return OuterTable.Width; }
            set { 
                OuterTable.Width = value;
            
            }
        }
        public Unit Height
        {
            get { return OuterTable.Height; }
            set { OuterTable.Height = value;
            Button1.Style.Add("height",value.ToString());
            Button2.Style.Add("height", value.ToString());
            Button3.Style.Add("height", value.ToString());
            Button4.Style.Add("height", value.ToString());

            }
        }

        public string Title
        {
            get { return TitleLabel.Text; }
            set
            {
                //2/11/2016 NS modified for VSPLUS-2568
                if (value == "Office365")
                {
                    TitleLabel.Text = "Office 365";
                }
                else
                {
                    TitleLabel.Text = value;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}