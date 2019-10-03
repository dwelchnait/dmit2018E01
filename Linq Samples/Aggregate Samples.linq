<Query Kind="Statements">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//using multiple steps to obtain the required data query

//create a list showing whether a particular track length
//  is greater than, less than or the average track length (Milliseconds)
//list the track name, Milliseconds and either Long, Short or Average

//problem, I need the average track length before testing
// the individual track length against the average
 
//solution

//What is the average track length?
//var resultavg = (from x in Tracks
//				select x.Milliseconds).Average();
var resultavg = Tracks.Average(x => x.Milliseconds);
//resultavg.Dump();

//create query using the average track length.
var resultreport = from x in Tracks
				   select new
				   {
				   		song = x.Name,
						length = x.Milliseconds,
						LongShortAvg = x.Milliseconds > resultavg ? "Long" :
									   x.Milliseconds < resultavg ? "Short" : "Average"
				   };
//resultreport.Dump();

//list all the playlists which have a track showing the playlist name,
// number of tracks on the playlist, the cost of the playlist,
// the total storage size for the playlist in megabytes.

var result3 = from pl in Playlists
			where pl.PlaylistTracks.Count() > 0
			select new
			{
				name = pl.Name,
				trackcount = pl.PlaylistTracks.Count(),
				cost = pl.PlaylistTracks.Sum(plt => plt.Track.UnitPrice),
//				storage = pl.PlaylistTracks
//							.Sum(plt => plt.Track.Bytes/100000.0)
				storage = (from plt in pl.PlaylistTracks
							select plt.Track.Bytes/1000000.0).Sum()
			};
//result3.Dump();

//list all albums with tracks showing the album title, artist name,
// number of tracks and the album cost

var result4 = from x in Albums
				where x.Tracks.Count() > 0
				select new
				{
					title = x.Title,
					artist = x.Artist.Name,
					trackcount = x.Tracks.Count()
				};
//result4.Dump();


//What is the maximum album count for all the artists.
var result5 = (Artists.Select(x => x.Albums.Count())).Max();
result5.Dump();
//var maxcount = result5.Max();
//maxcount.Dump();








