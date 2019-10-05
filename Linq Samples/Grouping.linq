<Query Kind="Expression">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//group record collection a single field on the record
//the selected grouping field is referred to as the group Key
from x in Tracks
group x by x.GenreId

//group record collection using multiple fields on the record
//the multiple fields become a group key instance
//referring to a property in the group key instance is by Key.Property
from x in Tracks
group x by new {x.GenreId, x.MediaTypeId}

//place the grouping of the large data collection into a temporary data collection
//ANY further reporting on the groups with the temporary data collection
//      will use the temporary data collection name as its data source

//report the groups
from x in Tracks
group x by x.GenreId into gGenre
select (gGenre)

//details on each group
from x in Tracks
group x by x.GenreId into gGenre
select new
{
	groupid = gGenre.Key,
	tracks = gGenre.ToList()

}

//selected fields from each group
from x in Tracks
group x by x.GenreId into gGenre
select new
{
	groupid = gGenre.Key,
	tracks = from x in gGenre
			select new
			{
				trackid = x.TrackId,
				song = x.Name,
				artist = x.Album.Artist.Name,
				songlength = x.Milliseconds/1000000.0
			}

}

//refer to a specific key property
from x in Tracks
group x by new {x.GenreId, x.MediaTypeId} into gTracks
select new
{
	genre = gTracks.Key.GenreId,
	media = gTracks.Key.MediaTypeId,
	trackcount = gTracks.Count()
}

//you can also group by class
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

//create a list of albums by ReleaseYear
//showing the year, number of albums in that year, 
//list of albums showing album title, count of tracks for each album.

from x in Albums
group x by x.ReleaseYear into gRYear
select new
{
	year = gRYear.Key,
	albumcount = gRYear.Count(),
	anAblum = from y in gRYear
			  select new
			  {
			  	title = y.Title,
				trackcount = (from t in y.Tracks
							  select t).Count()
			  }
}

//order the previous report by the number of albums per year descending
//(what was the most productive year), order within count by year ascending.
//report only albums from 1990 and later

//tip once you have group, all further clauses are against the group
from x in Albums
//where x.ReleaseYear >=  1990
group x by x.ReleaseYear into gRYear
where gRYear.Key>= 1990
orderby gRYear.Count() descending, gRYear.Key
select new
{
	year = gRYear.Key,
	albumcount = gRYear.Count(),
	anAblum = from y in gRYear
			  select new
			  {
			  	title = y.Title,
				trackcount = (from t in y.Tracks
							  select t).Count()
			  }
}































