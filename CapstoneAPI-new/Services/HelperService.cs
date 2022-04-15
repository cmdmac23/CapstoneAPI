using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapstoneAPI_new.Services
{
    public class HelperService
    {
        public static bool isEmail(string email)
        {
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$";
            Regex rg = new Regex(pattern);

            return rg.Match(email).Success;
        }
    }
}
