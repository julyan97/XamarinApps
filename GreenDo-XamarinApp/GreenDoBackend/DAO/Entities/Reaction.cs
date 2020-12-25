using GreenDoBackend.DAO.JoinEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDoBackend.DAO.Entities
{
    public class Reaction
    {
        [Key]
        public string Type { get; set; }
        [Required]
        public string Code { get; set; }

        //reaction - video connection
        [JsonIgnore]
        public IEnumerable <ReactionVideo> ReactionVideos { get; set; }
    }
}
