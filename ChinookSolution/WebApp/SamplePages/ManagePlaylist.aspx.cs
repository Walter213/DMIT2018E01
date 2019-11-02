﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chinook.Data.POCOs;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.POCOs;
using DMIT2018Common.UserControls;
//using WebApp.Security;
#endregion

namespace Jan2018DemoWebsite.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ArtistName.Text))
            {
                // using MessageUserControl to display a message
                MessageUserControl.ShowInfo("Missing Data", "Enter a partial artist name.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SearchArg.Text = ArtistName.Text;
                    TracksBy.Text = "Artist";

                    // causes ODS to execute
                    TracksSelectionList.DataBind();
                }, "Tracks search", "Select from the following list to add to your playlist");
            }
        }

        protected void MediaTypeFetch_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                SearchArg.Text = MediaTypeDDL.SelectedValue;
                TracksBy.Text = "MediaType";

                // causes ODS to execute
                TracksSelectionList.DataBind();
            }, "Tracks search", "Select from the following list to add to your playlist");
        }

        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                SearchArg.Text = GenreDDL.SelectedValue;
                TracksBy.Text = "Genre";

                // causes ODS to execute
                TracksSelectionList.DataBind();
            }, "Tracks search", "Select from the following list to add to your playlist");
        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                // using MessageUserControl to display a message
                MessageUserControl.ShowInfo("Missing Data", "Enter a partial album title name.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    SearchArg.Text = AlbumTitle.Text;
                    TracksBy.Text = "Album";

                    // causes ODS to execute
                    TracksSelectionList.DataBind();
                }, "Tracks search", "Select from the following list to add to your playlist");
            }
        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data", "PlayList Name is required to fetch a play list");
            }
            else
            {
                string playlistname = PlaylistName.Text;
                // until we do security, we will use a hard coded username
                string username = "HansenB";

                // do a standard query lookup to your controller
                //  use MessageUserControl for error handling
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();

                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(playlistname, username);

                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();
                }, "Playlist Tracks", "See current tracks on playlist below");
            }
        }

        protected void MoveDown_Click(object sender, EventArgs e)
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
                sysmgr.MoveTrack("HansenB", PlaylistName.Text, trackid, tracknumber, direction);
                List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(
                        PlaylistName.Text, "HansenB");
                PlayList.DataSource = datainfo;
                PlayList.DataBind();
            }, "Success", "Track has been moved");
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data", "PlayList Name is required to fetch a playlist");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Required Data", "No playlist is avaliable. Retrieve your playlist.");
                }
                else
                {
                    // traverse the gridview and collect list of tracks to remove
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
                            trackstodelete.Add(int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text));
                        }
                    }
                    if (rowsSelected == 0)
                    {
                        MessageUserControl.ShowInfo("Required Data", "You must selected atleast one track to remove.");
                    }
                    else
                    {
                        // send list of tracks to remove by BLL
                        MessageUserControl.TryRun(() =>
                        {
                            PlaylistTracksController sysmgr = new PlaylistTracksController();

                            // There is ONLY one call to add the data to the database
                            sysmgr.DeleteTracks("HansenB", PlaylistName.Text, trackstodelete);

                            // refresh the playlist which is a READ
                            List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(PlaylistName.Text, "HansenB");

                            PlayList.DataSource = datainfo;
                            PlayList.DataBind();

                        }, "Remove Track(s)", "Track Has Been removed from playlist");
                    }
                }
            }
        }

        protected void TracksSelectionList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //do we have the playlist name
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data", "PlayList Name Is Required To Add A Track");
            }
            else
            {
                // collect the required data for the event
                string playlistname = PlaylistName.Text;

                // the username will come from the form security
                //  so until security is added we will use HansenB
                string username = "HansenB";

                // obtain the trackid from the ListView
                //  the trackid will be in the CommandArg property of the 
                //  ListViewCommandEventArgs e instance
                // the CommandArgument in e is returned as an object
                //  case it to string, then you can .Parse the string
                int trackid = int.Parse(e.CommandArgument.ToString());

                // using the obtained data, issue your call to the BLL method
                // this work will be done within TryRun()
                MessageUserControl.TryRun(() => 
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();

                    // There is ONLY one call to add the data to the database
                    sysmgr.Add_TrackToPLaylist(playlistname, username, trackid);

                    // refresh the playlist which is a READ
                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(playlistname, username);

                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();

                },"Adding A Track", "Track Has Been Added To The PlayList");
            }
        }
    }
}