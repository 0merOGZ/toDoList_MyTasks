﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tod.Models;

#nullable disable

namespace tod.Migrations
{
    [DbContext(typeof(TodoContext))]
    [Migration("20250709062717_SyncModelWithDb")]
    partial class SyncModelWithDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("tod.Models.Category", b =>
                {
                    b.Property<string>("categoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("categoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("categoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            categoryId = "personal",
                            categoryName = "Personal"
                        },
                        new
                        {
                            categoryId = "work",
                            categoryName = "Work"
                        },
                        new
                        {
                            categoryId = "school",
                            categoryName = "School"
                        },
                        new
                        {
                            categoryId = "relationship",
                            categoryName = "Relationship"
                        },
                        new
                        {
                            categoryId = "health",
                            categoryName = "Health"
                        },
                        new
                        {
                            categoryId = "finance",
                            categoryName = "Finance"
                        },
                        new
                        {
                            categoryId = "other",
                            categoryName = "Other"
                        });
                });

            modelBuilder.Entity("tod.Models.Status", b =>
                {
                    b.Property<string>("statusId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("statusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("statusId");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            statusId = "pending",
                            statusName = "Pending"
                        },
                        new
                        {
                            statusId = "completed",
                            statusName = "Completed"
                        });
                });

            modelBuilder.Entity("tod.Models.Team", b =>
                {
                    b.Property<int>("teamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("teamId"));

                    b.Property<string>("teamInvitationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("teamLeader")
                        .HasColumnType("int");

                    b.Property<string>("teamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("teamId");

                    b.HasIndex("teamLeader");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("tod.Models.Todo", b =>
                {
                    b.Property<int>("todId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("todId"));

                    b.Property<DateTime?>("ArchivedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<string>("categoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("dueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("statusId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("todDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("todName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("urgency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("userId")
                        .HasColumnType("int");

                    b.HasKey("todId");

                    b.HasIndex("categoryId");

                    b.HasIndex("statusId");

                    b.HasIndex("userId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("tod.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userId"));

                    b.Property<int?>("teamId")
                        .HasColumnType("int");

                    b.Property<string>("userMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.HasIndex("teamId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("tod.Models.Team", b =>
                {
                    b.HasOne("tod.Models.User", "Leader")
                        .WithMany()
                        .HasForeignKey("teamLeader")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Leader");
                });

            modelBuilder.Entity("tod.Models.Todo", b =>
                {
                    b.HasOne("tod.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tod.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("statusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tod.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("Category");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("tod.Models.User", b =>
                {
                    b.HasOne("tod.Models.Team", "Team")
                        .WithMany("Members")
                        .HasForeignKey("teamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("tod.Models.Team", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
