<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="WebApp.Security.AccessDenied" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-danger text-center">
        <div class="jumbotron">
            <h1>Access Denied</h1>
            <asp:Button ID="GoTo" runat="server" Text="Go To Home" CssClass="btn btn-primary" OnClick="GoTo_Click" />
        </div>
    </div>
</asp:Content>
