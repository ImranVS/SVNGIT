<%@ Page Title="VitalSigns Plus - Logs" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReadVSLogs.aspx.cs" Inherits="VSWebUI.Configurator.ReadVSLogs" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--	<script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            //alert(postBackElement.id);

            if ((postBackElement.id).indexOf('HistoryComboBox') >= 0) {
                //alert('is combo - display');
                $get('ContentPlaceHolder1_Label2').style.display = 'block';
            }
        }
        function EndRequest(sender, args) {
            if ((postBackElement.id).indexOf('HistoryComboBox') >= 0) {
                $get('ContentPlaceHolder1_Label2').style.display = 'none';
            }
        }
        function OnMemoInit(s, e) {
            s.SetHeight(s.GetInputElement().scrollHeight);
        }
    </script>--%>
	<script type="text/javascript">
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_initializeRequest(InitializeRequest);
		prm.add_endRequest(EndRequest);
		var postBackElement;
		function InitializeRequest(sender, args) {
			if (prm.get_isInAsyncPostBack())
				args.set_cancel(true);
			postBackElement = args.get_postBackElement();
			//alert(postBackElement.id);

			if ((postBackElement.id).indexOf('ReadLogsComboBox') >= 0) {
				//alert('is combo - display');
				$get('ContentPlaceHolder1_Label2').style.display = 'block';
			}
		}
		function EndRequest(sender, args) {
			if ((postBackElement.id).indexOf('ReadLogsComboBox') >= 0) {
				$get('ContentPlaceHolder1_Label2').style.display = 'none';
			}
		}
		function OnMemoInit(s, e) {
			s.SetHeight(s.GetInputElement().scrollHeight);
		}
    </script>
    <table class="tableWidth100Percent">
        <tr>
            <td colspan="2">
                <div class="header" id="servernamelbldisp" runat="server">VitalSigns Logs</div>
            </td>
                           
        </tr>
        <tr>
            <td>
                <table style="height: 100%">
                    <tr>
                       <%-- <td>
                            <dx:ASPxComboBox ID="HistoryComboBox" runat="server" AutoPostBack="True" ClientInstanceName="startAjaxRequest"
                                        ClientIDMode="AutoID" onselectedindexchanged="HistoryComboBox_SelectedIndexChanged" 
                                        ValueType="System.String">
                                    </dx:ASPxComboBox>
                        </td>--%>
						<td>
						
						</td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Loading file. Please wait ..." CssClass="lblsmallFont" style="display: none"></asp:Label>
                        </td>
                    </tr>
					<tr>
					<td>
					
                            <dx:ASPxComboBox ID="ReadLogsComboBox" runat="server" AutoPostBack="True" ClientInstanceName="startAjaxRequest"
                                        ClientIDMode="AutoID" 
                                        ValueType="System.String" onselectedindexchanged="ReadLogsComboBox_SelectedIndexChanged" 
								>
                                    </dx:ASPxComboBox>
                        </td>
					
					</tr>
                </table>
            </td>
            
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxMemo ID="HistoryMemo" ClientInstanceName="memo" runat="server" Height="400px" Width="100%" 
                        AutoResizeWithContainer="True">
                    </dx:ASPxMemo>
                </ContentTemplate>
                <Triggers>
                 <%--   <asp:AsyncPostBackTrigger ControlID="HistoryComboBox" />--%>
					 <asp:AsyncPostBackTrigger ControlID="ReadLogsComboBox" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>