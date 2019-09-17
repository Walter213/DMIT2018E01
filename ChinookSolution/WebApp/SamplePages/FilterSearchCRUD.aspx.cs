﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional NameSpaces
using ChinookSystem.BLL;
using ChinookSystem.Data.Entites;
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

                // set the max value for the Range Validation control
                // RangeEditReleaseYear
                RangeEditReleaseyear.MaximumValue = DateTime.Today.Year.ToString();
            }
        }

        protected void BindArtistList()
        {
            ArtistController sysmgr = new ArtistController();

            List<Artist> info = sysmgr.Artist_List();

            info.Sort((x, y) => x.Name.CompareTo(y.Name));   // for descending info.Sort((x, y) => y.Name.CompareTo(x.Name)); 

            ArtistList.DataSource = info;
            ArtistList.DataTextField = nameof(Artist.Name);
            ArtistList.DataValueField = nameof(Artist.ArtistId);

            ArtistList.DataBind();
            // ArtistList.Items.Insert(0, "Select ...");
        }

        // in code behind to be called from ODS
        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Standard lookup
            GridViewRow agvrow = AlbumList.Rows[AlbumList.SelectedIndex];

            // retrieve the value from a web control located within the gridview cell
            string albumid = (agvrow.FindControl("AlbumId") as Label).Text;

            // error handling will need to be added
            MessageUserControl.TryRun(() =>
            {
                //place your processing code

                // connect to controller
                AlbumController sysmgr = new AlbumController();
                Album datainfo = sysmgr.Album_Get(int.Parse(albumid));

                if (datainfo == null)
                {
                    // clear the controls
                    //ClearControls();

                    // throw an exception
                    throw new Exception("Record no longer exists on file");
                }
                else
                {
                    EditAlbumID.Text = datainfo.AlbumId.ToString();
                    EditTitle.Text = datainfo.Title;
                    EditAlbumArtistList.SelectedValue = datainfo.ArtistId.ToString();
                    EditReleaseYear.Text = datainfo.ReleaseYear.ToString();
                    EditReleaseLabel.Text = datainfo.ReleaseLabel == null ? "" : datainfo.ReleaseLabel;
                }
            }, "Find Album", "Album found");  // you dont have to add this // strings on this line are success message
        }
    }
}