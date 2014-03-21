<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Organizations.aspx.cs" Inherits="Organizations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div dir="rtl">
    

        <table>
            <tr>
                <td>
                    קוד הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationNameTB" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    שם הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationDesTB" runat="server"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    מייל הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationEmailTB" runat="server"></asp:TextBox>
                </td>
            </tr>
            
                <tr>
            <td>
                עיר:
            </td>
            <td>
                <asp:DropDownList ID="CityDDL" runat="server" DataSourceID="CitiesDataSource" DataTextField="CityDes" DataValueField="City">
                </asp:DropDownList>
                <asp:SqlDataSource ID="CitiesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:bikechallangeDBConnectionString %>" SelectCommand="SELECT [CityDes], [CityName], [City] FROM [Cities]"></asp:SqlDataSource>
                <asp:RequiredFieldValidator ID="RequiredFieldValidato" runat="server" ErrorMessage="Value missing" ControlToValidate="CityDDL" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
            <tr>
                <td>
                    כתובת הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationAddressTB" runat="server"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    טלפון הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationPhoneTB" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    סוג הארגון:</td>
                <td>
                    <asp:TextBox ID="OrganizationTypeTB" runat="server"></asp:TextBox>
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

