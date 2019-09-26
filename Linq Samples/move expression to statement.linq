<Query Kind="Statements">
  <Connection>
    <ID>502dd813-ceef-4a93-8663-148ad56997b5</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

//when using the Language C# Statement(s)
//your work will need to confirm to C# statement syntax
//ie  datatype variable = expression;

//Can navigational properties by used in queries
//create a list of albums by Deep Purple
//order by release year and title
//Show only the title, artist name, release year and release label

//use the navigational properties to obtain the Artist data
//new {....} create a new dataset (class definition)
var results = from x in Albums
				where x.Artist.Name.Equals("Deep Purple")
				orderby x.ReleaseYear, x.Title
				select new
				{
					Title = x.Title,
					ArtistName = x.Artist.Name,
					RYear = x.ReleaseYear,
					RLabel = x.ReleaseLabel
				};
				
//to display contents of a variable in LinqPad
//using the method .Dump()
//this method is only used in LinqPad, it is NOT a C# method
results.Dump();
				
				
				
				
				
				
				
				
				
				