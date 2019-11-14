<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityAdmin.aspx.cs" Inherits="WebApp.Security.SecurityAdmin" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-header">Website Security - Users and Roles</h1>
<div class="row">
    <div class="col-md-12">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
</div>
<div class="row">
    <div class="col-md-9">
        <h2>Users</h2>
        <asp:ListView ID="UsersListView" runat="server"
            DataSourceID="UsersDataSource" DataKeyNames="Id"
            InsertItemPosition="FirstItem"
            ItemType="WebApp.Models.ApplicationUser">
            <EditItemTemplate>
                <tr style="">
                    <td style="white-space:nowrap;">
                        <asp:Button runat="server" CommandName="Update" Text="Update" CssClass="btn btn-default" ID="UpdateButton" />
                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-default" ID="CancelButton" />
                    </td>
                    <td>
                        <asp:TextBox Text='<%# BindItem.UserName %>' runat="server" ID="UserNameTextBox" CssClass="form-control" Enabled="false" />
                    </td>
                    <td> <asp:DropDownList ID="EmployeeList" runat="server" AppendDataBoundItems="true" Enabled="false"
                            DataSourceID="EmployeeListODS" 
                            DataTextField="DisplayText" 
                            DataValueField="IDValueField"
                            selectedvalue='<%# Item.EmployeeId.HasValue?Item.EmployeeId:0 %>'
                            >
                        <asp:ListItem Value="0">employee ...</asp:ListItem>
                         </asp:DropDownList>
                      
                    
                    </td>
                    <td>
                        <asp:TextBox Text='<%# BindItem.Email %>' runat="server" ID="EmailTextBox" CssClass="form-control" />
                        <asp:TextBox Text='<%# BindItem.PhoneNumber %>' runat="server" ID="PhoneNumberTextBox" CssClass="form-control" />

                    </td>
                </tr>
            </EditItemTemplate>
            <InsertItemTemplate>
                <tr class="bg-info">
                    <td style="white-space:nowrap;">
                        <asp:Button runat="server" CommandName="Insert" Text="Insert" CssClass="btn btn-default" ID="InsertButton" />
                        <asp:Button runat="server" CommandName="Cancel" Text="Clear" CssClass="btn btn-default" ID="CancelButton" />
                    </td>
                    <td>
                        <asp:TextBox Text='<%# BindItem.UserName %>' runat="server" ID="UserNameTextBox" CssClass="form-control" />
                    </td>
                    <td>
                        <asp:DropDownList ID="EmployeeList" runat="server" AppendDataBoundItems="true"
                            DataSourceID="EmployeeListODS" 
                            DataTextField="DisplayText" 
                            DataValueField="IDValueField"
                            selectedvalue='<%# Bind("EmployeeId") %>'>
                             <asp:ListItem Value="0">employee ...</asp:ListItem>
                        </asp:DropDownList>
                      
                    </td>
                    <td>
                        <asp:TextBox Text='<%# BindItem.Email %>' runat="server" ID="EmailTextBox" CssClass="form-control" />
                        <asp:TextBox Text='<%# BindItem.PhoneNumber %>' runat="server" ID="PhoneNumberTextBox" CssClass="form-control" />
                    </td>
                </tr>
            </InsertItemTemplate>
            <ItemTemplate>
                <tr style="">
                    <td style="white-space:nowrap;">
                        <asp:Button runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-default" ID="DeleteButton" />
                        <asp:Button runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-default" ID="EditButton" />
                    </td>
                    <td>
                        <asp:Label Text='<%# Item.UserName %>' runat="server" ID="UserNameLabel" />
                        <br />
                        <small>(<asp:Label Text='<%# Item.Id %>' runat="server" ID="IdLabel" />)</small>
                    </td>
                    <td> <asp:DropDownList ID="EmployeeList" runat="server" AppendDataBoundItems="true" Enabled="false"
                            DataSourceID="EmployeeListODS" 
                            DataTextField="DisplayText" 
                            DataValueField="IDValueField"
                            selectedvalue='<%# Item.EmployeeId.HasValue?Item.EmployeeId:0 %>'
                            >
                        <asp:ListItem Value="0">employee ...</asp:ListItem>
                         </asp:DropDownList>
                       
                    
                    </td>
                    <td>
                        <asp:Label Text='<%# Item.Email %>' runat="server" ID="EmailLabel" />
                        <asp:Label Text='<%# Item.PhoneNumber %>' runat="server" ID="PhoneNumberLabel" />
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table runat="server" id="itemPlaceholderContainer" 
                                class="table table-condensed table-hover">
                                <tr runat="server" style="">
                                    <th runat="server"></th>
                                    <th runat="server">UserName (Id)</th>
                                    <th runat="server">Employee </th>
                                    <th runat="server">Email / Phone Number</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="">
                            <asp:DataPager runat="server" ID="DataPager1">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" ButtonCssClass="btn btn-default"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField NumericButtonCssClass="btn btn-default"></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" ButtonCssClass="btn btn-default"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>
    </div>
    <div class="col-md-3">
        <h2>Roles</h2>
        <asp:ListView ID="RolesListView" runat="server"
            DataSourceID="RolesDataSource" DataKeyNames="Id" InsertItemPosition="FirstItem"
            ItemType="Microsoft.AspNet.Identity.EntityFramework.IdentityRole">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div>
                    <asp:LinkButton runat="server" CommandName="Delete" ID="DeleteButton"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Edit" ID="EditButton"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                    <%# Item.Name %>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <div>
                    <asp:LinkButton runat="server" CommandName="Update" ID="UpdateButton"><i class="glyphicon glyphicon-ok"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Cancel" ID="CancelButton"><i class="glyphicon glyphicon-arrow-left"></i></asp:LinkButton>
                    <asp:TextBox ID="RoleName" runat="server" Text="<%# BindItem.Name %>"></asp:TextBox>
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="bg-info">
                    <asp:LinkButton runat="server" CommandName="Insert" ID="InsertButton"><i class="glyphicon glyphicon-plus"></i></asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="Cancel" ID="CancelButton"><i class="glyphicon glyphicon-ban-circle"></i></asp:LinkButton>
                    <asp:TextBox ID="RoleName" runat="server" Text="<%# BindItem.Name %>"></asp:TextBox>
                </div>
            </InsertItemTemplate>
        </asp:ListView>
        <br /><br />
        <h2>Assign Employee Role</h2>
        <asp:DropDownList ID="EmployeeListForRoles" runat="server" AppendDataBoundItems="true"  
            DataSourceID="EmployeeListForRolesODS" 
            DataTextField="UserName" 
            DataValueField="Id">
                <asp:ListItem Value="0">employee ...</asp:ListItem>
            </asp:DropDownList>

        <asp:LinkButton ID="RefreshAssignEmployees" runat="server"   OnClick="RefreshRoleEmployees_Click" CausesValidation="False"><span class="glyphicon glyphicon-refresh"></span></asp:LinkButton>

        <br /><br />

        <asp:DropDownList ID="RoleList" runat="server" AppendDataBoundItems="true"  
            DataSourceID="RoleListForRolesODS" 
            DataTextField="Name" 
            DataValueField="Id">
                <asp:ListItem Value="0">role ...</asp:ListItem>
            </asp:DropDownList>
        
          <asp:LinkButton ID="RefreshAssignRoles" runat="server"   OnClick="RefreshRoleEmployees_Click" CausesValidation="False"><span class="glyphicon glyphicon-refresh"></span></asp:LinkButton>
        
        <br /><br />
        <asp:Button ID="AddRole" runat="server" Text="Add Role to User" CssClass="btn btn-primary" OnClick="AddRole_Click" height="36px" width="231px" />
        <asp:Button ID="DeleteRole" runat="server" Text="Delete Role from User" CssClass="btn" OnClick="DeleteRole_Click" height="36px" width="231px"/>

    </div>
