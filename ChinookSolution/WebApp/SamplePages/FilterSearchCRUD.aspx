<%@ Page Title="CRUD Review" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FilterSearchCRUD.aspx.cs" Inherits="WebApp.SamplePages.FilterSearchCRUD" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <h1>Filter Search</h1>
    <blockquote class="alert alert-info">
        This page will review filter search techniques. This page will using
        code-behind and ObjectDataSource on multi-record controls. This page will
        use various form controls. This will review event driven logic reuired for a CRUD process.
    </blockquote>

    <div class="col-md-offset-1">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        <br /><br />
        <!-- Validation controls-->
        <asp:RequiredFieldValidator ID="RequiredFieldEditTitle" runat="server" 
            ErrorMessage="Title field is required" Display="None"
             ForeColor="Firebrick" SetFocusOnError="true"
             ControlToValidate="EditTitle">
        </asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldEditReleaseYear" runat="server" 
            ErrorMessage="Year field is required" Display="None"
             ForeColor="Firebrick" SetFocusOnError="true"
             ControlToValidate="EditReleaseYear">
        </asp:RequiredFieldValidator>
        <asp:RangeValidator ID="RangeEditReleaseYear" runat="server" 
            ErrorMessage="Year needs to be between 1950 and today"
             Display="None" ForeColor="Firebrick" SetFocusOnError="true"
             ControlToValidate="EditReleaseYear"
             Type="Integer" MinimumValue="1950"></asp:RangeValidator>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <br />
        <asp:Label ID="label1" runat="server" Text="Select an artist:"></asp:Label>
        &nbsp;&nbsp;
        <asp:DropDownList ID="ArtistList" runat="server"></asp:DropDownList>
        &nbsp;&nbsp;
        <asp:LinkButton ID="FetchAlbums" runat="server" CausesValidation="false" >Fetch Albums</asp:LinkButton>
        <br />
        <asp:GridView ID="AlbumList" runat="server"
            AutoGenerateColumns="false" AllowPaging="True"
            PageSize="5" CssClass="table table-striped"
            GridLines="Horizontal" BorderStyle="None" 
            DataSourceID="AlbumListODS" 
            OnSelectedIndexChanged="AlbumList_SelectedIndexChanged">
            <Columns>
                <asp:CommandField SelectText="View" ShowSelectButton="True" CausesValidation="false"></asp:CommandField>
                <asp:TemplateField HeaderText="Album">
                    <ItemTemplate>
                        <asp:Label ID="AlbumTitle" runat="server"
                             Text='<%# Eval("Title") %>'></asp:Label>
                        <asp:Label ID="AlbumID" runat="server"
                             Text='<%# Eval("AlbumId") %>' Visible="false" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year">
                      <ItemTemplate>
                        <asp:Label ID="Label7" runat="server"
                             Text='<%# Eval("ReleaseYear") %>'></asp:Label>
                      </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Label">
                      <ItemTemplate>
                        <asp:Label ID="Label8" runat="server"
                             Text='<%# Eval("ReleaseLabel") %>'></asp:Label>
                      </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No albums for selected artist.
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <asp:Label ID="label2" runat="server" Text="ALbum Id:"></asp:Label>
        &nbsp;&nbsp;
        <asp:Label ID="EditAlbumID" runat="server"></asp:Label>
        <br />
        <asp:Label ID="label3" runat="server" Text="Title:"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="EditTitle" runat="server" ></asp:TextBox>
        <br />
         <asp:Label ID="label4" runat="server" Text="Artist" MaxLength="160"></asp:Label>
        &nbsp;&nbsp;
       <asp:DropDownList ID="EditAlbumArtistList" runat="server" 
           DataSourceID="EditArtistListODS" 
           DataTextField="Name" 
           DataValueField="ArtistId"></asp:DropDownList>
        <br />
         <asp:Label ID="label5" runat="server" Text="Year:"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="EditReleaseYear" runat="server"></asp:TextBox>
        <br />
         <asp:Label ID="label6" runat="server" Text="Label:"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="EditReleaseLabel" runat="server" MaxLength="50"></asp:TextBox>
        <br />
        <asp:LinkButton ID="Add" runat="server" OnClick="Add_Click" >Add</asp:LinkButton> &nbsp;&nbsp;
        <asp:LinkButton ID="Update" runat="server" OnClick="Update_Click" >Update</asp:LinkButton> &nbsp;&nbsp;
        <asp:LinkButton ID="Remove" runat="server" CausesValidation="false" 
            OnClick="Remove_Click"
            OnClientClick="return confirm('Are you sure you wish to remove this album from the collection.')" >Remove</asp:LinkButton>
    </div>
    <asp:ObjectDataSource ID="AlbumListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Album_FindByArtist" 
        TypeName="ChinookSystem.BLL.AlbumController"
         OnSelected="CheckForException"
        >
        <SelectParameters>
            <asp:ControlParameter ControlID="ArtistList" 
                PropertyName="SelectedValue" DefaultValue="0" 
                Name="artistid" Type="Int32">
            </asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="EditArtistListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Artist_List" 
        TypeName="ChinookSystem.BLL.ArtistController"
         OnSelected="CheckForException">
    </asp:ObjectDataSource>
</asp:Content>
