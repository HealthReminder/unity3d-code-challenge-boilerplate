The unit tests implemented test the class that stores persitent data.
The PersistentData is a static class responsible for saving data and retrieving it later on even after changing scenes.
The PersistentData is almost like a "cloud".

The first unit test routine:
Saves the inventory class to the "cloud"
Changes scene
Retrieve the inventory class from the "cloud"
Compare inventories

The second unit test routine:
Saves the player's money amount to the "cloud"
Changes scene
Retrieve the player's money amount from the "cloud"
Compare amounts

The priority of testing the persistency of data is due to the following conclusion:
To justify the design choice of having multiple scenes, it should be safe to assume that the project
Is going to scale to the point of actually needing to be divided into different scenes.
That means that the project may have lots of objects and information (Like an MMORPG would).
This assumption means that the ideal way to save data is in the simplest format possible. 
(Aspect that would save time if networking was implemented)
Because of the constant use of the persistent data and the need for it to distill information as much as it can
It is of the highest importance to test it in isolation.

