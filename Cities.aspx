<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Cities.aspx.cs" Inherits="Cities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div dir="rtl">
    

        <table>
            <tr>
                <td>
                    קוד העיר:</td>
                <td>
                    <asp:TextBox ID="CityNameTB" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    שם העיר:</td>
                <td>
                    <asp:TextBox ID="CityDesTB" runat="server"></asp:TextBox>
                    </td>
            </tr>
        </table>
    


    </div>
    <asp:Button ID="loadFromDBBTN" runat="server" onclick="loadFromDBBTN_Click" 
        Text="Load From DB" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="showDSBTN" runat="server" Text="Show DataSet" 
        onclick="showDSBTN_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="updateDSBTN" runat="server" Text="update Data Set" 
        onclick="updateDSBTN_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="updateDBBTN" runat="server" Text="update DataBase" 
        onclick="updateDBBTN_Click" />

        <br />
        <br />
    <asp:PlaceHolder ID="tablePH" runat="server"></asp:PlaceHolder>
</asp:Content>

