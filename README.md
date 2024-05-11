# Student-Activity-Simulation
***NOTE** This was a proof-of-concept project for my University, and as such was tailored specifically to the UMBC campus. If you wish to modify the code to suit it to a different College, do not download the executable and instead modify the Unity project to your specific use-case. 

## Description
The project, in its simplicity, is a mathmatics-based approach to determining the locations of Students on the UMBC campus at any given time. The Project has interactive UI elements drawn to the screen, but the main logic takes place "under the hood" and will be documented below. 

This simulation represents an algorithmic artificial intelligence model where students decide where and when to go based off of a variety of factors
1. Walking speed
2. If they even need to move
3. Distance to destination building
4. If there is an available path between their current location and their destination location

See the UML Diagram Below for more details on how everything is calculated.

##### Inspiration
As a student-athlete living in the freshman dorm halls, I see both sides of the coin when it comes to low attendance at university-wide events. Many of these events are planned for freshmen and other underclassmen to meet each other and become more familiar with university culture, but a major and ongoing problem is that these events are not planned with enough forethought in their scheduling and most students cannot attend, despite wanting to. There have been countless events and mandatory RA floor meetings that I have missed due to either being in class, at practice, or in transit. I truly believe that these events would have much higher attendance if only those in charge of scheduling had more resources available to gain insight on the typical student’s schedule.
 

## UML Diagram of Project

**INSERT UML HERE**

## Different Classes
In an effort to make the project as organized as possible, there are many classes throughout the project. Half of which pertain to the actual logic of the Students in the simulation, and the other half dealing with the pathfinding algorithm that students use. 

##### Custom Time Class
This class has values for seconds, minutes, hours, and days. It runs on a timer and essentially is a custom-made 7 day timer that the rest of the program uses to run.

##### Player Class
The Player class is the most basic class in the project. It simply is the building block for Student objects (child class) and takes in information such as name, age, and address for the student. 

##### Place Class
The Place class is a simple class, as it just has the name of the place and float values for startX, endX, startY, endY. This implementation of a Place limits objects to being rectangular, but for a proof-of-concept project like this all campus buildings can be estimated to a rectangle of best fit. 

##### Activity Class
The activity class is another building-block class that lets users create Activity Objects. An Activity consists of a name, Place object of where it is being held, CustomTime objects of when it starts and ends, and information such as weekly frequency of activity, total times completed, and priority. The priority is decided by the end-user, but the goal is to determine a way to rank importance of events. For example, if a Student had a frat party and a final at the same time, the final would have a higher priority and thus would be the Activity that the Student does. 

##### Student Class
The Student class is the heart of the project, and utilizes all other classes in different ways. 

1. First and foremost, it takes two Place objects representing the Student's current place and their destination. If the student does not need to travel anywhere, their destination will be equal to the current place. The student also takes a Vector3 representing its current world position, so that calculations about distance can be made more accurately.

2. The student also has two lists of Activities, representing the list of total activities the Student has and the list of Activities the student has that day. At midnight (Simulation time) the list of total activities is sorted through and if the activity takes place on the current day, it gets added to the daily schedule. From there, Activies at the same time are filtered by priority, then by amount of times weekly then amount of total times completed. Once the daily list is completed, the student will base all movement calculations based off of the next Activity in the daily list. 

## Pathfinding Aspect
This project uses an A* pathfinding algorithm to determine the paths that the students use. Essentially, the entire map of the UMBC campus is divided into 10x10 (pixel) grid objects, and stored in a 2d array of such grid objects. This allows the program to convert a grid into world space easily, which means the program does not need to keep track of which grid Students are on, it can mathmatically convert their world space into a grid index. This improves time complexity tenfold as this operation runs in O(1), whereas keeping an array of students in a 2d array of grids would run in O(n^3). 

Each grid object, by default, is set to be walkable. At the start of runtime, the project will run through the grid and determine whether the grid object lays on top of a building, or other non-walkable object. These nonwalkables are stored for later use, and that particular grid is no longer traversalbe. In the event that a Student is inside a building currently, and needs to walk to a different building, all grid spaces over both of those buildings are temporarily marked as walkable, and reverted back at the comletion of traversal.

From there, the program uses the A* algorithm to determine which grid objects to add to the path, and then the students walk across that path smoothly. 

# Other Information

## Future Upgrades
In an attempt to further develop this project, the following could be added:
1. Teacher objects (could be represented by red circles rather than green)
2. Student-Athlete objects which account for their practices and games.
3. Differentiating between Graduate and Undergraduate students (schedules would remain similar would likely just be a different color visually)


## Use Cases
As mentioned in the top of the file, this is a very niche project with specific use-cases. The main use case for this project is for UMBC to gain insight on the whereabouts of their students at any given date and time. This will allow them to plan better school-wide functions that will engage their students more. While I do not have access to each student’s name, schedule, residence, and athlete status, the UMBC staff does. It is my aim for them to download this project, add each student’s data from their database as a student in the simulation, and have accurate enough calculations that the simulation will correctly predict the location and business of each added student. 


