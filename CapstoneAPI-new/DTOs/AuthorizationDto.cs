using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneAPI_new.DTOs
{
    public class AuthorizationHeader
    {
        [FromHeader]
        public string username { get; set; }
        [FromHeader]
        public string password { get; set; }
    }

    public class AuthorizationJWT
    {
        public string user { get; set; }
    }
}
