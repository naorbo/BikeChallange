using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewRider : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Menu menu = (Menu)Master.FindControl("NavigationMenu");
        menu.Items[0].Selected = true;

    }
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void InsertBTN_Click(object sender, EventArgs e)
    {
        User user;

        try
        {
            user = new User(UserNameTB.Text, FnameTB.Text, LnameTB.Text, GenderList.SelectedValue, EmailTB.Text, StreetTB.Text, CityTB.Text, PhoneTB.Text, BicycleTypeButtonList1.SelectedValue, imgPathTB.Text);
            //user = new User(nameTB.Text, Convert.ToDouble(priceTB.Text), imgURLTB.Text, Convert.ToInt16(amountTB.Text), Convert.ToInt16(discountRBL.SelectedIndex));
        }
        catch (Exception ex)
        {
            //Response.Write("illegal values to the products attributes - error message is " + ex.Message);
            LabelErrorMessage.Text = "illegal values to the user attributes - error message is " + ex.Message;
            return;
        }

        try
        {
            int numEffected = user.insert();
            //Response.Write("num of effected rows are " + numEffected.ToString());
            LabelErrorMessage.Text = "Number of effected rows are " + numEffected.ToString();
        }
        catch (Exception ex)
        {
            //Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
            LabelErrorMessage.Text = ex.Message;
        }
    }
}