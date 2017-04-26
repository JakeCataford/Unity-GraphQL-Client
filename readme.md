Unity GraphQL Client
==========

This is an experimental GraphQL client for Unity that uses statically declared queries instead of a query builder. It's influenced by a swift/ios project called Apollo that aims to accomplish similar goals (and then some)

The goal is to be able to generate classes before the player is built from queries that are pre-defined in the editor. These classes represent the queries and their return objects, for example, lets say we are connecting to the well known [Star Wars API](http://graphql.org/swapi-graphql/)


Consider a query that gets all of the films from the database:
```graphql
query FetchAllFilms {
  allFilms(first: 10) {
    edges {
      node {
        id
        title
        releaseDate
      }
    }
  }
}
```

To create this query using this project, you would right click in the project folder and create a GraphQL Query Asset. In the editor for that asset you would edit your query in plain text, exactly as above, and name it whatever you want the generated class to be named.

After you are done editing this file, the editor generates a file with the types that you use to use the api internally:

```c#
var client = Client.Create("http://graphql.org/swapi-graphql");
client.query(new FetchAllFilmsQuery(), onSuccess: (result) => {}, onError: (error) => {});
```

Say you had a query that uses variables like this:

```graphql
query FetchFilm($id: ID!) {
  film(id: $id) {
        title
        releaseDate
  }
}
```

In this case we would generate a class that takes the variables as arguements in the constructor:

```c#
var client = Client.Create("http://graphql.org/swapi-graphql");
client.query(new FetchFilmQuery(id: "ZmlsbXM6MQ=="), onSuccess: (result) => {}, onError: (error) => {});
```

Part of the strength of statically defined queries is that we could generate generate types for the return information from the query that only contains methods for the fields you selected.

Consider the FetchFilm query from above:

```c#
var client = Client.Create("http://graphql.org/swapi-graphql");
client.query(new FetchFilmQuery(id: "ZmlsbXM6MQ=="), onSuccess: (result) => {
  Debug.Log(result.data.film.title);
  Debug.Log(result.data.film.relaseDate);

  Debug.Log(result.data.director); // would not compile, this method would not exist on the result type
}, onError: (error) => {});

// The compiled type might look something like this:

public class FetchFilmQuery {
  public string id;
  public FetchFilmQuery(string id) {
    this.id = id
  }

  public string QueryContent {
    get {
      return @"
        query FetchFilm($id: ID!) {
          film(id: $id) {
                title
                releaseDate
          }
        }
      ";
    }
  }

  class Result {
    class Data {
      class Film {
        public string title;
        public string desctiption;
      }
    }
  }
}
```

Of course, none of this is actually implemented yet and is subject to change, but this is the idea going forward with this project.


