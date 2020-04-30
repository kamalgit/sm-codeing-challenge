# BR StatMilk Coding Exercise

This is a small [dotnet core](https://www.microsoft.com/net) project that presents a few challenges to the user.  Instructions for completion are as followed.  To get the project running, execute the following:

      git clone git@github.com:br/sm-coding-challenge.git
      cd sm-coding-challenge
      dotnet restore
      dotnet run

## Problem

There are various third-party APIs that we ingest data from, including Turner/Sports Data, Twitter and Instagram. Our systems then store this data and return it for future date.  Our services must be fast and reliable given an influx in service downtime/degredation from providers as well as unexpected traffic spikes from our users.

One of the endpoints we ingest data from is similiar to this gist: https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json. This json is a subset of one week's box score data for NFL.

Currently this project has an 'IDataProvider' interface that has a a single method to get a player by a source Player ID.  This method fetches the above data and returns the first instance of a player if found.  Please update the project to add/meet the list of items in the requirements section.

## Requirements

* All urls on the home page should return without error
    * Kamal: All urls are good, except for "Inefficient Get Players". To be honest, I would need more time to be able to figure it out.
* Update the DataProvider interface and implementation to use async/await
    * Kamal: Done
* Refactor the DataProvider implementation as you see fit.  Comment on the changes you made.
    * Kamal: Done
* Add missing player attributes to the fetch so all data from the data provider is returned to the front-end
    * Kamal: Done
* Duplicates should be removed from the existing GetPlayers result(s)
    * Kamal: Done
* Implement the "LatestPlayers" method to return a structure similar to: https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/output.json.  Note, the concept of latest player is simply from a client's "needs".  Latest is in reference to the latest fetch of the above basic.json endpoint.  For this exercise the results will always be the same.
    * Kamal: I have tried to get further clarification on this part, but didn't get a proper answer to my questions. I made some assumptions and noted it down in my implementation.
* All responses should be performant.  None of these should take longer than a few miliseconds.  There are multiple solutions/avenues to this but think about the frequency that you fetch the data and different ways to mitigate iterating over too much data or making multiple requests.
    * Kamal: I used redis for caching. This would make it pretty fast, but not for the first time.
* If you remove/change/invalidate the url from the DataProvider fetch method, the system should still "work" (fail gracefully - up to you on your definition of work).
    * Kamal: Done. I have added a middleware class to handle any exception and return the error message in json.

### Considerations in Design

* What would happen if the remote endpoint goes down?  What are some ways to mitigate this that are transparent to the user?
    * Kamal: Well, I have added redis to cache the data. This would make the data available for clients till the cache expires or till the redis server is down. May be, as a next step, we presist the cache in disk rather than memory.
* What are some ways to prevent multiple calls to the data endpoint?
    * Kamal: I used caching to prevent that.
* This data set is not updated very frequently (once a week), what are some ways we could take advantage of this in our system?
    * Kamal: Based on that we can configure cache expiration to last longer. But causion is needed, not to miss data updates if we set longer than appropriate expiry intervals.

## Notes

There is no limit to what is acceptable as far as libraries, changes, and methodologies goes.  Feel free to add/remove/change methods, abstractions, etc. to facilitate your needs.  Feel free to comment on areas that are dubious or may present challenges in a real-world environment.

## Kamal: Prerequisites
Redis is a prerequisite, to be able to run the project, you need to run the following commands first.

    choco install redis-64
    redis-server
