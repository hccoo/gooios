﻿// <auto-generated />
using Gooios.ActivityService.Domains.Aggregates;
using Gooios.ActivityService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Gooios.ActivityService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180522225024_AddProductMarkForActivity")]
    partial class AddProductMarkForActivity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Gooios.ActivityService.Domains.Aggregates.GrouponActivity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnName("count");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<DateTime>("End")
                        .HasColumnName("end");

                    b.Property<DateTime>("LastUpdOn")
                        .HasColumnName("updated_on");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnName("product_id")
                        .HasMaxLength(80);

                    b.Property<string>("ProductMark")
                        .IsRequired()
                        .HasColumnName("product_mark")
                        .HasMaxLength(80);

                    b.Property<DateTime>("Start")
                        .HasColumnName("start");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("unit_price");

                    b.HasKey("Id");

                    b.ToTable("groupon_activities");
                });

            modelBuilder.Entity("Gooios.ActivityService.Domains.Aggregates.GrouponParticipation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("BuyCount")
                        .HasColumnName("buy_count");

                    b.Property<string>("GrouponActivityId")
                        .IsRequired()
                        .HasColumnName("groupon_activity_id")
                        .HasMaxLength(80);

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnName("order_id")
                        .HasMaxLength(80);

                    b.Property<DateTime>("ParticipateTime")
                        .HasColumnName("participate_time");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("groupon_participations");
                });
#pragma warning restore 612, 618
        }
    }
}
