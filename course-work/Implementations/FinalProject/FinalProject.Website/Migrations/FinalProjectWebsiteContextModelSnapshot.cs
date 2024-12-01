﻿// <auto-generated />
using System;
using FinalProject.Website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinalProject.Website.Migrations
{
    [DbContext(typeof(FinalProjectWebsiteContext))]
    partial class FinalProjectWebsiteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FinalProject.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("WagoonsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("HasCashDesk")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("PlatfromsCount")
                        .HasColumnType("int");

                    b.Property<int>("TracksCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Timetable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFinalStation")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStartStation")
                        .HasColumnType("bit");

                    b.Property<int>("PlatformNumber")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("TrainId");

                    b.ToTable("Timetable");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Train", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.Property<string>("TrainNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StationId");

                    b.ToTable("Train");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Timetable", b =>
                {
                    b.HasOne("FinalProject.Data.Entities.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalProject.Data.Entities.Train", "Train")
                        .WithMany("Timetables")
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");

                    b.Navigation("Train");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Train", b =>
                {
                    b.HasOne("FinalProject.Data.Entities.Category", "Category")
                        .WithMany("Trains")
                        .HasForeignKey("CategoryId");

                    b.HasOne("FinalProject.Data.Entities.Station", null)
                        .WithMany("Trains")
                        .HasForeignKey("StationId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Category", b =>
                {
                    b.Navigation("Trains");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Station", b =>
                {
                    b.Navigation("Trains");
                });

            modelBuilder.Entity("FinalProject.Data.Entities.Train", b =>
                {
                    b.Navigation("Timetables");
                });
#pragma warning restore 612, 618
        }
    }
}
