﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using NoteBase.Models;
using System;

namespace NoteBase.Migrations
{
    [DbContext(typeof(DbModel))]
    [Migration("20180502212624_NoteBase")]
    partial class NoteBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("NoteBase.Models.Note", b =>
                {
                    b.Property<int>("Note_Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("Header");

                    b.Property<long>("Timestamp");

                    b.Property<int>("User_Id");

                    b.HasKey("Note_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("DbSetNotes");
                });

            modelBuilder.Entity("NoteBase.Models.Shares", b =>
                {
                    b.Property<int>("Note_Id");

                    b.Property<int>("Owner_Id");

                    b.Property<int>("User_Id");

                    b.HasKey("Note_Id", "Owner_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("DbSetShares");
                });

            modelBuilder.Entity("NoteBase.Models.Users", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("User_Id");

                    b.ToTable("DbSetUsers");
                });

            modelBuilder.Entity("NoteBase.Models.Note", b =>
                {
                    b.HasOne("NoteBase.Models.Users", "User")
                        .WithMany("Notes")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoteBase.Models.Shares", b =>
                {
                    b.HasOne("NoteBase.Models.Note", "Note")
                        .WithMany()
                        .HasForeignKey("Note_Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoteBase.Models.Users", "User")
                        .WithMany("Shares")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
