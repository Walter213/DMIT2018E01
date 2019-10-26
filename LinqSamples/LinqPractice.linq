<Query Kind="Expression">
  <Connection>
    <ID>6e0cea61-2a5a-4796-8119-57868bef7188</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

// sample of query syntax to dump the artist data
//from x in Artists
//select x

// sample of method syntax to dump the Artist data
//Artist
//	.Select (x => x)
	
// sort datainfo.Sort((x,y) => x.AttributeName.CompareTo(y.AttributeName))

// find any artist whos name contains the string "son"
//from x in Artists
//where x.Name.Contains("son")
//select x

//Artists
//	.Where(x => x.Name.Contains("son"))
//	.Select (x => x)
	
// create a list of albums released in 1970
// orderby title
//Albums.OrderBy(x => x.Title)
//	.Where (x => x.ReleaseYear == 1970)
//	.Select (x => x)
	
//from x in Albums
//where x.ReleaseYear == 1970
//orderby x.Title
//select x   

// create a list of albums released between 2007 and 2018
// orderby ReleaseYear then by Title
//from x in Albums
//	where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2018
//orderby x.ReleaseYear, x.Title
//select x

//from x in Albums
//	where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2018
//orderby x.ReleaseYear descending, x.Title
//select x

// note the difference in the method names using the method syntax
// a descending orderby is .OrderByDescending
// secondary and beyond is ordering is .ThenBy
//Albums
	//.Where (x => ((x.ReleaseYear >= 2007) && (x.ReleaseYear <= 2018))
	//.OrderByDescending (x => x.ReleaseYear)
	//.ThenBy (x => x.Title)
	
// Can navigational properties by used in queries
// create a list of albums by Deep Purple
// orderby by release year and title, artist name, releaseyear and release label

// use the navigational properties to obtain the Artist data
// new {....} create a new dataset (class definition)
//from x in Albums
//where x.Artist.Name.Contains("Deep Purple")
//orderby x.ReleaseYear, x.Title
//select x
//{
	//Title = x.Title,
	//ArtistName = x.Artist.Name,
	//RYear = x.ReleaseYear,
	//RLabel = x.ReleaseLabel
//}






// When using the Language C# statement(s)
// your work will need to confirm to C# statement syntax
// ei datatype variable = expression;

//var results = from x in Albums
//	where x.Artist.Name.Equals("Deep Purple")
//	orderby x.ReleaseYear, x.Title
//	select x
//	{
//		Title = x.Title,
//		ArtistName = x.Artist.Name,
//		RYear = x.ReleaseYear,
//		RLabel = x.ReleaseLabel
//	};
	
// to display the contents of a variable in LinqPad
// use the method .Dump()
// this method is only used in LinqPad, it is NOT a C# method
//results.Dump();





// C# program
// Define other methods and classes here
//void Main()
//{
//	var results = from x in Albums
//	where x.Artist.Name.Equals("Deep Purple")
//	orderby x.ReleaseYear, x.Title
//	select new AlbumsOfArtist
//	{
//		Title = x.Title,
//		ArtistName = x.Artist.Name,
//		RYear = x.ReleaseYear,
//		RLabel = x.ReleaseLabel
//	};
//	results.Dump();
//}

// or this is possible

//void Main()
//{
//	string artistname = "Deep Purple";
//	var results = from x in Albums
//	where x.Artist.Name.Contains(artistname)
//	orderby x.ReleaseYear, x.Title
//	select new AlbumsOfArtist
//	{
//		Title = x.Title,
//		ArtistName = x.Artist.Name,
//		RYear = x.ReleaseYear,
//		RLabel = x.ReleaseLabel
//	};
//	results.Dump();
//}

public class AlbumsOfArtist
{
	public string Title {get; set;}
	public string ArtistName {get; set;}
	public int RYear {get; set;}
	public string RLabel {get; set;}
}


// create a list of all Customers in alphabetic order order by lastname, firstname
// who live in the US with an yahoo email. List fullname (last, first), city
// state and email only. create the class definition of this list
//void Main(){
//	var list = from x in Customers
//	where  x.Country.Equals("USA") && x.Email.Contains("yahoo.com")
//	orderby x.LastName, x.FirstName
//	select new CustomerEmail
//	{
//		Name = x.LastName + ", " + x.FirstName,
//		City = x.City,
//		State = x.State,
//		Email = x.Email,
//	};
//	list.Dump();
//}

public class CustomerEmail
{
	public string Name {get; set;}
	public string City {get; set;}
	public string State {get; set;}
	public string Email {get; set;}
}

// who is the artist who sang Rag Doll (track), List the Artistname, 
// the album title, release year and label, along with the track composer
// This will be one of the most incoming questions for test and exercises

//void Main (){
//	
//	var whosang = from x in Tracks
//		where x.Name.Equals("Rag Doll")
//		select new
//		{
//			ArtistName = x.Album.Artist.Name,
//			AlbumTitle = x.Album.Title,
//			AlbumYear = x.Album.ReleaseYear,
//			AlbumLabel = x.Album.ReleaseLabel,
//			Composer = x.Composer
//		};
//	whosang.Dump();
//}

