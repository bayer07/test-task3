Prerequisites:
MS SQL Server
.Net Core 5
NodeJs

The application consists from three parts:
- my-app
- TestTask3.API
- TestTask3.Worker
my-app is front end application based on React.
TestTask3.API is API for front end.
TestTask3.Worker is a scalable worker to handle html files to pdf files. The worker can be scaled by adding a new instances and configuration of application.json file. delayMs - delay time betweb a loop in milliseconds. chunckSize - size of chunk from SQL to handle. ConnectionStrings - string to connect to SQL.

What to improve?
- Use Queue to better parallel work.
- Beaustify front end by CSS and better markup.