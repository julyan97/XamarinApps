using System;
using System.Collections.Generic;
using System.Text;

namespace Models.API.Requests
{
    public class PostUserVideo
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string VideoName{ get; set; }
        public string VideoLink { get; set; }
        public string Description { get; set; }

    }
}
