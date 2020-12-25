using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenDoBackend.DAO.Entities
{
    public class LevelStats
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public LevelUpConfig Config { get; set; }
        [Required]
        [ForeignKey(nameof(Config))]
        public int Level { get; set; }
        
        [Required]
        public int Experience { get; set; }

        
    }
}