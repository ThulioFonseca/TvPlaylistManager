﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TvPlaylistManager.Infrastructure.Data;

#nullable disable

namespace TvPlaylistManager.Infrastructure.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250301134349_AddUniqueIndexEpgSource")]
    partial class AddUniqueIndexEpgSource
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("TvPlaylistManager.Domain.Models.Epg.EpgChannel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChannelEpgId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("EpgSourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IconUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Keywords")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelEpgId");

                    b.HasIndex("EpgSourceId");

                    b.ToTable("EpgChannel");
                });

            modelBuilder.Entity("TvPlaylistManager.Domain.Models.Epg.EpgSource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique();

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("EpgSources");
                });

            modelBuilder.Entity("TvPlaylistManager.Domain.Models.Epg.EpgChannel", b =>
                {
                    b.HasOne("TvPlaylistManager.Domain.Models.Epg.EpgSource", "EpgSource")
                        .WithMany("Channels")
                        .HasForeignKey("EpgSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EpgSource");
                });

            modelBuilder.Entity("TvPlaylistManager.Domain.Models.Epg.EpgSource", b =>
                {
                    b.Navigation("Channels");
                });
#pragma warning restore 612, 618
        }
    }
}
