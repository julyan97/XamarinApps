using GreenDoBackend.DAO.JoinEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDoBackend.DAO.Entities
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<Video> PostedVideos { get; set; }
        [Required]
        public Guid LevelStatsId { get; set; }
        public LevelStats LevelStats { get; set; }

        public Guid ReactionResourcesId { get; set; }
        public ReactionResources ReactionResources { get; set; }

        [JsonIgnore]
        public ICollection<ReactionVideo> Videos_ReactedAt { get; set; }

    }
}
