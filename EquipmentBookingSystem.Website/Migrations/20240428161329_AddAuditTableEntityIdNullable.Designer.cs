﻿// <auto-generated />
using System;
using EquipmentBookingSystem.Website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EquipmentBookingSystem.Website.Migrations
{
    [DbContext(typeof(WebsiteDbContext))]
    [Migration("20240428161329_AddAuditTableEntityIdNullable")]
    partial class AddAuditTableEntityIdNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingItem", b =>
                {
                    b.Property<Guid>("BookingsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BookingsId", "ItemsId");

                    b.HasIndex("ItemsId");

                    b.ToTable("BookingItem");
                });

            modelBuilder.Entity("EquipmentBookingSystem.Website.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("EquipmentBookingSystem.Website.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BookedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookedFor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BookingEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("BookingStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("EquipmentBookingSystem.Website.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("BookingItem", b =>
                {
                    b.HasOne("EquipmentBookingSystem.Website.Models.Booking", null)
                        .WithMany()
                        .HasForeignKey("BookingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EquipmentBookingSystem.Website.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
