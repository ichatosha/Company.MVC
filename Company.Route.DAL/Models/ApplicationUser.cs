﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{ 
    public class ApplicationUser : IdentityUser // <string> as default
    {


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAgree { get; set; }
        



    }
}
