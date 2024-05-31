using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using Xunit;

namespace APIRequestTest
{
    public class UnitTest1
    {
        private readonly string baseUrl = "https://api.restful-api.dev/objects";
        private readonly RestClient client;
        private static string addedObjectId="";
        private static string addedObjectId2="";

        //Constructor
        public UnitTest1()
        {
            client = new RestClient(baseUrl);
        }


        [Fact]
        public void Test_GetAllObjects()
        {
            // Initialization & Execution of GET request
            var request = new RestRequest("", Method.Get);
            var response = client.Execute(request);

            // Assertions
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(response.Content));

            // Print response
            System.Console.WriteLine("GET Request Response:");
            System.Console.WriteLine(response.Content);

            // Parse and assert JSON
            var jsonResponse = JArray.Parse(response.Content);
            Assert.NotEmpty(jsonResponse);

            foreach (var getObject in jsonResponse)
            {
                Assert.NotNull(getObject["id"]);
                Assert.NotNull(getObject["name"]);
                Assert.NotNull(getObject["data"]);
            }
        }



        [Fact]
        public void Test_AddObject()
        {
            // Request Body for POST
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(new
            {
                name = "Ishani MacBook Pro",
                data = new
                {
                    year = 2025,
                    price = 2002.99,
                    CPU_model = "Intel Core i9",
                    Hard_disk_size = "2 TB"
                }
            });

            // Execution of the POST request
            var response = client.Execute(request);

            // Assertion
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(response.Content));

            // Printing the response
            Console.WriteLine("Response Content:");
            Console.WriteLine(response.Content);

            // Parse and assert JSON
            var jsonResponse = JObject.Parse(response.Content);
            addedObjectId = jsonResponse["id"]?.ToString() ?? string.Empty;
            Assert.NotNull(addedObjectId);
            Assert.Equal("Ishani MacBook Pro", jsonResponse["name"]?.ToString() ?? string.Empty);
            Assert.Equal(2025, jsonResponse["data"]?["year"]?.Value<int>() ?? 0);
            Assert.Equal(2002.99, jsonResponse["data"]?["price"]?.Value<double>() ?? 0.0);      
           
        }
    
        [Fact]
        public void Test_GetSingleObject()
        {
            // GET Request
            var request = new RestRequest($"{addedObjectId}", Method.Get);
            var response = client.Execute(request);

            // Assertion
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(response.Content));

            // Printing the response
            Console.WriteLine("Response Content:");
            Console.WriteLine(response.Content);

            // Parse and assert JSON
            var jsonResponse = JObject.Parse(response.Content);
            Assert.NotNull(jsonResponse["id"]);
            Assert.Equal(addedObjectId, jsonResponse["id"]?.ToString() ?? string.Empty);
            Assert.Equal("Ishani MacBook Pro", jsonResponse["name"]?.ToString() ?? string.Empty);
            Assert.Equal(2025, jsonResponse["data"]?["year"]?.Value<int>() ?? 0);
            Assert.Equal(2002.99, jsonResponse["data"]?["price"]?.Value<double>() ?? 0.0);
        }


 
        [Fact]
        public void Test_UpdateObject()
        {
            // Using the previous method ID
            Assert.False(string.IsNullOrEmpty(addedObjectId), "No object ID to update.");

            // Put Request
            var putRequest = new RestRequest($"{addedObjectId}", Method.Put);
            putRequest.AddJsonBody(new
            {
                name = "Updated Ishani MacBook Pro",
                data = new
                {
                    year = 2029,
                    price = 5678.99,
                    CPU_model = "Intel Core i10",
                    Hard_disk_size = "5 TB"
                }
            });

            // Updating the Object
            var putResponse = client.Execute(putRequest);

            // Assertion
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.False(string.IsNullOrEmpty(putResponse.Content));

            // Printing the response to console
            Console.WriteLine("Response Content:");
            Console.WriteLine(putResponse.Content);

            // Parse and assert JSON
            var putJsonResponse = JObject.Parse(putResponse.Content);
            addedObjectId2 = putJsonResponse["id"]?.ToString() ?? string.Empty;
            Assert.NotNull(addedObjectId2);
            Assert.Equal(addedObjectId, putJsonResponse["id"]?.ToString() ?? string.Empty);
            Assert.Equal("Updated Ishani MacBook Pro", putJsonResponse["name"]?.ToString() ?? string.Empty);
            Assert.Equal(2029, putJsonResponse["data"]?["year"]?.Value<int>() ?? 0);
            Assert.Equal(5678.99, putJsonResponse["data"]?["price"]?.Value<double>() ?? 0.0);
        }


[Fact]
        public void Test_DeleteObject()
        {
            // Adding the object
            Test_AddObject();

            // Adding the method
            var deleteRequest = new RestRequest($"{addedObjectId}", Method.Delete);

            // Deleting the Object
            var deleteResponse = client.Execute(deleteRequest);

            // Assertion
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            // Verify the deletion
            var getRequest = new RestRequest($"{addedObjectId}", Method.Get);
            var getResponse = client.Execute(getRequest);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }


    
       





    }
}