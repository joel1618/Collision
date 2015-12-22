# Collision

Collision is a C# MVC 5 application written to consume real time flight position, calculate a projected future position,
and then collision test projected positions against each other in an attempt to find currently operating aircraft that are on
a real time collision course with other currently operating aircraft.

Prerequisites:
<br/>Visual Studio 2015 Community Edition
<br/>Sql Server 2012+

<br/>To get started:
<br/>1. Fork or download the repository and open in Visual Studio 2015.
<br/>2. Set the 'Collision' application as your startup project.
   <br/>2.1 You may be required to restore nuget packages.
<br/>3. Run the application
<br/>4. After the application has started you should be able to go to sql server and find a database created called 'Collision'
   <br/>4.1 Go back to the project in Visual Studio
<br/>5. Set the 'Collision.Console' application as your startup project.
<br/>6. Run the application
<br/>7. The console should output indicating what is happening.  
<br/>8. If you run 'select * from Position' in the sql database you should see positions actively being evaluated.

<br/>The application by default emulates the aircraft data as a reliable api to receive this data from is limited and expensive.  


