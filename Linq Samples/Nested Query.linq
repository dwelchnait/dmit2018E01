<Query Kind="Expression">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//create a list of all album contain the album title and artist 
//along with all the tracks (song name, genre, length) of that album 

from x in Albums
select new
{
	title = x.Title,
	artist = x.Artist.Name,
	tracks = from y in x.Tracks
			 select new
			 {
			 	song = y.Name,
				genre = y.Genre.Name,
				length = y.Milliseconds
			 }

}

from x in Albums
select new
{
	title = x.Title,
	artist = x.Artist.Name,
	tracks = from y in Tracks
			where x.AlbumId == y.AlbumId
			 select new
			 {
			 	song = y.Name,
				genre = y.Genre.Name,
				length = y.Milliseconds
			 }

}