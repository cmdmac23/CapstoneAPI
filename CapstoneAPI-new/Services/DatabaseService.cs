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

            string encryptedPass = HelperService.encryptPassword(user.password);

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

                if (reader.GetString(2) == encryptedPass)
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

            string encryptedPass = HelperService.encryptPassword(user.password);

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.account WHERE username = '" + user.username + "'";
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (!reader.HasRows)
            {
                if (HelperService.isEmail(user.email))
                {
                    query = "INSERT INTO db_a84892_cmac23.account (username, password, email) VALUES ('" + user.username + "', '" + encryptedPass + "', '" + user.email + "')";
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

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("infocusapplication@gmail.com");
                mail.To.Add(user.email);
                mail.Subject = "inFocus: Forgot Password";
                mail.IsBodyHtml = true;

                string body = "<a href=\"http://cmac23-001-site1.etempurl.com/reset\">Click here to reset your password for inFocus</a>";

                mail.Body = body;

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

            string encryptedPass = HelperService.encryptPassword(user.password);

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == encryptedPass)
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

            string encryptedPass = HelperService.encryptPassword(user.password);

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == encryptedPass)
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

            string encryptedPass = HelperService.encryptPassword(user.password);
            string encryptedPassNew = HelperService.encryptPassword(user.newPassword);

            ConnectionService.OpenConnection();

            string query = "SELECT password FROM db_a84892_cmac23.account WHERE userid = " + user.userId;
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader.GetString(0) == encryptedPass)
                {
                    reader.Close();

                    query = "UPDATE db_a84892_cmac23.account SET password = '" + encryptedPassNew + "' WHERE userid = " + user.userId;
                    results = new MySqlCommand(query, ConnectionService.connection);
                    results.ExecuteReader();

                    result = new ContentResult { Content = "{\"text\": \"Success\"}", StatusCode = 200, ContentType = "application/json" };
                }

                reader.Close();
            }

            ConnectionService.CloseConnection();
            return result;
        }

        public static object resetPassword(UserLogin user)
        {
            var result = new ContentResult { Content = "{\"text\": \"Password updated\"}", StatusCode = 200, ContentType = "application/json" };

            string encryptedPass = HelperService.encryptPassword(user.password);

            string query = "UPDATE db_a84892_cmac23.account SET password = '" + encryptedPass + "' WHERE email = '" + user.email + "'";
            noResult(query);

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

        public static object newToDoList(ToDoList list)
        {
            string query = null;

            query = "INSERT INTO db_a84892_cmac23.todolist (userid, title, grp, listitem, fromUser, toUser, completed) VALUES('" + list.userId + "', '" + list.title + "', '" + list.group + "', '" + list.listItem + "','" + list.fromUser + "', '" + list.toUser + "', '" + list.completed + "');";

            noResult(query);

            return new ContentResult { Content = "{\"text\": \"Entry sent\"}", StatusCode = 200, ContentType = "application/json" };
        }

        public static object newToDoListFull(ToDoList list)
        {
            string query = null;

            // insert initial todolist object into database
            query = "INSERT INTO db_a84892_cmac23.todolist (userid, title, grp, listitem, fromUser, toUser, completed) VALUES ('" + list.userId + "', '" + list.title + "', '" + list.group + "', '" + list.listItem + "','" + list.fromUser + "', '" + list.toUser + "', '" + list.completed + "');";
            noResult(query);

            ConnectionService.OpenConnection();
            query = "SELECT listid FROM db_a84892_cmac23.todolist WHERE userid = " + list.userId + " ORDER BY listid DESC LIMIT 1";
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();
            
            // get listid of the entry you just created
            reader.Read();
            list.listId = reader.GetInt32(0);
            reader.Close();

            ConnectionService.CloseConnection();

            // add each list item into table
            foreach(ToDoListItem item in list.listItemArray)
            {
                query = "INSERT INTO db_a84892_cmac23.todolistitem (listid, itemname, difficulty, completed) VALUES (" + list.listId + ", '" + item.itemName + "', " + item.difficulty + ", " + item.completed + ")";
                noResult(query);
            }

            return new ContentResult { Content = "{\"text\": \"Entry sent\"}", StatusCode = 200, ContentType = "application/json" };
        }

        public static object updateToDoList(ToDoList list)
        {
            string query = null;

            // insert initial todolist object into database
            query = "UPDATE db_a84892_cmac23.todolist SET userid = " + list.userId + ", title = '" + list.title + "', grp = '" + list.group + "', listitem = '" + list.listItem + "', fromuser = '" + list.fromUser + "', touser = '" + list.toUser + "', completed = " + list.completed + " WHERE listid = " + list.listId;
            noResult(query);

            ConnectionService.OpenConnection();
            query = "SELECT listid FROM db_a84892_cmac23.todolist WHERE userid = " + list.userId + " ORDER BY listid DESC LIMIT 1";
            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            // get listid of the entry you just created
            reader.Read();
            list.listId = reader.GetInt32(0);
            reader.Close();

            ConnectionService.CloseConnection();

            // add each list item into table
            foreach (ToDoListItem item in list.listItemArray)
            {
                query = "UPDATE db_a84892_cmac23.todolistitem SET listid = '" + item.listId + "', itemname = '" + item.itemName + "', difficulty = '" + item.difficulty + "', completed = " + item.completed + " WHERE listitemId = " + item.listItemId;
                noResult(query);
            }

            return new ContentResult { Content = "{\"text\": \"Update sent\"}", StatusCode = 200, ContentType = "application/json" };
        }

        public static ToDoListArray toDoLists(ToDoList user)
        {
            var toDoListList = new List<ToDoList>();
            var toDoListItemList = new List<ToDoListItem>();
            ToDoListArray listArray = new ToDoListArray();

            ConnectionService.OpenConnection();

            string query = "SELECT * FROM db_a84892_cmac23.todolist WHERE userid = " + user.userId + " OR toUser = (SELECT username FROM db_a84892_cmac23.account WHERE userid = " + user.userId + ")";

            var results = new MySqlCommand(query, ConnectionService.connection);
            var reader = results.ExecuteReader();

            while (reader.Read())
            {
                toDoListList.Add(new ToDoList { listId = (int)reader[0], userId = (int)reader[1], title = (string)reader[2], group = (string)reader[3], fromUser = (string)reader[5], toUser = (string)reader[6], completed = (int)reader[7] });
            }

            reader.Close();

            foreach(ToDoList listHeader in toDoListList)
            {
                query = "SELECT * FROM db_a84892_cmac23.todolistitem WHERE listid = " + listHeader.listId;
                results = new MySqlCommand(query, ConnectionService.connection);
                reader = results.ExecuteReader();
                while (reader.Read())
                {
                    toDoListItemList.Add(new ToDoListItem { listItemId = (int)reader[0], listId = (int)reader[1], itemName = (string)reader[2], difficulty = (int)reader[3], completed = (int)reader[4] });
                }
                reader.Close();
                listHeader.listItemArray = toDoListItemList.ToArray();
                toDoListItemList.Clear();
            }

            ConnectionService.CloseConnection();

            listArray.listArray = toDoListList.ToArray();

            return listArray;
        }

        public static void updateToDoCompletion(int listItemId, int completed, int points, int userid)
        {
            string query1 = "UPDATE db_a84892_cmac23.todolistitem SET completed = " + completed + " WHERE listitemid = " + listItemId + "; ";
            string query2 = "UPDATE db_a84892_cmac23.account SET points = " + points + " WHERE userid = " + userid;

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
