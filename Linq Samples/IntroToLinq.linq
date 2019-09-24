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