</div>

<asp:ObjectDataSource ID="UsersDataSource" runat="server"
    DataObjectTypeName="WebApp.Models.ApplicationUser"
    DeleteMethod="DeleteUser"
    InsertMethod="AddUser"
    OldValuesParameterFormatString="original_{0}"
    SelectMethod="ListUsers"
    TypeName="WebApp.Security.SecurityController"
    UpdateMethod="UpdateUser"
    OnUpdated="CheckForExceptions"
    OnInserted="CheckForExceptions"
    OnDeleted="CheckForExceptions"></asp:ObjectDataSource>

<asp:ObjectDataSource ID="RolesDataSource" runat="server"
    DataObjectTypeName="Microsoft.AspNet.Identity.EntityFramework.IdentityRole"
    DeleteMethod="DeleteRole"
    InsertMethod="AddRole"
    OldValuesParameterFormatString="original_{0}"
    SelectMethod="ListRoles"
    TypeName="WebApp.Security.SecurityController"
    UpdateMethod="UpdateRole"
    OnUpdated="CheckForExceptions"
    OnInserted="CheckForExceptions"
    OnDeleted="CheckForExceptions"></asp:ObjectDataSource>

<asp:ObjectDataSource ID="EmployeeListODS" runat="server" 
    OldValuesParameterFormatString="original_{0}" 
    SelectMethod="Employee_ListNames" 
    TypeName="ChinookSystem.BLL.EmployeeController"
    OnSelected="CheckForExceptions">

</asp:ObjectDataSource>
<%--<asp:ObjectDataSource ID="CustomerListODS" runat="server" 
    OldValuesParameterFormatString="original_{0}" 
    SelectMethod="Customer_ListNames" 
    TypeName="ChinookSystem.BLL.CustomerController"
    OnSelected="CheckForExceptions">

</asp:ObjectDataSource>--%>
<asp:ObjectDataSource ID="EmployeeListForRolesODS" runat="server" 
    DataObjectTypeName="WebApp.Models.ApplicationUser"
    OldValuesParameterFormatString="original_{0}" 
    SelectMethod="ListUserEmployees"
    TypeName="WebApp.Security.SecurityController"
    OnSelected="CheckForExceptions">
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="RoleListForRolesODS" runat="server"
    DataObjectTypeName="Microsoft.AspNet.Identity.EntityFramework.IdentityRole"
    OldValuesParameterFormatString="original_{0}"
    SelectMethod="ListRoles"
    TypeName="WebApp.Security.SecurityController"
    OnSelected="CheckForExceptions">
</asp:ObjectDataSource>
</asp:Content>
