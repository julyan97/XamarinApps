using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenDoBackend.DAO.Entities
{
    public class LevelUpConfig
    {
        [Key]
        public int Level { get; set; }
        [Required]
        public int RequiredExperience { get; set; }
        [Required]
        public int MaxRandomCare { get; set; }
        [Required]
        public int MaxRandomHeart { get; set; }

        [JsonIgnore]
        public ICollection<LevelStats> LevelStatsList { get; set; }



    }
}