<%@ Page Title="VitalSigns Plus - Executive Summary" Language="C#" AutoEventWireup="true" MasterPageFile="~/DashboardSite.Master" CodeBehind="SummaryLandscape.aspx.cs" Inherits="VSWebUI.Dashboard.SummaryLandscape" %>
<%@ MasterType VirtualPath="~/DashboardSite.master" %> 
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





    <%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxp" %>

<%@ Register Src="~/Controls/StatusBox.ascx" TagName="StatusBox" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <style type="text/css">
        .boxshadow
        {
            width: 100%;
            border-left: 1px solid #C0C0C0;
            border-top: 1px solid #C0C0C0;
            border-right: 2px solid Gray;
            border-bottom: 2px solid Gray;
        }  
        /*Mukund 10Jun2014 
        VSPLUS-673: Executive Summary should have the same right-click menu as other screens*/    
       .ahrefdom
       {         
           color:Black;
           font-family:Tahoma;
           font-size:12px;
           font-weight:bold;
           }
         a.ahrefdom:hover
        { 
        color:Black;
                   font-family:Tahoma;
                   font-size:12px;
                   font-weight:bold;
        }
           .dtldomOther
       { 
           color:#808080;font-family:Tahoma;font-size:11px;
       }
       .dtldomWhite
       { 
           color:White;font-family:Tahoma;font-size:11px;
       }
           .dtldomBlack
       { 
           color:Black;font-family:Tahoma;font-size:11px;
       }
       ul.context-menu-list.context-menu-root
       {
      width:176px;

    
      top: 354px;
    left: 648px;
    z-index: 1;

     
       }
 ul.context-menu-list.context-menu-root li.context-menu-item.icon.icon-edit
      {
       float: left;
	     width:150px;
	     height:25px;
      }
     ul.context-menu-list.context-menu-root li.context-menu-item.icon.icon-cut
      {
       float: left;
       width:150px;
 height:25px;
      }
   ul.context-menu-list.context-menu-root li.context-menu-item.icon.icon-copy
      {
      	 float:left;

        width:150px;
         height:25px;
      }
 
       /*-------------------------*/
    </style>
<script type="text/javascript" >
    $(document).ready(function () {
        $('.alert-success').delay(10000).fadeOut("slow", function () {
        });
    });

//Mukund 10Jun2014,  VSPLUS-673: Executive Summary should have the same right-click menu as other screens
    function ScanNow(strName, strType) {
        //alert("test");
        PageMethods.ScanNow(strName, strType, OnSuccess, OnFail); 
    }
    function SuspendNow(strName) {
        var vhfName = document.getElementById("ContentPlaceHolder1_SuspendPopupControl_hfName");
        vhfName.value = strName;
        FeedPopupControl.Show();
    }
    function OnSuccess(result) {
        // Page element to display feedback.
        //alert(result);
    }
    function OnFail(error) {
        // Display the error.    
        //alert(error);
    }
//-------------------------------------------
</script>
   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Mukund 10Jun2014,  VSPLUS-673: Executive Summary should have the same right-click menu as other screens<script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>--%>
    <%--<script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>--%>
    <script src="../js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
  <%-- ---------------------------------------------%>
   <div class="header" id="servernamelbldisp" runat="server">Executive Summary</div>

<div id="ErrorMsg" class="alert alert-danger" runat="server" style="display: none">The settings were not updated</div>
<div id="SuccessMsg" class="alert alert-success" runat="server" style="display: none">Temporarily Suspended monitoring</div>

                <dx:ASPxDataView ID="ASPxDataView2" runat="server" BackColor="Transparent"
                                        ondatabound="ASPxDataView2_DataBound" 
                                        ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="True" AllowPaging="False" Layout="Flow">
                                        <ItemTemplate>
                                            <dxp:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
                                                <PanelCollection>
                                                    <dxp:PanelContent ID="PanelContent1" runat="server" 
                                                        SupportsDisabledAttribute="True">
                                                        <div id="divmenu" runat="server">                                                       
                                                       
                                                        </div>                 
                                                    </dxp:PanelContent>
                                                </PanelCollection>
                                                <Border BorderColor="Gray" BorderStyle="None" BorderWidth="0px" />
                                            </dxp:ASPxPanel>
                                        </ItemTemplate>
                                        <ItemStyle BackColor="Transparent" Height="50px" HorizontalAlign="Left" 
                                            Width="160px" Wrap="True" VerticalAlign="Top">
                                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" 
                                            PaddingTop="0px" />
                                        <Border BorderStyle="None" BorderWidth="0px" />
<Paddings PaddingLeft="0px" PaddingTop="0px" PaddingRight="0px" PaddingBottom="0px"></Paddings>

<Border BorderStyle="None" BorderWidth="0px"></Border>
                                        </ItemStyle>
                                    </dx:ASPxDataView>
           

           <dx:ASPxPopupControl ID="SuspendPopupControl" runat="server" 
        AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton"
        EnableViewState="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="True" ShowOnPageLoad="False" Width="400px"
        Height="70px" FooterText="To resize the control use the resize grip or the control's edges"
        HeaderText="Suspend Monitoring" ClientInstanceName="FeedPopupControl" 
        EnableHierarchyRecreation="True" Theme="MetropolisBlue">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControlPopup" runat="server">
                <table id="FeedBackTable" class="EditorsTable" style="width: 100%; height: 100%;">
            <tr>
                <td class="Label">
                    <asp:HiddenField ID="hfName" runat="server" />
                    <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration (mins):" 
                        Wrap="False"></dx:ASPxLabel>
                </td>
                <td>
                    <dx:ASPxTextBox ID="TbDuration" runat="server" Width="100%" EnableViewState="False">
                    <MaskSettings Mask="&lt;1..120&gt;" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                    <RequiredField ErrorText="Enter Time to Suspend Monitoring." IsRequired="True" />
                    <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                         ValidationExpression="^\d+$" />
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
            </tr>   
            <tr>
                <td class="Label" colspan="2">
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" 
                        Text="You may temporarily suspend monitoring for a maximum duration of two hours. If you need to suspend monitoring for more than two hours, please use the Maintenance Windows functionality in the VitalSigns Configurator."></dx:ASPxLabel>
                </td>
            </tr>       
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr> 
            <tr>
            <td colspan="2">
                <dx:ASPxButton ID="test123" runat="server" onclick="BtnApply_Click"
                                                        AutoPostBack="False" 
                                                                CausesValidation="False" EnableTheming="True" 
                                                                Text="Apply" CssClass="sysButton"></dx:ASPxButton>
                                                                </td>
            </tr>
        </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
     </asp:Content>
