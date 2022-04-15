﻿using CapstoneAPI_new.DTOs;
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

        //-------------------------------------------------------------
        //  ACCOUNT MANAGEMENT
        // ------------------------------------------------------------

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

        [HttpPost("user/update/username")]
        public object updateUsername([FromBody] UpdateUserLogin user)
        {
            return DatabaseService.updateUsername(user);
        }

        [HttpPost("user/update/password")]
        public object updatePassword([FromBody] UpdateUserLogin user)
        {
            return DatabaseService.updatePassword(user);
        }

        [HttpPost("noresults")]
        public object noResultsRequest([FromBody] DatabaseQuery query)
        {
            DatabaseService.noResult(query.query);

            return new OkResult();
        }




        //-------------------------------------------------------------
        //  PLANNER MANAGEMENT
        // ------------------------------------------------------------

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



        //-------------------------------------------------------------
        //  TO DO LIST MANAGEMENT
        // ------------------------------------------------------------


    }
}
