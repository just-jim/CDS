﻿// <auto-generated />
using System;
using CDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CDS.Infrastructure.Migrations
{
    [DbContext(typeof(CdsDbContext))]
    [Migration("20240430225013_CreateTables")]
    partial class CreateTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CDS.Domain.AssetAggregate.Asset", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FileFormat")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FileSize")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("Assets", (string)null);
                });

            modelBuilder.Entity("CDS.Domain.ContentDistributionAggregate.ContentDistribution", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("DistributionChannel")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateOnly>("DistributionDate")
                        .HasColumnType("date");

                    b.Property<string>("DistributionMethod")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("ContentDistributions", (string)null);
                });

            modelBuilder.Entity("CDS.Domain.OrderAggregate.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateOnly>("OrderDate")
                        .HasColumnType("date");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalAssets")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("CDS.Domain.AssetAggregate.Asset", b =>
                {
                    b.OwnsOne("CDS.Domain.AssetAggregate.Entities.Briefing", "Briefing", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("AssetId")
                                .HasColumnType("text");

                            b1.Property<string>("CreatedBy")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<DateOnly?>("CreatedDate")
                                .HasColumnType("date");

                            b1.HasKey("Id", "AssetId");

                            b1.HasIndex("AssetId")
                                .IsUnique();

                            b1.ToTable("Briefings", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AssetId");
                        });

                    b.Navigation("Briefing")
                        .IsRequired();
                });

            modelBuilder.Entity("CDS.Domain.ContentDistributionAggregate.ContentDistribution", b =>
                {
                    b.OwnsMany("CDS.Domain.ContentDistributionAggregate.Entities.AssetContentDistribution", "AssetContentDistributions", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ContentDistributionId")
                                .HasColumnType("uuid");

                            b1.Property<string>("AssetId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("FileUrl")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)");

                            b1.HasKey("Id", "ContentDistributionId");

                            b1.HasIndex("ContentDistributionId");

                            b1.ToTable("AssetContentDistributions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ContentDistributionId");
                        });

                    b.Navigation("AssetContentDistributions");
                });

            modelBuilder.Entity("CDS.Domain.OrderAggregate.Order", b =>
                {
                    b.OwnsMany("CDS.Domain.OrderAggregate.Entities.AssetOrder", "AssetOrders", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("OrderId")
                                .HasColumnType("text");

                            b1.Property<string>("AssetId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("Id", "OrderId");

                            b1.HasIndex("OrderId");

                            b1.ToTable("AssetOrders", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("AssetOrders");
                });
#pragma warning restore 612, 618
        }
    }
}