﻿// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-19-2022
// ***********************************************************************
// <copyright file="20220419201303_first.Designer.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes2022.Server.Data;

#nullable disable

namespace Notes2022.Server.Migrations
{
    /// <summary>
    /// Class first.
    /// Implements the <see cref="Migration" />
    /// </summary>
    /// <seealso cref="Migration" />
    [DbContext(typeof(NotesDbContext))]
    [Migration("20220419201303_first")]
    partial class first
    {
        /// <summary>
        /// Builds the target model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Notes2022.Server.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("Ipref0")
                        .HasColumnType("int");

                    b.Property<int>("Ipref1")
                        .HasColumnType("int");

                    b.Property<int>("Ipref2")
                        .HasColumnType("int");

                    b.Property<int>("Ipref3")
                        .HasColumnType("int");

                    b.Property<int>("Ipref4")
                        .HasColumnType("int");

                    b.Property<int>("Ipref5")
                        .HasColumnType("int");

                    b.Property<int>("Ipref6")
                        .HasColumnType("int");

                    b.Property<int>("Ipref7")
                        .HasColumnType("int");

                    b.Property<int>("Ipref8")
                        .HasColumnType("int");

                    b.Property<int>("Ipref9")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MyGuid")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref0")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref1")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref2")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref3")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref4")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref5")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref6")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref7")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref8")
                        .HasColumnType("bit");

                    b.Property<bool>("Pref9")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeZoneID")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Audit", b =>
                {
                    b.Property<long>("AuditID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AuditID"), 1L, 1);

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("AuditID");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.HomePageMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("Posted")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("HomePageMessage");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.LinkedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AcceptFrom")
                        .HasColumnType("bit");

                    b.Property<int>("HomeFileId")
                        .HasColumnType("int");

                    b.Property<string>("HomeFileName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("RemoteBaseUri")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RemoteFileName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Secret")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("SendTo")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("LinkedFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.LinkQueue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("Activity")
                        .HasColumnType("int");

                    b.Property<string>("BaseUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enqueued")
                        .HasColumnType("bit");

                    b.Property<string>("LinkGuid")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("LinkedFileId")
                        .HasColumnType("int");

                    b.Property<string>("Secret")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("LinkQueue");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Mark", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("MarkOrdinal")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.Property<int>("ArchiveId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<long>("NoteHeaderId")
                        .HasColumnType("bigint");

                    b.Property<int>("NoteOrdinal")
                        .HasColumnType("int");

                    b.Property<int>("ResponseOrdinal")
                        .HasColumnType("int");

                    b.HasKey("UserId", "NoteFileId", "MarkOrdinal");

                    b.HasIndex("NoteFileId");

                    b.HasIndex("UserId", "NoteFileId");

                    b.ToTable("Mark");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteAccess", b =>
                {
                    b.Property<string>("UserID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("ArchiveId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<bool>("DeleteEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("EditAccess")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadAccess")
                        .HasColumnType("bit");

                    b.Property<bool>("Respond")
                        .HasColumnType("bit");

                    b.Property<bool>("SetTag")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewAccess")
                        .HasColumnType("bit");

                    b.Property<bool>("Write")
                        .HasColumnType("bit");

                    b.HasKey("UserID", "NoteFileId", "ArchiveId");

                    b.ToTable("NoteAccess");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteContent", b =>
                {
                    b.Property<long>("NoteHeaderId")
                        .HasColumnType("bigint");

                    b.Property<string>("NoteBody")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteHeaderId");

                    b.ToTable("NoteContent");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoteFileName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NoteFileTitle")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NumberArchives")
                        .HasColumnType("int");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("NoteFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("ArchiveId")
                        .HasColumnType("int");

                    b.Property<string>("AuthorID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("BaseNoteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DirectorMessage")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("LinkGuid")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int");

                    b.Property<int>("NoteOrdinal")
                        .HasColumnType("int");

                    b.Property<string>("NoteSubject")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("RefId")
                        .HasColumnType("bigint");

                    b.Property<int>("ResponseCount")
                        .HasColumnType("int");

                    b.Property<int>("ResponseOrdinal")
                        .HasColumnType("int");

                    b.Property<DateTime>("ThreadLastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LinkGuid");

                    b.HasIndex("NoteFileId");

                    b.HasIndex("NoteFileId", "ArchiveId");

                    b.ToTable("NoteHeader");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Search", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ArchiveId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("BaseOrdinal")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<long>("NoteID")
                        .HasColumnType("bigint")
                        .HasColumnOrder(4);

                    b.Property<int>("Option")
                        .HasColumnType("int");

                    b.Property<int>("ResponseOrdinal")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("Search");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Sequencer", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "NoteFileId");

                    b.ToTable("Sequencer");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.SQLFile", b =>
                {
                    b.Property<long>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("FileId"), 1L, 1);

                    b.Property<string>("Comments")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Contributor")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("FileId");

                    b.ToTable("SQLFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.SQLFileContent", b =>
                {
                    b.Property<long>("SQLFileId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("SQLFileId");

                    b.ToTable("SQLFileContent");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NoteFileId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Tags", b =>
                {
                    b.Property<string>("Tag")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<long>("NoteHeaderId")
                        .HasColumnType("bigint");

                    b.Property<int>("ArchiveId")
                        .HasColumnType("int");

                    b.Property<int>("NoteFileId")
                        .HasColumnType("int");

                    b.HasKey("Tag", "NoteHeaderId");

                    b.HasIndex("NoteFileId");

                    b.HasIndex("NoteHeaderId");

                    b.HasIndex("NoteFileId", "ArchiveId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.TZone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Offset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OffsetHours")
                        .HasColumnType("int");

                    b.Property<int>("OffsetMinutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TZone");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Abbreviation = "GMT",
                            Name = "Greenwich Mean Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 2,
                            Abbreviation = "WAKT",
                            Name = "Wake Island Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 3,
                            Abbreviation = "CHAST",
                            Name = "Chatham Standard Time",
                            Offset = "UTC+12:45",
                            OffsetHours = 12,
                            OffsetMinutes = 45
                        },
                        new
                        {
                            Id = 4,
                            Abbreviation = "NZDT",
                            Name = "New Zealand Daylight Time",
                            Offset = "UTC+13",
                            OffsetHours = 13,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 5,
                            Abbreviation = "PHOT",
                            Name = "Phoenix Island Time",
                            Offset = "UTC+13",
                            OffsetHours = 13,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 6,
                            Abbreviation = "TKT",
                            Name = "Tokelau Time",
                            Offset = "UTC+13",
                            OffsetHours = 13,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 7,
                            Abbreviation = "TOT",
                            Name = "Tonga Time",
                            Offset = "UTC+13",
                            OffsetHours = 13,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 8,
                            Abbreviation = "CHADT",
                            Name = "Chatham Daylight Time",
                            Offset = "UTC+13:45",
                            OffsetHours = 13,
                            OffsetMinutes = 45
                        },
                        new
                        {
                            Id = 9,
                            Abbreviation = "LINT",
                            Name = "Line Islands Time",
                            Offset = "UTC+14",
                            OffsetHours = 14,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 10,
                            Abbreviation = "AZOST",
                            Name = "Azores Standard Time",
                            Offset = "UTC-01",
                            OffsetHours = -1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 11,
                            Abbreviation = "CVT",
                            Name = "Cape Verde Time",
                            Offset = "UTC-01",
                            OffsetHours = -1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 12,
                            Abbreviation = "EGT",
                            Name = "Eastern Greenland Time",
                            Offset = "UTC-01",
                            OffsetHours = -1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 13,
                            Abbreviation = "BRST",
                            Name = "Brasilia Summer Time",
                            Offset = "UTC-02",
                            OffsetHours = -2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 14,
                            Abbreviation = "FNT",
                            Name = "Fernando de Noronha Time",
                            Offset = "UTC-02",
                            OffsetHours = -2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 15,
                            Abbreviation = "GST",
                            Name = "South Georgia and the South Sandwich Islands",
                            Offset = "UTC-02",
                            OffsetHours = -2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 16,
                            Abbreviation = "PMDT",
                            Name = "Saint Pierre and Miquelon Daylight time",
                            Offset = "UTC-02",
                            OffsetHours = -2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 17,
                            Abbreviation = "UYST",
                            Name = "Uruguay Summer Time",
                            Offset = "UTC-02",
                            OffsetHours = -2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 18,
                            Abbreviation = "NDT",
                            Name = "Newfoundland Daylight Time",
                            Offset = "UTC-02:30",
                            OffsetHours = -2,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 19,
                            Abbreviation = "ADT",
                            Name = "Atlantic Daylight Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 20,
                            Abbreviation = "AMST",
                            Name = "Amazon Summer Time (Brazil)[1]",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 21,
                            Abbreviation = "ART",
                            Name = "Argentina Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 22,
                            Abbreviation = "BRT",
                            Name = "Brasilia Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 23,
                            Abbreviation = "TVT",
                            Name = "Tuvalu Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 24,
                            Abbreviation = "PETT",
                            Name = "Kamchatka Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 25,
                            Abbreviation = "NZST",
                            Name = "New Zealand Standard Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 26,
                            Abbreviation = "MHT",
                            Name = "Marshall Islands",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 27,
                            Abbreviation = "DDUT",
                            Name = "Dumont d'Urville Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 28,
                            Abbreviation = "EST",
                            Name = "Eastern Standard Time (Australia)",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 29,
                            Abbreviation = "PGT",
                            Name = "Papua New Guinea Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 30,
                            Abbreviation = "VLAT",
                            Name = "Vladivostok Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 31,
                            Abbreviation = "ACDT",
                            Name = "Australian Central Daylight Savings Time",
                            Offset = "UTC+10:30",
                            OffsetHours = 10,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 32,
                            Abbreviation = "CST",
                            Name = "Central Summer Time (Australia)",
                            Offset = "UTC+10:30",
                            OffsetHours = 10,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 33,
                            Abbreviation = "LHST",
                            Name = "Lord Howe Standard Time",
                            Offset = "UTC+10:30",
                            OffsetHours = 10,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 34,
                            Abbreviation = "AEDT",
                            Name = "Australian Eastern Daylight Savings Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 35,
                            Abbreviation = "BST",
                            Name = "Bougainville Standard Time[4]",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 36,
                            Abbreviation = "KOST",
                            Name = "Kosrae Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 37,
                            Abbreviation = "CLST",
                            Name = "Chile Summer Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 38,
                            Abbreviation = "LHST",
                            Name = "Lord Howe Summer Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 39,
                            Abbreviation = "NCT",
                            Name = "New Caledonia Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 40,
                            Abbreviation = "PONT",
                            Name = "Pohnpei Standard Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 41,
                            Abbreviation = "SAKT",
                            Name = "Sakhalin Island time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 42,
                            Abbreviation = "SBT",
                            Name = "Solomon Islands Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 43,
                            Abbreviation = "SRET",
                            Name = "Srednekolymsk Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 44,
                            Abbreviation = "VUT",
                            Name = "Vanuatu Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 45,
                            Abbreviation = "NFT",
                            Name = "Norfolk Time",
                            Offset = "UTC+11:00",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 46,
                            Abbreviation = "FJT",
                            Name = "Fiji Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 47,
                            Abbreviation = "GILT",
                            Name = "Gilbert Island Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 48,
                            Abbreviation = "MAGT",
                            Name = "Magadan Time",
                            Offset = "UTC+12",
                            OffsetHours = 12,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 49,
                            Abbreviation = "MIST",
                            Name = "Macquarie Island Station Time",
                            Offset = "UTC+11",
                            OffsetHours = 11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 50,
                            Abbreviation = "CHUT",
                            Name = "Chuuk Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 51,
                            Abbreviation = "FKST",
                            Name = "Falkland Islands Standard Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 52,
                            Abbreviation = "GFT",
                            Name = "French Guiana Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 53,
                            Abbreviation = "PET",
                            Name = "Peru Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 54,
                            Abbreviation = "CST",
                            Name = "Central Standard Time (North America)",
                            Offset = "UTC-06",
                            OffsetHours = -6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 55,
                            Abbreviation = "EAST",
                            Name = "Easter Island Standard Time",
                            Offset = "UTC-06",
                            OffsetHours = -6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 56,
                            Abbreviation = "GALT",
                            Name = "Galapagos Time",
                            Offset = "UTC-06",
                            OffsetHours = -6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 57,
                            Abbreviation = "MDT",
                            Name = "Mountain Daylight Time (North America)",
                            Offset = "UTC-06",
                            OffsetHours = -6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 58,
                            Abbreviation = "MST",
                            Name = "Mountain Standard Time (North America)",
                            Offset = "UTC-07",
                            OffsetHours = -7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 59,
                            Abbreviation = "PDT",
                            Name = "Pacific Daylight Time (North America)",
                            Offset = "UTC-07",
                            OffsetHours = -7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 60,
                            Abbreviation = "AKDT",
                            Name = "Alaska Daylight Time",
                            Offset = "UTC-08",
                            OffsetHours = -8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 61,
                            Abbreviation = "CIST",
                            Name = "Clipperton Island Standard Time",
                            Offset = "UTC-08",
                            OffsetHours = -8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 62,
                            Abbreviation = "PST",
                            Name = "Pacific Standard Time (North America)",
                            Offset = "UTC-08",
                            OffsetHours = -8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 63,
                            Abbreviation = "AKST",
                            Name = "Alaska Standard Time",
                            Offset = "UTC-09",
                            OffsetHours = -9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 64,
                            Abbreviation = "GAMT",
                            Name = "Gambier Islands",
                            Offset = "UTC-09",
                            OffsetHours = -9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 65,
                            Abbreviation = "GIT",
                            Name = "Gambier Island Time",
                            Offset = "UTC-09",
                            OffsetHours = -9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 66,
                            Abbreviation = "HADT",
                            Name = "Hawaii-Aleutian Daylight Time",
                            Offset = "UTC-09",
                            OffsetHours = -9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 67,
                            Abbreviation = "MART",
                            Name = "Marquesas Islands Time",
                            Offset = "UTC-09:30",
                            OffsetHours = -9,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 68,
                            Abbreviation = "MIT",
                            Name = "Marquesas Islands Time",
                            Offset = "UTC-09:30",
                            OffsetHours = -9,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 69,
                            Abbreviation = "CKT",
                            Name = "Cook Island Time",
                            Offset = "UTC-10",
                            OffsetHours = -10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 70,
                            Abbreviation = "HAST",
                            Name = "Hawaii-Aleutian Standard Time",
                            Offset = "UTC-10",
                            OffsetHours = -10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 71,
                            Abbreviation = "HST",
                            Name = "Hawaii Standard Time",
                            Offset = "UTC-10",
                            OffsetHours = -10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 72,
                            Abbreviation = "TAHT",
                            Name = "Tahiti Time",
                            Offset = "UTC-10",
                            OffsetHours = -10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 73,
                            Abbreviation = "NUT",
                            Name = "Niue Time",
                            Offset = "UTC-11",
                            OffsetHours = -11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 74,
                            Abbreviation = "EST",
                            Name = "Eastern Standard Time (North America)",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 75,
                            Abbreviation = "ECT",
                            Name = "Ecuador Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 76,
                            Abbreviation = "EASST",
                            Name = "Easter Island Standard Summer Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 77,
                            Abbreviation = "CST",
                            Name = "Cuba Standard Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 78,
                            Abbreviation = "PMST",
                            Name = "Saint Pierre and Miquelon Standard Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 79,
                            Abbreviation = "PYST",
                            Name = "Paraguay Summer Time (South America)[8]",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 80,
                            Abbreviation = "ROTT",
                            Name = "Rothera Research Station Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 81,
                            Abbreviation = "SRT",
                            Name = "Suriname Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 82,
                            Abbreviation = "UYT",
                            Name = "Uruguay Standard Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 83,
                            Abbreviation = "NST",
                            Name = "Newfoundland Standard Time",
                            Offset = "UTC-03:30",
                            OffsetHours = -3,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 84,
                            Abbreviation = "NT",
                            Name = "Newfoundland Time",
                            Offset = "UTC-03:30",
                            OffsetHours = -3,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 85,
                            Abbreviation = "AMT",
                            Name = "Amazon Time (Brazil)[2]",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 86,
                            Abbreviation = "AST",
                            Name = "Atlantic Standard Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 87,
                            Abbreviation = "BOT",
                            Name = "Bolivia Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 88,
                            Abbreviation = "FKST",
                            Name = "Falkland Islands Summer Time",
                            Offset = "UTC-03",
                            OffsetHours = -3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 89,
                            Abbreviation = "CDT",
                            Name = "Cuba Daylight Time[5]",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 90,
                            Abbreviation = "COST",
                            Name = "Colombia Summer Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 91,
                            Abbreviation = "ECT",
                            Name = "Eastern Caribbean Time (does not recognise DST)",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 92,
                            Abbreviation = "EDT",
                            Name = "Eastern Daylight Time (North America)",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 93,
                            Abbreviation = "FKT",
                            Name = "Falkland Islands Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 94,
                            Abbreviation = "GYT",
                            Name = "Guyana Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 95,
                            Abbreviation = "PYT",
                            Name = "Paraguay Time (South America)[9]",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 96,
                            Abbreviation = "VET",
                            Name = "Venezuelan Standard Time",
                            Offset = "UTC-04:30",
                            OffsetHours = -4,
                            OffsetMinutes = -30
                        },
                        new
                        {
                            Id = 97,
                            Abbreviation = "ACT",
                            Name = "Acre Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 98,
                            Abbreviation = "CDT",
                            Name = "Central Daylight Time (North America)",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 99,
                            Abbreviation = "COT",
                            Name = "Colombia Time",
                            Offset = "UTC-05",
                            OffsetHours = -5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 100,
                            Abbreviation = "CLT",
                            Name = "Chile Standard Time",
                            Offset = "UTC-04",
                            OffsetHours = -4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 101,
                            Abbreviation = "ChST",
                            Name = "Chamorro Standard Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 102,
                            Abbreviation = "AEST",
                            Name = "Australian Eastern Standard Time",
                            Offset = "UTC+10",
                            OffsetHours = 10,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 103,
                            Abbreviation = "CST",
                            Name = "Central Standard Time (Australia)",
                            Offset = "UTC+09:30",
                            OffsetHours = 9,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 104,
                            Abbreviation = "EEDT",
                            Name = "Eastern European Daylight Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 105,
                            Abbreviation = "EEST",
                            Name = "Eastern European Summer Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 106,
                            Abbreviation = "FET",
                            Name = "Further-eastern European Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 107,
                            Abbreviation = "IDT",
                            Name = "Israel Daylight Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 108,
                            Abbreviation = "IOT",
                            Name = "Indian Ocean Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 109,
                            Abbreviation = "MSK",
                            Name = "Moscow Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 110,
                            Abbreviation = "SYOT",
                            Name = "Showa Station Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 111,
                            Abbreviation = "IRST",
                            Name = "Iran Standard Time",
                            Offset = "UTC+03:30",
                            OffsetHours = 3,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 112,
                            Abbreviation = "AMT",
                            Name = "Armenia Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 113,
                            Abbreviation = "AZT",
                            Name = "Azerbaijan Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 114,
                            Abbreviation = "GET",
                            Name = "Georgia Standard Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 115,
                            Abbreviation = "GST",
                            Name = "Gulf Standard Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 116,
                            Abbreviation = "MUT",
                            Name = "Mauritius Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 117,
                            Abbreviation = "RET",
                            Name = "R?union Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 118,
                            Abbreviation = "SAMT",
                            Name = "Samara Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 119,
                            Abbreviation = "SCT",
                            Name = "Seychelles Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 120,
                            Abbreviation = "VOLT",
                            Name = "Volgograd Time",
                            Offset = "UTC+04",
                            OffsetHours = 4,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 121,
                            Abbreviation = "AFT",
                            Name = "Afghanistan Time",
                            Offset = "UTC+04:30",
                            OffsetHours = 4,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 122,
                            Abbreviation = "IRDT",
                            Name = "Iran Daylight Time",
                            Offset = "UTC+04:30",
                            OffsetHours = 4,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 123,
                            Abbreviation = "HMT",
                            Name = "Heard and McDonald Islands Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 124,
                            Abbreviation = "MAWT",
                            Name = "Mawson Station Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 125,
                            Abbreviation = "EAT",
                            Name = "East Africa Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 126,
                            Abbreviation = "AST",
                            Name = "Arabia Standard Time",
                            Offset = "UTC+03",
                            OffsetHours = 3,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 127,
                            Abbreviation = "WAST",
                            Name = "West Africa Summer Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 128,
                            Abbreviation = "USZ1",
                            Name = "Kaliningrad Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 129,
                            Abbreviation = "IBST",
                            Name = "International Business Standard Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 130,
                            Abbreviation = "UCT",
                            Name = "Coordinated Universal Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 131,
                            Abbreviation = "UTC",
                            Name = "Coordinated Universal Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 132,
                            Abbreviation = "WET",
                            Name = "Western European Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 133,
                            Abbreviation = "Z",
                            Name = "Zulu Time (Coordinated Universal Time)",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 134,
                            Abbreviation = "EGST",
                            Name = "Eastern Greenland Summer Time",
                            Offset = "UTC+00",
                            OffsetHours = 0,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 135,
                            Abbreviation = "BST",
                            Name = "British Summer Time (British Standard Time from Feb 1968 to Oct 1971)",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 136,
                            Abbreviation = "CET",
                            Name = "Central European Time",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 137,
                            Abbreviation = "DFT",
                            Name = "AIX specific equivalent of Central European Time[6]",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 138,
                            Abbreviation = "IST",
                            Name = "Irish Standard Time[7]",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 139,
                            Abbreviation = "MVT",
                            Name = "Maldives Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 140,
                            Abbreviation = "MET",
                            Name = "Middle European Time Same zone as CET",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 141,
                            Abbreviation = "WEDT",
                            Name = "Western European Daylight Time",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 142,
                            Abbreviation = "WEST",
                            Name = "Western European Summer Time",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 143,
                            Abbreviation = "CAT",
                            Name = "Central Africa Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 144,
                            Abbreviation = "CEDT",
                            Name = "Central European Daylight Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 145,
                            Abbreviation = "CEST",
                            Name = "Central European Summer Time (Cf. HAEC)",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 146,
                            Abbreviation = "EET",
                            Name = "Eastern European Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 147,
                            Abbreviation = "HAEC",
                            Name = "Heure Avanc?e d'Europe Centrale francised name for CEST",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 148,
                            Abbreviation = "IST",
                            Name = "Israel Standard Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 149,
                            Abbreviation = "MEST",
                            Name = "Middle European Summer Time Same zone as CEST",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 150,
                            Abbreviation = "SAST",
                            Name = "South African Standard Time",
                            Offset = "UTC+02",
                            OffsetHours = 2,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 151,
                            Abbreviation = "WAT",
                            Name = "West Africa Time",
                            Offset = "UTC+01",
                            OffsetHours = 1,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 152,
                            Abbreviation = "ORAT",
                            Name = "Oral Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 153,
                            Abbreviation = "PKT",
                            Name = "Pakistan Standard Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 154,
                            Abbreviation = "TFT",
                            Name = "Indian/Kerguelen",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 155,
                            Abbreviation = "BDT",
                            Name = "Brunei Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 156,
                            Abbreviation = "CHOT",
                            Name = "Choibalsan",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 157,
                            Abbreviation = "CIT",
                            Name = "Central Indonesia Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 158,
                            Abbreviation = "CST",
                            Name = "China Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 159,
                            Abbreviation = "CT",
                            Name = "China time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 160,
                            Abbreviation = "HKT",
                            Name = "Hong Kong Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 161,
                            Abbreviation = "IRKT",
                            Name = "Irkutsk Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 162,
                            Abbreviation = "MST",
                            Name = "Malaysia Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 163,
                            Abbreviation = "MYT",
                            Name = "Malaysia Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 164,
                            Abbreviation = "PST",
                            Name = "Philippine Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 165,
                            Abbreviation = "AWST",
                            Name = "Australian Western Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 166,
                            Abbreviation = "SGT",
                            Name = "Singapore Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 167,
                            Abbreviation = "ULAT",
                            Name = "Ulaanbaatar Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 168,
                            Abbreviation = "WST",
                            Name = "Western Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 169,
                            Abbreviation = "CWST",
                            Name = "Central Western Standard Time (Australia) unofficial",
                            Offset = "UTC+08:45",
                            OffsetHours = 8,
                            OffsetMinutes = 45
                        },
                        new
                        {
                            Id = 170,
                            Abbreviation = "AWDT",
                            Name = "Australian Western Daylight Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 171,
                            Abbreviation = "EIT",
                            Name = "Eastern Indonesian Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 172,
                            Abbreviation = "JST",
                            Name = "Japan Standard Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 173,
                            Abbreviation = "KST",
                            Name = "Korea Standard Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 174,
                            Abbreviation = "TLT",
                            Name = "Timor Leste Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 175,
                            Abbreviation = "YAKT",
                            Name = "Yakutsk Time",
                            Offset = "UTC+09",
                            OffsetHours = 9,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 176,
                            Abbreviation = "ACST",
                            Name = "Australian Central Standard Time",
                            Offset = "UTC+09:30",
                            OffsetHours = 9,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 177,
                            Abbreviation = "SST",
                            Name = "Singapore Standard Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 178,
                            Abbreviation = "SST",
                            Name = "Samoa Standard Time",
                            Offset = "UTC-11",
                            OffsetHours = -11,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 179,
                            Abbreviation = "ACT",
                            Name = "ASEAN Common Time",
                            Offset = "UTC+08",
                            OffsetHours = 8,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 180,
                            Abbreviation = "THA",
                            Name = "Thailand Standard Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 181,
                            Abbreviation = "TJT",
                            Name = "Tajikistan Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 182,
                            Abbreviation = "TMT",
                            Name = "Turkmenistan Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 183,
                            Abbreviation = "UZT",
                            Name = "Uzbekistan Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 184,
                            Abbreviation = "YEKT",
                            Name = "Yekaterinburg Time",
                            Offset = "UTC+05",
                            OffsetHours = 5,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 185,
                            Abbreviation = "IST",
                            Name = "Indian Standard Time",
                            Offset = "UTC+05:30",
                            OffsetHours = 5,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 186,
                            Abbreviation = "SLST",
                            Name = "Sri Lanka Standard Time",
                            Offset = "UTC+05:30",
                            OffsetHours = 5,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 187,
                            Abbreviation = "NPT",
                            Name = "Nepal Time",
                            Offset = "UTC+05:45",
                            OffsetHours = 5,
                            OffsetMinutes = 45
                        },
                        new
                        {
                            Id = 188,
                            Abbreviation = "BDT",
                            Name = "Bangladesh Daylight Time (Bangladesh Daylight saving time keeps UTC+06 offset) [3]",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 189,
                            Abbreviation = "BIOT",
                            Name = "British Indian Ocean Time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 190,
                            Abbreviation = "BST",
                            Name = "Bangladesh Standard Time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 191,
                            Abbreviation = "WIT",
                            Name = "Western Indonesian Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 192,
                            Abbreviation = "BTT",
                            Name = "Bhutan Time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 193,
                            Abbreviation = "OMST",
                            Name = "Omsk Time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 194,
                            Abbreviation = "VOST",
                            Name = "Vostok Station Time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 195,
                            Abbreviation = "CCT",
                            Name = "Cocos Islands Time",
                            Offset = "UTC+06:30",
                            OffsetHours = 6,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 196,
                            Abbreviation = "MMT",
                            Name = "Myanmar Time",
                            Offset = "UTC+06:30",
                            OffsetHours = 6,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 197,
                            Abbreviation = "MST",
                            Name = "Myanmar Standard Time",
                            Offset = "UTC+06:30",
                            OffsetHours = 6,
                            OffsetMinutes = 30
                        },
                        new
                        {
                            Id = 198,
                            Abbreviation = "CXT",
                            Name = "Christmas Island Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 199,
                            Abbreviation = "DAVT",
                            Name = "Davis Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 200,
                            Abbreviation = "HOVT",
                            Name = "Khovd Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 201,
                            Abbreviation = "ICT",
                            Name = "Indochina Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 202,
                            Abbreviation = "KRAT",
                            Name = "Krasnoyarsk Time",
                            Offset = "UTC+07",
                            OffsetHours = 7,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 203,
                            Abbreviation = "KGT",
                            Name = "Kyrgyzstan time",
                            Offset = "UTC+06",
                            OffsetHours = 6,
                            OffsetMinutes = 0
                        },
                        new
                        {
                            Id = 204,
                            Abbreviation = "BIT",
                            Name = "Baker Island Time",
                            Offset = "UTC-12",
                            OffsetHours = -12,
                            OffsetMinutes = 0
                        });
                });

            modelBuilder.Entity("Notes2022.Shared.LinkLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("LinkLog");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notes2022.Server.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Mark", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.NoteFile", "NoteFile")
                        .WithMany()
                        .HasForeignKey("NoteFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NoteFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteContent", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.NoteHeader", null)
                        .WithOne("NoteContent")
                        .HasForeignKey("Notes2022.Server.Entities.NoteContent", "NoteHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Notes2022.Server.Entities.SQLFileContent", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.SQLFile", "SQLFile")
                        .WithOne("Content")
                        .HasForeignKey("Notes2022.Server.Entities.SQLFileContent", "SQLFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SQLFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Subscription", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.NoteFile", "NoteFile")
                        .WithMany()
                        .HasForeignKey("NoteFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NoteFile");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.Tags", b =>
                {
                    b.HasOne("Notes2022.Server.Entities.NoteHeader", null)
                        .WithMany("Tags")
                        .HasForeignKey("NoteHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Notes2022.Server.Entities.NoteHeader", b =>
                {
                    b.Navigation("NoteContent");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Notes2022.Server.Entities.SQLFile", b =>
                {
                    b.Navigation("Content");
                });
#pragma warning restore 612, 618
        }
    }
}
