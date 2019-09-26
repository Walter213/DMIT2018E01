<Query Kind="Expression">
  <Connection>
    <ID>6e0cea61-2a5a-4796-8119-57868bef7188</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

// sample of query syntax to dump the artist data
from x in Artists
select x

// sample of method syntax to dump the Artist data
Artist
	.Select (x => x)
	
// sort datainfo.Sort((x,y) => x.AttributeName.CompareTo(y.AttributeName))

// find any artist whos name contains the string "son"
from x in Artists
where x.Name.Contains("son")
select x

Artists
	.Where(x => x.Name.Contains("son"))
	.Select (x => x)
	
// create a list of albums released in 1970
// orderby title
Albums.OrderBy(x => x.Title)
	.Where (x => x.ReleaseYear == 1970)
	.Select (x => x)
	
from x in Albums
where x.ReleaseYear == 1970
orderby x.Title
select x   

// create a list of albums released between 2007 and 2018
// orderby ReleaseYear then by Title
from x in Albums
	where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2018
orderby x.ReleaseYear, x.Title
select x

from x in Albums
	where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2018
orderby x.ReleaseYear descending, x.Title
select x

// note the difference in the method names using the method syntax
// a descending orderby is .OrderByDescending
// secondary and beyond is ordering is .ThenBy
Albums
	.Where (x => ((x.ReleaseYear >= 2007) && (x.ReleaseYear <= 2018))
	.OrderByDescending (x => x.ReleaseYear)
	.ThenBy (x => x.Title)
	
// Can navigational properties by used in queries
// create a list of albums by Deep Purple
// orderby by release year and title, artist name, releaseyear and release label

// use the navigational properties to obtain the Artist data
// new {....} create a new dataset (class definition)
from x in Albums
where x.Artist.Name.Contains("Deep Purple")
orderby x.ReleaseYear, x.Title
select x
{
	Title = x.Title,
	ArtistName = x.Artist.Name,
	RYear = x.ReleaseYear,
	RLabel = x.ReleaseLabel
}






// When using the Language C# statement(s)
// your work will need to confirm to C# statement syntax
// ei datatype variable = expression;

var results = from x in Albums
	where x.Artist.Name.Equals("Deep Purple")
	orderby x.ReleaseYear, x.Title
	select x
	{
		Title = x.Title,
		ArtistName = x.Artist.Name,
		RYear = x.ReleaseYear,
		RLabel = x.ReleaseLabel
	};
	
// to display the contents of a variable in LinqPad
// use the method .Dump()
// this method is only used in LinqPad, it is NOT a C# method
results.Dump();



// C# program
// Define other methods and classes here
void Main()
{
	var results = from x in Albums
	where x.Artist.Name.Equals("Deep Purple")
	orderby x.ReleaseYear, x.Title
	select new AlbumsOfArtist
	{
		Title = x.Title,
		ArtistName = x.Artist.Name,
		RYear = x.ReleaseYear,
		RLabel = x.ReleaseLabel
	};
	results.Dump();
}

public class AlbumsOfArtist
{
	public string Title {get; set;}
	public string ArtistName {get; set;}
	public int RYear {get; set;}
	public string RLabel {get; set;}
}