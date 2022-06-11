# Database

## Database_Sqlite3
Implements SQLite3 database management. See Third-party libraries for more information about SQLite3.

## MySQLite reader

There are some SQLite database readers and feel free to choose the best one that fits your needs, two options are:
- SQLite studio [Open source]: https://sqlitestudio.pl/
- DB Browser for SQLite: https://sqlitebrowser.org/

# Database Scheme
The scheme for the frameworks is described in Database_scheme_content.h, there you will find a short description for each field, table and how they relate to each other.

## Database stcructures (DBS)
All system data structures can be found here. All data structures inherit from IAM_DBS and implement all needed virtual functions as save, remove or others that describe how the data should be accessed from the database. These structures allow an OOP structure on the generated data.

# About Database factory
Using the factory pattern we get the corresponding implementation as defined by the system directive.
