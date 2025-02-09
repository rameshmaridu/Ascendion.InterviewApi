To Run the application, just clone the branch and Run on Visual Studio.

Implementation:

API controller contains only one Get method, which takes Number of Best stories as a parameter.
below is the format of the api URL
https://localhost:7153/BestStories/GetBestStories?numberOfStories=0

Assumption:
not clear about the IDs of best stories API result set that its already sorted with the Highest scored stories or not.
So added a parameter to Config file which takes care about this.

App Config Parameters:
AbsoluteExpiration: which talks about the Cache properties.
SlidingExpiration: which talks about the Cache properties.
StoryIdURL: URL which is used to fetch story IDs
StoryURL: URL which is used to fetch story details
IsStoryIdsSorted: this indicates whether StoryIds form StoryId URL is sorted or not. if true, this is will initially takes only required Ids from list and fetch the Stories and then Sort the stories else, service will fetch all the stories and then sort and then return required number of stories both the cases it uses Cache so operations performance will not be effected.

Improvements:
Authentication can be implemented.

