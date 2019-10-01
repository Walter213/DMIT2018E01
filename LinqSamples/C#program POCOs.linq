<Query Kind="Program">
  <Connection>
    <ID>6e0cea61-2a5a-4796-8119-57868bef7188</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	var results = from x in Albums
		where x.Tracks.Count() > 25
		select new
		{
			AlbumTitle = x.Title,
			AlbumArtist = x.Artist.Name,
			Trackcount = x.Tracks.Count(),
			PlayTime = x.Tracks.Sum(z => z.Milliseconds),
			Tracks = (from y in x.Tracks
				// where x.AlbumID == y.AlbumID
				select new TrackPOCO
				{
					SongName = y.Name,
					SongGenre = y.Genre.Name,
					SongLength = y.Milliseconds
				}).ToList()
		};
	results.Dump();
}

// Define other methods and classes here
public class TrackPOCO
{
	public string SongName {get; set;}
	public string SongGenre {get; set;}
	public int SongLength {get; set;}
}

public class AlbumDTO
{
	public string AlbumTitle {get; set;}
	public string AlbimArtist {get; set;}
	public int TrackCount {get; set;}
	public int PlayTime {get; set;}
	public List<TrackPOCO> AlbumTracks {get; set;}
}