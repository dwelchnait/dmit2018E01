using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.Security
{
    public partial class SecurityAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Administrators"))
                {

                }
                else
                {
                    //redirect to a page that states no authorization fot the request action
                    Response.Redirect("~/Security/AccessDenied.aspx");
                }
            }
            else
            {
                //redirect to login page
                Response.Redirect("~/Account/Login.aspx");
            }
        }
        protected void CheckForExceptions(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void AddRole_Click(object sender, EventArgs e)
        {
            if (EmployeeListForRoles.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Manage User Role","Select an employee from the Add User Role employees.");
            }
            else if (RoleList.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Manage User Role", "Select a role from the Add User Role roles.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SecurityController sysmgr = new SecurityController();
                    sysmgr.AddUserRole(EmployeeListForRoles.SelectedValue, RoleList.SelectedValue);
                }, "Success", "User Role created");
            }
        }

        protected void DeleteRole_Click(object sender, EventArgs e)
        {
            if (EmployeeListForRoles.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Manage User Role", "Select an employee from the Add User Role employees.");
            }
            else if (RoleList.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Manage User Role", "Select a role from the Add User Role roles.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SecurityController sysmgr = new SecurityController();
                    sysmgr.RemoveUserRole(EmployeeListForRoles.SelectedValue, RoleList.SelectedValue);
                }, "Success", "User Role removed");
            }
        }

        protected void RefreshRoleEmployees_Click(object sender, EventArgs e)
        {
            EmployeeListForRoles.Items.Clear();
            RoleList.Items.Clear();
            EmployeeListForRoles.Items.Insert(0, "emloyee ...");
            RoleList.Items.Insert(0, "role ...");
            EmployeeListForRoles.DataBind();
            RoleList.DataBind();
        }


    }
}