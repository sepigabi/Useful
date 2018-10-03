var originalObject = [
  {"first":"Gretchen","last":"Kuphal","email":"greenwolf54@gmail.com","address":"416 Lesch Road","created":"March 1, 2012","balance":"$9,782.26"},
{"first":"Morton","last":"Mayer","email":"salmonsquirrel25@gmail.com","address":"1602 Bernhard Parkway","created":"April 29, 2017","balance":"$6,596.11"},
{"first":"Catalina","last":"Daugherty","email":"Catalina.Daugherty@filomena.name","address":"11893 Kali Vista","created":"October 16, 2008","balance":"$6,372.86"},
{"first":"Orpha","last":"Heaney","email":"turquoisewolf22@gmail.com","address":"8090 Chris Stream","created":"November 21, 2015","balance":"$9,596.26"},
{"first":"Reva","last":"Mohr","email":"Reva.Mohr@oda.net","address":"0291 Kailyn Stravenue","created":"November 6, 2014","balance":"$4,768.37"},
{"first":"Loma","last":"Keeling","email":"turquoisegiraffe09@gmail.com","address":"84460 Samson Knoll","created":"June 13, 2017","balance":"$9,361.16"}
];

var duplicateObject = JSON.parse(JSON.stringify( originalObject ));
