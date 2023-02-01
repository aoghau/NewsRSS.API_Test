# NewsRSS.API_Test
A Web API application that allows user to add RSS feeds and check news from them

The database was implemented using PostgreSQL.
Authorization system uses JWT bearer token. This means that with every request to restricted endpoints, you need to pass the JWT token in the request header. Swagger canot do that, Postman, however, can.

Endpoints
1. Register(string name, string password) - adds a user into the database with given credentials
2. Login(string name, string password) - generates a JWT token and returns it to you if the credentials are valid. The JWT token is needed to access further endpoints

[Authorization required further]

3. Add Feed(string url) - adds an RSS feed with given url, it must be valid
4. Get active feeds - returns active feeds
5. Add news - adds news from the added RSS feeds
6. Get news from a date(DateTime date) - gets all unread news from the date specified. Date must be in the format DD-Mon-YYYY
7. Set news as read - sets all unread news as read
