using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneAPI_new.DTOs;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Mail;

namespace CapstoneAPI_new.Services
{
    public class DatabaseService
    {
        public static object verifyUser(UserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Incorrect password\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.account WHERE username = '" + user.username + "'";
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (!reader.HasRows)
            {
                result = new ContentResult { Content = "{\"text\": \"No user found\"}", StatusCode = 200, ContentType = "application/json" };
            }
            else
            {
                reader.Read();

                if (reader.GetString(2) == user.password)
                {
                    int userId = reader.GetInt16(0);
                    result = new ContentResult { Content = "{\"text\": \"Successful login\", \"userid\": " + userId + ", \"points\": " + reader.GetInt16(4) + "}", StatusCode = 200, ContentType = "application/json" };
                }

                reader.Close();
            }            

            ConnectionService.CloseConnection();
            return result;         
        }


        public static object createUser(UserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"That username is already taken\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.account WHERE username = '" + user.username + "'";
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (!reader.HasRows)
            {
                if (HelperService.isEmail(user.email))
                {
                    query = "INSERT INTO db_a84892_cmac23.account (username, password, email) VALUES ('" + user.username + "', '" + user.password + "', '" + user.email + "')";
                    noResult(query);
                    result =  new ContentResult { Content = "{\"text\": \"Account created\"}", StatusCode = 200, ContentType = "application/json" };
                }
                else
                    result = new ContentResult { Content = "{\"text\": \"The email provided is invalid\"}", StatusCode = 200, ContentType = "application/json" };
            }

            reader.Close();
            ConnectionService.CloseConnection();
            return result;
        }

        public static object forgotPassword(UserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Account not found\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE email = '" + user.email + "'";
            
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                string password = reader.GetString(0);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("infocusapplication@gmail.com");
                mail.To.Add(user.email);
                mail.Subject = "Forgot Password";
                mail.Body = "The password for your account is:   " + password;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("infocusapplication", "Zbn5+9KNhj");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                result = new ContentResult { Content = "{\"text\": \"Email sent\"}", StatusCode = 200, ContentType = "application/json" };
            }

            reader.Close();
            
            ConnectionService.CloseConnection();
            return result;
        }

        public static object updateEmail(UpdateUserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Incorrect password\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == user.password)
                {
                    reader.Close();

                    query = "UPDATE db_a84892_cmac23.account SET email = '" + user.newEmail + "' WHERE userid = " + user.userId;
                    results = new MySqlCommand(query, ConnectionService.connection);
                    results.ExecuteReader();

                    result = new ContentResult { Content = "{\"text\": \"Success\"}", StatusCode = 200, ContentType = "application/json" };
                }

