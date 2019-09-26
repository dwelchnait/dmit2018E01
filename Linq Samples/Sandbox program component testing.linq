<Query Kind="Program">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	string artistname = "th";
	var results = from x in Albums
				where x.Artist.Name.Contains(artistname)
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

// Define other methods and classes here
public class AlbumsOfArtist
{
	public string Title {get;set;}
	public string ArtistName {get;set;}
	public int RYear {get;set;}
	public string RLabel {get;set;}
}