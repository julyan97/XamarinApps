using GreenDoBackend.DAO.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDoBackend.DAO.JoinEntities
{
    public class ReactionVideo
    {
        // Reaction connection
        [Required]
        public string ReactionId { get; set; }
        public Reaction Reaction { get; set; }


        //Video connection
        [Required]
        public Guid VideoId { get; set; }
        public Video Video { get; set; }

        //User connection
        [Required]
        public Guid ReacterId { get; set; }
        public User Reacter { get; set; }

        [Required]
        public DateTime ReactedAt { get; set; }



    }
}
