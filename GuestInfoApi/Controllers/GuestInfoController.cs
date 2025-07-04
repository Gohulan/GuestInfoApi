using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using GuestInfoApi.Models;

namespace GuestInfoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKeyAuth]

    public class GuestInfoController : ControllerBase
    {
        private const string ConnectionString =
            @"Server=SG11LP\SQLEXPRESS;Database=sgdemo;User Id=sa;Password=#1mymicros;TrustServerCertificate=True;";

        [HttpGet]
        public ActionResult<IEnumerable<GuestInfo>> GetAllGuests()
        {
            var guests = new List<GuestInfo>();
            const string query = "SELECT GuestName, MobileNumber FROM GuestInfo";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                guests.Add(new GuestInfo
                {
                    GuestName = reader["GuestName"]?.ToString(),
                    MobileNumber = reader["MobileNumber"]?.ToString()
                });
            }

            return Ok(guests);
        }

        [HttpPost("name-by-mobile")]
        public ActionResult<string> GetGuestNameByMobile([FromBody] GuestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.MobileNumber))
                return BadRequest("Mobile number is required.");

            const string query = "SELECT GuestName FROM GuestInfo WHERE MobileNumber = @MobileNumber";
            string? guestName = ExecuteScalar(query, new SqlParameter("@MobileNumber", request.MobileNumber));

            return guestName != null ? Ok(guestName) : NotFound("Guest not found.");
        }

        [HttpPost("mobile-by-name")]
        public ActionResult<string> GetMobileByGuestName([FromBody] GuestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.GuestName))
                return BadRequest("Guest name is required.");

            const string query = "SELECT MobileNumber FROM GuestInfo WHERE GuestName = @GuestName";
            string? mobileNumber = ExecuteScalar(query, new SqlParameter("@GuestName", request.GuestName));

            return mobileNumber != null ? Ok(mobileNumber) : NotFound("Mobile number not found.");
        }

        [HttpPost("add")]
        public ActionResult AddGuest([FromBody] GuestInfo guest)
        {
            if (guest == null || string.IsNullOrEmpty(guest.GuestName) || string.IsNullOrEmpty(guest.MobileNumber))
                return BadRequest("GuestName and MobileNumber are required.");

            const string insertQuery = "INSERT INTO GuestInfo (GuestName, MobileNumber) VALUES (@GuestName, @MobileNumber)";
            int rowsAffected = ExecuteNonQuery(
                insertQuery,
                new SqlParameter("@GuestName", guest.GuestName),
                new SqlParameter("@MobileNumber", guest.MobileNumber)
            );

            return rowsAffected > 0
                ? Ok("Guest inserted successfully.")
                : StatusCode(500, "Failed to insert guest.");
        }

        [HttpDelete("delete")]
        public ActionResult DeleteGuestByMobile([FromBody] GuestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.MobileNumber))
                return BadRequest("Mobile number is required.");

            const string deleteQuery = "DELETE FROM GuestInfo WHERE MobileNumber = @MobileNumber";
            int rowsAffected = ExecuteNonQuery(
                deleteQuery,
                new SqlParameter("@MobileNumber", request.MobileNumber)
            );

            return rowsAffected > 0
                ? Ok("Guest deleted successfully.")
                : NotFound("Guest not found.");
        }

        [HttpPut("update-name")]
        public ActionResult UpdateGuestName([FromBody] GuestInfo guest)
        {
            if (guest == null || string.IsNullOrEmpty(guest.MobileNumber) || string.IsNullOrEmpty(guest.GuestName))
                return BadRequest("MobileNumber and GuestName are required.");

            const string updateQuery = "UPDATE GuestInfo SET GuestName = @GuestName WHERE MobileNumber = @MobileNumber";
            int rowsAffected = ExecuteNonQuery(
                updateQuery,
                new SqlParameter("@GuestName", guest.GuestName),
                new SqlParameter("@MobileNumber", guest.MobileNumber)
            );

            return rowsAffected > 0
                ? Ok("Guest name updated successfully.")
                : NotFound("Guest not found.");
        }

        [HttpPatch("update")]
        public ActionResult PatchGuest([FromBody] GuestUpdateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.CurrentMobileNumber))
                return BadRequest("CurrentMobileNumber is required.");

            var updates = new List<string>();
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(request.NewGuestName))
            {
                updates.Add("GuestName = @NewGuestName");
                parameters.Add(new SqlParameter("@NewGuestName", request.NewGuestName));
            }

            if (!string.IsNullOrEmpty(request.NewMobileNumber))
            {
                updates.Add("MobileNumber = @NewMobileNumber");
                parameters.Add(new SqlParameter("@NewMobileNumber", request.NewMobileNumber));
            }

            if (updates.Count == 0)
                return BadRequest("At least one field to update must be provided.");

            string updateQuery = $"UPDATE GuestInfo SET {string.Join(", ", updates)} WHERE MobileNumber = @CurrentMobileNumber";
            parameters.Add(new SqlParameter("@CurrentMobileNumber", request.CurrentMobileNumber));

            int rowsAffected = ExecuteNonQuery(updateQuery, parameters.ToArray());

            return rowsAffected > 0
                ? Ok("Guest record updated successfully.")
                : NotFound("Guest not found.");
        }

        [HttpHead("exists/{mobileNumber}")]
        public IActionResult CheckGuestExists(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
                return BadRequest("Mobile number is required.");

            const string query = "SELECT COUNT(1) FROM GuestInfo WHERE MobileNumber = @MobileNumber";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MobileNumber", mobileNumber);

            connection.Open();
            var count = (int)command.ExecuteScalar();

            return count > 0 ? Ok() : NotFound();
        }

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Append("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS,HEAD");
            return Ok();
        }


        /// <summary>
        /// Helper to execute scalar queries.
        /// </summary>
        private string? ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();
            var result = command.ExecuteScalar();
            return result?.ToString();
        }

        /// <summary>
        /// Helper to execute non-query commands.
        /// </summary>
        private int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}
