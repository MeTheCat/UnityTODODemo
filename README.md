# Unity TODO Game/App
	
    Show a list of tasks
    Add/Edit task functionality
    Exception handling

Game/UI logic is mainly done in TasksController

    UI buttons are locked down until async request completes to prevent multiple clicks
    There are no error popups for exceptions - just console messages or placeholder comments

## Potential improvements
    There is no Retry button/logic if loading fails
    Needs pagination, to load up x items and then load more when scrolling down
    Would be nice to save data to playerprefs, and load it up if there is no connection (offline mode)

![image](https://github.com/MeTheCat/UnityTODODemo/assets/49048115/2c896e36-3714-4556-ba75-1cf877ba2adc)
