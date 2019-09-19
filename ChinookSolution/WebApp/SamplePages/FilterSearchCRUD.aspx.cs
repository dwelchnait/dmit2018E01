using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.Entities;
#endregion

namespace WebApp.SamplePages
{
    public partial class FilterSearchCRUD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindArtistList();
                //set the max value for the Validation control
                //RangeEditReleaseYear
                RangeEditReleaseYear.MaximumValue = DateTime.Today.Year.ToString();
            }
        }

        protected void BindArtistList()
        {
            ArtistController sysmgr = new ArtistController();
            List<Artist> info = sysmgr.Artist_List();
            info.Sort((x, y) => x.Name.CompareTo(y.Name));
            ArtistList.DataSource = info;
            ArtistList.DataTextField = nameof(Artist.Name);
            ArtistList.DataValueField = nameof(Artist.ArtistId);
            ArtistList.DataBind();
            //ArtistList.Items.Insert(0, "select ...");
        }

        //in code behind to be called from ODS
        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }


        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //standard lookup
            GridViewRow agvrow = AlbumList.Rows[AlbumList.SelectedIndex];
            //retreive the value from a web control located within
            //   the GridView cell
            string albumid = (agvrow.FindControl("AlbumId") as Label).Text;

            //error handling will need to be added
            MessageUserControl.TryRun(() =>
            {
                //place your processing code
                AlbumController sysmgr = new AlbumController();
                Album datainfo = sysmgr.Album_Get(int.Parse(albumid));
                
                if (datainfo == null)
                {
                    ClearControls();
                    throw new Exception("Record no longer exists on file.");
                }
                else
                {
                    EditAlbumID.Text = datainfo.AlbumId.ToString();
                    EditTitle.Text = datainfo.Title;
                    EditAlbumArtistList.SelectedValue = datainfo.ArtistId.ToString();
                    EditReleaseYear.Text = datainfo.ReleaseYear.ToString();
                    EditReleaseLabel.Text =
                        datainfo.ReleaseLabel == null ? "" : datainfo.ReleaseLabel;
                }
            },"Find Album", "Album found"); //strings on this line are success message
           
        }

        protected void ClearControls()
        {
            EditAlbumID.Text = "";
            EditTitle.Text = "";
            EditReleaseYear.Text = "";
            EditReleaseLabel.Text = "";
            EditAlbumArtistList.SelectedIndex = 0;
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string albumtitle = EditTitle.Text;
                int albumyear = int.Parse(EditReleaseYear.Text);
                string albumlabel = EditReleaseLabel.Text == "" ? 
                        null : EditReleaseLabel.Text;
                int albumartist = int.Parse(EditAlbumArtistList.SelectedValue);

                Album theAlbum = new Album();
                theAlbum.Title = albumtitle;
                theAlbum.ArtistId = albumartist;
                theAlbum.ReleaseYear = albumyear;
                theAlbum.ReleaseLabel = albumlabel;

                MessageUserControl.TryRun(() =>
                {
                    AlbumController sysmgr = new AlbumController();
                    int albumid = sysmgr.Album_Add(theAlbum);
                    EditAlbumID.Text = albumid.ToString();
                    if (AlbumList.Rows.Count > 0)
                    {
                        AlbumList.DataBind();
                    }
                },"Successful","Album added");
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int editalbumid = 0;
                string albumid = EditAlbumID.Text;
                if (string.IsNullOrEmpty(albumid))
                {
                    MessageUserControl.ShowInfo("Attention","Lookup the album before editing");
                }
                else if (!int.TryParse(albumid, out editalbumid))
                {
                    MessageUserControl.ShowInfo("Attention","Current albumid is invalid. Preform lookukp again.");
                }
                else
                {
                    Album theAlbum = new Album();
                    theAlbum.AlbumId = editalbumid;  //include pkey
                    theAlbum.Title = EditTitle.Text;
                    theAlbum.ArtistId = int.Parse(EditAlbumArtistList.SelectedValue);
                    theAlbum.ReleaseYear = int.Parse(EditReleaseYear.Text); ;
                    theAlbum.ReleaseLabel = EditReleaseLabel.Text == "" ?
                            null : EditReleaseLabel.Text;

                    MessageUserControl.TryRun(() =>
                    {
                        AlbumController sysmgr = new AlbumController();
                        int rowsaffected = sysmgr.Album_Update(theAlbum);
                        if (rowsaffected > 0)
                        {
                            AlbumList.DataBind();
                        }
                        else
                        {
                            throw new Exception("Album was not found. Repeat lookup and update again.");
                        }
                    }, "Successful", "Album updated");
                }
            }
        }

        protected void Remove_Click(object sender, EventArgs e)
        {
            int editalbumid = 0;
            string albumid = EditAlbumID.Text;
            if (string.IsNullOrEmpty(albumid))
            {
                MessageUserControl.ShowInfo("Attention", "Lookup the album before editing");
            }
            else if (!int.TryParse(albumid, out editalbumid))
            {
                MessageUserControl.ShowInfo("Attention", "Current albumid is invalid. Preform lookukp again.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    AlbumController sysmgr = new AlbumController();
                    int rowsaffected = sysmgr.Album_Delete(editalbumid);
                    if (rowsaffected > 0)
                    {
                        AlbumList.DataBind();
                        EditAlbumID.Text = "";
                    }
                    else
                    {
                        throw new Exception("Album was not found. Repeat lookup and remove again.");
                    }
                }, "Successful", "Album Removed");
            }
        }
    }
}