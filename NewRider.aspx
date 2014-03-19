<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NewRider.aspx.cs" Inherits="NewRider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table dir="rtl">
        <tr>
            <td>
                אי מייל:
            </td>
            <td>
                <asp:TextBox ID="EmailTB" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="EmailTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                שם פרטי:
            </td>
            <td>
                <asp:TextBox ID="FnameTB" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="nameTBValidator" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="FnameTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td>
                שם משפחה:
            </td>
            <td>
                
                <asp:TextBox ID="LnameTB" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="LnameTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td colspan="2">
                
            <asp:RadioButtonList ID="GenderList" runat="server">
                    <asp:ListItem>זכר</asp:ListItem>
                    <asp:ListItem>נקבה</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="GenderList" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>

         
        <tr>
            <td>  <h1>הפרופיל שלך</h1>
            </td>
        </tr>
      
        <tr>
            <td>
                שם משתמש:
            </td>
            <td>
                <asp:TextBox ID="UserNameTB" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="priceTBRequiredFieldValidator" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="UserNameTB" CssClass="validators"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="UserTBValidation" ControlToValidate="UserNameTB"
                    runat="server" CssClass="validators" ErrorMessage="invalid uaername." ValidationExpression="^[a-zA-Z][a-zA-Z0-9]{5,11}$">
                    <!-- username has to be start with letters and contains letters and number characters in 6 to 12 length. -->
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                תמונה:
            </td>
            <td>
                <asp:TextBox ID="imgPathTB" runat="server"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="imgURLTBRequiredFieldValidator" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="imgURLTB" CssClass="validators"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="imgURLTBREValidator" runat="server" ErrorMessage="invalid URL"
                    CssClass="validators" ControlToValidate="imgURLTB" ValidationExpression="(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?">
                </asp:RegularExpressionValidator>--%>
            </td>
        </tr>

        <tr>
        <td>   <h1>כתובת</h1>
        </td>
        </tr>
       
        <tr>
            <td>
                רחוב:
            </td>
            <td>
                <asp:TextBox ID="StreetTB" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="amountTBRequiredFieldValidator" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="StreetTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                עיר:
            </td>
            <td>
                <asp:TextBox ID="CityTB" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="CityTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                טלפון:
            </td>
            <td>
                <asp:TextBox ID="PhoneTB" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="PhoneTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>


         <tr>
        <td> <h1>המסלול שלך לעבודה</h1>
        </td>
        </tr>
      
                
        <tr>
            <td>
                סוג המסלול:
            </td>
            <td>
                <asp:TextBox ID="RideTB" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="RideTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
        <td colspan="2">
                
            <asp:RadioButtonList ID="BicycleTypeButtonList1" runat="server">
                    <asp:ListItem>הרים</asp:ListItem>
                    <asp:ListItem>כביש</asp:ListItem>
                    <asp:ListItem>חשמליות</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="BicycleTypeButtonList1" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                מה המרחק המסלול? (כיוון אחד)
            </td>
            <td>
                <asp:TextBox ID="LengthTB" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="LengthTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr> 
         <tr>
            <td>  <h1>איך נראה שבוע העבודה שלך?</h1>
            </td>
            <td>
                <asp:CheckBoxList ID="WorkWeekCheckBoxList" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">ראשון</asp:ListItem>
                    <asp:ListItem Selected="True">שני</asp:ListItem>
                    <asp:ListItem Selected="True">שלישי</asp:ListItem>
                    <asp:ListItem Selected="True">רביעי</asp:ListItem>
                    <asp:ListItem Selected="True">חמישי</asp:ListItem>
                    <asp:ListItem>שישי</asp:ListItem>
                    <asp:ListItem>שבת</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
       
        <tr>
            <td> <h1>הקבוצה שלך</h1>
            </td>
        </tr>    
        <tr>
            <td>
                שם הקבוצה:
            </td>
            <td>
                <asp:TextBox ID="TeamNameTB" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Value missing"
                    ControlToValidate="TeamNameTB" CssClass="validators"></asp:RequiredFieldValidator>
            </td>
        </tr>
            
        <tr>

            <td>
            </td>
            <td>
                <asp:Button ID="InsertBTN" runat="server" Text="Insert" OnClick="InsertBTN_Click" />
            </td>
        </tr>
        <tr><td colspan="2">
            <asp:Label ID="LabelErrorMessage" runat="server" Text=""></asp:Label></td></tr>
    </table>
</asp:Content>