                reader.Close();
            }

            ConnectionService.CloseConnection();
            return result;
        }

        public static object updateUsername(UpdateUserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Incorrect password\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == user.password)
                {
                    reader.Close();

                    query = "UPDATE db_a84892_cmac23.account SET username = '" + user.newUsername + "' WHERE userid = " + user.userId;
                    results = new MySqlCommand(query, ConnectionService.connection);
                    results.ExecuteReader();

                    result = new ContentResult { Content = "{\"text\": \"Success\"}", StatusCode = 200, ContentType = "application/json" };
                }

                reader.Close();
            }

            ConnectionService.CloseConnection();
            return result;
        }

        public static object updatePassword(UpdateUserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Incorrect password\"}", StatusCode = 200, ContentType = "application/json" };

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == user.password)
                {
                    reader.Close();

                    query = "UPDATE db_a84892_cmac23.account SET password = '" + user.newPassword + "' WHERE userid = " + user.userId;
                    results = new MySqlCommand(query, ConnectionService.connection);
                    results.ExecuteReader();

                    result = new ContentResult { Content = "{\"text\": \"Success\"}", StatusCode = 200, ContentType = "application/json" };
                }

                reader.Close();
            }

            ConnectionService.CloseConnection();
            return result;
        }

        public static object updatePoints(int userid, int points)
        {
            var result = new ContentResult { Content = "{\"text\": \"Success\"}", StatusCode = 200, ContentType = "application/json" };

            string query = "UPDATE db_a84892_cmac23.account SET points = " + points + " WHERE userid = " + userid;
            noResult(query);

            return result;
        }


        public static void noResult(string query)
        {
            ConnectionService.OpenConnection();

            var results = new MySqlCommand(query, ConnectionService.connection);
            results.ExecuteNonQuery();

            ConnectionService.CloseConnection();
        }

        public static void noResultDouble(string query1, string query2)
        {
            ConnectionService.OpenConnection();

            var results = new MySqlCommand(query1, ConnectionService.connection);
            results.ExecuteNonQuery();
            results = new MySqlCommand(query2, ConnectionService.connection);
            results.ExecuteNonQuery();

            ConnectionService.CloseConnection();
        }

        public static object newPlannerEntry(PlannerEntry entry)
        {
            string query = null;

            if (entry.reminder != null && entry.reminder != "")
                 query = "INSERT INTO db_a84892_cmac23.plannerevent (userid, title, descr, grp, dt, location, reminder, difficulty, fromUser, toUser, completed) VALUES('" + entry.userId + "', '" + entry.title + "', '" + entry.description + "', '" + entry.group + "', '" + entry.dateTime + "', '" + entry.location + "', '" + entry.reminder + "', '" + entry.difficulty + "', '" + entry.fromUser + "', '" + entry.toUser + "', '" + entry.completed + "');";
            else
                query = "INSERT INTO db_a84892_cmac23.plannerevent (userid, title, descr, grp, dt, location, difficulty, fromUser, toUser, completed) VALUES('" + entry.userId + "', '" + entry.title + "', '" + entry.description + "', '" + entry.group + "', '" + entry.dateTime + "', '" + entry.location + "', '" + entry.difficulty + "', '" + entry.fromUser + "', '" + entry.toUser + "', '" + entry.completed + "');";

            noResult(query);

            return new ContentResult { Content = "{\"text\": \"Entry sent\"}", StatusCode = 200, ContentType = "application/json" };
        }

        public static object updatePlannerEntry(PlannerEntry entry)
        {
            string query = null;

            if (entry.reminder != null)
                query = "UPDATE db_a84892_cmac23.plannerevent SET userid = " + entry.userId + ", title = '" + entry.title + "', descr = '" + entry.description + "', grp = '" + entry.group + "', dt = '" + entry.dateTime + "', location = '" + entry.location + "', reminder = '" + entry.reminder + "', difficulty = " + entry.difficulty + ", fromUser = '" + entry.fromUser + "', toUser = '" + entry.toUser + "', completed = " + entry.completed + " WHERE eventid = " + entry.eventId;
            else
                query = "UPDATE db_a84892_cmac23.plannerevent SET userid = " + entry.userId + ", title = '" + entry.title + "', descr = '" + entry.description + "', grp = '" + entry.group + "', dt = '" + entry.dateTime + "', location = '" + entry.location + "', reminder = NULL, difficulty = " + entry.difficulty + ", fromUser = '" + entry.fromUser + "', toUser = '" + entry.toUser + "', completed = " + entry.completed + " WHERE eventid = " + entry.eventId;



            noResult(query);

            return new ContentResult { Content = "{\"text\": \"Update sent\"}", StatusCode = 200, ContentType = "application/json" };
        }


        public static PlannerEntryArray plannerEntries(PlannerEntry user)
        {
            var plannerList = new List<PlannerEntry>();
            PlannerEntryArray entryArray = new PlannerEntryArray();

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.plannerevent WHERE userid = " + user.userId + " OR toUser = (SELECT username FROM db_a84892_cmac23.account WHERE userid = " + user.userId + ")";

            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            while (reader.Read())
            {
                string reminder = null;
                string date = ((DateTime)reader[5]).ToString("yyyy-MM-dd HH:mm:ss");
                if (!DBNull.Value.Equals(reader[7]))
                    reminder = ((DateTime)reader[7]).ToString("yyyy-MM-dd HH:mm:ss");
                plannerList.Add(new PlannerEntry {eventId = (int)reader[0], userId = (int)reader[1], title = (string)reader[2], description = (string)reader[3], group = (string)reader[4], dateTime = date, location = (string)reader[6], reminder = reminder, difficulty = (int)reader[8], fromUser = (string)reader[9], toUser = (string)reader[10], completed = (int)reader[11]});
            }

            reader.Close();

            ConnectionService.CloseConnection();

            entryArray.entryArray = plannerList.ToArray();

            return entryArray;
        }

        public static void updatePlannerCompletion(int eventid, int completed, int points, int userid)
        {
            string query1 = "UPDATE db_a84892_cmac23.plannerevent SET completed = " + completed + " WHERE eventid = " + eventid + "; ";
            string query2 = "UPDATE db_a84892_cmac23.account SET points = " + points + " WHERE userid = " + userid;

            //Console.WriteLine(query1 + query2);

            //noResultDouble(query1, query2);

            noResult(query1 + query2);

            //updatePoints(userid, points);
        }


        public static void unlockReward(RewardItem rewardInfo)
        {
            string query1 = "INSERT INTO db_a84892_cmac23.rewards (userid, plantid, label) VALUES (" + rewardInfo.userId + ", " + rewardInfo.plantId + ", '" + rewardInfo.label + "'); ";
            string query2 = "UPDATE db_a84892_cmac23.account SET points = points - " + rewardInfo.points + " WHERE userid = " + rewardInfo.userId + ";";

            noResult(query1 + query2);
        }

        public static RewardArray getRewards(RewardItem user)
        {
            var rewardList = new List<RewardItem>();
            RewardArray rewardArray = new RewardArray();

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.rewards WHERE userid = " + user.userId;
           
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            while (reader.Read())
            {
                rewardList.Add(new RewardItem { userId = (int)reader[1], plantId = (int)reader[2], label = (string)reader[3] });
            }

            reader.Close();

            ConnectionService.CloseConnection();

            rewardArray.rewardArray = rewardList.ToArray();

            return rewardArray;

        }

    }
}
