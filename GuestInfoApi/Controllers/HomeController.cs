using Microsoft.AspNetCore.Mvc;

namespace GuestInfoApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ContentResult Index()
        {
            var html = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <title>GuestInfo API</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f0f2f5;
            color: #333;
            margin: 0;
            padding: 0;
        }
        header {
            background-color: #2c3e50;
            color: white;
            padding: 20px;
            text-align: center;
        }
        main {
            max-width: 900px;
            margin: 40px auto;
            padding: 20px;
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        h1, h2 {
            margin-top: 0;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            text-align: left;
            padding: 12px;
            border-bottom: 1px solid #ddd;
        }
        th {
            background-color: #0078D4;
            color: white;
        }
        code {
            background-color: #eef;
            padding: 2px 6px;
            border-radius: 4px;
        }
        footer {
            text-align: center;
            color: #777;
            margin-top: 40px;
            font-size: 0.9em;
        }
    </style>
</head>
<body>
    <header>
        <h1>GuestInfo API</h1>
        <p>A Simple RESTful API for Guest Management</p>
    </header>
    <main>
        <h2>Available Endpoints</h2>
        <table>
            <tr>
                <th>Method</th>
                <th>Endpoint</th>
                <th>Description</th>
                <th>Sample Body</th>
            </tr>
            <tr>
                <td>GET</td>
                <td><code>/api/guestinfo</code></td>
                <td>Retrieve all guests</td>
                <td>N/A</td>
            </tr>
            <tr>
                <td>POST</td>
                <td><code>/api/guestinfo/name-by-mobile</code></td>
                <td>Get guest name by mobile number</td>
                <td><code>{ ""mobileNumber"": ""1234567890"" }</code></td>
            </tr>
            <tr>
                <td>POST</td>
                <td><code>/api/guestinfo/mobile-by-name</code></td>
                <td>Get mobile number by guest name</td>
                <td><code>{ ""guestName"": ""John Doe"" }</code></td>
            </tr>
            <tr>
                <td>POST</td>
                <td><code>/api/guestinfo/add</code></td>
                <td>Add a new guest</td>
                <td><code>{ ""guestName"": ""John Doe"", ""mobileNumber"": ""1234567890"" }</code></td>
            </tr>
            <tr>
                <td>PUT</td>
                <td><code>/api/guestinfo/update-name</code></td>
                <td>Update guest name by mobile number</td>
                <td><code>{ ""guestName"": ""New Name"", ""mobileNumber"": ""1234567890"" }</code></td>
            </tr>
            <tr>
                <td>PATCH</td>
                <td><code>/api/guestinfo/update</code></td>
                <td>Partially update guest details</td>
                <td><code>{ ""currentMobileNumber"": ""1234567890"", ""newGuestName"": ""New Name"" }</code></td>
            </tr>
            <tr>
                <td>DELETE</td>
                <td><code>/api/guestinfo/delete</code></td>
                <td>Delete guest by mobile number</td>
                <td><code>{ ""mobileNumber"": ""1234567890"" }</code></td>
            </tr>
            <tr>
                <td>HEAD</td>
                <td><code>/api/guestinfo/exists/{mobileNumber}</code></td>
                <td>Check if a guest exists</td>
                <td>N/A</td>
            </tr>
            <tr>
                <td>OPTIONS</td>
                <td><code>/api/guestinfo</code></td>
                <td>Retrieve supported methods</td>
                <td>N/A</td>
            </tr>
        </table>
        <p>Use Postman or any HTTP client to test the API.</p>
    </main>
    <footer>
        &copy; 2024 Cubemak Labs | Somanathan Gohulan
    </footer>
</body>
</html>";
            return Content(html, "text/html");
        }
    }
}
