using Demo4OAuth.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Demo4OAuth.AuthorizationServer.Model
{
    public class User
    {
        private string portrait;
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Portrait
        {
            set
            {
                try
                {
                    this.portrait = value;
                    var imgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + value);
                    var img = Image.FromFile(imgPath);
                    if (img != null)
                    {
                        PortraitBase64 = ImageTool.ImageToBase64(img);
                    }
                }
                catch (System.Exception)
                {
                }

            }
            get { return this.portrait; }
        }
        [NotMapped]
        public string PortraitBase64 { get; set; }
    }
}