using GreenDoBackend.DAO.Entities;
using GreenDoBackend.DAO.JoinEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDoBackend.DAO
{
    public class GreenDoDBCobtext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<ReactionVideo> reactionVideos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=GreenDo;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReactionVideo>()
                        .HasKey(rv => new { rv.ReactionId, rv.VideoId });
            modelBuilder.Entity<ReactionVideo>()
                .HasOne(rv => rv.Video)
                .WithMany(v => v.VideoReactions)
                .HasForeignKey(rv => rv.VideoId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ReactionVideo>()
                .HasOne(rv => rv.Reaction)
                .WithMany(r => r.ReactionVideos)
                .HasForeignKey(rv => rv.ReactionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReactionVideo>()
                .HasOne(rv => rv.Reacter)
                .WithMany(u => u.Videos_ReactedAt)
                .HasForeignKey(rv => rv.ReacterId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ReactionResources)
                .WithOne(rr => rr.OfUser)
                .HasForeignKey<ReactionResources>(rr => rr.OfUserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
