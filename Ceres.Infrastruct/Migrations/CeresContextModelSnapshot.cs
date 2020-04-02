﻿// <auto-generated />
using System;
using Ceres.Infrastruct.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ceres.Infrastruct.Migrations
{
    [DbContext(typeof(CeresContext))]
    partial class CeresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ceres.Domain.Models.Agenter", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Image")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("OID");

                    b.ToTable("Agenter");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Component", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("Important")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("NameCode")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.HasKey("OID");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Customer", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("AgenterOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<double>("InitHeight")
                        .HasColumnType("float");

                    b.Property<double>("InitWeight")
                        .HasColumnType("float");

                    b.Property<Guid>("LastOperaterOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SupporterOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("OID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerAssistDing", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid>("CustomerOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionnaireGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SupporterOid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OID");

                    b.ToTable("CustomerAssistDing");
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerDiet", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("CurrentDiet")
                        .HasColumnType("varchar(5000)")
                        .HasMaxLength(5000);

                    b.Property<Guid>("CustomerOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SupporterOid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OID");

                    b.ToTable("CustomerDiet");
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerDislikeFood", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid>("CustomerOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FoodOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OperaterOid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OID");

                    b.ToTable("CustomerDislikeFood");
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerJob", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerOid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OID");

                    b.ToTable("CustomerJob");
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerService", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("OID");

                    b.ToTable("CustomerService");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Food", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Classify")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("Click")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Coding")
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Image")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("OID");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("Ceres.Domain.Models.FoodComponent", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ComponentOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FoodOid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("OID");

                    b.ToTable("FoodComponent");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Service", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Image")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("OID");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Supporter", b =>
                {
                    b.Property<Guid>("OID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("OID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Image")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("OID");

                    b.ToTable("Supporter");
                });

            modelBuilder.Entity("Ceres.Domain.Models.Agenter", b =>
                {
                    b.OwnsOne("Ceres.Domain.Models.AgenterAddress", "Address", b1 =>
                        {
                            b1.Property<Guid>("AgenterOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.HasKey("AgenterOID");

                            b1.ToTable("Agenter");

                            b1.WithOwner()
                                .HasForeignKey("AgenterOID");
                        });
                });

            modelBuilder.Entity("Ceres.Domain.Models.Customer", b =>
                {
                    b.OwnsOne("Ceres.Domain.Models.CustomerAddress", "Address", b1 =>
                        {
                            b1.Property<Guid>("CustomerOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.HasKey("CustomerOID");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerOID");
                        });
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerDiet", b =>
                {
                    b.OwnsOne("Ceres.Domain.Models.Current", "Current", b1 =>
                        {
                            b1.Property<Guid>("CustomerDietOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DailyComponentPercentage")
                                .IsRequired()
                                .HasColumnType("varchar(5000)")
                                .HasMaxLength(5000);

                            b1.Property<string>("DailyEnergy")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.HasKey("CustomerDietOID");

                            b1.ToTable("CustomerDiet");

                            b1.WithOwner()
                                .HasForeignKey("CustomerDietOID");
                        });

                    b.OwnsOne("Ceres.Domain.Models.Discard", "Discard", b1 =>
                        {
                            b1.Property<Guid>("CustomerDietOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Reason")
                                .HasColumnType("varchar(500)")
                                .HasMaxLength(500);

                            b1.HasKey("CustomerDietOID");

                            b1.ToTable("CustomerDiet");

                            b1.WithOwner()
                                .HasForeignKey("CustomerDietOID");
                        });

                    b.OwnsOne("Ceres.Domain.Models.Recommend", "Recommend", b1 =>
                        {
                            b1.Property<Guid>("CustomerDietOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DailyComponentPercentage")
                                .IsRequired()
                                .HasColumnType("varchar(5000)")
                                .HasMaxLength(5000);

                            b1.Property<string>("DailyEnergy")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.Property<string>("DailyFoodComponent")
                                .IsRequired()
                                .HasColumnType("varchar(5000)")
                                .HasMaxLength(5000);

                            b1.HasKey("CustomerDietOID");

                            b1.ToTable("CustomerDiet");

                            b1.WithOwner()
                                .HasForeignKey("CustomerDietOID");
                        });

                    b.OwnsOne("Ceres.Domain.Models.SupportOperate", "LastOperate", b1 =>
                        {
                            b1.Property<Guid>("CustomerDietOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Oid")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("Time")
                                .HasColumnType("datetime");

                            b1.HasKey("CustomerDietOID");

                            b1.ToTable("CustomerDiet");

                            b1.WithOwner()
                                .HasForeignKey("CustomerDietOID");
                        });
                });

            modelBuilder.Entity("Ceres.Domain.Models.CustomerJob", b =>
                {
                    b.OwnsOne("Ceres.Domain.Models.Job", "Job", b1 =>
                        {
                            b1.Property<Guid>("CustomerJobOID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.Property<string>("Strength")
                                .IsRequired()
                                .HasColumnType("varchar(40)")
                                .HasMaxLength(40);

                            b1.HasKey("CustomerJobOID");

                            b1.ToTable("CustomerJob");

                            b1.WithOwner()
                                .HasForeignKey("CustomerJobOID");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
