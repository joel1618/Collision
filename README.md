# Collision

Collision is a C# MVC 5 application written to consume real time flight position, calculate a projected future position,
and then collision test projected positions against each other in an attempt to find currently operating aircraft that are on
a real time collision course with other currently operating aircraft.

In order to run this application a user needs:
Visual Studio 2015 Community Edition
Sql Server 2012+

To get started:
1. Fork or download the repository and open in Visual Studio 2015.
2. Set the 'Collision' application as your startup project.
   2.1 You may be required to restore nuget packages.
3. Run the application
4. After the application has started you should be able to go to sql server and find a database created called 'Collision'
   4.1 Go back to the project in Visual Studio
5. Set the 'Collision.Console' application as your startup project.
6. Run the application
7. The console should output indicating what is happening.  
8. If you run 'select * from Position' in the sql database you should see positions actively being evaluated.

The application by default emulates the aircraft data as a reliable api to receive this data from is limited and expensive.  


