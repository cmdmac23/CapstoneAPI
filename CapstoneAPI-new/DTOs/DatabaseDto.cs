using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneAPI_new.DTOs
{
    public class DatabaseQuery
    {
        public string query { get; set; }
    }

    public class Response
    {
        public string text { get; set; }
        public int code { get; set; }
        public int userid { get; set; }
    }

    public class UserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class UpdateUserLogin
    {
        public int userId { get; set; }
        public string newUsername { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public string newEmail { get; set; }

    }

    public class PlannerEntry
    {
        public int eventId { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string group { get; set; }
        public string dateTime { get; set; }
        public string location { get; set; }
        public string reminder { get; set; }
        public int difficulty { get; set; }
        public string fromUser { get; set; }
        public string toUser { get; set; }      
        public int completed { get; set; }
    }

    public class PlannerEntryArray
    {
        public PlannerEntry[] entryArray { get; set; }
    }

    public class List
    {
        public int id { get; set; }
        public string title { get; set; }
        public bool isShared { get; set; }
        public string toUser { get; set; }
    }

    public class ListEntry
    {
        public string name { get; set; }
        public Boolean completed { get; set; }
    }
}