// create a list of album released in 2001.
// list the album title, artist name, label
// since the release label is null, use the string Unknown
// C# Expression

from x in Albums
	where x.ReleaseYear == 2001
	select new
	{
		AlbumTitle = x.Title,
		ArtistName = x.Artist.Name,
		Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
	}

// List of all albums specifing if they were released in the 70's, 80's
// 90's or modern music > 2000
// list the title and decade

from x in Albums
select new
{
	Title = x.Title,
	decade = x.ReleaseYear > 1969 & x.ReleaseYear < 1980 ? "70's" :
			 x.ReleaseYear > 1979 & x.ReleaseYear < 1990 ? "80's" :
			 x.ReleaseYear > 1989 & x.ReleaseYear < 1990 ? "90's" : "Modern"
}

// create a list of all albums containing the album title and artist along
// with all the tracks (song name, genre, length) of that album
// aggregates are executed against a collection of records
//	These aggregates are	.Count(), .Sum(x => x.field), .Min(x => x.field), .Max(x => x.field), .Average(x => x.field)

from x in Albums
where x.Tracks.Count() > 25
select new
{
	Title = x.Title,
	Artist = x.Artist.Name,
	trackcount = x.Tracks.Count(),
	playtime = x.Tracks.Sum(z => z.Milliseconds),
	Tracks = from y in x.Tracks
		// where x.AlbumID == y.AlbumID
		select new
		{
			Song = y.Name,
			Genre = y.Genre,
			Length = y.Milliseconds
		}
}


// another day another lesson

// group record collection a single field on the record
// the selected grouping field is referred to as the group key
from x in Tracks
group x by new {x.GenreId}

// group record collection using multiple field on the record
//  the multiple field become a group key instance
//  referring to a property in the group key instance is by Key.Property
from x in Tracks
group x by new {x.GenreId, x.MediaTypeId}

//place the grouping of the large data collection into a temporary data
// collection ANY further reporting on the groups with the temporary data
//   collection will use the temporary data collection name as its data source

// report on each group
from x in Tracks
group x by x.GenreId into gGenre
select (gGenre)

// details on each group
from x in Tracks
group x by x.GenreId into gGenre
select new
{
	groupid = gGenre.Key,
	tracks = gGenre.ToList()
}

// selected fields from each group
from x in Tracks
group x by x.GenreId into gGenre
select new
{
	groupid = gGenre.Key,
	tracks = from x in gGenre
			select new
			{
				trackid = x.TrackId,
				song = x.Album.Artist.Name,
				length = x.Milliseconds/1000000.0
			}
}

// refer to a specific key property
from x in Tracks
group x by new {x.GenreId, x.MediaTypeId} into gTracks
select new
{
	genre = gTracks.Key.GenreId,
	media = gTracks.Key.MediaTypeId,
	trackcount = gTracks.Count()
}

// you can also group by class
from x in Tracks
group x by x.Genre into gTracks
select (gTracks)

from x in Tracks
group x by x.Genre into gTracks
select new
{
	genre = gTracks.Key.GenreId,
	name = gTracks.Key.Name,
	trackcount = gTracks.Count()
}

from x in Tracks
group x by x.Album into gTracks
select new
{
	name = gTracks.Key.Title,
	artist = gTracks.Key.Artist.Name,
	trackcount = gTracks.Count()
}

// create a list of albums by releaseyear
//  by showing the year, number of albums in that 
// year, album title, count of
// tracks for each album.

from x in Albums
group x by x.ReleaseYear into CTracks
select new
{
	ReleaseYear = CTracks.Key,
	AlbumCount = CTracks.Count(),
	Title = from y in CTracks
			select new
			{
				Title = y.Title,
				TrackCount = (from t in y.Tracks
								select t).Count()
			}
}

// orderby the previous report by the number of albums
//  per year descending

from x in Albums
group x by x.ReleaseYear into CTracks
orderby CTracks.Count() descending
select new
{
	ReleaseYear = CTracks.Key,
	AlbumCount = CTracks.Count(),
	Title = from y in CTracks
			select new
			{
				Title = y.Title,
				TrackCount = (from t in y.Tracks
								select t).Count()
			}
}

// What was the most productive year
//  order within count by year ascending

from x in Albums
group x by x.ReleaseYear into CTracks
orderby CTracks.Count() descending, CTracks.Key
select new
{
	ReleaseYear = CTracks.Key,
	AlbumCount = CTracks.Count(),
	Title = from y in CTracks
			select new
			{
				Title = y.Title,
				TrackCount = (from t in y.Tracks
								select t).Count()
			}
}

// report only albums from 1990 and later

from x in Albums where ReleaseYear >= 1990
group x by x.ReleaseYear into CTracks
// where CTracks.Key>= 1990 or this is possible
orderby CTracks.Count() descending, CTracks.Key
select new
{
	ReleaseYear = CTracks.Key,
	AlbumCount = CTracks.Count(),
	Title = from y in CTracks
			select new
			{
				Title = y.Title,
				TrackCount = (from t in y.Tracks
								select t).Count()
			}
}