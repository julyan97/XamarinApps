using System;
using System.Collections.Generic;
using System.Text;

namespace Models.API.Requests
{
    public class PostUserReaction
    {
        public Guid UserId { get; set; }
        public Guid VideoId{ get; set; }
        public string ReacterName { get; set; }
        public string ReactionType { get; set; }
        public string Change { get; set; }
    }
}
