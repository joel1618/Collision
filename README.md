# Collision

Collision is a C# MVC 5 application written to consume real time flight position data, calculate a projected future position,
and then collision test projected positions against each other in an attempt to find currently operating aircraft that are on
a real time collision course with other currently operating aircraft.

Prerequisites:
<br/>1. Visual Studio 2015 Community Edition
<br/>2. Sql Server 2012+

<br/>To get started:
<br/>1. Fork or download the repository and open in Visual Studio 2015.
<br/>2. Set the 'Collision' application as your startup project.
<br/>&nbsp;&nbsp;&nbsp;2.1 You may be required to restore nuget packages.
<br/>3. Run the application
<br/>4. After the application has started you should be able to go to sql server and find a database created called 'Collision'
<br/>&nbsp;&nbsp;&nbsp;4.1 Go back to the project in Visual Studio
<br/>5. Set the 'Collision.Console' application as your startup project.
<br/>6. Run the application.
<br/>7. The console should output indicating what is happening.
<br/>8. The position table should be updated with the latest position information.

<br/>The application by default emulates aircraft flight data using the same data type as flightstats.com api.  Flightstats.com's api is an expensive and limited api for real world application at this time.


