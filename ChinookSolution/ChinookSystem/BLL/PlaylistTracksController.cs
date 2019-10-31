using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.Entites;
using ChinookSystem.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {
                // what would happen if there is no match for the incoming parameter value
                // we need to ensure that the results have a valid value, this value will
                //  need to resolve to either a null OR an IEnumerable<T> collection.
                // to achieve a valid value you will need to determine using .FirstOrDefault()
                //  whether data exists or not

                var results = (from x in context.Playlists
                               where x.UserName.Equals(username)
                                && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();

                // if the playlist does NOT exist .FirstOrDefault returns null
                if (results == null)
                {
                    return null;
                }
                else
                {
                    // if the placelist does exists, do query for the playlist tracks
                    var theTracks = from x in context.PlaylistTracks
                                    where x.PlaylistId.Equals(results.PlaylistId)
                                    orderby x.TrackNumber
                                    select new UserPlaylistTrack
                                    {
                                        TrackID = x.TrackId,
                                        TrackNumber = x.TrackNumber,
                                        TrackName = x.Track.Name,
                                        Milliseconds = x.Track.Milliseconds,
                                        UnitPrice = x.Track.UnitPrice
                                    };
                    return theTracks.ToList();
                }
            }
        }//eom
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookContext())
            {
                // use the BusinessRuleException to throw errors to the web page
                List<string> reasons = new List<string>();
                PlaylistTrack newTrack = null;
                int tracknumber = 0;

                // Part #1: Determining If The PlayList Exists
                // Query the table using the playlistname and username
                //  if the playlist exist on will get a record
                //  if it does not exists , one will get a null
                // To ENSURE these results the query will be wrap in a.FirstOrDefault()
                //Playlist exists = context.Playlists
                //                    .Where(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                //                        && x.Name.Equals(playlistname, StringComparison.OrdinalIgnoreCase))
                //                    .Select(x => x)
                //                    .FirstOrDefault();
                // or this is possible instead of that
                Playlist exists = (from x in context.Playlists
                                   where x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                                            && x.Name.Equals(playlistname, StringComparison.OrdinalIgnoreCase)
                                  select x).FirstOrDefault();

                // does the playlist exists

                if (exists == null)
                {
                    // this is a new playlist
                    // create the playlist record
                    exists = new Playlist();
                    exists.Name = playlistname;
                    exists.UserName = username;

                    //stage the add
                    exists = context.Playlists.Add(exists);

                    // since this is a new playlist, the track number will be 1
                    tracknumber = 1;
                }
                else
                {
                    // since the playlist exists, so may the track exist
                    //  on the playlisttracks
                    newTrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);

                    if (newTrack == null)
                    {
                        tracknumber = exists.PlaylistTracks.Count() + 1;
                    }
                    else
                    {
                        reasons.Add("Track already exists on the playlist");
                    }
                }

                // Part Two: Create The PlayListTrack Entry
                // If there are any reasons NOT to create then throw
                //  the business rules exeption
                if (reasons.Count > 0)
                {
                    //issue with adding the track
                    throw new BusinessRuleException("Adding Track To PlayList", reasons);
                }
                else
                {
                    // Use the playlist navgation to PlayListTracks 
                    //  to do the add to PlayListTracks
                    newTrack = new PlaylistTrack();
                    newTrack.TrackId = trackid;
                    newTrack.TrackNumber = tracknumber;

                    // how do i fill the PlayListID IF the playlist is brand new
                    //  a brand new playlist DOES NOT YET have an id
                    // NOTE: the PKey for PlayList may not yet exists using
                    //  using the navigation property on the playlist entity
                    //  one can let HashSet handle the PlaylistId pkey value
                    //  to be properly created on PlayList AND placed correctly
                    //   into the "child" record of PlayListTracks
                    // ANOTHER NOTE: IF YOU DONT USE A NAVIGATION PROPERTY THIS WILL NOT WORK

                    // what is wrong to the attempt
                    //  newTrack.PlaylistId = exists.PlaylistId;

                    //playlist track staging
                    exists.PlaylistTracks.Add(newTrack);

                    // physically add any/all data to the database
                    // commit
                    context.SaveChanges();
                }
            }  
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookContext())
            {
                // get playlistID
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                              && x.Name.Equals(playlistname, StringComparison.OrdinalIgnoreCase)
                              select x).FirstOrDefault();
                if (exists == null)
                {
                    throw new Exception("Playlist does not exist.");
                }
                else
                {
                    PlaylistTrack movetrack = (from x in exists.PlaylistTracks
                                               where x.TrackId == trackid
                                               select x).FirstOrDefault();
                    if (movetrack == null)
                    {
                        throw new Exception("Playlist Track does not exist.");
                    }
                    else
                    {
                        PlaylistTrack otherTrack = null;
                        // up or down
                        if (direction.Equals("up"))
                        {
                            //up
                            if (tracknumber == 1)
                            {
                                throw new Exception("Track 1 cannot be moved up");
                            }
                            else
                            {
                                PlaylistTrack othertrack = (from x in exists.PlaylistTracks
                                                           where x.TrackNumber == movetrack.TrackNumber - 1
                                                           select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    throw new Exception("Playlist is corrupt. fetch playlist again");
                                }
                                else
                                {
                                    movetrack.TrackNumber -= 1;
                                    othertrack.TrackNumber += 1;
                                }
                            }
                        }
                        else
                        {
                            //down
                            if (tracknumber == exists.PlaylistTracks.Count())
                            {
                                throw new Exception("Last cannot be moved down");
                            }
                            else
                            {
                                PlaylistTrack othertrack = (from x in exists.PlaylistTracks
                                                            where x.TrackNumber == movetrack.TrackNumber + 1
                                                            select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    throw new Exception("Playlist is corrupt. fetch playlist again");
                                }
                                else
                                {
                                    movetrack.TrackNumber += 1;
                                    othertrack.TrackNumber -= 1;
                                }
                            }
                        }// eof up or down
                        //staging
                        context.Entry(movetrack).Property(y => y.TrackNumber).IsModified = true;
                        context.Entry(otherTrack).Property(y => y.TrackNumber).IsModified = true;

                        //commit
                        context.SaveChanges();
                    }
                }
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {
               //code to go here


            }
        }//eom
    }
}
