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

// to get the albums with tracks and without tracks you can use .Union()
// in a union you need to ensure cast typing is correct 
// 	column cast types match identitcally
//  each query has the same number of columns
//	same order of cloumns\
// Create a list of all albums, show the title, number of tracks,
// total cost of tracks, average length (milliseconds) of the tracks

// problems exist with albums with no tracks
// summing and averages need data to work. If an album
//	has not tracks, you work to get an abort

// create 2 queries A with tracks and B without tracks

// syntax: (query1).Union(query2).Union(query3).Ordebey(first sort).ThenBy(sortn)

var unionsample = (from x in Albums
		where x.Tracks.Count() > 0
		select new
		{
			title = x.Title,
			trackcount = x.Tracks.Count(),
			priceoftracks = x.Tracks.Sum(y => y.UnitPrice),
			avglengthA = x.Tracks.Average(y => y.Milliseconds)/1000.0,
			avglengthB = x.Tracks.Average(y => y.Milliseconds/1000.0)
		}).Union(
		from x in Albums
		where x.Tracks.Count() == 0
		select new
		{
			title = x.Title,
			trackcount = 0,
			priceoftracks = 0.00m,
			avglengthA = 0.00,
			avglengthB = 0.00
		}).OrderBy(y => y.trackcount).ThenBy(y => y.title);
unionsample.Dump();

// boolean filters .All() or .Any()
// .Any() method iterates throught the entire collection to see if
// 	any of the items match the specific condition
// returns a true or false
// an instance of the collection that recieves a true is selected
//  for processing
Genres.OrderBy(x => x.Name).Dump();

// list Genres that have tracks which are not on any playlist
var genretrack = from x in Genres
		where x.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0)
		orderby x.Name
		select new
		{
			name = x.Name
		};
genretrack.Dump();

//.All() method iterates through the entire collection to see if 
//  all of the items match the specified condition
// returns a true or false
// an instance of the collection that recieves a true is selected for processing

// list Genres that have all their tracks appearing at least once on a playlist
var populargenres = from x in Genres
		where x.Tracks.All(tr => tr.PlaylistTracks.Count() > 0)
		orderby x.Name
		select new
		{
			name = x.Name,
			thetracks = (from y in x.Tracks
						where y.PlaylistTracks.Count() > 0
						select new
						{
							song = y.Name,
							count = y.PlaylistTracks.Count
						})
		};
populargenres.Dump();

// sometimes you have two lists that need to be compared
// Usually you are looking for items that area the same (in both collections)
//  OR you are looking for items that are different
// in either case: you are comparing one collection to a second collection

// obtain a distinct list of all playlist tracks for Roberto Almeida (username AlmeidaR)

var almeida = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("Almeida")
				orderby x.Track.Name
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name
				}).Distinct();
almeida.Dump();

// obtain a distinct list of all playlist tracks forMichelle Brooks (username BrooksM)

var brooks = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("Brooks")
				orderby x.Track.Name
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name
				}).Distinct();
brooks.Dump();

// list tracks that both Roberto and Michelle like

var likes = almeida.Where(a => brooks.Any(b => b.id == a.id))
.OrderBy(a => a.genre).Select(a => a);
likes.Dump();

// list Robert's Tracks that Michelle does not have

var almeidaif = almeida.Where(a => !brooks.Any(b => b.id == a.id))
.OrderBy(a => a.genre)
.Select(a => a);
almeidaif.Dump();

// list Michelle's Tracks that Robert does not have

var brooksif = brooks.Where(a => !almeida.Any(b => b.id == a.id))
.OrderBy(a => a.genre)
.Select(a => a);
brooksif.Dump();

// Joins (A Question on the lab)
// joins can be used where navigational properties DO NOT exist
// joins can be used between associate entites
// scenario pkey = fkey

// the left side of the join should be the support data
// the right side of the join is the record collection to be processed

// List albums showing title, releaseyear, label, artistname, and track count

var results = from xRightSide in Albums
		join yLeftSide in Artists
		on xRightSide.ArtistId equals yLeftSide.ArtistId
		select new
		{
			title = xRightSide.Title,
			year = xRightSide.ReleaseYear,
			label = xRightSide.ReleaseLabel == null ? "Unknown" 
				: xRightSide.ReleaseLabel,
			artistjoin = yLeftSide.Name,
			artistnav = xRightSide.Artist.Name,
			trackcount = xRightSide.Tracks.Count()
		};
results.Dump();
// www.dotnetlearners.com/linq can be used for information if i need