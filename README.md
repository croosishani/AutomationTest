Clone the Project and open it through VS Code
If you haven't downloaded the extensions please install the extensions: C# Dev kit, C#, Dev Containers, Docker & .NET Install Tool
---------Go to Terminal and install these--------
dotnet new xunit -o APIRequestTest
cd APIRequestTest
dotnet add package RestSharp
dotnet add package Newtonsoft.Json

and finally run the command: dotnet restore

---To run the tests use the following command----
dotnet test
