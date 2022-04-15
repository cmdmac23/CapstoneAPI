using CapstoneAPI_new.DTOs;
using CapstoneAPI_new.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CapstoneAPI_new.Controllers
{
    [ApiController]
    [Route("api/database")]
    //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DatabaseController
    {
        [HttpPost("test")]
        public void test([FromBody] DatabaseQuery query)
        {
            ConnectionService.OpenConnection();

            var results = new MySqlCommand(query.query, ConnectionService.connection);
            results.ExecuteNonQuery();

            ConnectionService.CloseConnection();
        }

        [HttpPost("login")]
        public object login([FromBody] UserLogin user)
        {
            Console.WriteLine(user.username);

            return DatabaseService.verifyUser(user);
        }

        [HttpPost("createuser")]
        public object createUser([FromBody] UserLogin user)
        {
            return DatabaseService.createUser(user);
        }

        [HttpPost("forgotpassword")]
        public object forgotPassword([FromBody] UserLogin user)
        {
            return DatabaseService.forgotPassword(user);
        }

        [HttpPost("user/update/email")]
        public object updateEmail([FromBody] UpdateUserLogin user)
        {
            return DatabaseService.updateEmail(user);
        }

        [HttpPost("noresults")]
        public object noResultsRequest([FromBody] DatabaseQuery query)
        {
            DatabaseService.noResult(query.query);

            return new OkResult();
        }

        [HttpPost("planner/entries/add")]
        public object addPlannerEntry([FromBody] PlannerEntry query)
        {
            return DatabaseService.newPlannerEntry(query);
        }

        [HttpPost("planner/entries/update")]
        public object updatePlannerEntry([FromBody] PlannerEntry query)
        {
            return DatabaseService.updatePlannerEntry(query);
        }

        [HttpPost("planner/entries")]
        public object getPlannerEntries([FromBody] PlannerEntry user)
        {
            var plannerArray = new PlannerEntryArray();

            plannerArray = DatabaseService.plannerEntries(user);

            var json = JsonSerializer.Serialize(plannerArray);

            return new ContentResult { Content = json, StatusCode = 200, ContentType = "text/json" };
        }

        [HttpPost("planner/entries/complete")]
        public object updatePlannerCompleted([FromBody] PlannerEntry entry)
        {
            DatabaseService.updatePlannerCompletion(entry.eventId, entry.completed);

            return new OkResult();
        }

        [HttpGet("todo/list")]
        public object getList([FromBody] DatabaseQuery query)
        {
            var toDoList = new List<ListEntry>();

            toDoList = DatabaseService.toDoEntries(query.query);

            var json = JsonSerializer.Serialize(toDoList);

            return new ContentResult { Content = json, StatusCode = 200, ContentType = "text/json" };
        }
    }
}
