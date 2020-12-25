﻿// <auto-generated />
using System;
using GreenDoBackend.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GreenDoBackend.Migrations
{
    [DbContext(typeof(GreenDoDBCobtext))]
    [Migration("20201024100617_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.LevelStats", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("Level");

                    b.ToTable("LevelStats");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.LevelUpConfig", b =>
                {
                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaxRandomCare")
                        .HasColumnType("int");

                    b.Property<int>("MaxRandomHeart")
                        .HasColumnType("int");

                    b.Property<int>("RequiredExperience")
                        .HasColumnType("int");

                    b.HasKey("Level");

                    b.ToTable("LevelUpConfig");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.Reaction", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Type");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.ReactionResources", b =>
                {
                    b.Property<Guid>("UserReactionResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Available")
                        .HasColumnType("int");

                    b.Property<Guid>("OfUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserReactionResourceId");

                    b.HasIndex("OfUserId")
                        .IsUnique();

                    b.ToTable("ReactionResources");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LevelStatsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ReactionResourcesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LevelStatsId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.JoinEntities.ReactionVideo", b =>
                {
                    b.Property<string>("ReactionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReactedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ReacterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReactionId", "VideoId");

                    b.HasIndex("ReacterId");

                    b.HasIndex("VideoId");

                    b.ToTable("ReactionVideo");
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.LevelStats", b =>
                {
                    b.HasOne("GreenDoBackend.DAO.Entities.LevelUpConfig", "Config")
                        .WithMany("LevelStatsList")
                        .HasForeignKey("Level")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.ReactionResources", b =>
                {
                    b.HasOne("GreenDoBackend.DAO.Entities.User", "OfUser")
                        .WithOne("ReactionResources")
                        .HasForeignKey("GreenDoBackend.DAO.Entities.ReactionResources", "OfUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.User", b =>
                {
                    b.HasOne("GreenDoBackend.DAO.Entities.LevelStats", "LevelStats")
                        .WithOne("User")
                        .HasForeignKey("GreenDoBackend.DAO.Entities.User", "LevelStatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GreenDoBackend.DAO.Entities.Video", b =>
                {
                    b.HasOne("GreenDoBackend.DAO.Entities.User", "CreatedBy")
                        .WithMany("PostedVideos")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GreenDoBackend.DAO.JoinEntities.ReactionVideo", b =>
                {
                    b.HasOne("GreenDoBackend.DAO.Entities.User", "Reacter")
                        .WithMany("Videos_ReactedAt")
                        .HasForeignKey("ReacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GreenDoBackend.DAO.Entities.Reaction", "Reaction")
                        .WithMany("ReactionVideos")
                        .HasForeignKey("ReactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GreenDoBackend.DAO.Entities.Video", "Video")
                        .WithMany("VideoReactions")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
