﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShamblesCom.Server.DB;

namespace ShamblesCom.Server.Migrations
{
    [DbContext(typeof(ShamblesDBContext))]
    [Migration("20210810131840_AddTeamImage")]
    partial class AddTeamImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nickname")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Nickname")
                        .IsUnique();

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.DriverProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("DriverId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("BLOB");

                    b.Property<string>("ImageMimeType")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("SeasonId", "DriverId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Race", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LivestreamLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrackId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VodLink")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("TrackId");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.RaceResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DriverId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("FastestLap")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Finished")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Penalties")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RaceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartPosition")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("RaceId");

                    b.HasIndex("TeamId");

                    b.ToTable("RaceResults");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LastId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LastSeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NextSeasonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LastId")
                        .IsUnique();

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.ShamblesUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.SiteSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CurrentHomeSeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LiveRaceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LiveStreamLinks")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrentHomeSeasonId");

                    b.HasIndex("LiveRaceId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("BLOB");

                    b.Property<string>("ImageMimeType")
                        .HasColumnType("TEXT");

                    b.Property<string>("MainColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecondaryColor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("Name", "SeasonId")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId", "Name")
                        .IsUnique();

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Category", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Game", "Game")
                        .WithMany("Categories")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.DriverProfile", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Driver", "Driver")
                        .WithMany("Profiles")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShamblesCom.Server.DB.Models.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Race", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Season", "Season")
                        .WithMany("Races")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShamblesCom.Server.DB.Models.Track", "Track")
                        .WithMany("Races")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.RaceResult", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Driver", "Driver")
                        .WithMany("RaceResults")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShamblesCom.Server.DB.Models.Race", "Race")
                        .WithMany("RaceResults")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShamblesCom.Server.DB.Models.Team", "Team")
                        .WithMany("RaceResults")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Race");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Season", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Category", "Category")
                        .WithMany("Seasons")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShamblesCom.Server.DB.Models.Season", "Last")
                        .WithOne("Next")
                        .HasForeignKey("ShamblesCom.Server.DB.Models.Season", "LastId");

                    b.Navigation("Category");

                    b.Navigation("Last");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.SiteSettings", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Season", "CurrentHomeSeason")
                        .WithMany()
                        .HasForeignKey("CurrentHomeSeasonId");

                    b.HasOne("ShamblesCom.Server.DB.Models.Race", "LiveRace")
                        .WithMany()
                        .HasForeignKey("LiveRaceId");

                    b.Navigation("CurrentHomeSeason");

                    b.Navigation("LiveRace");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Team", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Season", "Season")
                        .WithMany("Teams")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Track", b =>
                {
                    b.HasOne("ShamblesCom.Server.DB.Models.Game", "Game")
                        .WithMany("Tracks")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Category", b =>
                {
                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Driver", b =>
                {
                    b.Navigation("Profiles");

                    b.Navigation("RaceResults");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Game", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Race", b =>
                {
                    b.Navigation("RaceResults");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Season", b =>
                {
                    b.Navigation("Next");

                    b.Navigation("Races");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Team", b =>
                {
                    b.Navigation("RaceResults");
                });

            modelBuilder.Entity("ShamblesCom.Server.DB.Models.Track", b =>
                {
                    b.Navigation("Races");
                });
#pragma warning restore 612, 618
        }
    }
}
