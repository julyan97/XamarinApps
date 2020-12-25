using GreenDoBackend.DAO.JoinEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenDoBackend.DAO.Entities
{
    public class Video
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        // user connection
        [Required]
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        //vide - reaction connection
        [JsonIgnore]
        public IEnumerable<ReactionVideo> VideoReactions { get; set; }

    }
}