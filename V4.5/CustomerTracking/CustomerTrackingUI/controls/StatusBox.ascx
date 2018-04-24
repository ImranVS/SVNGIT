<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatusBox.ascx.cs" Inherits="VSWebUI.StatusBox" %>
<asp:Table ID="OuterTable" BorderColor="#cccccc" BorderWidth="0" CellPadding="0"
    CellSpacing="0"  runat="server">
    <asp:TableRow>
        <asp:TableCell Width="100%">
            <asp:Table ID="TitleTable" BorderWidth="0" CellPadding="0" CellSpacing="0" Width="100%"
                runat="server" >
                <asp:TableRow>
                    <asp:TableCell style="padding-left:10px">
                      <a href="" id="aTitle" runat="server"  class="statusHeader">   <asp:Label ID="TitleLabel" runat="server" /></a>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell Width="100%" style="padding-left:1%;padding-right:1%;padding-bottom:1%;">
            <asp:Table ID="Table1" CellPadding="0" CellSpacing="0"
                Width="100%" runat="server">
                <asp:TableRow  BackColor="Transparent">
                    <asp:TableCell HorizontalAlign="Center" Style="width: 25%">
                        <a href="" id="a1" runat="server" >
                            <div id="Button1" runat="server" style="
                                position: relative">
                                
                                <div style="left:3px;top:0px;position: absolute">
                                    <asp:Label ID="Label11" runat="server"  />
                                </div>                               
                                <div style="right:3px;  bottom: 5px; position: absolute">
                                    <asp:Label ID="Label12" runat="server"  />
                                </div>
                              
                            </div>
                        </a>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Style="width: 25%">
                                 <a href="" id="a2" runat="server"  class="statusbutton2">
                            <div id="Button2" runat="server" style="
                                position: relative">
                                
                                <div style="left:3px;top:0px;position: absolute">
                                    <asp:Label ID="Label21" runat="server"  />
                                </div>                               
                                <div style="right:3px;  bottom: 5px; position: absolute">
                                    <asp:Label ID="Label22" runat="server"  />
                                </div>
                              
                            </div>
                        </a>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Style="width: 25%">
                                <a href="" id="a3" runat="server"  class="statusbutton3">
                            <div id="Button3" runat="server" style="
                                position: relative">
                                
                                <div style="left:3px;top:0px;position: absolute">
                                    <asp:Label ID="Label31" runat="server"  />
                                </div>                               
                                <div style="right:3px;  bottom: 5px; position: absolute">
                                    <asp:Label ID="Label32" runat="server"  />
                                </div>
                              
                            </div>
                        </a>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Style="width: 25%">
                              <a href="" id="a4" runat="server"  class="statusbutton4">
                            <div id="Button4" runat="server" style="
                                position: relative">
                                
                                <div style="left:3px;top:0px;position: absolute">
                                    <asp:Label ID="Label41" runat="server"  />
                                </div>                               
                                <div style="right:3px;  bottom: 5px; position: absolute">
                                    <asp:Label ID="Label42" runat="server"  />
                                </div>
                              
                            </div>
                        </a>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
