using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenDoBackend.DAO.Entities
{
    public class ReactionResources
    {
        [Key]
        public Guid UserReactionResourceId { get; set; }
        [Required]
        public int Available { get; set; }
        //connectios to User and Reaction
        [Required]
        public Guid OfUserId { get; set; }
        public User OfUser{ get; set; }
    }
}