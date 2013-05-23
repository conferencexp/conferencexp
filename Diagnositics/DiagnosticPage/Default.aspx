<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="edu.washington.cs.cct.cxp.diagnostics._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ConferenceXP Diagnostics</title>
</head>
<body>

<h1> ConferenceXP Diagnostics </h1>

   <form id="form1" runat="server">
   <div>
    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True">
    </asp:DropDownList>&nbsp;
       <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged"
           Text="Advanced View" AutoPostBack="True" /></div>
    
    <h3>
        <asp:Button ID="RefreshButton2" runat="server" OnClick="RefreshButton2_Click" Text="Refresh" />&nbsp;</h3>
       <h3>
        Sender Information
       </h3>
 
    <div>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    </div>
    
    <h3> Throughput Differential (data received - data sent)</h3>
    
    <div>
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" OnRowDataBound="GridView2_RowDataBound">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
       
    </div>
    
    <h3> Loss Rate </h3>
    
        <div>
        <asp:GridView ID="GridView3" runat="server" CellPadding="4" ForeColor="#333333" OnRowDataBound="GridView3_RowDataBound">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
            <asp:Button ID="RefreshButton" runat="server" OnClick="RefreshButton_Click" Text="Refresh" /><br />
            <br />
            <br />
            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged"
                Text="Use logging" /><br />
            &nbsp;<asp:Label ID="Label2" runat="server" Text="Log file: "></asp:Label>
            <asp:HyperLink ID="HyperLink1" runat="server">HyperLink</asp:HyperLink><br />
            <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
        <em>* Note: Times are provided by individual machines; clocks are therefore not necessarily
            synchronized.<br />
        </em>
        <br />
        </div>
       
    </form>
</body>
</html>
