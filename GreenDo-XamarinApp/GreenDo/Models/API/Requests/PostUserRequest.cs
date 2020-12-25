using System;
using System.Collections.Generic;
using System.Text;

namespace Models.API.Requests
{
    public class PostUserRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

}
