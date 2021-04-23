List<UserDTO> result = new List<UserDTO>();
var users = db.Users
              .Where(u => ...)
              .OrderBy(u => u.Name)
              .ToList();
result.AddRange(users);

var me = new UserDTO() { Name = "Akarki"};
var search = result.BinarySearch(me, Comparer<UserDTO>.Create((u1, u2) => u1.Name.CompareTo(u2.Name)));
// search: Ha nem találja, akkor egy negatív szám, ami a bit szintű negáltja a következő legnagyobb indexnek (vagyis annak az indexnek a negáltja ahová be kéne szúrni)
//Ha nincs nagyobb elem, akkor a bit szintű negáltja a Count-nak (vagyis szintén annak az indexnek ahová be keéne szúrni)
if(search < 0)
{
  result.Insert(~search, me);
}
else
{
  result.Insert(search, me);
}
//https://stackoverflow.com/questions/13170896/insert-into-a-list-alphabetically-c-sharp
//https://debugmode.net/2010/09/18/inserting-element-in-sorted-generic-list-list-using-binary-search/
//https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.binarysearch?view=net-5.0
