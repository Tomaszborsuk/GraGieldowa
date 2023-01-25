﻿// <auto-generated />
using System;
using GraGieldowa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GraGieldowa.Model.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("GraGieldowa.Model.ClosedPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CloseDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ClosePrice")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("NetProfit")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpenPrice")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ROIPercent")
                        .HasColumnType("TEXT");

                    b.Property<string>("StockName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ClosedPositions");
                });

            modelBuilder.Entity("GraGieldowa.Model.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("GraGieldowa.Model.OpenPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("StockName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("OpenPositions");
                });

            modelBuilder.Entity("GraGieldowa.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("XTBPassword")
                        .HasColumnType("TEXT");

                    b.Property<string>("XTBUserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
