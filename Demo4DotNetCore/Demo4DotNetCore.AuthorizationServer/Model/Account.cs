using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    public class Account
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Portrait { get; set; }
        public bool IsLocked { get; set; }
    }
}