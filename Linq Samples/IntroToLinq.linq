<Query Kind="Expression">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//sample of query syntax to dump the Artist data
from x in Artists
select x

//sample of method syntax to dump the Artist data
Artists
   .Select (x => x)
   
//sort  datainfo.Sort((x,y) => x.AttributeName.CompareTo(y.AttributeName))

//find any artist whose name cantains the string 'son"
from x in Artists
where x.Name.Contains("son")
select x

Artists
	.Where(x => x.Name.Contains("son"))
	.Select(x => x)
	
//create a list of albums released in 1970
//orderby title
Albums.OrderBy(x => x.Title)
	.Where(x => x.ReleaseYear == 1970)
	.Select(x => x)
	
from x in Albums
orderby x.Title
where x.ReleaseYear == 1970
select x

//create a list of albums release between 2007 and 2018
//order by release year then by title
from x in Albums
where x.ReleaseYear >= 2007
   && x.ReleaseYear <=2018
orderby x.ReleaseYear descending, x.Title
select x

//note the difference in method names using the method syntax
// a descending orderby is .OrderByDescending
// secondary and beyond ordering is .ThenBy
Albums
   .Where (x => ((x.ReleaseYear >= 2007) && (x.ReleaseYear <= 2018)))
   .OrderByDescending (x => x.ReleaseYear)
   .ThenBy (x => x.Title)

//Can navigational properties by used in queries
//create a list of albums by Deep Purple
//order by release year and title
//Show only the title, artist name, release year and release label

//use the navigational properties to obtain the Artist data
//new {....} create a new dataset (class definition)
from x in Albums
where x.Artist.Name.Equals("Deep Purple")
orderby x.ReleaseYear, x.Title
select new
{
	Title = x.Title,
	ArtistName = x.Artist.Name,
	RYear = x.ReleaseYear,
	RLabel = x.ReleaseLabel
}

















