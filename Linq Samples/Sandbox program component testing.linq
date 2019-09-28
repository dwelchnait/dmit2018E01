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
	//results.Dump();
	
	//create a list of all customers in alphabetic order by lastname, firstname 
	//who live in the US with an yahoo email. List full name (last, fist), city,
	//state and email only. create the class definition of this list.
	
	var customerlist = from x in Customers
					   where x.Country.Equals("USA")
					      && x.Email.Contains("yahoo.com")
						orderby x.LastName, x.FirstName
						select new YahooCustomers
						{
							Name = x.LastName + ", " + x.FirstName,
							City = x.City,
							State = x.State,
							Email = x.Email
						};
	//customerlist.Dump();
	
	//who is the artist who sang Rag Doll (track). List the Artist Name,
	//the Album title, release year and label, along with the song (track) 
	//composer.
	
	var whosang = from x in Tracks
					where x.Name.Equals("Rag Doll")
					select new
					{
						ArtistName = x.Album.Artist.Name,
						AlbumTitle = x.Album.Title,
						AlbumYear = x.Album.ReleaseYear,
						AlbumLabel = x.Album.ReleaseLabel,
						Composer = x.Composer
					};
	whosang.Dump();
	
}

// Define other methods and classes here
public class AlbumsOfArtist
{
	public string Title {get;set;}
	public string ArtistName {get;set;}
	public int RYear {get;set;}
	public string RLabel {get;set;}
}

public class YahooCustomers
{
	public string Name {get;set;}
	public string City {get;set;}
	public string State {get;set;}
	public string Email {get;set;}
}