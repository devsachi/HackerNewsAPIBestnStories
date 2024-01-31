# Best Stories
RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API.

## Environment
.NET 8.0, Visual Studio 2022 , C#

## Run the application
1. Clone https://github.com/vml19/best_stories.git](https://github.com/devsachi/HackerNewsAPIBestnStories.git
2. Open HackerNewsAPI.sln in Visual Studio 2022 and run HackerNewsAPI project
3. Application should be up and running at https://localhost:7273/swagger/index.html
4. Test GET /api/stories/{count} by inputting value for count, also the GET method can be tested using http://localhost:7273/api/stories/2 as well
5. Unit test projects can be executed from Test -> Test Explorer

## Assumptions
First time fetch would be slow as it fetches and caches all the data.
Not Authenticated


## Future enhancements if I had more time
1. API response would be improved by having a a backend task which runs periodically to fetch the new stories and caching it.
2. Caching can also be improved, although distributed caching in this scope would be a bit of overkill
3. More logging is needed 
4. Exception handing can be better
5. More comments in the code
6. Use docker
7. Use HeathCheck API
8. More Testing for the API
9. Versioning of the Controller should be provided
   
