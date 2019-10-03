<Query Kind="Statements">
  <Connection>
    <ID>6e0cea61-2a5a-4796-8119-57868bef7188</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

// using a multiple steps to obtain the required data query

// create a list showing whether a particular track length
//  is greater than, less than, or the average track length

// problems, I need the average track length before testing
//  the individual track length against the average

//solution

// what is the average track length?

// var resultsavg = (from x in Tracks
//	select x.Milliseconds).Average();
// resultsavg.Dump();

// or

var resultavg = Tracks.Average(x => x.Milliseconds);
resultavg.Dump();

// create query using the average track length.

var resultreport = from x in Tracks
	select new
	{
		Song = x.Name,
		Length = x.Milliseconds,
		LongShortAvg = x.Milliseconds > resultavg ? "Long" :
						x.Milliseconds < resultavg ? "Short": "Average"
	};
resultreport.Dump();

// list all the playlist which have a track showing the playlist name,
//  number of tracks on the playlist, the cost of the playlist, and
//  the total storage size for the playlist in megabytes.

var test = from pl in Playlists
	where pl.PlaylistTracks.Count() > 0
	select new
	{
		name = pl.Name,
		trackcount = pl.PlaylistTracks.Count,
		cost = pl.PlaylistTracks.Sum(plt => plt.Track.UnitPrice),
		storage = pl.PlaylistTracks
					.Sum(plt => plt.Track.Bytes/1000000.0)
//		storage = (from plt in pl.PlaylistTracks
//					select plt.Track.Bytes/1000000.0).Sum()
	};
test.Dump();

// list all albums with tracks showing the album title, artist name,
//  number of tracks and the albumcost
	
var allalbums = from x in Albums
	where x.Tracks.Count() > 0
	select new
	{
		title = x.Title,
		artist = x.Artist.Name,
		trackcount = x.Tracks.Count()
	};
allalbums.Dump();

// what is the maximum album count for all the artists

var MaxCountAlbum = (Artists.Select(x => x.Albums.Count())).Max();
MaxCountAlbum.Dump();
//var maxcount = MaxCountAlbum.Max();
//maxcount.Dump();