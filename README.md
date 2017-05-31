Google Genomics API Client for .NET
==============================

Provides a strong-typed .NET DLL for Google Genomics API queries in [.NET Framework](www.microsoft.com/net)

This api-client-.NET open source project was created by the bioinformatics team at [Sequencing.com](https://sequencing.com)


Getting started
---------------

This .NET client allows users to call the Google Genomics API using the string-type .NET DLL and also allow for deep embedding in your software development projects.

•	To use, please have Visual Studio or [Web Matrix](www.microsoft.com/web/webmatrix/) installed. Web Matrix is free.

•	Download the project and open in either of the IDE as mentioned above.

•	You may use the sample web project to familiarize yourself with the functionality.

•	If you prefer to use just the binaries, however, then please copy them into your software adding a reference to the binaries.

•	To view a full list of methods use the intellisense provided by either of the tools.

•	You may also view the API documentation that will soon be online.

Troubleshooting
---------------
    
•	Please make sure that you have the correct JSON file with your login info as it will be required for authenticating requests.

•	If you get runtime errors please make sure that you have given your web server full rights to save files to local disk.

•	Note that not all of Google's APIs are callable at this time. The docs have a list 
<http://google-genomics.readthedocs.org/en/latest/auth_requirements.html> of which APIs are available.


Code layout
---------------

•	The code in this API implementation is organized in MVVM development pattern.

•	Code is also structured to fully leverage the object oriented programming techniques.

•	JSON and other newer technologies have been used whenever possible.

Project status
---------------

•	Functionality to connect to and query Google Genomics is provided.

•	This is an ongoing project. The bioinformatics team at [Sequencing.com](https://sequencing.com) will continue to add new code and we welcome the public to do the same.

Goals
---------------

•	Provide a strong-type .NET DLL and command line interface to the Google Genomics APIs (to make importing, querying, and other methods more accessible)

•	Provide an example of how to use the generated .NET client library.

Current status
---------------

This code is getting improvements!

Instead of being just a simple wrapper around API calls, the code will provide a strong-typed .NET DLL for integrating into your projects and well as command line will start providing additional functionality to make things simpler for callers. 

Please file feature requests (or email us directly at gittaca@sequencing.com) for additional things this awesome command line can do to make your life easier!

Adding new functionality and to Learn More
---------------

If you would like to see specific functionality added to this project or if you'd like to learn more about the other exciting projects at [Sequencing.com](https://sequencing.com), please contact the Sequencing.com bioinformatics team by emailing gittaca@sequencing.com.
