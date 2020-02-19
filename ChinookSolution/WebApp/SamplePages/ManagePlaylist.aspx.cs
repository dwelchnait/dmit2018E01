using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using DMIT2018Common.UserControls;
using WebApp.Security;

#endregion

namespace Jan2018DemoWebsite.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
            //if (Request.IsAuthenticated)
            //{
            //    if (User.IsInRole("Customers") || User.IsInRole("Customer Service"))
            //    {
            //        var username = User.Identity.Name;
            //        SecurityController securitymgr = new SecurityController();
            //        int? customerid = securitymgr.GetCurrentUserCustomerId(username);
            //        if(customerid.HasValue)
            //        {
            //            MessageUserControl.TryRun(() => {
            //                CustomerController sysmgr = new CustomerController();
            //                Customer info = sysmgr.Customer_Get(customerid.Value);
            //                CustomerName.Text = info.FullName;
            //            });
            //        }
            //        else
            //        {
            //            MessageUserControl.ShowInfo("UnRegistered User", "This user is not a registered customer");
            //            CustomerName.Text = "Unregistered User";
            //        }
            //    }
            //    else
            //    {
            //        //redirect to a page that states no authorization fot the request action
            //        Response.Redirect("~/Security/AccessDenied.aspx");
            //    }
            //}
            //else
            //{
            //    //redirect to login page
            //    Response.Redirect("~/Account/Login.aspx");
            //}
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ArtistName.Text))
            {
                //using MessageUserControl to display a message
                MessageUserControl.ShowInfo("Missing Data", 
                    "Enter a partial artist name.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SearchArg.Text = ArtistName.Text;
                    TracksBy.Text = "Artist";
                    TracksSelectionList.DataBind(); //cause the ODS to execute
                }, "Track Search",
                "Select from the following list to add to your playlist");
            }
               

         }

        protected void MediaTypeFetch_Click(object sender, EventArgs e)
        {
            
            MessageUserControl.TryRun(() =>
            {
                SearchArg.Text = MediaTypeDDL.SelectedValue;
                TracksBy.Text = "MediaType";
                TracksSelectionList.DataBind(); //cause the ODS to execute
            }, "Track Search",
            "Select from the following list to add to your playlist");


        }

        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                SearchArg.Text = GenreDDL.SelectedValue;
                TracksBy.Text = "Genre";
                TracksSelectionList.DataBind(); //cause the ODS to execute
            }, "Track Search",
            "Select from the following list to add to your playlist");

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                //using MessageUserControl to display a message
                MessageUserControl.ShowInfo("Missing Data",
                    "Enter a partial album title.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SearchArg.Text = AlbumTitle.Text;
                    TracksBy.Text = "Album";
                    TracksSelectionList.DataBind(); //cause the ODS to execute
                }, "Track Search",
                "Select from the following list to add to your playlist");
            }

        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data",
                    "Play list name is required to fetch a play list");
            }
            else
            {
                string playlistname = PlaylistName.Text;
                //until we do security, we will use a hard coded username
                //string username = "HansenB";

                //Once security is implemented you can obtain the
                //user name from User.Identity class property .Name
                string username = User.Identity.Name;
                //do a standard query lookup to your control
                //use MessageUserControl for error handling
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(
                        playlistname, username);
                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();
                },"Playlist Tracks","See current tracks on playlist below");
            }
 
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            List<string> reasons = new List<string>();
            //is there a playlist?
            //    no msg
            if(PlayList.Rows.Count == 0)
            {
                reasons.Add("There is no playlist present. Fetch your playlist.");
            }
            //is there a playlist name??
            //    no msg
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                reasons.Add("You must have a playlist name.");
            }
            //traverse playlist to collect selected row(s)
            //> 1 row selected
            //    bad msg
            int trackid = 0;
            int tracknumber = 0;
            int rowsSelected = 0;
            CheckBox playlistselection = null;
            for (int rowindex=0; rowindex < PlayList.Rows.Count; rowindex++)
            {
                //access the checkbox control on the indexed GridViewRow
                //set the CheckBox pointer to this checkbox control
                playlistselection = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                if (playlistselection.Checked)
                {
                    //increase selected number of rows
                    rowsSelected++;
                    //gather the data needed for the BLL call
                    trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                    tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                }
            }
            if (rowsSelected != 1)
            {
                reasons.Add("Select only one track to move.");
            }
            //check if last track
            //    bad msg
            if (tracknumber == PlayList.Rows.Count)
            {
                reasons.Add("Last track cannot be moved down");
            }
            //validation good
            if (reasons.Count == 0)
            {
                //   yes: move track
                MoveTrack(trackid, tracknumber, "down");
            }
            else
            {
                //    no: display all errors
                MessageUserControl.TryRun(() => {
                    throw new BusinessRuleException("Track Move Errors:", reasons);
                });
            }
            
            
 
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            List<string> reasons = new List<string>();
            //is there a playlist?
            //    no msg
            if (PlayList.Rows.Count == 0)
            {
                reasons.Add("There is no playlist present. Fetch your playlist.");
            }
            //is there a playlist name??
            //    no msg
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                reasons.Add("You must have a playlist name.");
            }
            //traverse playlist to collect selected row(s)
            //> 1 row selected
            //    bad msg
            int trackid = 0;
            int tracknumber = 0;
            int rowsSelected = 0;
            CheckBox playlistselection = null;
            for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
            {
                //access the checkbox control on the indexed GridViewRow
                //set the CheckBox pointer to this checkbox control
                playlistselection = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                if (playlistselection.Checked)
                {
                    //increase selected number of rows
                    rowsSelected++;
                    //gather the data needed for the BLL call
                    trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                    tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                }
            }
            if (rowsSelected != 1)
            {
                reasons.Add("Select only one track to move.");
            }
            //check if last track
            //    bad msg
            if (tracknumber == 1)
            {
                reasons.Add("First track cannot be moved up");
            }
            //validation good
            if (reasons.Count == 0)
            {
                //   yes: move track
                MoveTrack(trackid, tracknumber, "up");
            }
            else
            {
                //    no: display all errors
                MessageUserControl.TryRun(() => {
                    throw new BusinessRuleException("Track Move Errors:", reasons);
                });
            }

        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
            MessageUserControl.TryRun(() => {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                sysmgr.MoveTrack(User.Identity.Name, PlaylistName.Text, trackid, tracknumber, direction);
                List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(
                        PlaylistName.Text, User.Identity.Name);
                PlayList.DataSource = datainfo;
                PlayList.DataBind();
            },"Success","Track has been moved");
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data",
                    "Play list name is required to add a track");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Required Data",
                   "No playlist is available. Retreive your playlist.");
                }
                else
                {
                    //traverse the geidview and collect the list of tracks to remove
                    List<int> trackstodelete = new List<int>();
                    int rowsSelected = 0;
                    CheckBox playlistselection = null;
                    for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
                    {
                        //access the checkbox control on the indexed GridViewRow
                        //set the CheckBox pointer to this checkbox control
                        playlistselection = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                        if (playlistselection.Checked)
                        {
                            //increase selected number of rows
                            rowsSelected++;
                            //gather the data needed for the BLL call
                            trackstodelete.Add(int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") 
                                                                as Label).Text));

                        }
                    }
                    if (rowsSelected == 0)
                    {
                        MessageUserControl.ShowInfo("Required Data",
                  "You must select at least on track to remove.");
                    }
                    else
                    {
                        //send list of tracks to be remove by BLL
                        MessageUserControl.TryRun(() =>
                        {
                            PlaylistTracksController sysmgr = new PlaylistTracksController();
                            //there is ONLY one call to remove the data to the database
                            sysmgr.DeleteTracks(User.Identity.Name, PlaylistName.Text, trackstodelete);
                            //refresh the playlist is a READ
                            List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(
                                PlaylistName.Text, User.Identity.Name);
                            PlayList.DataSource = datainfo;
                            PlayList.DataBind();
                        }, "Remove Track(s)", "Track has been removed from the playlist");
                    }
                }
            }
        }
        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            //do we have the playlist name
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data",
                    "Play list name is required to add a track");
            }
            else
            {
                //collect the required data for the event
                string playlistname = PlaylistName.Text;
                //the username will come from the form security
                //so until security is added, we will use HansenB
                string username = User.Identity.Name;
                //obtain the track id from the ListView
                //the track id will be in the CommandArg property of the 
                //    ListViewCommandEventArgs e instance
                //the Commandarg in e is return as an object
                //case it to a string, then you can .Parse the string
                int trackid = int.Parse(e.CommandArgument.ToString());

                //using the obtained data, issue your call to the BLL method
                //this work will be done within a TryRun()
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    //there is ONLY one call to add the data to the database
                    sysmgr.Add_TrackToPLaylist(playlistname, username, trackid);
                    //refresh the playlist is a READ
                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(
                        playlistname, username);
                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();
                },"Adding a Track","Track has been added to the playlist");
            }

        }

    }
}